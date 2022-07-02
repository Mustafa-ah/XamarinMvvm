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
    public class OrderDataService : IOrderDataService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserDataService _userDataService;


        private ICartRepository _cartRepo;

        public OrderDataService(IOrderRepository orderRepository, 
            IUserDataService userDataService,
            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _userDataService = userDataService;
            _cartRepo = cartRepository;
        }

        public ICartRepository _cartRepository
        {
            get
            {
                return _cartRepo;
            }

            set
            {
                _cartRepo = value;
            }
        }

        public async Task<Order> DeserializeOrder(string orderJson)
        {
            return await _orderRepository.DeserializeOrder(orderJson);
        }

        public async Task<List<PaymentMethod>> GetPaymentMethods(User user)
        {
            return await _orderRepository.GetPaymentMethods(user);
        }

        public async Task<Order> GetSavedOrder()
        {
            return await _orderRepository.GetSavedOrder();
        }

        public async Task<List<UserAdress>> GetUserAdresses(User user)
        {
            return await _userDataService.GetUserAdresses(user);
        }

        public async Task<List<Order>> GetUserOrders(User user)
        {
            return await _orderRepository.GetUserOrders(user);
        }

        public async Task<Order> PostOrder(Order order, User user)
        {
            return await _orderRepository.PostOrder(order, user);
        }

        public async Task<bool> SaveOrder(Order order)
        {
            return await _orderRepository.SaveOrder(order);
        }

        public async Task<string> SerializeeOrder(Order orderJson)
        {
            return await _orderRepository.SerializeeOrder(orderJson);
        }
    }
}
