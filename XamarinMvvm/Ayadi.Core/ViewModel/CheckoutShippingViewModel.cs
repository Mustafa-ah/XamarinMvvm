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
using Ayadi.Core.Messages;

namespace Ayadi.Core.ViewModel
{
    public class CheckoutShippingViewModel : BaseViewModel , ICheckoutShippingViewModel
    {
        private readonly IOrderDataService _orderDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private readonly MvxSubscriptionToken _token;

        public bool ShouldClose { get; set; }

        private Order _order;
        public Order CurrentOrder
        {
            get { return _order; }
            set { _order = value; RaisePropertyChanged(() => CurrentOrder); }
        }

        private string _orderJson;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public CheckoutShippingViewModel(IMvxMessenger messenger,
           IOrderDataService orderDataService,
            IConnectionService connectionService,
            IDialogService dialogService):base(messenger)
        {
            _orderDataService = orderDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
            _token = Messenger.SubscribeOnMainThread<OrderMessage>(OnOrderPosted);
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
                //CurrentOrder = await _orderDataService.GetSavedOrder();
                CurrentOrder = await _orderDataService.DeserializeOrder(_orderJson);
                IsBusy = false;
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                   TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                // maybe we can navigate to a start page here, for you to add to this code base!
            }
        }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        public MvxCommand CheckoutCommand
        { get { return new MvxCommand(() => Checkout()); } }

        private async void Checkout()
        {
            try
            {
                CurrentOrder.Shipping_method = "Ground";
                CurrentOrder.Shipping_rate_computation_method_system_name = "Shipping.FixedOrByWeight";

                string orderJson = await _orderDataService.SerializeeOrder(CurrentOrder);
                ShowViewModel<CheckoutPaymentViewModel>(new { OrderJson = orderJson });
                //bool _Saved = await _orderDataService.SaveOrder(CurrentOrder);
                //if (_Saved)
                //{
                //    ShowViewModel<CheckoutPaymentViewModel>();
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
