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
        ObservableDataSource<int> o;

        public ChartView() {
            InitializeComponent();

            o = new ObservableDataSource<int>(new int[6] { 0, 1, 2, 3, 4, 5 });
            o.SetXYMapping(x => { return new Point(x, Math.Sin(x)); });

            this.chartPlotter.AddLineGraph(o);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            o.AppendMany(new int[4] { 6, 7, 8, 9 });
        }
    }

    
}
