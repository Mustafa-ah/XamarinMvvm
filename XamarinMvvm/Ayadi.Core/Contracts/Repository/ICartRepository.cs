using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface ICartRepository
    {
        //ofline
        void SaveCart(List<Store> Cartls);
        Task<List<Store>> GetSavedCart();

        Task SaveShopingCartToLocal(List<ShoppingCart> shopList);
        Task<List<ShoppingCart>> GetShopingCartFromLocal();

        void SetActiveShoppingList(List<ShoppingCart> shopList);
        List<ShoppingCart> GetActiveShoppingList();

        //online
        Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList ,User user);
        Task<ShoppingCart> PutShoppingCartItem(ShoppingCart shopList, User user);
        Task<List<ShoppingCart>> GetShoppingCartItems(User user);
        Task<List<ShoppingCart>> GetShoppingCartItemsFromAPI(User user);

        // just for home
        Task<List<ShoppingCart>> GetShoppingCartItems_Simplified(User user);
        Task<string> DeleteShoppingCartItem(string shopListId, User user);
    }
}
