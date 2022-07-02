using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface ISettingDataService
    {
        void setArLang();
        void setEnLang();

        void ClearSavedData();

        Task<bool> SavedLangId(string langId);
    }
}
