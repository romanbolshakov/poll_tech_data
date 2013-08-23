using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.ViewModel {
    public class ItemWithValuesViewModel: ViewModelBase {

        private Model.ItemWithValues _itemWithValues;

        public string ItemName {
            get { return _itemWithValues.Name; }
        }

        public string AdvancedItemCaption {
            get { return String.Format("{0} [{1}]", _itemWithValues.Caption, _itemWithValues.Name); }
        }

        public string ItemUnit {
            get { return _itemWithValues.Unit; }
        }

        public DateTime LastValueTimestamp {
            get {
                Model.ItemValue lastItemValue = _itemWithValues.GetLastValue;
                if (lastItemValue == null) return DateTime.Now;
                return _itemWithValues.GetLastValue.Timestamp; }
        }

        public object LastValue {
            get {
                Model.ItemValue lastItemValue = _itemWithValues.GetLastValue;
                if (lastItemValue == null) return null;
                return _itemWithValues.GetLastValue.Value; }
        }

        public ItemWithValuesViewModel() {
            _itemWithValues = new Model.ItemWithValues();
        }

        public ItemWithValuesViewModel(Model.Item item) {
            _itemWithValues = new Model.ItemWithValues(item);
        }

        internal void AddNewValue(Model.ItemValue newItemValue) {
            _itemWithValues.AddNewValue(newItemValue);
            OnPropertyChanged("LastValueTimestamp");
            OnPropertyChanged("LastValue");
        }
    }
}
