using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;

namespace Ayadi.Core.Messages
{
    public class FavouriteChangedMessage : MvxMessage
    {
        public FavouriteChangedMessage(object sender) : base(sender)
        {
        }

        public List<Product> NewFavList { get; set; }
    }
}
