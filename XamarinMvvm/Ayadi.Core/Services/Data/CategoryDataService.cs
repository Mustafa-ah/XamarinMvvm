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
    public class CategoryDataService : ICategoryDataService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryDataService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetAllCategories(User user)
        {
            return await _categoryRepository.GetAllCategories(user);
        }

        public async Task<List<Category>> GetHomeCategories(User user)
        {
            return await _categoryRepository.GetHomeCategories(user);
        }
    }
}
