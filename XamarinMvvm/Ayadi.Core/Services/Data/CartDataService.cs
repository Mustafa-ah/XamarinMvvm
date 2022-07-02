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
    public class CartDataService : ICartDataService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IStoreDataService _storeDataService;
        private readonly IOrderDataService _orderDataService;

        public CartDataService(ICartRepository cartRepository, 
            IStoreDataService storeDataService,
            IOrderDataService orderDataService)
        {
            _cartRepository = cartRepository;
            _storeDataService = storeDataService;
            _orderDataService = orderDataService;
        }

        public async Task<string> DeleteShoppingCartItem(string shopListId, User user)
        {
           return await _cartRepository.DeleteShoppingCartItem(shopListId, user);
        }

        public async Task<List<BaseModel>> GetSavedCart()
        {
            return await getProducts();
        }

        public async Task<List<ShoppingCart>> GetShoppingCartItems(User user)
        {
            return await _cartRepository.GetShoppingCartItems(user);
        }

        public async Task<List<Store>> GetStorsNamesByIds(int[] ids, User user)
        {
            return await _storeDataService.GetStorsNamesByIds(ids, user);
        }

        public async Task<ShoppingCart> PostShoppingCartItem(ShoppingCart shopList, User user)
        {
            return await _cartRepository.PostShoppingCartItem(shopList, user);
        }

        public async Task<ShoppingCart> PutShoppingCartItem(ShoppingCart shopList, User user)
        {
            return await _cartRepository.PutShoppingCartItem(shopList, user);
        }

        public void SaveCart(List<BaseModel> Cartls)
        {
            List<Store> StorList = saveProducts(Cartls);
             _cartRepository.SaveCart(StorList);
        }

        //helper Methods
        private async Task<List<BaseModel>> getProducts()
        {
            List<BaseModel> ls = new List<BaseModel>();
            try
            {
                List<Store> storList = await _cartRepository.GetSavedCart();
                if (storList != null && storList.Count > 0)
                {
                    foreach (Store stor in storList)
                    {
                        ls.Add(stor);
                        if (stor.StoreProducts != null && stor.StoreProducts.Count > 0)
                        {
                            foreach (Product product in stor.StoreProducts)
                            {
                                ls.Add(product);
                            }
                        }
                        
                    }
                }
                
            }
            catch (Exception)
            {

                //throw;//x
            }
            return ls;
        }
        private List<Store> saveProducts(List<BaseModel> Cartls)
        {
            List<Store> ls = new List<Store>();
            Store CurrentStor = new Store();
            try
            {
                if (Cartls != null)
                {
                    for (int i = 0; i < Cartls.Count; i++)
                    {
                        if (Cartls[i] is Store)
                        {
                            CurrentStor = (Store)Cartls[i];
                            CurrentStor.StoreProducts = new List<Product>();
                            ls.Add(CurrentStor);
                        }
                        else
                        {
                            CurrentStor.StoreProducts.Add((Product)Cartls[i]);
                        }
                    }
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return ls;
        }

        public async Task<bool> SaveOrder(Order order)
        {
            return await _orderDataService.SaveOrder(order);
        }

        public async Task<string> SerializeeOrder(Order orderJson)
        {
            return await _orderDataService.SerializeeOrder(orderJson);
        }

        public async Task<List<ShoppingCart>> GetShoppingCartItems_Simplified(User user)
        {
            return await _cartRepository.GetShoppingCartItems_Simplified(user);
        }

        public void SaveShopingCartToLocal(List<ShoppingCart> shopList)
        {
            _cartRepository.SaveShopingCartToLocal(shopList);
        }

        public async Task<List<ShoppingCart>> GetShopingCartFromLocal()
        {
            return await _cartRepository.GetShopingCartFromLocal();
        }

        public async Task<List<ShoppingCart>> GetShoppingCartItemsFromAPI(User user)
        {
            return await _cartRepository.GetShoppingCartItemsFromAPI(user);
        }

        public List<ShoppingCart> GetActiveShoppingList()
        {
            return _cartRepository.GetActiveShoppingList();
        }
    }
}
