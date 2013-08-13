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

namespace techdata_charting_wpf {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        List<DateTime> dates;
        List<int> y_values;
        List<int> x_values;

        ObservableDataSource<int> y_valuesDataSource;
        ObservableDataSource<int> x_valuesDataSource;
        
        public MainWindow() {
            InitializeComponent();
            FillTestData();
           
            y_valuesDataSource = new ObservableDataSource<int>(y_values);
            y_valuesDataSource.SetYMapping(y => y);

            x_valuesDataSource = new ObservableDataSource<int>(x_values);
            x_valuesDataSource.SetXMapping(x => x);

            var compositeDataSource = new CompositeDataSource(x_valuesDataSource, y_valuesDataSource);

            chartPlotter.AddLineGraph(compositeDataSource);
                        
            chartPlotter.Viewport.FlowDirection = FlowDirection.RightToLeft;
            //chartPlotter.Viewport.FitToView();

        }

        private void FillTestData() {
            dates = new List<DateTime>();
            dates.Add(new DateTime(2013, 08, 01));
            dates.Add(new DateTime(2013, 08, 02));
            dates.Add(new DateTime(2013, 08, 03));
            dates.Add(new DateTime(2013, 08, 04));
            dates.Add(new DateTime(2013, 08, 05));
            dates.Add(new DateTime(2013, 08, 06));

            y_values = new List<int>();
            y_values.AddRange(new int[6] { 0, 1, 2, 3, 4, 5 });

            x_values = new List<int>();
            x_values.AddRange(new int[6] { 0, 1, 2, 3, 4, 5 });
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            x_valuesDataSource.Collection.Add(6);
            y_valuesDataSource.Collection.Add(6);
        }
    }
}
