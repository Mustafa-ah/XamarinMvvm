using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface ISettingRepository
    {
        void ClearSavedData();
        Task<bool> SavedLangId(string langId);
    }
}
