using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class PaymentMethod : BaseModel
    {
        public string PaymentMethodType { get; set; }
        public string PaymentMethodDescription { get; set; }
        public string FriendlyName { get; set; }
        public string SystemName { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal? AdditionalFee { get; set; }
        public string LogoUrl { get; set; }
        public int? Id { get; set; }
    }
}
