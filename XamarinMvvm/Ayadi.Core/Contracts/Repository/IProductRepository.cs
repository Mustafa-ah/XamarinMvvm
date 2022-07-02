using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetHomeProductsFromAPI(User user);
        Task<List<Product>> GetHomeProducts(User user);

        Task<List<Product>> GetCategoryProducts(int category_id, User user, int page);
        Task<Product> GetProduct(int product_id, User user);
        Task<List<Product>> GetAllProducts(User user, int page);
        Task<List<Product>> GetStoreProducts(string storName, User user);
        Task<List<Product>> SearchProducts(string productName, string categoryId, 
            string storeId, string priceFrom, string PriceTo, User user, int page);

        // filltering
        Task<List<Product_category_mappings>> GetProductsIdsFromCategory(string catId);

        //review 
        Task<List<Review>> GetProductReviews(User user, int productId);
        Task<bool> PostProductReviews(User user, int productId, ReviewItems item);
    }
}
