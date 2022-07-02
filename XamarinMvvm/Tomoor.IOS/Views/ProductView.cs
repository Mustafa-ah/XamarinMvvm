
using System;
using System.Drawing;

using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using Ayadi.Core.ViewModel;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.IOS.Views
{
    
    public partial class ProductView : BaseView//MvxViewController<ProductViewModel>, IMvxOverridePresentationAttribute
    {
        public ProductView(IntPtr handle) : base(handle)
        {
        }

        protected ProductViewModel productViewModel
            => ViewModel as ProductViewModel;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.[MvxChildPresentation]
            base.DidReceiveMemoryWarning();

             
        // Release any cached data, images, etc that aren't in use.
    }

        #region View lifecycle

        protected override void CreateBindings()
        {
            var set =
               this.CreateBindingSet<ProductView, ProductViewModel>();

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

        //public MvxBasePresentationAttribute PresentationAttribute()
        //{
        //    return new MvxModalPresentationAttribute
        //    {
        //        ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
        //        ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve
        //    };
        //}

        #endregion
    }
}