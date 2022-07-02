
using System;
using System.Drawing;

using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using Ayadi.Core.ViewModel;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Binding.BindingContext;
using Tomoor.IOS.Adapters;

namespace Tomoor.IOS.Views
{
    public partial class FavouriteView : BaseView//MvxViewController<FavouriteView>, IMvxOverridePresentationAttribute
    {
        FavouriteTableViewSource favouriteTableSource;

        public FavouriteView(IntPtr handle) : base(handle)
        {
        }

        protected FavouriteView favouriteView => ViewModel as FavouriteView;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        protected override void CreateBindings()
        {
            favouriteTableSource = new FavouriteTableViewSource(TabelViewFavourite);

            var set =
            this.CreateBindingSet<FavouriteView, FavouriteViewModel>();

            set.Bind(btnClose).To(vm => vm.CloseViewCommand);

            set.Bind(favouriteTableSource).To(vm => vm.FavItems);

            set.Bind(favouriteTableSource)
                .For(source => source.SelectionChangedCommand)
                .To(vm => vm.ShowProductViewCommand);

            set.Apply();

            TabelViewFavourite.Source = favouriteTableSource;
            TabelViewFavourite.ReloadData();
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