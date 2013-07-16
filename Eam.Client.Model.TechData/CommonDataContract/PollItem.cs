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
        protected void OnValueUpdated(PollItemValue newPollItemValue) {
            if (ValueUpdatedEvent != null) {
                ValueUpdatedEvent(this, new PollItemValueUpdatedEventArgs(newPollItemValue));
            }
        }


        private System.Collections.Hashtable _hashtableItemValues;
        private DateTime _lastTimestamp;
        private DateTime _firstTimestamp;
        private int _maxValueCount = 10;

        public PollItem() {
            _hashtableItemValues = new System.Collections.Hashtable();
        }

        public void AddValue(DateTime timestamp, PollItemValue pollItemValue) {
            if (_hashtableItemValues.Count >= _maxValueCount)
                DeleteFirstKey();
            _hashtableItemValues.Add(timestamp, pollItemValue);
            _lastTimestamp = timestamp;
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
