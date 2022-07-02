using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class DeleteAdressMessage : MvxMessage
    {
        public DeleteAdressMessage(object sender, UserAdress adress) : base(sender)
        {
            Adress = adress;
        }
        public UserAdress Adress;
    }
}
