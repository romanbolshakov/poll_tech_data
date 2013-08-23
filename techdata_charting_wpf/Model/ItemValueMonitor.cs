using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    internal class ItemValueMonitor {

        private System.Windows.Threading.DispatcherTimer _updateValuesTimer;
        private System.Windows.Threading.DispatcherTimer _resetTimer;

        private string _itemName;
        private int _timerInterval;
        private DateTime _lastTimestamp;

        public event EventHandler<ItemValueCollectionEventArgs> NewItemValuesEvent;
        private void OnNewItemValues(Model.ItemValueCollection itemValueCollection) {
            if (NewItemValuesEvent != null) {
                NewItemValuesEvent(this, new ItemValueCollectionEventArgs(itemValueCollection));
            }
        }

        public event EventHandler ResetMonitorEvent;
        private void OnResetMonitor() {
            if (ResetMonitorEvent != null) {
                ResetMonitorEvent(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="itemName">item name</param>
        /// <param name="timerInterval">timer interval in ms</param>
        public ItemValueMonitor(string itemName, int timerInterval) {
            _itemName = itemName;
            _timerInterval = timerInterval;
            _lastTimestamp = DefineInitTimestamp();
            InitResetMonitorTimer();
            InitUpdateValueTimer();
        }

        private void InitResetMonitorTimer() {
            _resetTimer = new System.Windows.Threading.DispatcherTimer();
            _resetTimer.Tick += new EventHandler(_resetTimer_Tick);
            /// One day
            _resetTimer.Interval = new TimeSpan(1, 0, 0, 0);
            // 30 seconds
            //_resetTimer.Interval = new TimeSpan(0, 0, 0, 10);
        }

        private void InitUpdateValueTimer() {
            _updateValuesTimer = new System.Windows.Threading.DispatcherTimer();
            _updateValuesTimer.Interval = new TimeSpan(0, 0, 0, 0, _timerInterval);
            _updateValuesTimer.Tick += new EventHandler(_updateValueTimer_Tick);
        }

        internal void StartMonitor() {
            _updateValuesTimer.Start();
            _resetTimer.Start();
        }

        internal void StopMonitor() {
            _updateValuesTimer.Stop();
            _resetTimer.Stop();
        }

        private void _updateValueTimer_Tick(object sender, EventArgs e) {
            Model.ItemValueCollection itemValueCollection;
            //itemValueCollection = CreateTestCollection();
            itemValueCollection = ProjectContext.Current.DataLoader.LoadItemValueCollection(_itemName, _lastTimestamp);
            _lastTimestamp = UpdateLastTimestamp(itemValueCollection);
            OnNewItemValues(itemValueCollection);
        }

        void _resetTimer_Tick(object sender, EventArgs e) {
            StopMonitor();
            OnResetMonitor();
            StartMonitor();
        }

        private ItemValueCollection CreateTestCollection() {
            ItemValueCollection testCollection = new ItemValueCollection();
            ItemValue testItemValue = new ItemValue();
            testItemValue.ItemID = "SimulatedData.Random";
            testItemValue.Timestamp = DateTime.Now;
            Random rnd = new Random();
            testItemValue.Value = rnd.Next(900000);
            testCollection.Add(testItemValue);
            return testCollection;
        }

        private DateTime DefineInitTimestamp() {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        private DateTime UpdateLastTimestamp(ItemValueCollection itemValueCollection) {
            int lastIndex = itemValueCollection.Count - 1;
            ItemValue lastItemValue = itemValueCollection[lastIndex];
            return lastItemValue.Timestamp;
        }
    }
}
