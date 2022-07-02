using Ayadi.Core.Contracts.Repository;
using System;
using Akavache;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using Newtonsoft.Json.Linq;

namespace Ayadi.Core.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        
        public async Task<Order> GetSavedOrder()
        {
            try
            {
               // return await BlobCache.LocalMachine.GetObject<Order>("order");
                return await BlobCache.InMemory.GetObject<Order>("order");
            }
            catch (KeyNotFoundException ke)
            {
                return new Order();
            }
            catch (Exception)
            {
                return new Order();
                //throw;//x
            }
        }

        public async Task<Order> PostOrder(Order order, User user)
        {
            Order shopCart = new Order();
            try
            {
               // string tokenz = await GetAccessToken();
                string listJson = await SerializeObject(order);
                string newStr = "{\"order\":" + listJson + "}";
                string respons = await PostStringAsync(user.AccessToken, "/api/orders", newStr);

                if (!string.IsNullOrEmpty(respons))
                {
                    List<Order> lll = await GetOrdersList(respons);
                    if (lll.Count > 0)
                    {
                        shopCart = lll[0];
                    }
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return shopCart;
        }

        public async Task<bool> SaveOrder(Order order)
        {
            try
            {
                //await BlobCache.LocalMachine.InsertObject<Order>("order", order);
                await BlobCache.InMemory.InsertObject<Order>("order", order);
                return true;
            }
            catch (Exception)
            {
                return false;
                //throw;//x
            }
        }

        public async Task<string> SerializeeOrder(Order orderJson)
        {
            return await SerializeObject(orderJson);
           // string json = "";
           //await Task.Factory.StartNew(() => json = await SerializeObject(orderJson));
           // return json;
        }

        public async Task<Order> DeserializeOrder(string orderJson)
        {
            return await DeserializeTObject<Order>(orderJson);
        }

        public async Task<List<Order>> GetUserOrders(User user)
        {
            List<Order> lll = new List<Order>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new Order());
                string shortFields = ObjFields.Replace(",customer,billing_address,shipping_address,order_items,payment_method_system_name,shipping_method,shipping_rate_computation_method_system_name,", ",");

               // string url_ = $"/api/orders/customer/{user.Id}";
                string url_ = $"/api/orders?{shortFields}&customer_id={user.Id}&languageId={user.LangID}";
                string jsonString = await GetStringAsync(url_, user.AccessToken);
                lll = await GetOrdersList(jsonString);
            }
            catch (Exception)
            {

                //throw;//x
            }
            return lll;
        }

        private async Task<List<Order>> GetOrdersList(string jsonString)
        {
            List<Order> lll = new List<Order>();
            // FavouriteRepository favRepo = new FavouriteRepository();
            try
            {
                
                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["orders"] == null)
                {
                    return lll;
                }
                IList<JToken> _itemList = jsonObject_["orders"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    Order pro = await DeserializeTObject<Order>(itemList.ToString());
                    if (pro != null)
                    {
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

        public async Task<List<PaymentMethod>> GetPaymentMethods(User user)
        {
            List<PaymentMethod> lst = new List<PaymentMethod>();
            try
            {
                string url_ = "/api/paymentMethod";
                string jsonString = await GetStringAsync(url_, user.AccessToken);
                lst = await GetPaymenMethodsList(jsonString);
            }
            catch (Exception)
            {

                //throw;//x
            }
            return lst;
        }

        private async Task<List<PaymentMethod>> GetPaymenMethodsList(string jsonString)
        {
            List<PaymentMethod> lll = new List<PaymentMethod>();
            // FavouriteRepository favRepo = new FavouriteRepository();
            try
            {

                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["paymentMethods"] == null)
                {
                    return lll;
                }
                IList<JToken> _itemList = jsonObject_["paymentMethods"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    PaymentMethod pro = await DeserializeTObject<PaymentMethod>(itemList.ToString());
                    if (pro != null)
                    {
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
    }
}
