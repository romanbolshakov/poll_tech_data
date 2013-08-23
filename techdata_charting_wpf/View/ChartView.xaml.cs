using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Common;
using Microsoft.Research.DynamicDataDisplay.Filters;
using Microsoft.Research.DynamicDataDisplay.Navigation;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Microsoft.Research.DynamicDataDisplay.Properties;
using Microsoft.Research.DynamicDataDisplay.ViewportRestrictions;


namespace techdata_charting_wpf.View {
    /// <summary>
    /// Логика взаимодействия для ChartView.xaml
    /// </summary>
    public partial class ChartView : UserControl {
        public ChartView() {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            ViewModel.ChartWorkspaceViewModel viewModel = this.DataContext as ViewModel.ChartWorkspaceViewModel;
            viewModel.ItemValuesDataSource.SetXMapping(itemValueViewModel => { 
                return dateAxis.ConvertToDouble(itemValueViewModel.Timestamp); });
            viewModel.ItemValuesDataSource.SetYMapping(itemValueViewModel => {
                if (itemValueViewModel.Value.ToString().ToLower() == "true")
                    return 1;
                if (itemValueViewModel.Value.ToString().ToLower() == "false")
                    return 0;
                else
                    return Convert.ToDouble(itemValueViewModel.Value);
            });
        }

        

        

        
    }

}
