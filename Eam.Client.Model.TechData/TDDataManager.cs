using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// Class for technology data managing (store and access)
    /// </summary>
    public class TDDataManager {

        private Interfaces.IDataStore _externalDataStore;
        private Interfaces.IDataStore _logDataStore;

        private delegate void UpdateExternalDataStore(CommonDataContract.PollItemValue[] pollItemValues);
        private UpdateExternalDataStore _saveValues;

        private TDDataBuffer CurrentDataBuffer { get; set; }

        public DateTime GetLastUpdatedTimestamp {
            get { return CurrentDataBuffer.LastTimeStamp; }
        }

        internal TDDataManager(Interfaces.IDataStore dataStore) {
            _externalDataStore = dataStore;
            _saveValues = new UpdateExternalDataStore(_externalDataStore.SaveValues);
            _logDataStore = new InternalDAL.LogFileDataStore();
            CurrentDataBuffer = new TDDataBuffer();
        }

        internal void UpdateValues(CommonDataContract.PollItemValue[] pollItemValues) {
            UpdateValuesToDataBuffer(pollItemValues);
            //_saveValues.BeginInvoke(pollItemValues, null, null);
            _externalDataStore.SaveValues(pollItemValues);
            _logDataStore.SaveValues(pollItemValues);
        }

        internal void RegisterPollItems(string[] itemNames) {
            CurrentDataBuffer.RegisterPollItems(itemNames);
        }

        internal void RegisterPollItems(IList<CommonDataContract.PollItem> pollItems) {
            CurrentDataBuffer.RegisterPollItems(pollItems);
        }

        public CommonDataContract.PollItemValue GetLastPollItemValue(string pollItemName) {
            return CurrentDataBuffer.GetLastPollItemValue(pollItemName);
        }

        public void SubscribePollItem(string pollItemName) {
        }

        private void UpdateValuesToDataBuffer(CommonDataContract.PollItemValue[] pollItemValues) {
            string itemID;
            CommonDataContract.PollItem currentPollItem;
            foreach (CommonDataContract.PollItemValue currentItemValue in pollItemValues) {
                itemID = currentItemValue.ItemID;
                if (CurrentDataBuffer.Contains(itemID)) {
                    CurrentDataBuffer.RegisterPollItem(itemID);
                }
                currentPollItem = CurrentDataBuffer.GetPollItemByID(itemID);
                if (currentPollItem.IsPackage) {
                    UnpackAndSaveSubItems(currentPollItem, currentItemValue);
                }
                currentPollItem.AddValue(currentItemValue.Timestamp, currentItemValue);
            }
            CurrentDataBuffer.LastTimeStamp = DateTime.Now;
        }

        private void UnpackAndSaveSubItems(CommonDataContract.PollItem currentPollItem, CommonDataContract.PollItemValue currentItemValue) {
            int intValue = Convert.ToInt16(currentItemValue.Value);
            bool[] fullBits = new bool[16];
            char[] bits = Convert.ToString(intValue, 2).ToCharArray();
            for (int i = 1; i <= bits.Length; i++) {
                if (bits[bits.Length-i] == '1')
                    fullBits[fullBits.Length - i] = true;
            }
            
            CommonDataContract.PollItemValue newPollItemValue;
            foreach (var subItem in currentPollItem.SubItems) {
                newPollItemValue = new CommonDataContract.PollItemValue();
                newPollItemValue.ItemID = subItem.ItemName;
                newPollItemValue.Timestamp = currentItemValue.Timestamp;
                newPollItemValue.Value = fullBits[subItem.BitOffset];
                subItem.AddValue(newPollItemValue.Timestamp, newPollItemValue);
            }
        }
    }
}
