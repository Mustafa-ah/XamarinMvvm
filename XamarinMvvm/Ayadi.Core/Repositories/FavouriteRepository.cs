using Akavache;
using System.Reactive.Linq;
using Ayadi.Core.Contracts.Repository;
using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ayadi.Core.Repositories
{
    public class FavouriteRepository : BaseRepository, IFavouriteRepository
    {
        // never used till now

      //  private readonly ICartRepository _cartRepository; 

        //public FavouriteRepository(ICartRepository catRepository)
        //{
        //    _cartRepository = catRepository;
        //}

        public async Task<List<Product>> GetFavouriteProducts()
        {
            List<Product> lst = new List<Product>();
            try
            {
                lst = await BlobCache.LocalMachine.GetObject<List<Product>>("FavList");
            }
            catch (KeyNotFoundException ke)
            {
                return lst;
            }
            catch (Exception ex)
            {
                //throw;//x
            }
            return lst;
        }

        public async void SaveFavouriteProducts(List<Product> FavList)
        {
            try
            {
                await BlobCache.LocalMachine.InsertObject<List<Product>>("FavList", FavList);
            }
            catch (Exception ex)
            {
                string dd = ex.Message;
                //throw;//x
            }
        }

        // online

        public async Task<string> DeleteShoppingCartItem(string shopListId)
        {
            //return await _cartRepository.DeleteShoppingCartItem(shopListId);
            throw new NotImplementedException();
        }

     
        public Task<List<ShoppingCart>> GetShoppingCartItems(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList)
        {
            throw new NotImplementedException();
        }

       


    }
}
