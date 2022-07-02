using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Repository
{
    public interface ISponserRepository
    {
        Task<List<SponserSlider>> GetAllSponsers(User user);
    }
}
