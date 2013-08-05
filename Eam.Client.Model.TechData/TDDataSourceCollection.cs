using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eam.Client.Model.TechData {
    public class TDDataSourceCollection: System.Collections.IEnumerable {

        private List<TDDataSource> _items;

        public TDDataSourceCollection() {
            _items = new List<TDDataSource>();
        }

        public void AddDataSource(TDDataSource item) {
            _items.Add(item);
        }

        public void RemoveDataSource(TDDataSource item) {
            _items.Remove(item);
        }

        public System.Collections.IEnumerator GetEnumerator() {
            return _items.GetEnumerator();
        }

        public TDDataSource this[int index] {
            get { return _items[index]; }
        }
    }
}
