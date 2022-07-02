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
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;
using Ayadi.Core.Messages;

namespace Ayadi.Core.ViewModel
{
    public class CartViewModel : BaseViewModel , ICartViewModel
    {
        private readonly ICartDataService _cartDataService;
        private readonly ILoadingDataService _loadingDataService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;

        private readonly MvxSubscriptionToken _token;

        private User _AppUser;

        public bool ShouldClose { get; set; }
        //private readonly MvxSubscriptionToken _PlusToken;

            // wish lsit & shopping cart
        private List<ShoppingCart> ShoppingCartList { get; set; }

        public List<ShoppingCart> ShoppingList { get; set; }

        public List<Store> Stors { get; set; }

        private ObservableCollection<BaseModel> _cartItems;

        public ObservableCollection<BaseModel> CartItems
        {
            get { return _cartItems; }
            set { _cartItems = value; RaisePropertyChanged(() => CartItems); }
        }


        public CartViewModel(IMvxMessenger messenger,ICartDataService cartDataService,
             ILoadingDataService loadingDataService,
             IUserDataService userDataService,
             IDialogService dialogService) : base(messenger)
        {
            _cartDataService = cartDataService;
            _loadingDataService = loadingDataService;
            _dialogService = dialogService;
            _userDataService = userDataService;

            _token = Messenger.SubscribeOnMainThread<OrderMessage>(OnOrderPosted);
        }

        private void OnOrderPosted(OrderMessage obj)
        {
            ShouldClose = true;
            //  await _dialogService.ShowAlertAsync("No internet available", "Ayadi says...", "OK");
            Close(this);
        }

        #region Strings
        public string DeleteMsgString { get; set; }
        public string UndoString { get; set; }
        #endregion

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            try
            {
                await _loadingDataService.ShowFragmentLoading();
                _AppUser = await _userDataService.GetSavedUser();

                //ShoppingCartList = await _cartDataService.GetShoppingCartItemsFromAPI(_AppUser);
                ShoppingCartList = _cartDataService.GetActiveShoppingList();
                ShoppingCartList = ShoppingCartList.FindAll(sh => sh.Shopping_cart_type == Constants.Shopping_cart);
                int[] ids_ = ShoppingCartList.Select(s => s.Product.Vendor_id).ToArray();
                Stors = await _cartDataService.GetStorsNamesByIds(ids_, _AppUser);
                // CartItems = (await _cartDataService.GetSavedCart()).ToObservableCollection();
                SetCartItems();
                InitializeTotal();
                if (CartItems.Count == 0)
                {
                    Store st_ = new Store();
                    st_.Name = TextSource.GetText("EmpatyMsg");
                    CartItems.Add(st_);
                }
                _loadingDataService.HideFragmentLoading();

                #region initialize Strings
                DeleteMsgString = TextSource.GetText("deleteMsg_");
                UndoString = TextSource.GetText("undo_");
                #endregion
            }
            catch 
            {

            }
        }

        private void SetCartItems()
        {
            try
            {
                ShoppingList = new List<ShoppingCart>();
                CartItems = new ObservableCollection<BaseModel>();
                foreach (Store sto in Stors)
                {
                    CartItems.Add(sto);
                    ShoppingList.Add(new ShoppingCart() { Product = new Product() { Vendor_id = -1} });

                    for (int i = 0; i < ShoppingCartList.Count; i++)
                    {
                        if (ShoppingCartList[i].Product.Vendor_id.ToString() == sto.Id)
                        {
                            if (ShoppingCartList[i].Shopping_cart_type == Constants.Shopping_cart)
                            {
                                ShoppingCartList[i].Product.Quantity = ShoppingCartList[i].Quantity;
                                CartItems.Add(ShoppingCartList[i].Product);
                                ShoppingList.Add(ShoppingCartList[i]);
                                ShoppingCartList.Remove(ShoppingCartList[i]);
                            }

                        }
                    }
                }

                if (ShoppingCartList.Count > 0)
                {
                    CartItems.Add(new Store() { Id = "-1", Name = "Tommor Store" });
                    ShoppingList.Add(new ShoppingCart() { Product = new Product() { Vendor_id = -1 } });
                    foreach (var item in ShoppingCartList)
                    {
                        if (item.Shopping_cart_type == Constants.Shopping_cart)
                        {
                            item.Product.Quantity = item.Quantity;
                            CartItems.Add(item.Product);
                            ShoppingList.Add(item);
                        }
                            
                    }
                }

                if (CartItems.Count == 1)
                {
                    Store st_ = CartItems[0] as Store;
                    st_.Name = TextSource.GetText("EmpatyMsg");
                }
            }
            catch (Exception ex)
            {
                string fff = ex.Message;
                //throw;//x
            }
        }
        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set
            {
                _total = value;
                RaisePropertyChanged(() => Total);
            }
        }

        private int _itemsCount;
        public int ItemsCount
        {
            get { return _itemsCount; }
            set
            {
                _itemsCount = value;
                RaisePropertyChanged(() => ItemsCount);
            }
        }

        public void InitializeTotal()
        {
            decimal _total = 0;
            int counter = 0;
            try
            {
                foreach (BaseModel item in CartItems)
                {
                    if (item is Product)
                    {
                        Product _pro = item as Product;
                        decimal ItemPrice = _pro.Price * _pro.Quantity;
                        _total = _total + ItemPrice;
                        counter++;
                    }
                }
                Total = _total;
                ItemsCount = counter;
            }
            catch 
            {

                //throw;//x
            }
        }

        public void SaveCartList()
        {
            _cartDataService.SaveCart(CartItems.ToList());
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

                string yes = TextSource.GetText("yes");
                words[2] = yes;

                string no = TextSource.GetText("no");
                words[3] = no;

                string deleted = TextSource.GetText("deleted_");
                words[4] = deleted;

                string canceld = TextSource.GetText("cancelled_");
                words[5] = canceld;
            }
            catch 
            {

                //throw;//x
            }
            return words;
        }

        //commands  
        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        public MvxCommand CheckoutCommand
        { get { return new MvxCommand(() => Checkout()); } }

        private async void Checkout()
        {
            try
            {
                // update date if he go to signup and get back
                _AppUser = await _userDataService.GetSavedUser();

                if (_AppUser.IsGuestUser || _AppUser.Id == 0)
                {
                    ShowViewModel<LoginViewModel>();
                    return;
                }
                if (CartItems.Count < 2)
                {

                    await _dialogService.ShowAlertAsync(TextSource.GetText("EmpatyMsg"), TextSource.GetText("tomoor_"),
                        TextSource.GetText("ok_"));
                    return;
                }
                await _loadingDataService.ShowFragmentLoading();
                Order order_ = new Order();

                string orderJson = await _cartDataService.SerializeeOrder(order_);

                ShowViewModel<CheckoutAdressViewModel>(new { OrderJson = orderJson });

                _loadingDataService.HideFragmentLoading();

               // Close(this);
            }
            catch (Exception dd)
            {
                _loadingDataService.HideFragmentLoading();
                //throw;//x
            }
        }

        public async void RemoveProductFromCart(string shopItemId)
        {
            string responds_ = await _cartDataService.DeleteShoppingCartItem(shopItemId, _AppUser);
        }

        public async Task<ShoppingCart> PutCart(ShoppingCart _shoppingCart)
        {
            ShoppingCart cart_ = new ShoppingCart();
            try
            {
                if (_shoppingCart == null)
                {
                    return null;
                }
               // _shoppingCart.Quantity = SelectedProduct.Quantity;
                cart_ = await _cartDataService.PutShoppingCartItem(_shoppingCart, _AppUser);
            }
            catch 
            {
               
                //throw;//x
            }
            return cart_;
        }
    }
}
