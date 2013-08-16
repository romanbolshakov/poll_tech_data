using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.ViewModel {
    public class MainViewModel: ViewModelBase {
        
        private ChartViewModel _chartViewModel;
        public ChartViewModel TechdataChart {
            get {
                if (_chartViewModel == null) {
                    _chartViewModel = new ChartViewModel();
                }
                return _chartViewModel; 
            }
        }

        private ItemsViewModel _itemsViewModel;
        public ItemsViewModel TechdataItems {
            get {
                if (_itemsViewModel == null)
                    _itemsViewModel = new ItemsViewModel();
                return _itemsViewModel; 
            }
        }


    }
}
