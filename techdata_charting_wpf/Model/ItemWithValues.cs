using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    public class ItemWithValues{

        private Item _item;

        private int _maxValuesCount = 3;

        private ItemValueCollection _valueCollection;
        //internal ItemValueCollection ValueCollection {
        //    get { return _valueCollection; }
        //    set { _valueCollection = value; }
        //}

        public string Name {
            get { return _item.Name; }
        }
        public string Caption {
            get { return _item.Caption; }
        }
        public string Unit {
            get { return _item.Unit; }
        }


        public ItemValue GetLastValue {
            get {
                if (_valueCollection.Count == 0) return null;
                return _valueCollection[_valueCollection.Count - 1];
            }
        }

        public ItemWithValues(Item item) {
            _item = item;
            _valueCollection = new ItemValueCollection();
        }

        public ItemWithValues(): this(new Item()) {
        }

        internal void AddNewValue(ItemValue newItemValue) {
            if (_valueCollection.Count == _maxValuesCount) _valueCollection.RemoveAt(0);
            _valueCollection.Add(newItemValue);

        }
    }
}
