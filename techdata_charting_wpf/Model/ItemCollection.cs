using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    public class ItemCollection: IEnumerable<Item> {

        private List<Item> _items;

        public ItemCollection() {
            _items = new List<Item>();
        }

        internal void Add(Item item) {
            _items.Add(item);
        }

        public IEnumerator<Item> GetEnumerator() {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _items.GetEnumerator();
        }
    }
}
