using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class FavouriteProductMessage : MvxMessage
    {
        public FavouriteProductMessage(object sender, Product product) : base(sender)
        {
            _product = product;
        }
        public Product _product { get; set; }
    }
}
