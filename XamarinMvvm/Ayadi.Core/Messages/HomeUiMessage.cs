using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class HomeUiMessage : MvxMessage
    {
        public HomeUiMessage(object sender) : base(sender)
        {
        }
    }
}
