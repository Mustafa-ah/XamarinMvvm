using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories(User user);
        Task<List<Category>> GetHomeCategories(User user);
        Task<List<Category>> GetHomeCategoriesFromAPI(User user);
        Task<List<Category>> GetAllCategoriesFromAPI(User user);
    }
}
