using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface IStoreDataService
    {
        Task<List<Store>> GetAllStors(User user);
        Task<List<Store>> GetStorsNamesByIds(int[] ids, User user);
        Task<Store> GetStoreByName(string name, User user);
        Task<Store> GetStoreById(string id, User user);
        Task<List<Product>> GetStoreProducts(string storName, User user);
    }
}
