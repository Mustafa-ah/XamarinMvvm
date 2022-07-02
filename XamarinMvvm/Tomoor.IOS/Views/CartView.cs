
using System;
using System.Drawing;

using Foundation;
using UIKit;
using MvvmCross.Binding.BindingContext;
using Ayadi.Core.ViewModel;

namespace Tomoor.IOS.Views
{
    public partial class CartView : BaseView
    {
        public CartView(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        protected override void CreateBindings()
        {
            var set = this.CreateBindingSet<CartView, CartViewModel>();

            set.Bind(btnClose).To(vm => vm.CloseViewCommand);
            //set.Bind(_searchResultsTableViewSource)
            //    .For(source => source.SelectionChangedCommand)
            //    .To(vm => vm.ShowJourneyDetailsCommand);

            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
    }
}