using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;

namespace Ayadi.Core.Messages
{
    public class AdresessUpdatedMessage : MvxMessage
    {
        public AdresessUpdatedMessage(object sender, UserAdress newAdress) : base(sender)
        {
            NewAdress = newAdress;
        }
        public UserAdress NewAdress;
    }
}
