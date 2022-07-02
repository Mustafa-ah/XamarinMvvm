using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Category : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Se_name { get; set; }
        public Imager Image { get; set; }
        public bool IsSelected { get; set; }
        public int ProductCount { get; set; }
        public List<LocalizedNames> Localized_names { get; set; }
       // public string MyImage { get { return "http://ayadi.local/content/images/thumbs/0000016_gift-cards.jpeg"; } }

        public Category()
        {
            Localized_names = new List<LocalizedNames>();
        }
    }
}
