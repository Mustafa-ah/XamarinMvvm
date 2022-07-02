using Ayadi.Core.Contracts.Repository;
using System;
using Akavache;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;

namespace Ayadi.Core.Repositories
{
    class SettingRepository : ISettingRepository
    {
        private readonly IUserRepository _userRepo;

        public SettingRepository(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public void ClearSavedData()
        {
            try
            {
                //List<string> keys = new List<string>();
                //keys.Add(Constants.AllCategoriesKey);
                //keys.Add(Constants.HomeCategoriesKey);
                //keys.Add(Constants.HomeProductsKey);

                BlobCache.LocalMachine.Invalidate(Constants.AllCategoriesKey);
                BlobCache.LocalMachine.Invalidate(Constants.HomeCategoriesKey);
                BlobCache.LocalMachine.Invalidate(Constants.HomeProductsKey);

                //BlobCache.LocalMachine.Invalidate(keys);
            }
            catch (Exception)
            {

               // throw;
            }
            
        }

        public async Task<bool> SavedLangId( string langID)
        {
            try
            {
                if (!string.IsNullOrEmpty(langID))
                {
                    User appUser = await _userRepo.GetSavedUser();
                    appUser.LangID = langID;
                    await _userRepo.SaveUserToLocal(appUser);
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch  
            {
                return false;
            }
        }
    }
}
