using Ayadi.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface ICategoryDataService
    {
        Task<List<Category>> GetAllCategories(User user);
        Task<List<Category>> GetHomeCategories(User user);
    }
}
