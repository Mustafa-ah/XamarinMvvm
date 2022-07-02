using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Model;
using System.Collections.ObjectModel;
using Ayadi.Core.Repositories;

namespace Ayadi.Core.ViewModel
{
    public class StoreViewModel : BaseViewModel, IStoreViewModel
    {
        private readonly IStoreDataService _storeDataService;
        private readonly ILoadingDataService _loadingDataService;
        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        private string _storeDescription;

        public string StoreDescription
        {
            get { return _storeDescription; }
            set { _storeDescription = value; RaisePropertyChanged(() => StoreDescription); }
        }



        private string _storename;

        public StoreViewModel(IMvxMessenger messenger, IStoreDataService storeDataService,
              ILoadingDataService loadingDataService,
              IConnectionService connectionService,
               IUserDataService userDataService,
              IDialogService dialogService):base(messenger)
        {
            _storeDataService = storeDataService;
            _loadingDataService = loadingDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
            _userDataService = userDataService;
        }


        private Store _selectedStore;

        public Store SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; RaisePropertyChanged(() => SelectedStore); }
        }

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

        public void Init(string storename)
        {
            _storename = storename;
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

                //SelectedStore = new Store() { Name = "new store", Image = new Imager()
                //{ Src = "http://ayadi.local/content/images/thumbs/0002152_starbucks.png" },Description= "maybe we can navigate to a start page here, for you to add to this code base!"
                //};
                SelectedStore = await _storeDataService.GetStoreById(_storename, _AppUser);
                //StoreDescription = Regex.Replace(SelectedStore.Description, "<.*?>", String.Empty);
                if (!string.IsNullOrEmpty(SelectedStore.Description))
                {
                    StoreDescription = Utility.HelperTools.RemoveHTMLtags(SelectedStore.Description);
                }
                Products = (await _storeDataService.GetStoreProducts(SelectedStore.Id, _AppUser)).ToObservableCollection();
                _loadingDataService.HideFragmentLoading();
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
               TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                //maybe we can navigate to a start page here, for you to add to this code base!
            }

        }

        //commands 
        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

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
    }
}
