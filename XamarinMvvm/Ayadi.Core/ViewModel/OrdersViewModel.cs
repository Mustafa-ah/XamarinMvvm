using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Extensions;
using Ayadi.Core.Contracts.Services;
using System.Collections.ObjectModel;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;

namespace Ayadi.Core.ViewModel
{
    public class OrdersViewModel : BaseViewModel, IOrdersViewModel
    {
        private readonly IOrderDataService _orderDataService;
        private readonly ILoadingDataService _loadingDataService;
        private readonly IDialogService _dialogService;
        private readonly IUserDataService _userDataService;

        private User _AppUser;

        private ObservableCollection<Order> _orders;

        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { _orders = value; RaisePropertyChanged(() => Orders); }
        }

        public OrdersViewModel(IMvxMessenger messenger, IOrderDataService orderDataService,
            ILoadingDataService loadingDataService,
            IUserDataService userDataService,
            IDialogService dialogService):base(messenger)
        {
            _orderDataService = orderDataService;
            _loadingDataService = loadingDataService;
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
            await _loadingDataService.ShowFragmentLoading();
            _AppUser = await _userDataService.GetSavedUser();
            Orders = (await _orderDataService.GetUserOrders(_AppUser)).ToObservableCollection();
            // Orders = new ObservableCollection<Order>() { new Order() {Id= 455, Order_total = 525 } };
            //for (int i = 0; i < 5; i++)
            //{
            //    Order ord = new Order() { Id = 5445 + i, Order_status = "order Sate", Payment_status = "payment Sate", Shipping_status="not shipped" };
            //    Orders.Add(ord);
            //}
            _loadingDataService.HideFragmentLoading();
        }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }
    }
}
