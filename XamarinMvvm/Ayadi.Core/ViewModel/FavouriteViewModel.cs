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
using Ayadi.Core.Messages;
using MvvmCross.Core.ViewModels;
using Ayadi.Core.Repositories;

namespace Ayadi.Core.ViewModel
{
    public class FavouriteViewModel : BaseViewModel , IFavouriteViewModel
    {
        private readonly ICartDataService _cartDataService;
        private readonly ILoadingDataService _loadingDataService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;
        private readonly IConnectionService _connectionService;

        private User _AppUser;

        // list of favourite & cart products
        public List<ShoppingCart> shopList { get; set; }
        // list of favourite products only
        public List<ShoppingCart> WishList { get; set; }

        private ObservableCollection<Product> _favItems;

        public ObservableCollection<Product> FavItems
        {
            get { return _favItems; }
            set { _favItems = value; RaisePropertyChanged(() => FavItems); }
        }

        //     FavouriteViewModel.currency
        public FavouriteViewModel(IMvxMessenger messenger, ICartDataService cartDataService,
             ILoadingDataService loadingDataService,
             IConnectionService connectionService,
              IUserDataService userDataService,
             IDialogService dialogService):base(messenger)
        {
            _cartDataService = cartDataService;
            _loadingDataService = loadingDataService;
            _dialogService = dialogService;
            _connectionService = connectionService;
            _userDataService = userDataService;
        }

        #region Strings
        public string DeleteMsgString { get; set; }
        public string UndoString { get; set; }
        #endregion

        //public override void ViewDestroy()
        //{
        //    base.ViewDestroy();
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
                if (!_connectionService.CheckOnline())
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
              //  await _loadingDataService.ShowFragmentLoading();

                _AppUser = await _userDataService.GetSavedUser();
                BaseAppUser = _AppUser;

               // shopList = await _cartDataService.GetShoppingCartItems(_AppUser);
                shopList = _cartDataService.GetActiveShoppingList();
                fillList();

                FavItems = new ObservableCollection<Product>()
                {
                    new Product() {Name = "product 1" , Price = 120, Images =new List<Imager>()
                    { new Imager() {Src ="http://a.up-00.com/2017/11/151206634618921.jpg" } } },
                     new Product() {Name = "product 22" , Price = 120, Images =new List<Imager>()
                    { new Imager() {Src ="http://a.up-00.com/2017/11/151206634618921.jpg" } } },
                      new Product() {Name = "product 33" , Price = 120, Images =new List<Imager>()
                    { new Imager() {Src ="http://a.up-00.com/2017/11/151206634618921.jpg" } } }
                };

                #region initialize Strings
                DeleteMsgString = TextSource.GetText("deleteMsg_");
                UndoString = TextSource.GetText("undo_");
                #endregion
            }
            catch 
            {

                //throw;//x
            }
            
        }


        public async Task<bool> RemoveProductFromFavourite(string shopItemId)
        {
            try
            {
                string responds_ = await _cartDataService.DeleteShoppingCartItem(shopItemId, _AppUser);
                //   _productsDataService.SaveFavouriteProducts(FavItems.ToList());
                return responds_.Length == 2;
            }
            catch (Exception)
            {
                return false;
                //throw;//x
            }
           
        }

        public void FireMessage()
        {
            Messenger.Publish(
                          new FavouriteChangedMessage(this)
                          { NewFavList = FavItems.ToList() });//SubHomeViewModel
        }
        private void fillList()
        {
            try
            {
                WishList = new List<ShoppingCart>();
                FavItems = new ObservableCollection<Product>();
                if (shopList == null)
                {
                    return;
                }
                foreach (var item in shopList)
                {
                    if (item.Shopping_cart_type == "Wishlist")
                    {
                        item.Product.ShoppingCartId = item.Id;
                        FavItems.Add(item.Product);
                        WishList.Add(item);
                    }
                   
                }
            }
            catch 
            {

                //throw;//x
            }
        }


        //commands
        public MvxCommand<Product> ShowProductViewCommand
        {
            get
            {
                return new MvxCommand<Product>(OpenProduct);
            }
        }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        private async void OpenProduct(Product pro)
        {
            try
            {
                BaseRepository baseRepo = new BaseRepository();
                string proJson = await baseRepo.SerializeObject(pro);

                ShowViewModel<ProductViewModel>
                    (new { ProductId = pro.Id , productJson = proJson});
            }
            catch
            {

            }
        }
    }
}
