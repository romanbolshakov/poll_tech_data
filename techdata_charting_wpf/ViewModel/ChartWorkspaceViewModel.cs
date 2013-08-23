using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes;

namespace techdata_charting_wpf.ViewModel {
    public class ChartWorkspaceViewModel: ViewModelBase {

        private enum MonitorState {
            active,
            passive
        }

        private MonitorState _monitorState;
        private MonitorState CurrentMonitorState {
            get { return _monitorState; }
            set {
                if (_monitorState != value) {
                    _monitorState = value;
                    OnPropertyChanged("CurrentMonitorState");
                }
            }
        }

        private Model.ItemValueMonitor _itemValueMonitor;

        private ObservableDataSource<SingleItemValueViewModel> _itemValuesDataSource;
        public ObservableDataSource<SingleItemValueViewModel> ItemValuesDataSource {
            get { return _itemValuesDataSource; }
        }

        private ICommand _refreshDataCommand;
        public ICommand RefreshDataCommand {
            get {
                if (_refreshDataCommand == null) {
                    _refreshDataCommand = new Command.DelegateCommand(RefreshData, CanExecuteRefreshDataCommand);
                    
                }
                return _refreshDataCommand; }
        }

        private ICommand _startValueMonitorCommand;
        public ICommand StartValueMonitorCommand {
            get {
                if (_startValueMonitorCommand == null)
                    _startValueMonitorCommand = new Command.DelegateCommand(StartValueMonitor, CanExecuteStartMonitor);
                return _startValueMonitorCommand;
            }
        }

        private ICommand _stopValueMonitorCommand;
        public ICommand StopValueMonitorCommand {
            get {
                if (_stopValueMonitorCommand == null)
                    _stopValueMonitorCommand = new Command.DelegateCommand(StopValueMonitor, CanExecuteStopMonitor);
                return _stopValueMonitorCommand;
            }
        }

        private ICommand _switchToNextDayCommand;
        public ICommand SwitchToNextDayCommand {
            get { 
                if (_switchToNextDayCommand == null)
                    _switchToNextDayCommand = new Command.DelegateCommand(SwitchToNextDay, CanExecuteRefreshDataCommand);
                return _switchToNextDayCommand; 
            }
        }

        private ICommand _switchToNextWeekCommand;
        public ICommand SwitchToNextWeekCommand {
            get {
                if (_switchToNextWeekCommand == null)
                    _switchToNextWeekCommand = new Command.DelegateCommand(SwitchToNextWeek, CanExecuteRefreshDataCommand);
                return _switchToNextWeekCommand;
            }
        }

        private ICommand _switchToPrevDayCommand;
        public ICommand SwitchToPrevDayCommand {
            get {
                if (_switchToPrevDayCommand == null)
                    _switchToPrevDayCommand = new Command.DelegateCommand(SwitchToPrevDay, CanExecuteRefreshDataCommand);
                return _switchToPrevDayCommand;
            }
        }

        private ICommand _switchToPrevWeekCommand;
        public ICommand SwitchToPrevWeekCommand {
            get {
                if (_switchToPrevWeekCommand == null)
                    _switchToPrevWeekCommand = new Command.DelegateCommand(SwitchToPrevWeek, CanExecuteRefreshDataCommand);
                return _switchToPrevWeekCommand;
            }
        }

        private ICommand _switchToTodayCommand;
        public ICommand SwitchToTodayCommand {
            get {
                if (_switchToTodayCommand == null)
                    _switchToTodayCommand = new Command.DelegateCommand(SwitchToToday, CanExecuteRefreshDataCommand);
                return _switchToTodayCommand;
            }
        }

        private DateTime _dateFrom;
        public DateTime DateFrom {
            get { return _dateFrom; }
            set { 
                _dateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }

        private DateTime _dateTo;
        public DateTime DateTo {
            get { return _dateTo; }
            set {
                _dateTo = new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
                OnPropertyChanged("DateTo");
            }
        }

        private ObservableCollection<SingleItemViewModel> _itemCollection;
        public ObservableCollection<SingleItemViewModel> ItemCollection {
            get {
                if (_itemCollection == null) {
                    _itemCollection = CreateObservableItemCollection();
                }
                return _itemCollection; }
        }

        private SingleItemViewModel _selectedSingleItemViewModel;
        public SingleItemViewModel SelectedSingleItemViewModel {
            get { return _selectedSingleItemViewModel; }
            set { 
                _selectedSingleItemViewModel = value;
                OnPropertyChanged("SelectedSingleItemViewModel");
            }
        }

        private int _monitorRequestDelay;
        public int MonitorRequestDelay {
            get { return _monitorRequestDelay; }
            set { 
                _monitorRequestDelay = value;
                OnPropertyChanged("MonitorRequestDelay");
            }
        }

        public string HelpText {
            get { return String.Format("{0}\n{1}", "Монитор обновляется раз в сутки", "Для запуска монитора необходимо выбрать контроллируемый параметр"); }
        }

        public ChartWorkspaceViewModel() {
            _itemValuesDataSource = new ObservableDataSource<SingleItemValueViewModel>();
            DateTime now = DateTime.Now;
            DateFrom = new DateTime(now.Year, now.Month, now.Day, 0,0,0);
            DateTo = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            MonitorRequestDelay = 1000;
            _monitorState = MonitorState.passive;
        }


        private void RefreshData() {
            _itemValuesDataSource.Collection.Clear();
            Model.ItemValueCollection itemValueCollection = 
                ProjectContext.Current.DataLoader.LoadItemValueCollection(
                    SelectedSingleItemViewModel.ItemName, DateFrom, DateTo);
            UpdateItemValuesDataSource(itemValueCollection);
        }
        private bool CanExecuteRefreshDataCommand() {
            return (_selectedSingleItemViewModel != null 
                && CurrentMonitorState == MonitorState.passive);
        }

        private void StartValueMonitor() {
            _itemValuesDataSource.Collection.Clear();
            _itemValueMonitor = new Model.ItemValueMonitor(_selectedSingleItemViewModel.ItemName, _monitorRequestDelay);
            _itemValueMonitor.NewItemValuesEvent += new EventHandler<Model.ItemValueCollectionEventArgs>(_itemValueMonitor_NewItemValuesEvent);
            _itemValueMonitor.ResetMonitorEvent += new EventHandler(_itemValueMonitor_ResetMonitorEvent);
            _itemValueMonitor.StartMonitor();
            CurrentMonitorState = MonitorState.active;
        }
        private bool CanExecuteStartMonitor() {
            return (_selectedSingleItemViewModel != null 
                && MonitorRequestDelay > 0
                && CurrentMonitorState == MonitorState.passive);
        }

        private void StopValueMonitor() {
            if (_itemValueMonitor != null) {
                _itemValueMonitor.StopMonitor();
            }
            CurrentMonitorState = MonitorState.passive;
        }
        private bool CanExecuteStopMonitor() {
            return CurrentMonitorState == MonitorState.active;
        }
        
        private void _itemValueMonitor_NewItemValuesEvent(object sender, Model.ItemValueCollectionEventArgs e) {
            UpdateItemValuesDataSource(e.Collection);
        }

        private void _itemValueMonitor_ResetMonitorEvent(object sender, EventArgs e) {
            _itemValuesDataSource.Collection.Clear();
        }

        private ObservableCollection<SingleItemViewModel> CreateObservableItemCollection() {
            ObservableCollection<SingleItemViewModel> resultCollection = new ObservableCollection<SingleItemViewModel>();
            Model.ItemCollection modelItemCollection = ProjectContext.Current.DataLoader.LoadAllItemCollection();
            SingleItemViewModel itemViewModel;
            foreach (Model.Item modelItem in modelItemCollection) {
                itemViewModel = new SingleItemViewModel(modelItem);
                resultCollection.Add(itemViewModel);
            }
            return resultCollection;
        }

        private void UpdateItemValuesDataSource(Model.ItemValueCollection itemValueCollection) {
            SingleItemValueViewModel itemValueViewModel;
            List<SingleItemValueViewModel> newList = new List<SingleItemValueViewModel>();
            foreach (Model.ItemValue currentItemValue in itemValueCollection) {
                itemValueViewModel = new SingleItemValueViewModel(currentItemValue);
                newList.Add(itemValueViewModel);
            }
            _itemValuesDataSource.AppendMany(newList);
        }

        private void SwitchToNextDay() {
            DateFrom = DateFrom.AddDays(1);
            DateTo = DateTo.AddDays(1);
            RefreshData();
        }

        private void SwitchToNextWeek() {
            DateFrom = DateFrom.AddDays(7);
            DateTo = DateTo.AddDays(7);
            RefreshData();

        }

        private void SwitchToPrevDay() {
            DateFrom = DateFrom.AddDays(-1);
            DateTo = DateTo.AddDays(-1);
            RefreshData();

        }

        private void SwitchToPrevWeek() {
            DateFrom = DateFrom.AddDays(-7);
            DateTo = DateTo.AddDays(-7);
            RefreshData();

        }

        private void SwitchToToday() {
            DateTime now = DateTime.Now;
            DateFrom = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            DateTo = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            RefreshData();
        }
    }
}
