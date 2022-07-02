using Ayadi.Core.Model;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Messages
{
    public class TopHomeSliderMessage : MvxMessage
    {
        public TopHomeSliderMessage(object sender, List<Imager> imgList) : base(sender)
        {
            ImgList = imgList;
        }

        public List<Imager> ImgList  { get; set; }
    }
}
