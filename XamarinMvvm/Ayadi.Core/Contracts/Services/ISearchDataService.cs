using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface ISearchDataService
    {
        Task<List<Category>> GetAllCategories(User user);
        Task<List<Store>> GetAllStors(User user);
    }
}
