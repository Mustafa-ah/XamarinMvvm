using MvvmCross.Plugins.Messenger;

namespace Ayadi.Core.Messages
{
    public class ProductsViewMessage : MvxMessage
    {
        public ProductsViewMessage(object sender) : base(sender)
        {
        }

        public string ViewType { get; set; }
    }
}
