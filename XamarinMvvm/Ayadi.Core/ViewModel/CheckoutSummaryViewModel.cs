using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;
using Ayadi.Core.Model;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Messages;
using MvvmCross.Platform;

namespace Ayadi.Core.ViewModel
{
    public class CheckoutSummaryViewModel : BaseViewModel , ICheckoutSummaryViewModel
    {
        private readonly IOrderDataService _orderDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;
        private readonly IUserDataService _userDataService;

        private User _AppUser;

        private string _orderJson;

        private Order _order;
        public Order CurrentOrder
        {
            get { return _order; }
            set { _order = value; RaisePropertyChanged(() => CurrentOrder); }
        }

        private decimal _subTotal;
        public decimal SubTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; RaisePropertyChanged(() => SubTotal); }
        }

        private decimal _Total;
        public decimal Total
        {
            get { return _Total; }
            set { _Total = value; RaisePropertyChanged(() => Total); }
        }

        private decimal _shippingCost;
        public decimal ShippingCost
        {
            get { return _shippingCost; }
            set { _shippingCost = value; RaisePropertyChanged(() => ShippingCost); }
        }

        private decimal _paymentMethodFees;

        public decimal PaymentMethodFees
        {
            get { return _paymentMethodFees; }
            set { _paymentMethodFees = value; RaisePropertyChanged(() => PaymentMethodFees); }
        }


        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public bool ViewModelInitilized { get; set; }

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

        private List<ShoppingCart> _cartItemsList;

        #region Strings
        public string ProductsString { get; set; }
        public string PaymentString { get; set; }
        public string ShippingString { get; set; }
        public string AdressString { get; set; }
        public string SubtotalString { get; set; }
        public string BillingAdressString { get; set; }
        public string ShippingAdresslString { get; set; }
        #endregion

        public CheckoutSummaryViewModel(IMvxMessenger messenger,
            IOrderDataService orderDataService,
             IConnectionService connectionService,
              IUserDataService userDataService,
             IDialogService dialogService):base(messenger)
        {
            _orderDataService = orderDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
            _userDataService = userDataService;
        }

        public void Init(string OrderJson)
        {
            _orderJson = OrderJson;
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
                    var ShoppingCartList = await _orderDataService._cartRepository.GetShoppingCartItemsFromAPI(_AppUser);
                    _cartItemsList = ShoppingCartList.FindAll(s => s.Shopping_cart_type == Constants.Shopping_cart);
                    // CurrentOrder = await _or derDataService.GetSavedOrder();
                    CurrentOrder = await _orderDataService.DeserializeOrder(_orderJson);
                  
                    CurrentOrder.Order_items = GetOrderItems();

                    intitializeProducts();
                    IsBusy = false;
                    ViewModelInitilized = true;
                    PaymentMethodFees = (decimal)CurrentOrder.Paymet_Method.AdditionalFee;
                    ShippingCost = 0;
                    Total = SubTotal + ShippingCost + PaymentMethodFees;
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                  TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    // maybe we can navigate to a start page here, for you to add to this code base!
                }

                #region initialize Strings
                ProductsString = TextSource.GetText("products");
                ShippingString = TextSource.GetText("shiipping_");
                PaymentString = TextSource.GetText("payment_");
                AdressString = TextSource.GetText("adress_");
                SubtotalString = TextSource.GetText("subtotal");
                ShippingAdresslString = TextSource.GetText("shippngAdress");
                BillingAdressString = TextSource.GetText("billingAdress");
                #endregion
            }
            catch 
            {

                //throw;//x
            }
            
        }

        private List<OrderItems> GetOrderItems()
        {
            List<OrderItems> items_ = new List<OrderItems>();
            try
            {
                foreach (ShoppingCart cart_item in _cartItemsList)
                {
                    OrderItems it_ = new OrderItems();
                    it_.Product = cart_item.Product;
                    it_.Product_id = cart_item.Product_id.ToString();
                    it_.Quantity = cart_item.Quantity;
                    items_.Add(it_);
                }
                return items_;
            }
            catch 
            {
                return items_;
            }
        }

        private void intitializeProducts()
        {
            decimal subTotal_ = 0;

            ShippingCost = 0;
            Products = new ObservableCollection<Product>();
            if (CurrentOrder.Order_items != null)
            {
                foreach (var itemProduct in CurrentOrder.Order_items)
                {
                    itemProduct.Product.Quantity = itemProduct.Quantity;
                    Products.Add(itemProduct.Product);
                    subTotal_ = subTotal_ + (itemProduct.Product.Quantity * itemProduct.Product.Price);
                }
            }

            // bind to CurrentOrder.Order_total dosen't help because it haven't RaisePropertyChanged(() => CurrentOrder.Order_total)
            SubTotal = subTotal_;
            CurrentOrder.Order_Subtotal = subTotal_;
            CurrentOrder.Order_total = SubTotal + ShippingCost + PaymentMethodFees;
            Total = CurrentOrder.Order_total;
        }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        public MvxCommand CheckoutCommand
        { get { return new MvxCommand(() => Checkout()); } }

        private async void Checkout()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                IsBusy = true;
                CurrentOrder.Store_id = 1;
                CurrentOrder.Customer = new User() { Id = _AppUser.Id };
                CurrentOrder.Customer_id = _AppUser.Id;
                //foreach (var item in CurrentOrder.Order_items)
                //{
                //    item.Quantity = 0;
                //    item.Product.Quantity = 0;
                //}
                Order or = await _orderDataService.PostOrder(CurrentOrder, _AppUser);
               
                if (or.Id == 0)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("orderNotComplet"),
                        TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                }
                else
                {
                     _dialogService.ShowToast(TextSource.GetText("orderComplet"));
                   
                    Messenger.Publish(new OrderMessage(this) { PostedOrder = or });

                    Close(this);
                   // ShowViewModel<HomeViewModel>();

                }
                IsBusy = false;
            }
            catch 
            {
                IsBusy = false;
                //throw;//x
            }
        }
    }
}
