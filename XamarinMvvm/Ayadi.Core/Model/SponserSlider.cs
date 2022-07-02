using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class SponserSlider : BaseModel
    {
        public string SponsorSliderName { get; set; }
        public int Interval { get; set; }
        public List<Sponser> Images { get; set; }
    }
}
