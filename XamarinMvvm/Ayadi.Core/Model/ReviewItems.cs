using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class ReviewItems : BaseModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string WrittenOnStr { get; set; }
        public int Rating { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public string ReplyText { get; set; }
    }
}
