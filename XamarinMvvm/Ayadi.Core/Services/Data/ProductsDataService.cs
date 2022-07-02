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
    public class ProductsDataService : IProductsDataService
    {
        private readonly IProductRepository _productsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICartDataService _cartDataService;
       // private readonly IUserDataService _userDataService;
        private readonly IStoreDataService _storeDataService; 
        private readonly ISponserRepository _sponserRepository;

        public ProductsDataService(IProductRepository productsRepository, 
            ICategoryRepository categoryRepository,
             ISponserRepository sponserRepository,
             ICartDataService cartDataService,
            IStoreDataService storeDataService)
        {
            this._productsRepository = productsRepository;
            this._categoryRepository = categoryRepository;
            this._cartDataService = cartDataService;
            //this._userDataService = userDataService;
            this._sponserRepository = sponserRepository;
            this._storeDataService = storeDataService;
        }

        public async Task<List<Product>> GetHomeProducts(User user)
        {
            return await _productsRepository.GetHomeProducts(user);
        }

        public async Task<List<Category>> GetHomeCategories(User user)
        {
            return await _categoryRepository.GetHomeCategories(user);
        }

        public async Task<List<Product>> GetCategoryProducts(int category_id, User user, int page)
        {
            return await _productsRepository.GetCategoryProducts(category_id, user, page);
        }

        public async  Task<Product> GetProduct(int product_id, User user)
        {
            return await _productsRepository.GetProduct(product_id, user);
        }


        //public async Task<List<Product>> GetFavouriteProducts()
        //{
        //    return await _favouriteRepository.GetFavouriteProducts();
        //}

        //public void SaveFavouriteProducts(List<Product> FavList)
        //{
        //    _favouriteRepository.SaveFavouriteProducts(FavList);
        //}

        public async Task<List<Product>> GetAllProducts(User user, int page)
        {
            return await _productsRepository.GetAllProducts(user, page);
        }

        public async Task<List<SponserSlider>> GetAllSponsers(User user)
        {
            return await _sponserRepository.GetAllSponsers(user);
        }

        public async Task<List<Product>> SearchProducts(string productName, string categoryId, string storeId,
            string priceFrom, string PriceTo, User user, int page)
        {
            return await _productsRepository.SearchProducts(productName, categoryId,
                storeId, priceFrom, PriceTo, user, page);
        }

        public Task<List<Product_category_mappings>> GetProductsIdsFromCategory(string catId)
        {
            return _productsRepository.GetProductsIdsFromCategory(catId);
        }

        //public async Task<ShoppingCart> PostShoppingCartProducts(ShoppingCart shopList)
        //{
        //    return await _cartDataService.PostShoppingCartItem(shopList);
        //}

        public async Task<Store> GetStoreById(string id, User user)
        {
          return await  _storeDataService.GetStoreById(id, user);
        }

        public async Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList, User user)
        {
            return await _cartDataService.PostShoppingCartItem(shopList, user);
        }

        public async Task<string> DeleteShoppingCartItem(string shopListId, User user)
        {
            return await _cartDataService.DeleteShoppingCartItem(shopListId, user);
        }

        public async Task<List<Review>> GetProductReviews(User user, int productId)
        {
            return await _productsRepository.GetProductReviews(user, productId);
        }

        public async Task<bool> PostProductReviews(User user, int productId, ReviewItems item)
        {
            return await _productsRepository.PostProductReviews(user, productId, item);
        }

        public void SaveShopingCartToLocal()
        {
              _cartDataService.SaveShopingCartToLocal(new List<ShoppingCart>());
        }

        //public async Task<User> GetSavedUser()
        //{
        //    return await _userDataService.GetSavedUser();
        //}

        //public async Task<bool> SaveUserToLocal(User user)
        //{
        //    return await _userDataService.SaveUserToLocal(user);
        //}
    }
}
