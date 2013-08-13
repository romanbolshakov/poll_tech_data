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

        private TDDataBuffer CurrentDataBuffer { get; set; }

        public DateTime GetLastUpdatedTimestamp {
            get { return CurrentDataBuffer.LastTimeStamp; }
        }

        internal TDDataManager(Interfaces.IDataStore dataStore) {
            _externalDataStore = dataStore;
            _logDataStore = new InternalDAL.LogFileDataStore();
            CurrentDataBuffer = new TDDataBuffer();
        }

        internal void UpdateValues(CommonDataContract.PollItemValue[] pollItemValues) {
            List<CommonDataContract.PollItem> updatedPollItems = null;
            try {
                updatedPollItems = UpdateValuesToDataBuffer(pollItemValues);
            }
            catch (Exception) {
                throw;
            }
            try {
                if (updatedPollItems != null)
                    _externalDataStore.SaveValues(updatedPollItems);
            }
            catch (Exception) {
                throw;
            }
            try {
                if (updatedPollItems != null)
                    _logDataStore.SaveValues(updatedPollItems);
            }
            catch (Exception) {
                throw;
            }
        }

        /// <summary>
        /// Register PollItemNames into internal hashtable
        /// </summary>
        /// <param name="itemNames"></param>
        internal void RegisterPollItems(string[] itemNames) {
            CurrentDataBuffer.RegisterPollItems(itemNames);
        }

        internal void RegisterPollItems(IList<CommonDataContract.PollItem> pollItems) {
            CurrentDataBuffer.RegisterPollItems(pollItems);
        }

        /// <summary>
        /// Return Last poll item value of poll item by it name
        /// </summary>
        /// <param name="pollItemName"></param>
        /// <returns></returns>
        public CommonDataContract.PollItemValue GetLastPollItemValue(string pollItemName) {
            return CurrentDataBuffer.GetLastPollItemValue(pollItemName);
        }

        /// <summary>
        /// Subscribe on poll item
        /// (not implemented... in project);
        /// </summary>
        /// <param name="pollItemName"></param>
        public void SubscribePollItem(string pollItemName) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates values into data buffer. 
        /// Returning a list of updated poll items
        /// </summary>
        /// <param name="pollItemValues"></param>
        /// <returns></returns>
        private List<CommonDataContract.PollItem> UpdateValuesToDataBuffer(CommonDataContract.PollItemValue[] pollItemValues) {
            string itemID;
            List<CommonDataContract.PollItem> resultList = new List<CommonDataContract.PollItem>();
            CommonDataContract.PollItem currentPollItem;
            foreach (CommonDataContract.PollItemValue currentItemValue in pollItemValues) {
                itemID = currentItemValue.ItemID;
                if (CurrentDataBuffer.Contains(itemID)) {
                    CurrentDataBuffer.RegisterPollItem(itemID);
                }
                currentPollItem = CurrentDataBuffer.GetPollItemByID(itemID);
                if (currentPollItem.IsPackage) {
                    UnpackAndSaveSubItems(currentPollItem, currentItemValue, resultList);
                }
                currentPollItem.AddValue(currentItemValue.Timestamp, currentItemValue);
                resultList.Add(currentPollItem);
            }
            CurrentDataBuffer.LastTimeStamp = DateTime.Now;
            return resultList;
        }

        /// <summary>
        /// Unpack poll item. Save value into buffer. Save item in result list 
        /// </summary>
        /// <param name="currentPollItem">poll item</param>
        /// <param name="currentItemValue">poll item value</param>
        /// <param name="resultItemList">result updated poll item list</param>
        private void UnpackAndSaveSubItems(
            CommonDataContract.PollItem currentPollItem, 
            CommonDataContract.PollItemValue currentItemValue,
            List<CommonDataContract.PollItem> resultItemList) {
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
                resultItemList.Add(subItem);
            }
        }
        
    }
}
