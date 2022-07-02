using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface IStoreRepository
    {
        Task<List<Store>> GetAllStors(User user);
        Task<List<Store>> GetStorsNamesByIds(int[] ids, User user);
        Task<Store> GetStoreByName(string name, User user);
        Task<Store> GetStoreById(string id, User user);
    }
}
