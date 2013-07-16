using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    /// <summary>
    /// Class for storing technology data
    /// </summary>
    public class TDDataBuffer {
        private System.Collections.Hashtable _hashtablePollItems;
        private DateTime _lastTimeStamp;

        public DateTime LastTimeStamp {
            get { return _lastTimeStamp; }
        }

        public TDDataBuffer() {
            _hashtablePollItems = new System.Collections.Hashtable();
        }

        public object this[object key] {
            get {
                return _hashtablePollItems[key];
            }
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
    }
}
