using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Product_category_mappings : BaseModel
    {
        public int Id { get; set; }
        public int Product_id { get; set; }
        public int Category_id { get; set; }
    }
}
