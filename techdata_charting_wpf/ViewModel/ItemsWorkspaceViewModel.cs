using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace techdata_charting_wpf.ViewModel {
    public class ItemsWorkspaceViewModel: ViewModelBase {

        private ObservableCollection<ItemWithValuesViewModel> _itemsValuesCollection;
        public ObservableCollection<ItemWithValuesViewModel> ItemsValuesCollection {
            get { return _itemsValuesCollection; }
        }

        private ObservableCollection<CustomItemCollectionViewModel> _customItemCollections;
        public ObservableCollection<CustomItemCollectionViewModel> CustomItemCollections {
            get { return _customItemCollections; }
        }

        private CustomItemCollectionViewModel _selectedCustomItemCollection;
        public CustomItemCollectionViewModel SelectedCustomItemCollection {
            get { 
                return _selectedCustomItemCollection; 
            }
            set { 
                _selectedCustomItemCollection = value;
                OnPropertyChanged("SelectedCustomItemCollection");
            }
        }


        private ICommand _getCurrentStateCommand;
        public ICommand GetCurrentStateCommand {
            get {
                if (_getCurrentStateCommand == null) {
                    _getCurrentStateCommand = new Command.DelegateCommand(GetCurrentState);
                }
                return _getCurrentStateCommand; }
        }

        private ICommand _refreshCustomDataCommand;
        public ICommand RefreshCustomDataCommand {
            get {
                if (_refreshCustomDataCommand == null)
                    _refreshCustomDataCommand = new Command.DelegateCommand(RefreshCustomData);
                return _refreshCustomDataCommand; 
            }
        }

        private ICommand _updateCustomCollectionCommand;
        public ICommand UpdateCustomCollectionCommand {
            get {
                if (_updateCustomCollectionCommand == null)
                    _updateCustomCollectionCommand = new Command.DelegateCommand(UpdateCustomCollection);
                return _updateCustomCollectionCommand; 
            }
        }

        private DateTime _customDate;
        public DateTime CustomDate {
            get { return _customDate; }
            set { 
                _customDate = value;
                OnPropertyChanged("CustomDate");
            }
        }

        private TimeSpan _customTime;
        public TimeSpan CustomTime {
            get { return _customTime; }
            set {
                _customTime = value;
                OnPropertyChanged("CustomTime");
            }
        }

        public ItemsWorkspaceViewModel() {
            _itemsValuesCollection = new ObservableCollection<ItemWithValuesViewModel>();
            _customItemCollections = LoadCustomItemCollections();
            if (_customItemCollections.Count > 0)
                SelectedCustomItemCollection = _customItemCollections[0];
            UpdateCustomCollection();
            CustomDate = DateTime.Now.Date;
            CustomTime = DateTime.Now.TimeOfDay;
        }

        private ObservableCollection<CustomItemCollectionViewModel> LoadCustomItemCollections() {
            ObservableCollection<CustomItemCollectionViewModel> resultCollection = 
                new ObservableCollection<CustomItemCollectionViewModel>();
            List<Model.CustomItemCollection> listCustomCollection = 
                ProjectContext.Current.DataLoader.LoadCustomItemCollections();
            CustomItemCollectionViewModel ccViewModel;
            foreach (Model.CustomItemCollection currentCollection in listCustomCollection) {
                ccViewModel = new CustomItemCollectionViewModel(currentCollection);
                resultCollection.Add(ccViewModel);
            }
            return resultCollection;
        }

        private void GetCurrentState() {
            GetCurrentStateByDate(null);
        }

        private void GetCurrentStateByDate(DateTime? customDateTime) {
            Model.ItemValue newItemValue;
            foreach (ItemWithValuesViewModel currentItemWithValues in ItemsValuesCollection) {
                newItemValue = ProjectContext.Current.DataLoader.GetLastItemValue(currentItemWithValues.ItemName, customDateTime);
                currentItemWithValues.AddNewValue(newItemValue);
            }
            OnPropertyChanged("ItemsValuesCollection");
        }

        private void RefreshCustomData() {
            DateTime customDateTime = CustomDate.Add(CustomTime);
            GetCurrentStateByDate(customDateTime);
        }

        private void UpdateCustomCollection() {
            if (SelectedCustomItemCollection != null) {
                _itemsValuesCollection.Clear();
                Model.ItemCollection itemCollection = 
                ProjectContext.Current.DataLoader.LoadItemCollection(SelectedCustomItemCollection.CollectionID);
                ItemWithValuesViewModel itemWithValuesViewModel;
                foreach (Model.Item currentItem in itemCollection) {
                    itemWithValuesViewModel = new ItemWithValuesViewModel(currentItem);
                    _itemsValuesCollection.Add(itemWithValuesViewModel);
                }
                GetCurrentState();
            }
        }
    }
}
