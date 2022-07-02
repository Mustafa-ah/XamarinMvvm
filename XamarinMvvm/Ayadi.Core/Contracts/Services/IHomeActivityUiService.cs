using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface IHomeActivityUiService
    {
        void SetHomeFoucs();
        void SetCatsFoucs();
        void SetStoreFoucs();
        void SetAccountFoucs();
        void SetSettingFoucs();
    }
}
