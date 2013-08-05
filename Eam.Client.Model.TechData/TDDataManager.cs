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
            CurrentDataBuffer.UpdateValues(pollItemValues);
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
    }
}
