using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class SearchMessage : MvxMessage
    {
        public SearchMessage(object sender) : base(sender)
        {
        }

        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string KeyWord { get; set; }
        public string StoreId { get; set; }
        public string CategoryId { get; set; }
    }
}
