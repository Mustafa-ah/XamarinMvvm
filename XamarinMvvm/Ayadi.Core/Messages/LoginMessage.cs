using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class LoginMessage : MvxMessage
    {
        public LoginMessage(object sender) : base(sender)
        {
        }
    }
}
