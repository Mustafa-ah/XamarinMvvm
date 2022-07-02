using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class UpdateProductMessage : MvxMessage
    {
        public UpdateProductMessage(object sender, Product pro) : base(sender)
        {
            product = pro;
        }

        public Product product { get; set; }
    }
}
