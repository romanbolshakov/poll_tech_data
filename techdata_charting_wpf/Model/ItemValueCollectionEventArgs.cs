using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.Model {
    internal class ItemValueCollectionEventArgs: EventArgs {
        private ItemValueCollection _collection;
        internal ItemValueCollection Collection {
            get { return _collection; }
            private set { _collection = value; }
        }

        public ItemValueCollectionEventArgs(ItemValueCollection collection) {
            Collection = collection;
        }
    }
}
