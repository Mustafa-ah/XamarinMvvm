using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.ViewModel
{
    public class BaseViewModel : MvxViewModel, IDisposable
    {
        public void Dispose()
        {
            Messenger = null;
        }

        public User BaseAppUser;

        protected IMvxMessenger Messenger;

        public BaseViewModel(IMvxMessenger messenger)
        {
            Messenger = messenger;
        }

        public IMvxLanguageBinder TextSource =>
           new MvxLanguageBinder("", GetType().Name);

        protected async Task ReloadDataAsync()
        {
            try
            {
                await InitializeAsync(); 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected virtual Task InitializeAsync()
        {
            return Task.FromResult(0);
        }

        public ObservableCollection<Product> MainProducts;

        // just for test
        public async Task<bool> PostToFavouritesAsync(Product SelectedProduct)
        {
            Repositories.CartRepository cartRepo = new Repositories.CartRepository();
            ShoppingCart shop_;
            try
            {
                ShoppingCart shop = new ShoppingCart();
                shop.Customer_id = BaseAppUser.Id;// Constants.UserId;//UserId
                shop.Product = SelectedProduct;
                shop.Product_id = int.Parse(SelectedProduct.Id);
                shop.Quantity = 1;
                shop.Shopping_cart_type = Constants.Wish_list;
                shop_ = await cartRepo.PostShoppingCartItem(shop, BaseAppUser);
                if (shop_.Id != null)
                {
                    SelectedProduct.ISInFavourite = true;
                    SelectedProduct.ShoppingCartId = shop_.Id;
                }
                
                return shop_.Id != null;
            }
            catch (Exception)
            {
                return false;
                //throw;//x 
            }
        }

        public async Task<bool> RemoveFavouritesAsync(Product SelectedProduct)
        {
            try
            {
                Repositories.CartRepository cartRepo = new Repositories.CartRepository();
                string deleteRespondse = await cartRepo.DeleteShoppingCartItem(SelectedProduct.ShoppingCartId, BaseAppUser);
                bool deleted_ = deleteRespondse.Length == 2;
                if (deleted_)
                {
                    SelectedProduct.ISInFavourite = false;
                    SelectedProduct.ShoppingCartId = null;
                }
                
                return deleted_;

            }
            catch (Exception)
            {
                return false;
                //throw;//x 
            }
        }
    }
}
