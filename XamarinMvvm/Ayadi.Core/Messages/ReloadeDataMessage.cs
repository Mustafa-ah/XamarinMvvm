using MvvmCross.Plugins.Messenger;


namespace Ayadi.Core.Messages
{
    public class ReloadeDataMessage : MvxMessage
    {
        public ReloadeDataMessage(object sender) : base(sender)
        {
        }
        public bool ShouldReloade { get; set; }
    }
}
