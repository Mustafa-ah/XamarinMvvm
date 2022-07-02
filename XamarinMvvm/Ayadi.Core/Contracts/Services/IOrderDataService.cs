using Ayadi.Core.Contracts.Repository;
using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface IOrderDataService
    {
         ICartRepository _cartRepository { get; set; }
        Task<bool> SaveOrder(Order order);
        Task<Order> GetSavedOrder();
        Task<Order> PostOrder(Order order, User user);

        Task<List<Order>> GetUserOrders(User user);
        Task<List<PaymentMethod>> GetPaymentMethods(User user);

        Task<Order> DeserializeOrder(string orderJson);
        Task<string> SerializeeOrder(Order orderJson);

        Task<List<UserAdress>> GetUserAdresses(User user);
    }
}
