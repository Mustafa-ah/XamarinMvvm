using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Extensions;
using Ayadi.Core.Contracts.Services;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;

namespace Ayadi.Core.ViewModel
{
    public class SearchViewModel : BaseViewModel , ISearchViewModel
    {
        private readonly ISearchDataService _searchDataService;
        private readonly ILoadingDataService _loadingDataService;
        private readonly IConnectionService _connectionService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;
        // ObservableCollection<Category> _categories; 
        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; RaisePropertyChanged(() => Categories); }
        }

        private User _AppUser;

        private Category _selectedCategory;

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged(() => SelectedCategory); }
        }


        private ObservableCollection<Store> _stores;

        public ObservableCollection<Store> Stores
        {
            get { return _stores; }
            set { _stores = value; RaisePropertyChanged(() => Stores); }
        }

        private Store _selectedStore;

        public Store SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; RaisePropertyChanged(() => SelectedStore); }
        }

        private string _keyWord;

        public string KeyWord
        {
            get { return _keyWord; }
            set { _keyWord = value; RaisePropertyChanged(() => KeyWord); }
        }

        private string _minPrice;

        public string MinPrice
        {
            get { return _minPrice; }
            set { _minPrice = value; RaisePropertyChanged(() => _minPrice); }
        }

        private string _maxPrice;

        public string MaxPrice
        {
            get { return _maxPrice; }
            set { _maxPrice = value; RaisePropertyChanged(() => MaxPrice); }
        }


        public SearchViewModel(IMvxMessenger messenger, ISearchDataService searchDataService,
             ILoadingDataService loadingDataService,
             IUserDataService userDataService,
             IConnectionService connectionService,
             IDialogService dialogService):base(messenger)
        {
            _searchDataService = searchDataService;
            _loadingDataService = loadingDataService;
            _connectionService = connectionService;
            _userDataService = userDataService;
            _dialogService = dialogService;
        }


        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            if (_connectionService.CheckOnline())
            {
                await _loadingDataService.ShowFragmentLoading();
                _AppUser = await _userDataService.GetSavedUser();
                Categories = (await _searchDataService.GetAllCategories(_AppUser)).ToObservableCollection();
                Stores = (await _searchDataService.GetAllStors(_AppUser)).ToObservableCollection();
                //SelectedStore = Stores.FirstOrDefault();
                //SelectedCategory = Categories.FirstOrDefault();
                StoreSelected();
                CatSelected();
                SetSelectedItem();
                _loadingDataService.HideFragmentLoading();
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                 TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                //maybe we can navigate to a start page here, for you to add to this code base!
            }

        }


        private void SetSelectedItem()
        {
            try
            {
                string Catname = TextSource.GetText("Cat");
                Category cat = new Category();
                cat.Name = Catname;
                cat.IsSelected = true;
                //cat.Id = "null";
                Categories.Insert(0, cat);
                SelectedCategory = cat;

                //stor
                string storename = TextSource.GetText("Store");
                Store sto = new Store();
                sto.Name = storename;
                sto.IsSelected = true;
                //sto.Id = "null";
                Stores.Insert(0, sto);

                SelectedStore = sto;
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        // commands
        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        public MvxCommand PrepareForSearchCommand
        {
            get
            {
                return new MvxCommand(() => PrepareForSearch());
            }
        }

        public MvxCommand<Category> CatSelectedCommand
        {
            get
            {
                return new MvxCommand<Category>((se) => {
                    CatSelected();
                    SelectedCategory = se;
                    se.IsSelected = true;
                });
            }
        }

        public MvxCommand<Store> StoeSelectedCommand
        {
            get
            {
                return new MvxCommand<Store>((se) => {
                    StoreSelected();
                    SelectedStore = se;
                    se.IsSelected = true;
                });
            }
        }
        private void StoreSelected()
        {
            try
            {
                foreach (Store item in Stores)
                {
                    item.IsSelected = false;
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private void CatSelected()
        {
            try
            {
                foreach (Category item in Categories)
                {
                    item.IsSelected = false;
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private void PrepareForSearch()
        {
            try
            {
                // , string , string , string , string )
                //Messages.SearchMessage msg_ = new Messages.SearchMessage(this);
                //msg_.KeyWord = KeyWord;
                //msg_.MaxPrice = MaxPrice;
                //msg_.MinPrice = MinPrice;
                //msg_.CategoryId = SelectedCategory.Id;
                //msg_.StoreId = SelectedStore.Id;
                //Messenger.Publish(msg_);
                ShowViewModel<ProductsViewModel>(new {
                    CatId = -2 ,
                    title = TextSource.GetText("Search"),
                    SearchKeyWord = KeyWord,
                    SearchCatId = SelectedCategory.Id,
                    SearchStorId = SelectedStore.Id,
                    SearchMin = MinPrice,
                    SearchMax = MaxPrice
                });
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
        //private void initaiateVars()
        //{
        //    KeyWord = "";

        //}
    }
}
