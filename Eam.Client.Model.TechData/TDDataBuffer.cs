using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// Class for storing technology data
    /// </summary>
    internal class TDDataBuffer {
        /// <summary>
        /// hash-table for pair itemID(string)-PollItem(instance)
        /// </summary>
        private System.Collections.Hashtable _hashtablePollItems;
        private DateTime _lastTimeStamp;
        /// <summary>
        /// last time stamp for checking the updates
        /// </summary>
        public DateTime LastTimeStamp {
            get { return _lastTimeStamp; }
        }

        internal TDDataBuffer() {
            _hashtablePollItems = new System.Collections.Hashtable();
        }

        internal object this[object key] {
            get {
                return _hashtablePollItems[key];
            }
        }

        internal ICollection<CommonDataContract.PollItem> GetAllItems() {
            return null;
            //return _hashtablePollItems.Values;
        }

        internal void UpdateValues(CommonDataContract.PollItemValue[] pollItemValues) {
            string itemID;
            CommonDataContract.PollItem currentPollItem;
            foreach (CommonDataContract.PollItemValue currentItemValue in pollItemValues) {
                itemID = currentItemValue.ItemID;
                if (!_hashtablePollItems.Contains(itemID)) {
                    _hashtablePollItems.Add(itemID, new CommonDataContract.PollItem());
                }
                currentPollItem = _hashtablePollItems[itemID] as CommonDataContract.PollItem;
                currentPollItem.AddValue(currentItemValue.Timestamp, currentItemValue);
            }
            _lastTimeStamp = DateTime.Now;
        }

        internal void RegisterPollItems(string[] itemNames) {
            CommonDataContract.PollItem newPollItem;
            foreach (var item in itemNames) {
                newPollItem = new CommonDataContract.PollItem(item);
                AddPollItemToHashtable(newPollItem);
            }
        }

        internal void RegisterPollItems(IList<CommonDataContract.PollItem> pollItems) {
            foreach (var item in pollItems) {
                AddPollItemToHashtable(item);
            }
        }

        private void AddPollItemToHashtable(CommonDataContract.PollItem pollItem) {
            if (!_hashtablePollItems.Contains(pollItem.ItemName)) {
                _hashtablePollItems.Add(pollItem.ItemName, pollItem);
            }
        }

        public CommonDataContract.PollItemValue GetLastPollItemValue(string pollItemName) {
            if (_hashtablePollItems.Contains(pollItemName)) {
                CommonDataContract.PollItem pollItem = _hashtablePollItems[pollItemName] as CommonDataContract.PollItem;
                return pollItem.GetLastValue();
            }
            else return null;
        }
    }
}
