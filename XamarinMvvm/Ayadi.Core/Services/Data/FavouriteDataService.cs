using Ayadi.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using Ayadi.Core.Contracts.Repository;

namespace Ayadi.Core.Services.Data
{
    public class FavouriteDataService : IFavouriteDataService
    {
        //never user till now!!

        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteDataService(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }

        public async Task<string> DeleteShoppingCartItem(string shopListId)
        {
            return await _favouriteRepository.DeleteShoppingCartItem(shopListId);
        }

        public async Task<List<ShoppingCart>> GetShoppingCartItems(string userId)
        {
            return await _favouriteRepository.GetShoppingCartItems(userId);
        }

        public async Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList)
        {
            return await _favouriteRepository.PostShoppingCartItem(shopList);
        }
    }
}
