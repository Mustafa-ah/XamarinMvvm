using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class OrderItems : BaseModel
    {
        public int Quantity { get; set; }
        public string Product_id { get; set; }
        public Product Product { get; set; }
    }
}
