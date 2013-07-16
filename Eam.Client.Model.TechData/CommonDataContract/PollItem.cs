using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData.CommonDataContract {
    public class PollItem {

        public class PollItemValueUpdatedEventArgs: EventArgs{
            private PollItemValue _pollItemValue;

            public PollItemValue NewPollItemValue{
                get { return _pollItemValue; }
            }

            public PollItemValueUpdatedEventArgs(PollItemValue pollItemValue){
                _pollItemValue = pollItemValue;
            }
        }
        public event EventHandler<PollItemValueUpdatedEventArgs> ValueUpdatedEvent;
        protected void OnValueUpdated(object newPollItemValue) {
            if (newPollItemValue is PollItemValue) {
                if (ValueUpdatedEvent != null) {
                    ValueUpdatedEvent(this, new PollItemValueUpdatedEventArgs(newPollItemValue as PollItemValue));
                }
            }
        }

        protected void OnValueUpdatedInThread(PollItemValue newPollItemValue) {
            System.Threading.Thread updateValueThread = new System.Threading.Thread(OnValueUpdated);
            updateValueThread.IsBackground = true;
            updateValueThread.Start(newPollItemValue);
        }


        private System.Collections.Hashtable _hashtableItemValues;
        private DateTime _lastTimestamp;
        private DateTime _firstTimestamp;

        private int _maxValueCount;
        /// <summary>
        /// Max values in the hashtable (10 by default)
        /// </summary>
        protected int SetMaxValueCount {
            set { _maxValueCount = value; }
        }

        private string _itemName;
        /// <summary>
        /// Item name (equal ItemID)
        /// </summary>
        public string ItemName {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public PollItem() {
            _hashtableItemValues = new System.Collections.Hashtable();
            _maxValueCount = 10;
        }

        public PollItem(string itemName)
            : this() {
            this._itemName = itemName;
        }

        public void AddValue(DateTime timestamp, PollItemValue pollItemValue) {
            if (_hashtableItemValues.Count >= _maxValueCount)
                DeleteFirstKey();
            _hashtableItemValues.Add(timestamp, pollItemValue);
            _lastTimestamp = timestamp;
            OnValueUpdatedInThread(pollItemValue);
        }

        public PollItemValue GetLastValue() {
            return _hashtableItemValues[_lastTimestamp] as CommonDataContract.PollItemValue;
        }

        private void DeleteFirstKey() {
            _hashtableItemValues.Remove(_firstTimestamp);
            System.Collections.IDictionaryEnumerator ide = _hashtableItemValues.GetEnumerator();
            ide.Reset();
            ide.MoveNext();
            DateTime? tempDateTime = ide.Entry.Key as DateTime?;
            if (tempDateTime != null)
                _firstTimestamp = tempDateTime.Value;
        }

    }
}
