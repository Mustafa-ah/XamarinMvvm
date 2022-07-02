using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface IProductsDataService
    {
        Task<List<Product>> GetHomeProducts(User user);
        Task<List<Category>> GetHomeCategories(User user);
        Task<List<Product>> GetCategoryProducts(int category_id, User user, int page);
        Task<Product> GetProduct(int product_id, User user);
        Task<List<Product>> GetAllProducts(User user, int page);



        // cart repo
        Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList, User user);
        Task<string> DeleteShoppingCartItem(string shopListId, User user);
        void SaveShopingCartToLocal();
        //fav
        // Task<List<Product>> GetFavouriteProducts();
        //  void SaveFavouriteProducts(List<Product> FavList);

        //sponsers
        Task<List<SponserSlider>> GetAllSponsers(User user);

        // search
        Task<List<Product>> SearchProducts(string productName, string categoryId, string storeId, 
            string priceFrom, string PriceTo, User user, int page);

        // fillter
        Task<List<Product_category_mappings>> GetProductsIdsFromCategory(string catId);
        
        // store
        Task<Store> GetStoreById(string id, User user);

        //review 
        Task<List<Review>> GetProductReviews(User user, int productId);
        Task<bool> PostProductReviews(User user, int productId, ReviewItems item);
    }
}
