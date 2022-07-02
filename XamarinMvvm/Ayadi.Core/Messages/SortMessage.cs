using MvvmCross.Plugins.Messenger;

namespace Ayadi.Core.Messages
{
    public class SortMessage : MvxMessage
    {
        public SortMessage(object sender) : base(sender)
        {
        }

        public string SortBy { get; set; }
    }
}
