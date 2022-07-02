using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Messages;
using Ayadi.Core.Model;
using Ayadi.Core.Repositories;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ayadi.Core.ViewModel
{
    public class ProductViewModel : BaseViewModel, IProductViewModel
    {
        private readonly IProductsDataService _productsDataService;
        private readonly ICartDataService _cartDataService;
        private readonly IConnectionService _connectionService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        private int _ProductId;

        private ShoppingCart _shoppingCart { get; set; }
        private Product _wishListIetm { get; set; }
        private List<ShoppingCart> _shoppingCartList { get; set; }
        private List<Product> _wishList { get; set; }

        public bool IsProductInShoppingList { get; set; }

        public event EventHandler CartListReady;

        //product & store
        public List<BaseModel> Cartls;

        private string _productJson;

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set {
                _selectedProduct = value;
                RaisePropertyChanged(() => SelectedProduct);
            }
        }

        private int _rate;

        public int Rate
        {
            get { return _rate; }
            set { _rate = value; RaisePropertyChanged(() => Rate); }
        }


        //private Store _currentStore;

        //public Store CurrentStore
        //{
        //    get { return _currentStore; }
        //    set { _currentStore = value; RaisePropertyChanged(() => CurrentStore); }
        //}


        //private string _storeName;

        //public string StorName
        //{
        //    get { return _storeName; }
        //    set { _storeName = value; RaisePropertyChanged(() => StorName); }
        //}

        //private int _qountity;

        //public int Quantity
        //{
        //    get { return _qountity; }
        //    set
        //    {
        //        _qountity = value;
        //        RaisePropertyChanged(() => Quantity);
        //    }
        //}

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        //private string _productName;

        //public string ProductName
        //{
        //    get { return _productName; }
        //    set { _productName = value; RaisePropertyChanged(() => ProductName); }
        //}


        public ProductViewModel(IMvxMessenger messenger,
            IProductsDataService productsDataService,
             IConnectionService connectionService,
             ICartDataService cartDataService,
             IDialogService dialogService,
             IUserDataService userDataService) :base(messenger)
        {
            _productsDataService = productsDataService;
            _connectionService = connectionService;
            _cartDataService = cartDataService;
            _userDataService = userDataService;
            _dialogService = dialogService;

        }

        public void Init(int ProductId, string productJson)
        {
            _ProductId = ProductId;
            _productJson = productJson;
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            try
            {
                if (_connectionService.CheckOnline())
                {
                    IsBusy = true;
                    _AppUser = await _userDataService.GetSavedUser();
                   // SelectedProduct = await _productsDataService.GetProduct(_ProductId, _AppUser);
                    BaseRepository basRepo = new BaseRepository();
                    SelectedProduct = await basRepo.DeserializeTObject<Product>(_productJson);

                    // avoid devidby zero exception
                    if (SelectedProduct.Approved_total_reviews == 0)
                    {
                        Rate = 0;
                    }
                    else
                    {
                        Rate = (int)Math.Round((decimal)(SelectedProduct.Approved_rating_sum / SelectedProduct.Approved_total_reviews));
                    }

                    //if (SelectedProduct.Vendor_id != 0)
                    //{
                    //    CurrentStore = await _productsDataService.GetStoreById(SelectedProduct.Vendor_id.ToString());
                    //}
                    //else
                    //{
                    //    CurrentStore = new Store() { Id = "-1", Name = "Ayadi Store" };
                    //}
                    _wishList = new List<Product>();
                    // _shoppingCartList = await _cartDataService.GetShoppingCartItems(_AppUser);
                    _shoppingCartList = _cartDataService.GetActiveShoppingList();
                    SetProductQiantity();
                    //Cartls = await _cartDataService.GetSavedCart();//if key not exist => big crash
                    //SetQuantity();
                    CartListReady(this, new EventArgs());
                    IsBusy = false;
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                }
            }
            catch (Exception)
            {
                IsBusy = false;
                //throw;//x
            }
           // StorName = "new Stor";
            
        }

        private void SetQuantity()
        {
            try
            {
                SelectedProduct.Quantity = 1;
                foreach (BaseModel item in Cartls)
                {
                    if (item is Product)
                    {
                        Product prod = item as Product;
                        if (prod.Id == SelectedProduct.Id)
                        {
                            SelectedProduct.Quantity = prod.Quantity;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            
        }

        private void SetProductQiantity()
        {
            try
            {
                SelectedProduct.Quantity = 1;
                if (_shoppingCartList == null)
                {
                    return;
                }
                foreach (var item in _shoppingCartList)
                {
                    if (item.Product_id.ToString() == SelectedProduct.Id && item.Shopping_cart_type == Constants.Shopping_cart)
                    {
                        SelectedProduct.Quantity = item.Quantity;
                        IsProductInShoppingList = true;
                        _shoppingCart = item;
                    }
                    if ( item.Shopping_cart_type == Constants.Wish_list)
                    {
                        _wishList.Add(item.Product);
                        if (item.Product_id.ToString() == SelectedProduct.Id )
                        {
                            SelectedProduct.ISInFavourite = true;
                            item.Product.ShoppingCartId = item.Id;
                            _wishListIetm = item.Product;
                        }
                       
                    }

                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        public void SaveCartList()
        {
            foreach (BaseModel item in Cartls)
            {
                if (item is Product)
                {
                    Product prod = item as Product;
                    if (prod.Id == SelectedProduct.Id)
                    {
                        prod.Quantity = SelectedProduct.Quantity;
                        break;
                    }
                }
            }
            _cartDataService.SaveCart(Cartls);
        }

        public class SavedState
        {
            public int ProductId { get; set; }
        }

        public SavedState SaveState()
        {
            // MvxTrace.Trace("SaveState called");
            return new SavedState { ProductId = _ProductId };
        }

        public void ReloadState(SavedState savedState)
        {
            //  MvxTrace.Trace("ReloadState called with {0}",savedState.JourneyId);
            _ProductId = savedState.ProductId;
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            Messenger.Publish(new ReloadeDataMessage(this) { ShouldReloade = true });
        }

        // commands
        public MvxCommand ShowCartViewCommand
        { get { return new MvxCommand(() => { ShowViewModel<CartViewModel>(); Close(this); }); } }

        public MvxCommand ShowReviewsViewCommand
        { get { return new MvxCommand(() => ShowViewModel<ProductsReviewsViewModel>(new { ProductId = _ProductId })); } }

        public MvxCommand ShowReviewViewCommand
        { get { return new MvxCommand(() => MakeRating()); } }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        public MvxCommand BlusCommand
        { get { return new MvxCommand(() => plus()); } }

        public MvxCommand MinusCommand
        { get { return new MvxCommand(() => minus()); } }

        public ICommand AddToCartAsync
        {
            get
            {
                return new MvxCommand(async () => { _shoppingCart = await PostCart(Constants.Shopping_cart); });
            }
        }

        public ICommand UpdateProductAsyncCommand
        {
            get
            {
                return new MvxAsyncCommand(async () => { _shoppingCart = await PutCart(); });
            }
        }

        /*
        private void addToCart()
        {
            try
            {
                bool _storExist = false;
                bool _ProductExist = false;
                foreach (var item in Cartls)
                {
                    if (item is Store)
                    {
                        Store sto = item as Store;
                        if (sto.Id == CurrentStore.Id)
                        {
                            _storExist = true;
                           
                            foreach (var _pro in Cartls)
                            {
                                if (_pro is Product)
                                {
                                    Product newProduct = _pro as Product;
                                    if (newProduct.Id == SelectedProduct.Id)
                                    {
                                        _ProductExist = true;
                                        newProduct.Quantity = newProduct.Quantity + 1;
                                        break;
                                    }
                                }
                            }

                            if (!_ProductExist)
                            {
                                int _index = Cartls.IndexOf(item) + 1;
                                SelectedProduct.Quantity = 1;
                                Cartls.Insert(_index, SelectedProduct);
                            }

                            break;
                        }
                    }
                }
                if (!_storExist)
                {
                    //Store newStor = new Store()
                    //{ Id = SelectedProduct.vendor_name, Name = SelectedProduct.vendor_name };
                    Cartls.Add(CurrentStore);
                    Cartls.Add(SelectedProduct);
                }
                _cartDataService.SaveCart(Cartls);
            }
            catch (Exception ex)
            {
                string ms = ex.Message;
                //throw;//x
            }
        }
        */

        private async Task<ShoppingCart> PostCart(string cartType)
        {
            ShoppingCart cart_;
            try
            {
                ShoppingCart shop = new ShoppingCart();
                shop.Product = SelectedProduct;
                shop.Customer_id = _AppUser.Id;// Constants.UserId;//UserId
                int ProductId_ = 0;
                int.TryParse(SelectedProduct.Id, out ProductId_);
                shop.Product_id = ProductId_;
                shop.Quantity = SelectedProduct.Quantity;
                shop.Shopping_cart_type = cartType;
               // shop.Customer = new User();
                shop.Id = "0";
                cart_ = await _cartDataService.PostShoppingCartItem(shop, _AppUser);
            }
            catch (Exception)
            {
                cart_ = _shoppingCart;
                //throw;//x
            }
            return cart_;
        }

        private async void MakeRating()
        {
            _AppUser = await _userDataService.GetSavedUser();
            if (_AppUser.IsGuestUser || _AppUser.Id == 0 )
            {
                ShowViewModel<LoginViewModel>();
            }
            else
            {
                ShowViewModel<ProductsReviewViewModel>(new { ProductId = _ProductId });
            }
        }

        public async Task<ShoppingCart> PutCart()
        {
            ShoppingCart cart_;
            try
            {
                if (_shoppingCart == null)
                {
                    return null;
                }
                _shoppingCart.Quantity = SelectedProduct.Quantity;
                cart_ = await _cartDataService.PutShoppingCartItem(_shoppingCart, _AppUser);
            }
            catch (Exception)
            {
                cart_ = _shoppingCart;
                //throw;//x
            }
            return cart_;
        }

        public MvxAsyncCommand AddToFavouritesCommand
        {
            get
            {
                return new MvxAsyncCommand(() => PostToFavouritesAsync());
            }
        }

        public void FireMessage(Product product)
        {
            Messenger.Publish(
                          new FavouriteChangedMessage(this)
                          { NewFavList = _wishList });//SubHomeViewModel

            Messenger.Publish(new FavouriteProductMessage(this, product));
        }

        private async Task<bool> PostToFavouritesAsync()
        {
            ShoppingCart shop_;
            try
            {
                if (SelectedProduct.ISInFavourite)
                {
                    // remover from favourite
                    SelectedProduct.ISInFavourite = false;
                    string deleteRespondse = await _cartDataService.DeleteShoppingCartItem(_wishListIetm.ShoppingCartId, _AppUser);
                    Product product_ = _wishList.Find(p => p.Id == SelectedProduct.Id);
                    _wishList.Remove(product_);
                    FireMessage(product_);
                    return deleteRespondse.Length == 2;
                }
                else
                {
                    //add to favourite
                    SelectedProduct.ISInFavourite = true;
                    shop_ = await PostCart(Constants.Wish_list);
                    SelectedProduct.ShoppingCartId = shop_.Id;
                    _wishListIetm = SelectedProduct;
                    _wishList.Add(SelectedProduct);
                    FireMessage(SelectedProduct);
                    return shop_.Shopping_cart_type != null;
                }
                //
                
              //  return shop_.Shopping_cart_type == null;
            }
            catch (Exception)
            {
                return false;
                //throw;//x 
            }
        }
        /*
        private async Task<bool> AddToFavourites()
        {
            try
            {
                // ISInFavourite = ISInFavourite == false ? true : false;
                List<Product> ls = await _productsDataService.GetFavouriteProducts();
                if (SelectedProduct.ISInFavourite)
                {
                    SelectedProduct.ISInFavourite = false;
                    ls.Remove(SelectedProduct);
                }
                else
                {
                    SelectedProduct.ISInFavourite = true;
                    if (!ls.Contains(SelectedProduct))
                    {
                        ls.Add(SelectedProduct);
                    }

                }
                _productsDataService.SaveFavouriteProducts(ls);
            }
            catch (Exception)
            {

                //throw;//x
            }
            return true;
        }
        */

        private void plus()
        {
            SelectedProduct.Quantity = SelectedProduct.Quantity + 1;
        }
        private void minus()
        {
            SelectedProduct.Quantity = SelectedProduct.Quantity - 1;
        }

        //called via view
        public async void DeletProduct()
        {
            try
            {
                if (_shoppingCart == null)
                {
                    return;
                }
                IsBusy = true;
                string responds = await _cartDataService.DeleteShoppingCartItem(_shoppingCart.Id, _AppUser);
                IsBusy = false;
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        public string[] GetMessageWords()
        {
            string[] words = new string[6];
            try
            {
                string confirm = TextSource.GetText("confirmDelete_");
                words[0] = confirm;

                string Msg = TextSource.GetText("ruSure_");
                words[1] = Msg;

                string yes = TextSource.GetText("ok_");
                words[2] = yes;

                string no = TextSource.GetText("cancel_");
                words[3] = no;

                string deleted = TextSource.GetText("deleted_");
                words[4] = deleted;

                string canceld = TextSource.GetText("cancelled_");
                words[5] = canceld;
            }
            catch (Exception)
            {

                //throw;//x
            }
            return words;
        }
    }
}
