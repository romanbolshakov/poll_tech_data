using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace techdata_charting_wpf {
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application {

        private void Application_Startup(object sender, StartupEventArgs e) {
            Model.IDataLoader dataLoader = 
                new Model.SQLDataLoader(techdata_charting_wpf.Properties.Settings.Default.ConnectionString);
            ProjectContext.Create(dataLoader);

            MainWindow mainView = new MainWindow();
            ViewModel.MainViewModel mainViewModel = new ViewModel.MainViewModel();
            mainView.DataContext = mainViewModel;
            MainWindow = mainView;
            mainView.Show();
        }
    }
}
