using Ayadi.Core.Contracts.Repository;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Services.Data
{
    public class SearchDataService : ISearchDataService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStoreRepository _storeRepository;

        public SearchDataService(ICategoryRepository categoryRepository, IStoreRepository storeRepository)
        {
            _categoryRepository = categoryRepository;
            _storeRepository = storeRepository;

        }

        public async Task<List<Category>> GetAllCategories(User user)
        {
            return await _categoryRepository.GetAllCategories(user);
        }

        public async Task<List<Store>> GetAllStors(User user)
        {
            return await _storeRepository.GetAllStors(user);
        }
    }
}
