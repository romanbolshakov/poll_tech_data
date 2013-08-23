using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.ViewModel {

    /// <summary>
    /// Модкль представления для Коллекции контроллируемых параметров 
    /// (описание коллекции из БД)
    /// </summary>
    public class CustomItemCollectionViewModel: ViewModelBase {

        private Model.CustomItemCollection _customItemCollection;
        public int CollectionID {
            get { return _customItemCollection.ID; }
        }

        public CustomItemCollectionViewModel(Model.CustomItemCollection customItemCollection) {
            _customItemCollection = customItemCollection;
        }

        public override string ToString() {
            return String.Format("[{0}] {1}", _customItemCollection.ID, _customItemCollection.Name);
        }
    }
}
