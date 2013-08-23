using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    internal class ItemValueCollection: IEnumerable<ItemValue> {

        private List<ItemValue> _items;

        internal ItemValueCollection() {
            _items = new List<ItemValue>();
        }

        internal void Add(ItemValue item) {
            _items.Add(item);
        }

        public int Count {
            get { return _items.Count; }
        }

        public ItemValue this[int index] {
            get {
                if (_items.Count <= index) return null;
                return _items[index]; 
            }
        }

        public IEnumerator<ItemValue> GetEnumerator() {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _items.GetEnumerator();
        }

        internal void RemoveAt(int index) {
            _items.RemoveAt(index);
        }
    }
}
