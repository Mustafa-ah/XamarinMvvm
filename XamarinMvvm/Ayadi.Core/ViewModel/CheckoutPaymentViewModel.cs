﻿using System;
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
using System.Collections.ObjectModel;
using Ayadi.Core.Extensions;

namespace Ayadi.Core.ViewModel
{
    public class CheckoutPaymentViewModel : BaseViewModel, ICheckoutPaymentViewModel
    {
        private readonly IOrderDataService _orderDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;
        private readonly IUserDataService _userDataService;

        private readonly MvxSubscriptionToken _token;

        public bool ShouldClose { get; set; }

        private PaymentMethod _paymentMehod;

        private ObservableCollection<PaymentMethod> _paymentMethods;
        public ObservableCollection<PaymentMethod> PaymentMethods
        {
            get { return _paymentMethods; }
            set { _paymentMethods = value; RaisePropertyChanged(() => PaymentMethods); }
        }

        private Order _order;
        public Order CurrentOrder
        {
            get { return _order; }
            set { _order = value; RaisePropertyChanged(() => CurrentOrder); }
        }

        private string _orderJson;

        private bool _isBusy = true;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public CheckoutPaymentViewModel(IMvxMessenger messenger,
            IOrderDataService orderDataService,
            IUserDataService userDataService,
             IConnectionService connectionService,
             IDialogService dialogService):base(messenger)
        {
            _orderDataService = orderDataService;
            _connectionService = connectionService;
            _userDataService = userDataService;
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
                CurrentOrder = await _orderDataService.DeserializeOrder(_orderJson);
                User user = new User() { AccessToken = _userDataService.AccessToken };

                PaymentMethods = (await _orderDataService.GetPaymentMethods(user)).ToObservableCollection();
                // CurrentOrder = await _orderDataService.GetSavedOrder();
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

        public MvxCommand<PaymentMethod> SelectPaymentMethodCommand
        { get { return new MvxCommand<PaymentMethod>( (p)=> _paymentMehod = p); } }

        private async void Checkout()
        {
            try
            {
                if (_paymentMehod == null)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("ChoosePaymentMethod_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                //  CurrentOrder.Payment_method_system_name = "Payments.CashOnDelivery";
                CurrentOrder.Payment_method_system_name = _paymentMehod.SystemName;
                CurrentOrder.Paymet_Method = _paymentMehod;

                string orderJson = await _orderDataService.SerializeeOrder(CurrentOrder);
                ShowViewModel<CheckoutSummaryViewModel>(new { OrderJson = orderJson });
                //bool _Saved = await _orderDataService.SaveOrder(CurrentOrder);
                //if (_Saved)
                //{
                //    ShowViewModel<CheckoutPaymentViewModel>();
                //}
                //else
                //{
                //    await _dialogService.ShowAlertAsync("No action available", "Ayadi says...", "OK");
                //}
                //ShowViewModel<CheckoutSummaryViewModel>();

            }
            catch (Exception)
            {

                //throw;//x
            }
        }
    }
}
