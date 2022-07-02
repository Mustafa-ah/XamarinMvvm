using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Store : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public Imager Image { get; set; }
        public bool IsSelected { get; set; }
        public List<Product> StoreProducts { get; set; }
        public List<LocalizedNames> Localized_Vendor { get; set; }
        public string SearcName { get; set; }
        public Store()
        {
            StoreProducts = new List<Product>();
            Localized_Vendor = new List<LocalizedNames>();
        }
    }
}
