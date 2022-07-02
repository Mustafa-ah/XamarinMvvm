
using System;
using System.Drawing;

using Foundation;
using UIKit;
using MvvmCross.Binding.BindingContext;
using Ayadi.Core.ViewModel;
using CoreGraphics;
using MvvmCross.Binding.iOS.Views;
using Tomoor.IOS.Cells;
using Tomoor.IOS.Utility;

namespace Tomoor.IOS.Views
{
    public partial class StorsView : BaseTabView
    {
        public StorsView(IntPtr handle) : base(handle)
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
            try
            {
                UICollectionViewFlowLayout flowLayout = new UICollectionViewFlowLayout();
                flowLayout.ScrollDirection = UICollectionViewScrollDirection.Vertical;
                flowLayout.ItemSize = new CGSize(100, 130);

                StoresCollectionView.SetCollectionViewLayout(flowLayout, true);

                var source = new MvxCollectionViewSource(StoresCollectionView, StoresCollectionViewCell.Identifier);

                StoresCollectionView.Source = source;
                StoresCollectionView.ReloadData();

               // LoadingOverlay loading_ = new LoadingOverlay(View);

                var set = this.CreateBindingSet<StorsView, StorsViewModel>();

                //set.Bind(loading_).For(lo => lo.Visable)
                //    .To(vm => vm.is)

                set.Bind(source).To(vm => vm.Stors);

                set.Bind(source)
              .For(cv => cv.SelectionChangedCommand)
              .To(vm => vm.ShowStoreViewCommand);

                set.Apply();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
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

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetPositions();
        }

        private void SetPositions()
        {
            float headerViewH = HomeView.HeaderViewHeight;
            float tabBarH = HomeView.TabbarHeight;

            float Ypoint = headerViewH + 5;
            StoresCollectionView.Frame = new CGRect(10, Ypoint, ViewWidth - 20, ViewHieght - (Ypoint + tabBarH));
        }
    }
}