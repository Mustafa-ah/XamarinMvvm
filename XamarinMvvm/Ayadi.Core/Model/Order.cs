using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Order : BaseModel
    {
        public int Id { get; set; }
        public int Store_id { get; set; }

        public decimal Order_discount { get; set; }
        public decimal Order_total { get; set; }

        public int Customer_id { get; set; }

        public User Customer { get; set; }

        public UserAdress Billing_address { get; set; }
        public UserAdress Shipping_address { get; set; }

        public List<OrderItems> Order_items { get; set; }

        public string Payment_method_system_name { get; set; }
        public string Shipping_method { get; set; }

        public string Shipping_rate_computation_method_system_name { get; set; }

        
        public string Order_status { get; set; }
        public string Payment_status { get; set; }
        public string Shipping_status { get; set; }

        public PaymentMethod Paymet_Method { get; set; }
        [JsonIgnore]
        public decimal Order_Subtotal { get; set; }

    }
}
