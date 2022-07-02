using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Extensions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Messages;
using MvvmCross.Platform;
using Ayadi.Core.Repositories;

namespace Ayadi.Core.ViewModel
{
    public class ProductsViewModel : BaseViewModel , IProductsViewModel
    {
        IProductsDataService _productsDataService;
        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private int _CatId;

        private User _AppUser;

        public event EventHandler ProductsSorted;
        public event EventHandler ProductsViewChanged;
        public event EventHandler CategoryChosed;

        private readonly MvxSubscriptionToken _token;
        private readonly MvxSubscriptionToken _Viewtoken;
        private readonly MvxSubscriptionToken _Filtertoken;
        private readonly MvxSubscriptionToken _FavouriteToken;

        private ObservableCollection<Product> _products;
        //   private ObservableCollection<Product> _tempProducts;

        public Dictionary<int, ObservableCollection<Product>> ProductsDictionary;
        public bool IsPageAvailable { get; set; }
        public bool CanLoadMore { get; set; }
        public event EventHandler PagePrepared;

        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title); }
        }


        #region Search Obj
        private SearchMessage _SearchVars;
        #endregion

        #region Left Drawer
        private readonly Lazy<SortingViewModel> _sortingViewModel;
        public SortingViewModel SortingViewModel => _sortingViewModel.Value;
        #endregion

        public ProductsViewModel(IMvxMessenger messenger,
            IProductsDataService productsDataService,
              IConnectionService connectionService,
              IUserDataService userDataService,
        IDialogService dialogService) :base(messenger)
        {
            _productsDataService = productsDataService;
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;

            _token = messenger.Subscribe<SortMessage>(OnSortChosed);
            _Viewtoken = messenger.Subscribe<ProductsViewMessage>(OnViewChanged);
            _Filtertoken = messenger.Subscribe<FilterMesaage>(OnCatFillterd);
            _FavouriteToken = messenger.Subscribe<FavouriteProductMessage>(OnFavouriteProduct);

            _sortingViewModel = new Lazy<SortingViewModel>(Mvx.IocConstruct<SortingViewModel>);
        }

        private void OnFavouriteProduct(FavouriteProductMessage obj)
        {
            try
            {
                Product product_ = Products.FirstOrDefault(p => p.Id == obj._product.Id);
                if (product_ != null)
                {
                    product_.ISInFavourite = obj._product.ISInFavourite;
                    product_.ShoppingCartId = obj._product.ShoppingCartId;
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private void OnCatFillterd(FilterMesaage obj)
        {
            try
            {
                FilterByCatId(obj.Cat.Id);
                 CategoryChosed?.Invoke(this, new EventArgs());
                Title = obj.Cat.Name;
            }
            catch (Exception)
            {

                //throw;//x
            }
            
        }
        private async void FilterByCatId(string cattId)
        {
            try
            {
                if (_connectionService.CheckOnline())
                {
                    //   _loadingDataService.ShowLFragmentLoading();
                    IsBusy = true;
                    _CatId = int.Parse(cattId);
                      Products = (await _productsDataService.GetCategoryProducts(_CatId, _AppUser, 1)).ToObservableCollection();
                    IsBusy = false;
                    //_loadingDataService.HideFragmentLoading();
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
               TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    // maybe we can navigate to a start page here, for you to add to this code base!
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
        /*
        private async void FilterByCatId(string cattId)
        {
            try
            {
                if (_connectionService.CheckOnline())
                {
                    await _loadingDataService.ShowFragmentLoading();

                    if (_tempProducts == null)
                    {
                        _tempProducts = Products;
                    }
                    else
                    {
                        Products = _tempProducts;
                    }

                    List<Product_category_mappings> Ids_ = await _productsDataService.
                        GetProductsIdsFromCategory(cattId);

                    ObservableCollection<Product> newList = new ObservableCollection<Product>();
                    foreach (Product item in Products)
                    {
                        if (Ids_.Exists(id => id.Id.ToString() == item.Id))
                        {
                            newList.Add(item);
                        }
                    }

                    Products = newList;
                    _loadingDataService.HideFragmentLoading();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("No internet available", "Ayadi says...", "OK");
                    // maybe we can navigate to a start page here, for you to add to this code base!
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
        */
        private void OnViewChanged(ProductsViewMessage obj)
        {
            ProductsViewChanged?.Invoke(obj.ViewType, new EventArgs());
        }

        private void OnSortChosed(SortMessage obj)
        {
            Products = Sorting(obj.SortBy);
            ProductsSorted?.Invoke(this, new EventArgs());
        }

        private ObservableCollection<Product> Sorting(string sortBy)
        {
            ObservableCollection<Product> SortedList;
            try
            {
                switch (sortBy)
                {
                    case "ByNameAtoZ":
                        SortedList = Products.OrderBy(p => p.Name).ToObservableCollection();
                        break;

                    case "ByNameZtoA":
                        SortedList = Products.OrderByDescending(p => p.Name).ToObservableCollection();
                        break;

                    case "ByPriceMaxToMin":
                        SortedList = Products.OrderByDescending(p => p.Price).ToObservableCollection();
                        break;

                    case "ByPriceMinToMax":
                        SortedList = Products.OrderBy(p => p.Price).ToObservableCollection();
                        break;

                    case "ByDefaultPlace":
                        SortedList = Products.OrderBy(p => p.ISInFavourite).ToObservableCollection();
                        break;

                    case "ByLastUpdate":
                        SortedList = Products.OrderByDescending(p => p.Updated_on_utc).ToObservableCollection();
                        break;

                    default:
                        SortedList = Products;
                        break;
                }
            }
            catch (Exception)
            {
                SortedList = Products;
                //throw;//x
            }
            return SortedList;
        }

        //private void OnSearchPrepared(SearchMessage obj)
        //{
        //    _SearchVars = obj;
        //}

        public void Init(int CatId , string title, string SearchKeyWord, string SearchCatId, string SearchStorId, string SearchMin, string SearchMax)
        {
            _CatId = CatId;
            Title = title;
            _SearchVars = new SearchMessage(this)
            {
                KeyWord = SearchKeyWord,
                CategoryId = SearchCatId,
                StoreId = SearchStorId,
                MaxPrice = SearchMax,
                MinPrice = SearchMin
            };
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
                IsBusy = true;

                _AppUser = await _userDataService.GetSavedUser();
                BaseAppUser = _AppUser;

                ProductsDictionary = new Dictionary<int, ObservableCollection<Product>>();

                if (_CatId == -1)
                {
                    // from home page
                   // Products = (await _productsDataService.GetAllProducts(_AppUser,2)).ToObservableCollection();
                    Products = (await _productsDataService.GetHomeProducts(_AppUser)).ToObservableCollection();
                }
                else if (_CatId == -2)
                {
                    // from search page
                    Products = (await _productsDataService.SearchProducts(_SearchVars.KeyWord,_SearchVars.CategoryId,
                        _SearchVars.StoreId, _SearchVars.MinPrice, _SearchVars.MaxPrice, _AppUser, 1))
                        .ToObservableCollection();
                }
                else
                {
                    // from categor page
                    Products = (await _productsDataService.GetCategoryProducts(_CatId, _AppUser, 1)).ToObservableCollection();
                }
                MainProducts = Products;

                ProductsDictionary.Add(1, Products);

                CanLoadMore = true;
                 // SetFavouriteProducts();
                 IsBusy = false;
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                // maybe we can navigate to a start page here, for you to add to this code base!
            }
        }

        //private async void SetFavouriteProducts()
        //{
        //    try
        //    {
        //        if (Products == null)
        //        {
        //            return;
        //        }
        //        List<Product> SavedFavouriteProducts = await _productsDataService.GetFavouriteProducts();
        //        foreach (Product itemProduct in Products)
        //        {
        //            if (SavedFavouriteProducts.Contains<Product>(itemProduct))
        //            {
        //                itemProduct.ISInFavourite = true;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        //throw;//x
        //    }
        //}

        public class SavedState
        {
            public int CatId { get; set; }
        }

        public SavedState SaveState()
        {
           // MvxTrace.Trace("SaveState called");
            return new SavedState { CatId = _CatId };
        }

        public void ReloadState(SavedState savedState)
        {
          //  MvxTrace.Trace("ReloadState called with {0}",savedState.JourneyId);
            _CatId = savedState.CatId;
        }

        public void ShowSortView()
        {
            ShowViewModel<SortingViewModel>();
        }

        public void ShowFillterView()
        {
            ShowViewModel<FillteringViewModel>();
        }


        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            Messenger.Publish(new ReloadeDataMessage(this) { ShouldReloade = true });
        }

        // commands
        public MvxCommand ShowCartViewCommand
        { get { return new MvxCommand(() => ShowViewModel<CartViewModel>()); } }

        //public MvxCommand<Product> ShowProductViewCommand
        //{
        //    get
        //    {
        //        return new MvxCommand<Product>(selectedPro =>
        //        {
        //            ShowViewModel<ProductViewModel>
        //            (new { ProductId = selectedPro.Id });
        //        });
        //    }
        //}

        public MvxCommand<Product> ShowProductViewCommand
        {
            get
            {
                return new MvxCommand<Product>(OpenProduct);
            }
        }

        private async void OpenProduct(Product pro)
        {
            try
            {
                BaseRepository baseRepo = new BaseRepository();
                string proJson = await baseRepo.SerializeObject(pro);

                ShowViewModel<ProductViewModel>
                    (new { ProductId = pro.Id, productJson = proJson });
            }
            catch
            {

            }
        }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }


        //public async void PreparProductsPage(int page)
        //{
        //    try
        //    {
        //       // IsBusy = true;
        //        if (ProductsDictionary.ContainsKey(page))
        //        {
        //            IsPageAvailable = true;
        //        }
        //        else
        //        {
        //            //test
        //            var newProd = (await _productsDataService.GetAllProducts(_AppUser, page)).ToObservableCollection();
        //            ProductsDictionary.Add(page, newProd);
        //            IsPageAvailable = true;
        //            PagePrepared?.Invoke(page, new EventArgs());
        //        }
        //       // IsBusy = false;
        //    }
        //    catch (Exception)
        //    {
        //        IsBusy = false;
        //        //throw;//x
        //    }
        //}

        public async void LoadProductsPage(int page)
        {
            try
            {
                List<Product> newProductsPage = new List<Product>();
                if (_CatId == -1)
                {
                    newProductsPage = await _productsDataService.GetAllProducts(_AppUser, page);
                }
                else if (_CatId == -2)
                {
                    newProductsPage = await _productsDataService.SearchProducts(_SearchVars.KeyWord, _SearchVars.CategoryId,
                        _SearchVars.StoreId, _SearchVars.MinPrice, _SearchVars.MaxPrice, _AppUser, page);
                }
                else
                {
                    newProductsPage = await _productsDataService.GetCategoryProducts(_CatId, _AppUser, page);
                }
                //var newProds = (await _productsDataService.GetAllProducts(_AppUser, page)).ToObservableCollection();
                foreach (Product item in newProductsPage)
                {
                    Products.Add(item);
                }
                if (newProductsPage.Count == 0)
                {
                    CanLoadMore = false;
                }
                PagePrepared?.Invoke(page, new EventArgs());
            }
            catch 
            {
                IsBusy = false;

            }
        }
    }
}
