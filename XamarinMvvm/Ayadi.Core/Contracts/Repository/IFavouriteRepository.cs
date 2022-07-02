using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface IFavouriteRepository
    {
        //local
        Task<List<Product>> GetFavouriteProducts();
        void SaveFavouriteProducts(List<Product> FavList);

        //online
        Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList);
        Task<string> DeleteShoppingCartItem(string shopListId);
        Task<List<ShoppingCart>> GetShoppingCartItems(string userId);
    }
}
