using Ayadi.Core.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Akavache;
using System.Reactive.Linq;
using MvvmCross.Platform;

namespace Ayadi.Core.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        ICartRepository _cartRepo;
        public ProductRepository(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<List<Product>> GetCategoryProducts(int category_id, User user, int page)
        {
            List<Product> proList = new List<Product>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new Product());
                string url_ = $"/api/products?page={page}&category_id={category_id}&{ObjFields}&languageId={user.LangID}";
                //proList = await GetProductList("/api/products?category_id=" + category_id, user);
                proList = await GetProductList(url_, user);
            }
            catch (Exception)
            {

            }
            return proList;
        }

        List<ShoppingCart> SavedFavouriteProducts;


        public async Task<List<Product>> GetHomeProductsFromAPI(User user)
        {
            List<Product> proList = new List<Product>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new Product());
                string url_ = $"/api/products?limit=12&languageId={user.LangID}&{ObjFields}";
                proList = await GetProductList(url_, user);

                // mark favourite product
                //List<Product> savedList = await GetHomeProductsFromLocal();
                //if (savedList != null)
                //{

                //}
                SaveHomeProductsToLocal(proList);
            }
            catch (Exception)
            {

            }
            return proList;

           
            //return proList;
        }

        public async Task<List<Product>> GetHomeProducts(User user)
        {
            List<Product> proList = new List<Product>();
            try
            {
                List<Product> savedList = await GetHomeProductsFromLocal();
                if (savedList == null || savedList.Count == 0)
                {
                    proList = await GetHomeProductsFromAPI(user);
                }
                else
                {
                    proList = savedList;
                    SavedFavouriteProducts = GetFavouritProducts(user);
                    foreach (Product Prod in savedList)
                    {
                        SetFavouriteProducts(Prod);
                    }
                    proList = savedList;
                }
            }
            catch (Exception)
            {

            }
            return proList;
        }

        public async Task<Product> GetProduct(int product_id, User user)
        {
            Product pro;
            try
            {
                string ObjFields = await GetURLFieldsNames(new Product());
                string url_ = $"/api/products/{product_id}?{ObjFields}&languageId={user.LangID}";
                //List<Product> list = await GetProductList("/api/products/" + product_id, user);
                List<Product> list = await GetProductList(url_, user);
                pro = list[0];
            }
            catch (Exception)
            {
                pro = new Product();
            }
            return pro;
        }
        
        private List<ShoppingCart> GetFavouritProducts(User user)
        {
            
           // List<ShoppingCart> sh_ = new List<ShoppingCart>();
            try
            {
                ICartRepository cartrepo = Mvx.Resolve<ICartRepository>();
                return cartrepo.GetActiveShoppingList().FindAll(s => s.Shopping_cart_type == Constants.Wish_list);
                //List<ShoppingCart> sh_All = await _cartRepo.GetShoppingCartItems(user);
                //List<ShoppingCart> sh_All = await _cartRepo.GetShoppingCartItems(user);
                //foreach (var item_ in sh_All)
                //{
                //    if (item_.Shopping_cart_type == Constants.Wish_list)
                //    {
                //        sh_.Add(item_);
                //    }
                //}
            }
            catch (Exception)
            {
                return new List<ShoppingCart>();
              //  ClearUserCash();
                //throw;//x
            }
           // return sh_;
        }

        private async Task<List<Product>> GetProductList(string resource, User user)
        {
            List<Product> lll = new List<Product>();
           // FavouriteRepository favRepo = new FavouriteRepository();
            try
            {
                SavedFavouriteProducts = GetFavouritProducts(user);

                string jsonString = await GetStringAsync(resource, user.AccessToken);

                JObject jsonObject_ = JObject.Parse(jsonString);

                if (jsonObject_["products"] == null)
                {
                    return lll;
                }
                IList<JToken> _itemList = jsonObject_["products"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    Product pro = await DeserializeTObject<Product>(itemList.ToString());
                    if (pro != null)
                    {
                        if (pro.Localized_Product.Count > 0)
                        {
                            pro.Name = pro.Localized_Product[0].Localized_name;
                            pro.Short_description = pro.Localized_Product[0].Localized_shortDescription;
                            //if (!string.IsNullOrEmpty(pro.Localized_Product[0].Localized_name))
                            //{
                            //    pro.Name = pro.Localized_Product[0].Localized_name;
                            //}
                            //if (!string.IsNullOrEmpty(pro.Localized_Product[0].Localized_shortDescription))
                            //{
                            //    pro.Short_description = pro.Localized_Product[0].Localized_shortDescription;
                            //}
                        }
                        SetFavouriteProducts(pro);
                        lll.Add(pro);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                // result = new T();
                //throw;//x
            }


            return lll;

        }

        private void SetFavouriteProducts(Product pro)
        {
            try
            {
                ShoppingCart sho = SavedFavouriteProducts.Find(s => s.Product_id.ToString() == pro.Id);
                if (sho != null)
                {
                    pro.ISInFavourite = true;
                    pro.ShoppingCartId = sho.Id;
                }
                else
                {
                    pro.ISInFavourite = false;
                }
                //if (SavedFavouriteProducts.Contains<Product>(pro))
                //{
                //    string shopCartId_ = SavedFavouriteProducts.Find(s => s.)
                //    pro.ISInFavourite = true;
                //}
                // List<Product> SavedFavouriteProducts = await _productsDataService.GetFavouriteProducts();
                //foreach (Product itemProduct in SavedFavouriteProducts)
                //{
                //    if (SavedFavouriteProducts.Contains<Product>(pro))
                //    {
                //        pro.ISInFavourite = true;
                //    }
                //}

            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        public async Task<List<Product>> GetAllProducts(User user, int page)
        {
            List<Product> proList = new List<Product>();
            try
            {
                 string ObjFields = await GetURLFieldsNames(new Product());
                string url_ = $"/api/products?limit=12&page={page}&languageId={user.LangID}&{ObjFields}";
               // proList = await GetProductList("/api/products", user);
                proList = await GetProductList(url_, user);
            }
            catch (Exception)
            {

            }
            return proList;
        }

        public async Task<List<Product>> GetStoreProducts(string storName, User user)
        {
            List<Product> proList = new List<Product>();
            try
            {//api/products?languageId=1&vendor_name=wwwww
                string ObjFields = await GetURLFieldsNames(new Product());
                string url_ = $"/api/products?{ObjFields}&languageId={user.LangID}&vendor_id={storName}";
               // proList = await GetProductList("/api/products?vendor_name=" + storName, user);
                proList = await GetProductList(url_, user);
            }
            catch (Exception)
            {

            }
            return proList;
        }

        public async Task<List<Product>> SearchProducts(string productName, string categoryId, string storeId,
            string priceFrom, string PriceTo, User user, int page)
        {
            List<Product> proList = new List<Product>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new Product());
                string priceMin = priceFrom == null ? "" : string.Format("price_min={0}&", priceFrom);
                string priceMax = PriceTo == null ? "" : string.Format("price_max={0}&", PriceTo);
                string productName_ = productName == null ? "" : string.Format("product_name={0}&", productName);
                string vendorId_ = storeId == null ? "" : string.Format("vendor_id={0}&", storeId);
                string catId_ = categoryId == null ? "" : string.Format("category_id={0}&", categoryId);

                string langId_ = "&languageId=" + user.LangID;

                string Url_ = string.Format("/api/products?page={0}&{1}{2}{3}{4}{5}{6}{7}",page, priceMin, priceMax, 
                    productName_, vendorId_, catId_, ObjFields, langId_);

                //char lastChar = Url_[Url_.Length - 1];
                //if (lastChar == '?' || lastChar == '&')
                //{
                //    Url_ = Url_.Remove(Url_.Length - 1);
                //}

                //Url_ = Url_ + "?languageId=" + user.LangID;Parameters
               // string token = await GetAccessToken();

                proList = await GetProductList(Url_, user);

                //proList = await GetProductList(string.Format("/api/products?price_min={0}&price_max={1}&product_name={2}&vendor_id={3}&category_id={4}",
                //    priceFrom,PriceTo,productName,storeId,categoryId), token);


            }
            catch (Exception)
            {
                throw;//
            }
            return proList;
        }

        public async Task<List<Product_category_mappings>> GetProductsIdsFromCategory(string catId)
        {
            List<Product_category_mappings> lst = new List<Product_category_mappings>();
            try
            {
                string token = await GetAccessToken();
                string jsonString = await GetStringAsync("/api/product_category_mappings?category_id=" + catId, token);

                JObject jsonObject_ = JObject.Parse(jsonString);
                IList<JToken> _itemList = jsonObject_["product_category_mappings"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    Product_category_mappings pro =await  DeserializeTObject<Product_category_mappings>(itemList.ToString());
                    if (pro != null)
                    {
                        lst.Add(pro);
                    }
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return lst;
        }

        //public async Task<ShoppingCart> PostShoppingCartProducts(ShoppingCart shopList)
        //{
        //    ShoppingCart shopCart = new ShoppingCart();
        //    try
        //    {
        //        string tokenz = await GetAccessToken();
        //        string listJson = SerializeObject(shopList);
        //        string newStr = "{\"shopping_cart_item\":" + listJson + "}";
        //        string respons = await PostStringAsync(tokenz, "/api/shopping_cart_items", newStr);

        //        if (!string.IsNullOrEmpty(respons))
        //        {
        //            shopCart = await GetShoppingCartFromJson(respons);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        //throw;//x
        //    }
        //    return shopCart;
        //}

        //private async Task<ShoppingCart> GetShoppingCartFromJson(string cartJson)
        //{
        //    ShoppingCart cart = new ShoppingCart();
        //    try
        //    {
        //        await Task.Factory.StartNew(() =>
        //        {
        //            JObject jsonObject_ = JObject.Parse(cartJson);
        //            if (jsonObject_["errors"] != null)
        //            {
        //                cart = null;
        //                return;
        //            }
        //            IList<JToken> _itemList = jsonObject_["shopping_carts"].Children().ToList();
        //            cart = DeserializeTObject<ShoppingCart>(_itemList[0].ToString());
        //        }
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        cart = null;
        //        //throw;//x
        //    }
        //    return cart;
        //}


        // local staff

        private async Task<List<Product>> GetHomeProductsFromLocal()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<List<Product>>(Constants.HomeProductsKey);
            }
            catch (KeyNotFoundException ke)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
                //throw;//x
            }
        }

        private async void SaveHomeProductsToLocal(List<Product> ls)
        {
            try
            {
                 await BlobCache.LocalMachine.InsertObject<List<Product>>(Constants.HomeProductsKey, ls);
            }
            catch 
            {
                //throw;//x
            }
        }

        // there is no fields Parameter
        public async Task<List<Review>> GetProductReviews(User user, int productId)
        {
            List<Review> lst = new List<Review>();
            List<Review> Rlst = new List<Review>();
            try
            {
                string url_ = $"/api/product/{productId}/review";
                string jsonString = await GetStringAsync(url_, user.AccessToken);

                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["reviews"] == null)
                {
                    return lst;
                }
                IList<JToken> _itemList = jsonObject_["reviews"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    Review rev = await DeserializeTObject<Review>(itemList.ToString());
                    if (rev != null)
                    {
                        lst.Add(rev);
                    }
                    //if (itemList["Items"] != null)
                    //{
                    //    IList<JToken> _RemItemList = jsonObject_["Items"].Children().ToList();

                    //    Review rev = await DeserializeTObject<Review>(itemList.ToString());
                    //    if (rev != null)
                    //    {
                    //        lst.Add(rev);
                    //    }
                    //}

                }

                foreach (Review Ritem in lst)
                {
                    foreach (ReviewItems Rvitem in Ritem.Items)
                    {
                        Review R = new Review();
                        R.Id = Ritem.Id;
                        R.ProductId = Ritem.ProductId;
                        R.Item = Rvitem;
                        Rlst.Add(R);
                    }
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return Rlst;
        }

        public async Task<bool> PostProductReviews(User user, int productId, ReviewItems item)
        {
            try
            {
                string url_ = $"/api/product/{productId}/review";
                string jsonString = await SerializeObject(item);
                string respond = await PostStringAsync(user.AccessToken, url_, jsonString);

                return !respond.ToLower().Contains("errors");
            }
            catch 
            {
                return false;
            }
        }
    }
}
