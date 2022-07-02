using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class LocalizedNames : BaseModel
    {
        public int? language_id { get; set; }
        public string Localized_name { get; set; }
        public string Localized_shortDescription { get; set; }
        public string Localized_fullDescription { get; set; }
        public string Localized_Description { get; set; }
    }
}
