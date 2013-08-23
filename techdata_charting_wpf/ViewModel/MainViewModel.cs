using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace techdata_charting_wpf.ViewModel {
    public class MainViewModel: ViewModelBase {
        
        private ChartWorkspaceViewModel _chartWorkspaceViewModel;
        public ChartWorkspaceViewModel TechdataChartWorkspace {
            get {
                if (_chartWorkspaceViewModel == null) {
                    _chartWorkspaceViewModel = new ChartWorkspaceViewModel();
                }
                return _chartWorkspaceViewModel; 
            }
        }

        private ItemsWorkspaceViewModel _itemsWorkspaceViewModel;
        public ItemsWorkspaceViewModel TechdataItemsWorkspace {
            get {
                if (_itemsWorkspaceViewModel == null)
                    _itemsWorkspaceViewModel = new ItemsWorkspaceViewModel();
                return _itemsWorkspaceViewModel; 
            }
        }


    }
}
