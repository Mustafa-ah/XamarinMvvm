using Ayadi.Core.Contracts;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Extensions;
using System.Windows.Input;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Messages;
using Ayadi.Core.Utility;
using MvvmCross.Platform;
using MvvmCross.Localization;
using System.Threading;
using Ayadi.Core.Repositories;

namespace Ayadi.Core.ViewModel
{
    public class SubHomeViewModel : BaseViewModel, ISubHomeViewModel
    {
        private readonly IProductsDataService _productsDataService;
       // private readonly ILoadingDataService _loadingDataService;
        private readonly IConnectionService _connectionService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;

      //  CancellationTokenSource _cts;

        private User _AppUser;

        private ObservableCollection<Product> _products;

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
        //private bool _SouldSaveUser;ShoppingListMessage
        private bool _shouldReload;
        private readonly MvxSubscriptionToken _token;
        private readonly MvxSubscriptionToken _loginToken;
        private readonly MvxSubscriptionToken _ShouldReloadToken;
        private readonly MvxSubscriptionToken _ShoppingListMessageToken;

        private ObservableCollection<Category> _Categories;

        public ObservableCollection<Category> Categories
        {
            get
            {
                return _Categories;
            }
            set
            {
                _Categories = value;
                RaisePropertyChanged(() => Categories);
            }
        }

        private ObservableCollection<Sponser> _sponsers;

        public List<SponserSlider> sponserSlider { get; set; }

        public ObservableCollection<Sponser> Sponsers
        {
            get
            {
                return _sponsers;
            }
            set
            {
                _sponsers = value;
                RaisePropertyChanged(() => Sponsers);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public event EventHandler ViewModelInitialized;
        public SubHomeViewModel(IMvxMessenger messenger,
            IProductsDataService productsDataService,
             IUserDataService userDataService,
              IConnectionService connectionService,
              IDialogService dialogService):base(messenger)
        {
            _productsDataService = productsDataService;
           // _loadingDataService = loadingDataService;
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
           
            // *** you have to recive this token so messenger can work ***
            _token = Messenger.Subscribe<FavouriteChangedMessage>(OnFavouriteChange);
            _loginToken = Messenger.Subscribe<LoginMessage>(OnLogin);

            _ShouldReloadToken = messenger.Subscribe<ReloadeDataMessage>(OnReloadData);
            _ShoppingListMessageToken = messenger.Subscribe<ShoppingListMessage>(OnGetShoppingListMessage);
            // InitializeMessenger();ShoppingListMessage
        }

        // to update favourite products
        private async void OnGetShoppingListMessage(ShoppingListMessage obj)
        {
            Products = (await _productsDataService.GetHomeProducts(_AppUser)).ToObservableCollection();
        }

        private async void OnReloadData(ReloadeDataMessage obj)
        {
            if (obj.ShouldReloade)
            {
                await ReloadDataAsync();
            }
            //_shouldReload = obj.ShouldReloade;
        }

        private async void OnLogin(LoginMessage obj)
        {
            await ReloadDataAsync();
        }


        // home slider
        public List<Imager> SliderImages { get; set; }
        private void InitializeMessenger()
        {
            //Messenger.Subscribe<FavouriteChangedMessage>
            //    (async message => { await ReloadDataAsync();_shouldReload = true; });
           // _token = Messenger.Subscribe<FavouriteChangedMessage>(OnFavouriteChange);
        }

        private void OnFavouriteChange(FavouriteChangedMessage obj)
        {
           
            try
            {
                 _shouldReload = false;

                ObservableCollection<Product> TempProducts = new ObservableCollection<Product>();
                foreach (var hP in Products)
                {
                    if (obj.NewFavList.Exists(p => p.Id == hP.Id))
                    {
                        hP.ISInFavourite = true;
                    }
                    else
                    {
                        hP.ISInFavourite = false;
                    }
                    TempProducts.Add(hP);
                }
                // for updating ui
                Products = TempProducts;

              //  Products = new ObservableCollection<Product>();
                //ObservableCollection<Product> TempProducts = new ObservableCollection<Product>(Products);
                //// to update ui
                //Product NewPro = new Product() { Name = "temp" };
                //for (int i = 0; i < 5; i++)
                //{
                //    Products.Insert(0, NewPro);
                //}
                //foreach (var Pitem in TempProducts)
                //{
                //    Products.Add(Pitem);
                //}
                //Products.RemoveAt(0);
                //Products.RemoveAt(0);
                //Products.RemoveAt(0);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        //private void OnFavouriteChange(List<Product> NewFavList)
        //{
        //    _shouldReload = true;
        //}

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            try
            {
                _shouldReload = false;// if fire event from out side home(cats or stor or ...)
                if (_connectionService.CheckOnline())
                {
                    // _loadingDataService.ShowLFragmentLoading();
                    IsBusy = true;
                    _AppUser = await _userDataService.GetSavedUser();
                   
                    if (_AppUser.AccessToken == null)
                    {
                        _AppUser.AccessToken = await _userDataService.GetUserAccessToken();
                        await _userDataService.SaveUserToLocal(_AppUser);
                    }

                    if (string.IsNullOrEmpty(_AppUser.LangID))
                    {
                        _AppUser = await _userDataService.GetSavedUser();
                        if (string.IsNullOrEmpty(_AppUser.LangID))
                        {
                            _AppUser.LangID = "1";
                            // TextSource.
                        }
                    }
                    

                    Products = (await _productsDataService.GetHomeProducts(_AppUser)).ToObservableCollection();
                    // SetFavouriteProducts();
                    Categories = (await _productsDataService.GetHomeCategories(_AppUser)).ToObservableCollection();
                   

                    // pass customer Id to base viewModel at first time
                    if (_AppUser.Id == 0)
                    {
                        _AppUser = await _userDataService.GetSavedUser();
                    }
                    BaseAppUser = _AppUser;

                    sponserSlider = await _productsDataService.GetAllSponsers(_AppUser);

                    MainProducts = Products;
                    if (sponserSlider.Count > 0)
                    {
                        Sponsers = sponserSlider[0].Images.ToObservableCollection();
                    }
                    
                    IsBusy = false;
                   // _loadingDataService.HideFragmentLoading();

                    //_cts = new CancellationTokenSource();
                    //await Task.Factory.StartNew( async () =>
                    //{
                    //    sponserSlider = await _productsDataService.GetAllSponsers(_AppUser);

                    //    MainProducts = Products;
                    //    if (sponserSlider.Count > 0)
                    //    {
                    //        Sponsers = sponserSlider[0].Images.ToObservableCollection();
                    //    }
                    //}, _cts.Token);

                    //temp
                    SliderImages = new List<Imager>()
                     {
                    new Imager() {Src = "http://a.up-00.com/2017/11/151206634618921.jpg" },
                    new Imager() {Src = "http://a.up-00.com/2017/11/151206634628292.jpg" },
                    new Imager() {Src = "http://a.up-00.com/2017/11/151206634648183.jpg" },
                    new Imager() {Src = "http://a.up-00.com/2017/11/151206634686254.jpg" }
                     };
                    Messenger.Publish(new TopHomeSliderMessage(this, SliderImages));


                    ViewModelInitialized(this, new EventArgs());

                    //if (_SouldSaveUser)
                    //{
                    //    bool IsSaved = await _userDataService.SaveUserToLocal(_AppUser);
                    //}
                    //if (_AppUser.Id == 0)
                    //{
                    //    User userGuestId = await _userDataService.LoginAsGuest(_AppUser.AccessToken);
                    //    _AppUser.Id = userGuestId.Id;
                    //    _AppUser.IsGuestUser = true;
                    //    bool IsSaved = await _userDataService.SaveUserToLocal(_AppUser);

                    //    User test_ = await _userDataService.GetSavedUser();
                    //    int dww = test_.Id;
                    //}
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    // maybe we can navigate to a start page here, for you to add to this code base!
                }
                _userDataService.AccessToken = _AppUser.AccessToken;
            }
            catch (OperationCanceledException ec)
            {
                string dd = ec.Message;
            }
            catch (Exception)
            {

                //throw;//x
            }
           
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();
            if (_shouldReload)
            {
               // SetFavouriteProducts();
                await ReloadDataAsync();
                _shouldReload = false;
            }
            //if (_cts != null)
            //{
            //    _cts.Token.ThrowIfCancellationRequested();

            //    _cts.Cancel();
            //}
        }

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

        public MvxCommand<Category> ShowProductsViewCommand
        {
            get
            {
                return new MvxCommand<Category>(selectedCat =>
                {
                    ShowViewModel<ProductsViewModel>
                    (new { CatId = selectedCat.Id , title = selectedCat.Name});
                });
            }
        }

        public MvxCommand<Category> ShowAllProductsViewCommand
        {
            get
            {
                return new MvxCommand<Category>(selectedCat =>
                {
                    ShowViewModel<ProductsViewModel>
                    (new { CatId = -1 });
                });
            }
        }

        public MvxCommand ShowCategoryViewCommand
        { get { return new MvxCommand(() => { ShowViewModel<CategoryViewModel>();
            Messenger.Publish(new HomeUiMessage(this));
        }); } }


        //public async Task<bool> PostToFavouritesAsyncy(Product SelectedProduct)
        //{
        //    ShoppingCart shop_;
        //    try
        //    {
        //        ShoppingCart shop = new ShoppingCart();
        //        shop.Customer_id = _AppUser.Id;// Constants.UserId;//UserId
        //        shop.Product = SelectedProduct;
        //        shop.Product_id = int.Parse(SelectedProduct.Id);
        //        shop.Quantity = SelectedProduct.Quantity;
        //        shop.Shopping_cart_type = Constants.Wish_list;
        //        shop_ = await _productsDataService.PostShoppingCartItem(shop, _AppUser);
        //        SelectedProduct.ISInFavourite = true;
        //        SelectedProduct.ShoppingCartId = shop_.Id;
        //        return shop_.Id != null;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        //throw;//x 
        //    }
        //}

        //public async Task<bool> RemoveFavouritesAsyncy(Product SelectedProduct)
        //{
        //    try
        //    {
        //        string deleteRespondse = await _productsDataService.DeleteShoppingCartItem(SelectedProduct.ShoppingCartId, _AppUser);
        //        SelectedProduct.ISInFavourite = false;
        //        SelectedProduct.ShoppingCartId = null;
        //        return deleteRespondse.Length ==2;
                
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        //throw;//x 
        //    }
        //}
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

    }
}
