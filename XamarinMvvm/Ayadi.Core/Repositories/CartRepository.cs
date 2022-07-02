using Ayadi.Core.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using Akavache;
using System.Reactive.Linq;
using Newtonsoft.Json.Linq;

namespace Ayadi.Core.Repositories
{
    public class CartRepository : BaseRepository, ICartRepository
    {
        private List<ShoppingCart> _activeShoppingCart;
        public CartRepository ()
        {
            _activeShoppingCart = new List<ShoppingCart>();
        }
        //locla
        public async void SaveCart(List<Store> Cartls)
        {
            try
            {
                // string JsonList = Newtonsoft.Json.JsonConvert.SerializeObject(Cartls);
                // await BlobCache.LocalMachine.InsertObject<List<BaseModel>>("Cartlso", Cartls);
                await BlobCache.LocalMachine.InsertObject<List<Store>>("CartProducts", Cartls);
            }
            catch (Exception)
            {
                //throw;//x
            }
        }

        public async Task<List<Store>> GetSavedCart()
        {
            List<Store> Cartls = new List<Store>();
            try
            {
                // string JsonList =  await BlobCache.LocalMachine.GetObject<string>("CartJson");
                // Cartls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BaseModel>>(JsonList);
                //Cartls =  await BlobCache.LocalMachine.GetObject<List<BaseModel>>("Cartlso");
                Cartls = await BlobCache.LocalMachine.GetObject<List<Store>>("CartProducts");

            }
            catch (KeyNotFoundException ke)
            {
                return Cartls;
            }
            catch (Exception ex)
            {
                string ff = ex.Message;
                //throw;//x
            }
            return Cartls;
        }

        public async Task SaveShopingCartToLocal(List<ShoppingCart> shopList)
        {
            try
            {
                if (shopList != null)
                {
                    //_activeShoppingCart = shopList;
                    await BlobCache.LocalMachine.InsertObject<List<ShoppingCart>>(Constants.ShoppingCartrKey, _activeShoppingCart);
                    System.Diagnostics.Debug.WriteLine($".................. shopList Saved {shopList.Count}.................");
                }
            }
            catch
            {

            }
        }

        public async Task<List<ShoppingCart>> GetShopingCartFromLocal()
        {
            try
            {
                //return _activeShoppingCart;
               // await Task.Delay(5000);
                return await BlobCache.LocalMachine.GetObject<List<ShoppingCart>>(Constants.ShoppingCartrKey);
            }
            catch
            {
                return new List<ShoppingCart>();
            }
        }


        // online
        public async Task<string> DeleteShoppingCartItem(string shopListId, User user)
        {
            string response = "";
            try
            {
                //List<ShoppingCart> LocalShopingCarts = await GetShopingCartFromLocal();
                //ShoppingCart shopToDelete = LocalShopingCarts.Find(s => s.Id == shopListId);
                //if (shopToDelete != null)
                //{
                //    LocalShopingCarts.Remove(shopToDelete);
                //    await SaveShopingCartToLocal(LocalShopingCarts);
                //}
                ShoppingCart shopToDelete = _activeShoppingCart.Find(s => s.Id == shopListId);
                if (shopToDelete != null)
                {
                    _activeShoppingCart.Remove(shopToDelete);
                }
                // string tokenz = await GetAccessToken();
                response = await DeleteAsync(user.AccessToken, "/api/shopping_cart_items/" + shopListId);
                
               
               
            }
            catch (Exception)
            {
                response = null;
                //throw;//x
            }
            return response;
        }

        public async Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList, User user)
        {
            System.Diagnostics.Debug.WriteLine($".................. product {shopList.Product.Name}.................");
            ShoppingCart shopCart = new ShoppingCart();
            try
            {
                //List<ShoppingCart> LocalShopingCarts = await GetShopingCartFromLocal();
                //LocalShopingCarts.Add(shopList);
                //await SaveShopingCartToLocal(LocalShopingCarts);

                _activeShoppingCart.Add(shopList);

                string listJson = await SerializeObject(shopList);
                string newStr = "{\"shopping_cart_item\":" + listJson + "}";
                string respons = await PostStringAsync(user.AccessToken, "/api/shopping_cart_items", newStr);
                System.Diagnostics.Debug.WriteLine(user.AccessToken);
                System.Diagnostics.Debug.WriteLine(respons);
                if (!string.IsNullOrEmpty(respons))
                {
                    shopCart = await GetShoppingCartFromJson(respons);
                    if (shopCart.Id != null)
                    {
                        _activeShoppingCart.Find(s => s == shopList).Id = shopCart.Id;
                        //LocalShopingCarts.Find(s => s == shopList).Id = shopCart.Id;
                        //await SaveShopingCartToLocal(LocalShopingCarts);

                    }
                    else
                    {
                        _activeShoppingCart.Remove(shopList);
                        //LocalShopingCarts.Remove(shopList);
                        //await SaveShopingCartToLocal(LocalShopingCarts);
                    }
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return shopCart;
        }

        public async Task<ShoppingCart> PutShoppingCartItem(ShoppingCart shopList, User user)
        {
            ShoppingCart shopCart = new ShoppingCart();
            if (shopList.Id == null)
            {
                return shopCart;
            }
            try
            {
                //List<ShoppingCart> LocalShopingCarts = await GetShopingCartFromLocal();
                //LocalShopingCarts.Find(s => s.Id == shopList.Id ).Quantity = shopList.Quantity;
                //await SaveShopingCartToLocal(LocalShopingCarts);
                //  string tokenz = await GetAccessToken();

                _activeShoppingCart.Find(s => s.Id == shopList.Id).Quantity = shopList.Quantity;

                string listJson = await SerializeObject(shopList);
                string newStr = "{\"shopping_cart_item\":" + listJson + "}";
                string respons = await PutStringAsync(user.AccessToken, "/api/shopping_cart_items/" + shopList.Id, newStr);

                if (!string.IsNullOrEmpty(respons))
                {
                    shopCart = await GetShoppingCartFromJson(respons);
                    System.Diagnostics.Debug.WriteLine($".................. product {shopCart.Id}.................");
                    if (shopCart.Id == null)
                    {
                        _activeShoppingCart.Find(s => s.Id == shopList.Id).Quantity = shopCart.Quantity -1;
                        //LocalShopingCarts.Find(s => s.Id == shopList.Id).Quantity = shopCart.Quantity -1;
                        //await SaveShopingCartToLocal(LocalShopingCarts);
                    }
                }
            }
            catch (Exception ex)
            {
                string ff = ex.Message;
                //throw;//x
            }
            return shopCart;
        }

        public async Task<List<ShoppingCart>> GetShoppingCartItemsFromAPI(User user)
        {
            List<ShoppingCart> ls = new List<ShoppingCart>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new ShoppingCart());

                // exclude customer 
                // string shortFields = ObjFields.Replace("ObjFields", ",customer");

                string url_ = $"/api/shopping_cart_items/{user.Id}?{ObjFields}&languageId={user.LangID}";

                //string json = await GetStringAsync("/api/shopping_cart_items/" + userId , tokenz);
                string json = await GetStringAsync(url_, user.AccessToken);
                ls = await GetShoppingCartsFromJson(json);

            }
            catch (Exception)
            {

                //throw;//x
            }
            return ls;
        }

        public async Task<List<ShoppingCart>> GetShoppingCartItems(User user)
        {
            List<ShoppingCart> ls = new List<ShoppingCart>();
            try
            {
                //string ObjFields = await GetURLFieldsNames(new ShoppingCart());

                //// exclude customer 
                //// string shortFields = ObjFields.Replace("ObjFields", ",customer");

                //string url_ = $"/api/shopping_cart_items/{user.Id}?{ObjFields}&languageId={user.LangID}";

                ////string json = await GetStringAsync("/api/shopping_cart_items/" + userId , tokenz);
                //string json = await GetStringAsync(url_, user.AccessToken);
                //ls = await GetShoppingCartsFromJson(json);
                ls = await GetShopingCartFromLocal();
            }
            catch (Exception)
            {

                //throw;//x
            }
            return ls;
        }
        
        private async Task<ShoppingCart> GetShoppingCartFromJson(string cartJson)
        {

            ShoppingCart cart = new ShoppingCart();
            if (cartJson == null)
            {
                return cart;
            }
            try
            {
                JObject jsonObject_ = JObject.Parse(cartJson);
                if (jsonObject_["errors"] == null)
                {
                    IList<JToken> _itemList = jsonObject_["shopping_carts"].Children().ToList();
                    cart = await DeserializeTObject<ShoppingCart>(_itemList[0].ToString());
                }
            }
            catch 
            {

            }
            return cart;
        }

        private async Task<List<ShoppingCart>> GetShoppingCartsFromJson(string cartJson)
        {
            List<ShoppingCart> cart = new List<ShoppingCart>();
            try
            {
                JObject jsonObject_ = JObject.Parse(cartJson);
                if (jsonObject_["shopping_carts"] != null)
                {
                    IList<JToken> _itemList = jsonObject_["shopping_carts"].Children().ToList();
                    foreach (JToken item in _itemList)
                    {
                        ShoppingCart cartItem_ = await DeserializeTObject<ShoppingCart>(item.ToString());
                        if (cartItem_ != null)
                        {
                            if (cartItem_.Product.Localized_Product.Count > 0)
                            {
                                cartItem_.Product.Name = cartItem_.Product.Localized_Product[0].Localized_name;
                            }
                            cart.Add(cartItem_);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;//x
            }
            return cart;
        }

        public async Task<List<ShoppingCart>> GetShoppingCartItems_Simplified(User user)
        {
            List<ShoppingCart> ls = new List<ShoppingCart>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new ShoppingCart());

                // exclude product 
                 string shortFields = ObjFields.Replace(",product,", ",");

                string url_ = $"/api/shopping_cart_items/{user.Id}?{shortFields}&languageId={user.LangID}";
                //string json = await GetStringAsync("/api/shopping_cart_items/" + userId , tokenz);
                string json = await GetStringAsync(url_, user.AccessToken);
                ls = await GetShoppingCartsFromJson(json);
            }
            catch (Exception)
            {

                //throw;//x
            }
            return ls;
        }

        public void SetActiveShoppingList(List<ShoppingCart> shopList)
        {
            _activeShoppingCart = shopList;
        }

        public List<ShoppingCart> GetActiveShoppingList()
        {
            return _activeShoppingCart;
        }
    }
}
