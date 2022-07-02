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
    public class StoreDataService : IStoreDataService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;

        public StoreDataService(IStoreRepository soreRepository, IProductRepository productRepository)
        {
            _storeRepository = soreRepository;
            _productRepository = productRepository;
        }
        public async Task<List<Store>> GetAllStors(User user)
        {
            return await _storeRepository.GetAllStors(user);
        }

        public async Task<Store> GetStoreByName(string name, User user)
        {
            return await _storeRepository.GetStoreByName(name, user);
        }

        public async Task<List<Product>> GetStoreProducts(string storName , User user)
        {
            return await _productRepository.GetStoreProducts(storName, user);
        }

        public async Task<Store> GetStoreById(string id, User user)
        {
            return await _storeRepository.GetStoreById(id, user);
        }

        public async Task<List<Store>> GetStorsNamesByIds(int[] ids, User user)
        {
            return await _storeRepository.GetStorsNamesByIds(ids, user);
        }
    }
}
