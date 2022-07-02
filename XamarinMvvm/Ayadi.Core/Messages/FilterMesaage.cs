using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class FilterMesaage : MvxMessage
    {
        public FilterMesaage(object sender) : base(sender)
        {
        }
        public Category Cat { get; set; }
    }
}
