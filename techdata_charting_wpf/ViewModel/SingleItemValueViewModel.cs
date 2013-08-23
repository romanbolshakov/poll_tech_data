using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.ViewModel {
    public class SingleItemValueViewModel {

        private Model.ItemValue _itemValue;


        public string ItemID {
            get { return _itemValue.ItemID; }
        }
        public DateTime Timestamp {
            get { return _itemValue.Timestamp; }
        }
        public object Value {
            get { return _itemValue.Value; }
        }

        public SingleItemValueViewModel(Model.ItemValue itemValue) {
            _itemValue = itemValue;
        }
    }
}
