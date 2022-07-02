using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface IFavouriteDataService
    {
        //online
        Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList);
        Task<string> DeleteShoppingCartItem(string shopListId);
        Task<List<ShoppingCart>> GetShoppingCartItems(string userId);
    }
}
