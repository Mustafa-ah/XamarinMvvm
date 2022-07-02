using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface IOrderRepository
    {
        Task<bool> SaveOrder(Order order);
        Task<Order> GetSavedOrder();
        Task<Order> PostOrder(Order order, User user);

        Task<List<Order>> GetUserOrders(User user);
        Task<List<PaymentMethod>> GetPaymentMethods(User user);

        Task<Order> DeserializeOrder(string orderJson);
        Task<string> SerializeeOrder(Order orderJson);
    }
}
