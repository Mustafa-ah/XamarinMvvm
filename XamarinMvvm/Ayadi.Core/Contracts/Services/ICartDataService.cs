using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface ICartDataService
    {
        // ofline
        void SaveCart(List<BaseModel> Cartls);
        Task<List<BaseModel>> GetSavedCart();

        void SaveShopingCartToLocal(List<ShoppingCart> shopList);
        Task<List<ShoppingCart>> GetShopingCartFromLocal();

        List<ShoppingCart> GetActiveShoppingList();

        //online
        Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList, User user);
        Task<ShoppingCart> PutShoppingCartItem(ShoppingCart shopList, User user);
        Task<string> DeleteShoppingCartItem(string shopListId, User user);
        Task<List<ShoppingCart>> GetShoppingCartItems(User user);
        Task<List<ShoppingCart>> GetShoppingCartItemsFromAPI(User user);

        //for store name
        Task<List<Store>> GetStorsNamesByIds(int[] ids, User user);

        // just for home
        Task<List<ShoppingCart>> GetShoppingCartItems_Simplified(User user);

        //order
        Task<bool> SaveOrder(Order order);
        Task<string> SerializeeOrder(Order orderJson);
    }
}
