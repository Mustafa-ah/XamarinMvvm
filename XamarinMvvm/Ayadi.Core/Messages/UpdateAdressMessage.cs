using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class UpdateAdressMessage : MvxMessage
    {
        public UpdateAdressMessage(object sender, UserAdress adress) : base(sender)
        {
            Adress = adress;
        }
        public UserAdress Adress;
    }
}
