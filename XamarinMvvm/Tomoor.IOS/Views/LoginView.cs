
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
    public partial class LoginView : BaseView//MvxViewController<LoginViewModel>, IMvxOverridePresentationAttribute
    {
        public LoginView(IntPtr handle) : base(handle)
        {
        }

        protected LoginViewModel loginViewModel
          => ViewModel as LoginViewModel;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        protected override void CreateBindings()
        {

            var set =
               this.CreateBindingSet<LoginView, LoginViewModel>();

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