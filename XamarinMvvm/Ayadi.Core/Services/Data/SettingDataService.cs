using Ayadi.Core.Contracts.Services;
using Akavache;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Contracts.Repository;

namespace Ayadi.Core.Services.Data
{
    class SettingDataService : ISettingDataService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingDataService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public void ClearSavedData()
        {
            _settingRepository.ClearSavedData();
        }

        public async Task<bool> SavedLangId(string langId)
        {
            return await _settingRepository.SavedLangId(langId);
        }

        public void setArLang()
        {
            BlobCache.LocalMachine.InsertObject("Lang", "ar-SA");
        }

        public void setEnLang()
        {
            BlobCache.LocalMachine.InsertObject("Lang", "en-US");
        }
    }
}
