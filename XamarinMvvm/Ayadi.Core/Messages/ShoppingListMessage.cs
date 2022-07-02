using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class ShoppingListMessage : MvxMessage
    {
        public ShoppingListMessage(object sender, List<ShoppingCart> shopList) : base(sender)
        {
            ShoppingList = shopList;
        }
        public List<ShoppingCart> ShoppingList { get; set; }
    }
}
