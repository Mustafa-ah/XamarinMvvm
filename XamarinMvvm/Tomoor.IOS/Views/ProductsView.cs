
using System;
using System.Drawing;

using Foundation;
using UIKit;
using Ayadi.Core.ViewModel;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.IOS.Views
{
    public partial class ProductsView : BaseView
    {
        public ProductsView(IntPtr handle) : base(handle)
        {
        }

        protected ProductsViewModel productsViewModel
         => ViewModel as ProductsViewModel;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        protected override void CreateBindings()
        {
            var set =
               this.CreateBindingSet<ProductsView, ProductsViewModel>();

            set.Bind(btnClose).To(vm => vm.CloseViewCommand);
            //set.Bind(_searchResultsTableViewSource)
            //    .For(source => source.SelectionChangedCommand)
            //    .To(vm => vm.ShowJourneyDetailsCommand);

            set.Apply();
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
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