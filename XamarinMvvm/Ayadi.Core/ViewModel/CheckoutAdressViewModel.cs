using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Model;
using Ayadi.Core.Extensions;
using System.Collections.ObjectModel;
using Ayadi.Core.Messages;

namespace Ayadi.Core.ViewModel
{
    public class CheckoutAdressViewModel : BaseViewModel, ICheckoutAdressViewModel
    {
        private readonly IOrderDataService _orderDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;
        private readonly IUserDataService _userDataService;

        private UserAdress _SelectedBillingAdress;
        private UserAdress _SelectedShippingAdress;

        private bool _isShippingAdressIsBillingAdress;

        private readonly MvxSubscriptionToken _token;
        private readonly MvxSubscriptionToken _NewAdressToken;

        private User _AppUser;

        public bool ShouldClose { get; set; }

        public bool IsShippingAdressIsBillingAdress
        {
            get { return _isShippingAdressIsBillingAdress; }
            set { _isShippingAdressIsBillingAdress = value; RaisePropertyChanged(() => IsShippingAdressIsBillingAdress); }
        }

        private string _shipToTheSameAddress;

        public string ShipToTheSameAddress
        {
            get { return _shipToTheSameAddress; }
            set { _shipToTheSameAddress = value; RaisePropertyChanged(() => ShipToTheSameAddress); }
        }


        private ObservableCollection<UserAdress> _adresses;

        public ObservableCollection<UserAdress> Adresses
        {
            get { return _adresses; }
            set { _adresses = value; RaisePropertyChanged(() => Adresses); }
        }

        private string _orderJson;

        private Order _order;
        public Order CurrentOrder
        {
            get { return _order; }
            set { _order = value; RaisePropertyChanged(() => CurrentOrder); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public CheckoutAdressViewModel(IMvxMessenger messenger,
           IOrderDataService orderDataService,
           IUserDataService userDataService,
            IConnectionService connectionService,
            IDialogService dialogService):base(messenger)
        {
            _orderDataService = orderDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
            _userDataService = userDataService;

            _token = Messenger.SubscribeOnMainThread<OrderMessage>(OnOrderPosted);
            _NewAdressToken = messenger.Subscribe<AdresessUpdatedMessage>(OnAdressUpdated);
        }

        private void OnAdressUpdated(AdresessUpdatedMessage obj)
        {
            if (obj.NewAdress != null)
            {
                Adresses.Add(obj.NewAdress);
            }
        }

        private void OnOrderPosted(OrderMessage obj)
        {
            ShouldClose = true;
            //  await _dialogService.ShowAlertAsync("No internet available", "Ayadi says...", "OK");
            Close(this);
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
            if (_connectionService.CheckOnline())
            {
                IsBusy = true;
                _AppUser = await _userDataService.GetSavedUser();

                Adresses = (await _orderDataService.GetUserAdresses(_AppUser)).ToObservableCollection();
                CurrentOrder = await  _orderDataService.DeserializeOrder(_orderJson);
                //  CurrentOrder = await _orderDataService.GetSavedOrder();
                IsShippingAdressIsBillingAdress = true;
                ShipToTheSameAddress = TextSource.GetText("shipToSamAdress");
                IsBusy = false;
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                // maybe we can navigate to a start page here, for you to add to this code base!
            }
        }

        public MvxCommand<UserAdress> SelectBillingAdressCommand
        {
            get
            {
                return new MvxCommand<UserAdress>(selectedAdrs => 
                {
                    _SelectedBillingAdress = selectedAdrs;
                    if (IsShippingAdressIsBillingAdress)
                    {
                        _SelectedShippingAdress = selectedAdrs;
                    }
                });
            }
        }

        public MvxCommand<UserAdress> SelectShippingAdressCommand
        {
            get
            {
                return new MvxCommand<UserAdress>(selectedAdrs => _SelectedShippingAdress = selectedAdrs);
            }
        }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => { Close(this); }); } }

        public MvxCommand CheckoutCommand
        { get { return new MvxCommand(() => Checkout()); } }


        public MvxCommand ShowAdressViewCommand
        { get { return new MvxCommand(() => ShowViewModel<UserAdressViewModel>()); } }
       
        //public MvxCommand ShowCheViewCommand
        //{ get { return new MvxCommand(() => ShowViewModel<FavouriteViewModel>()); } }

        private async void Checkout()
        {
            try
            {
                if (_SelectedShippingAdress == null || _SelectedBillingAdress == null)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("chooseAdress"),
                       TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                CurrentOrder.Billing_address = _SelectedBillingAdress;
                CurrentOrder.Shipping_address = _SelectedShippingAdress;

                // in case skipping shipping method view model
                CurrentOrder.Shipping_method = "Ground";
                CurrentOrder.Shipping_rate_computation_method_system_name = "Shipping.FixedOrByWeight";

                string orderJson = await _orderDataService.SerializeeOrder(CurrentOrder);
                // ShowViewModel<CheckoutShippingViewModel>(new { OrderJson = orderJson });
               
                ShowViewModel<CheckoutPaymentViewModel>(new { OrderJson = orderJson });

                //bool _Saved = await _orderDataService.SaveOrder(CurrentOrder);
                //if (_Saved)
                //{
                   

                //    ShowViewModel<CheckoutShippingViewModel>(new { OrderJson = orderJson });
                //    ShowViewModel<CheckoutShippingViewModel>(new { });
                //}
                //else
                //{
                //    await _dialogService.ShowAlertAsync("No action available", "Ayadi says...", "OK");
                //}
 
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
    }
}
