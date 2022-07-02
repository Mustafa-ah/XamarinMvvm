using Ayadi.Core.Contracts.Repository;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Messages;
using Ayadi.Core.Repositories;
using Ayadi.Core.ViewModel;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Product : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Vendor_id { get; set; }
        public string Short_description { get; set; }


        
        public int Quantity { get; set; }
        public string Se_name { get; set; }

        // for sorting
        public string Updated_on_utc { get; set; }

        public List<Imager> Images { get; set; }

        public Product()
        {
            Images = new List<Imager>();
            Localized_Product = new List<LocalizedNames>();
        }

        //[JsonIgnore]
        public string ProductImage
        {
            get
            {
                if (Images.Count != 0)
                {
                    return Images[0].Src;
                }
                else
                {
                    return "";
                }
            }
        }
        public string ShoppingCartId { get; set; }
        public string vendor_name { get; set; }
       // public bool ISInFavourite { get; set; }

        private bool _isInFavourite;
        public bool ISInFavourite
        {
            get { return _isInFavourite; }
            set { _isInFavourite = value; RaisePropertyChanged(() => ISInFavourite); }
        }

        public float Approved_rating_sum { get; set; }
        public int Approved_total_reviews { get; set; }

        public List<LocalizedNames> Localized_Product { get; set; }

        //[JsonIgnore]
        //protected IMvxMessenger Messenger;
        //[JsonIgnore]
        //private MvxCommand _PlusCommand;
        //[JsonIgnore]
        //public IMvxCommand PlusCommand
        //{
        //    get
        //    {
        //        _PlusCommand = _PlusCommand ?? new MvxCommand(() => UpdateMessage());
        //        return _PlusCommand;
        //    }
        //}
        //private void UpdateMessage()
        //{
        //    if (Messenger == null)
        //    {
        //        Messenger = Mvx.Resolve<IMvxMessenger>();
        //    }
        //    Quantity = Quantity + 1;
        //    Messenger.Publish(new UpdateProductMessage(this, this));
        //}


        [JsonIgnore]
        public IMvxAsyncCommand AddToFavouritCommand
        {
            get
            {
                return new MvxAsyncCommand(() => FavouriteProduct(this));
            }
        }

        [JsonIgnore]
        public User _AppUser { get; set; }

        public async Task<bool> FavouriteProduct(Product SelectedProduct)
        {
            if (_AppUser == null)
            {
                UserRepository usRepo = new UserRepository();
                _AppUser = await usRepo.GetSavedUser();
            }
            if (this.ISInFavourite)
            {
               return await RemoveFavouritesAsync(SelectedProduct, _AppUser);
            }
            else
            {
                return await PostToFavouritesAsync(SelectedProduct, _AppUser);
            }
        }

        public async Task<bool> PostToFavouritesAsync(Product SelectedProduct, User user)
        {
            //CartRepository cartRepo = new CartRepository();
            ICartRepository cartRepo = Mvx.Resolve<ICartRepository>();

            ShoppingCart shop_;
            try
            {
                ShoppingCart shop = new ShoppingCart();
                shop.Customer_id = user.Id;// BaseAppUser.Id;// Constants.UserId;//UserId
                shop.Product = SelectedProduct;
                shop.Product_id = int.Parse(SelectedProduct.Id);
                shop.Quantity = 1;
                shop.Id = SelectedProduct.ShoppingCartId;
                shop.Shopping_cart_type = Constants.Wish_list;
                SelectedProduct.ISInFavourite = true;
                shop_ = await cartRepo.PostShoppingCartItem(shop, user);

                //if (shop_.Id != null)
                //{
                //    SelectedProduct.ISInFavourite = true;
                //    SelectedProduct.ShoppingCartId = shop_.Id;
                //}
                //else
                //{
                //    SelectedProduct.ISInFavourite = false;
                //}
                return shop_.Id != null;
            }
            catch (Exception)
            {
                return false;
                //throw;//x 
            }
        }

        public async Task<bool> RemoveFavouritesAsync(Product SelectedProduct, User user)
        {
            try
            {
                //CartRepository cartRepo = new CartRepository();
                ICartRepository cartRepo = Mvx.Resolve<ICartRepository>();
                SelectedProduct.ISInFavourite = false;
                string deleteRespondse = await cartRepo.DeleteShoppingCartItem(SelectedProduct.ShoppingCartId, user);
                bool deleted_ = deleteRespondse.Length == 2;
                if (deleted_)
                {
                    SelectedProduct.ISInFavourite = false;
                    SelectedProduct.ShoppingCartId = null;
                }
                else
                {
                    SelectedProduct.ISInFavourite = true;
                }
                return deleted_;

            }
            catch (Exception)
            {
                return false;
                //throw;//x 
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Product)
            {
                Product outProduct = obj as Product;
                return this.Id == outProduct.Id;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
