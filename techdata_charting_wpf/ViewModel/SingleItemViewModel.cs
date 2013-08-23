using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.ViewModel {
    public class SingleItemViewModel {
        
        private Model.Item _item;

        public SingleItemViewModel(Model.Item item){
            _item = item;
        }

        public string ItemName {
            get { return _item.Name; }
        }

        public override string ToString() {
            return String.Format("{0} [{1}]", _item.Caption, _item.Name);
        }
    }
}
