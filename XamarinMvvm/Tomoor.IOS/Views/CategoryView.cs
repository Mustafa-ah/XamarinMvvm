
using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreGraphics;
using MvvmCross.Binding.iOS.Views;
using Ayadi.Core.ViewModel;
using MvvmCross.Binding.BindingContext;
using Tomoor.IOS.Utility;
using System.Collections.Generic;

using Timeing = System.Timers.Timer;
using Tomoor.IOS.Cells;

namespace Tomoor.IOS.Views
{
    public partial class CategoryView : BaseTabView
    {
        public CategoryView(IntPtr handle) : base(handle)
        {
        }

        protected CategoryViewModel categoryViewModel
          => ViewModel as CategoryViewModel;

        List<float> pagingScrollOfSetList;
        Timeing TopSliderTimer;

        public static float CellWidth = 130;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetPositions();
            SetTopSliderImages();

            TopSliderTimer.Start();
        }
        protected override void CreateBindings()
        {
           

            CellWidth = (ViewWidth / 2) - 10;

            UICollectionViewFlowLayout ProductCollectionlayout = new UICollectionViewFlowLayout();
            ProductCollectionlayout.ScrollDirection = UICollectionViewScrollDirection.Vertical;
            ProductCollectionlayout.ItemSize = new CGSize(CellWidth, 100);

            CatsCollectionView.SetCollectionViewLayout(ProductCollectionlayout, true);

            var source = new MvxCollectionViewSource(CatsCollectionView, CatsCollectionViewCell.Identifier);

            CatsCollectionView.Source = source;

            CatsCollectionView.ReloadData();

            LoadingOverlay loading_ = new LoadingOverlay(View);

            var set = this.CreateBindingSet<CategoryView, CategoryViewModel>();
            set.Bind(loading_).For(v => v.Visable).To(vm => vm.IsBusy);
            set.Bind(source).To(vm => vm.Categories);

            set.Bind(source)
               .For(cv => cv.SelectionChangedCommand)
               .To(vm => vm.ShowProductsViewCommand);

            set.Apply();

            TopSliderTimer = new Timeing(3000);
            TopSliderTimer.Elapsed -= TopSliderTimer_Elapsed;
            TopSliderTimer.Elapsed += TopSliderTimer_Elapsed;

            catsTopSlider.ShowsHorizontalScrollIndicator = false;
            // categoryViewModel.ViewModelInitialized += CategoryViewModel_ViewModelInitialized;
        }

       

        private void TopSliderTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                ChangeTopSliderImage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

        private void SetPositions()
        {
            float Ypoint = 0;
            float Xpoint = 0;

            float _margin = 60;

            catsTopSlider.Frame = new CGRect(Ypoint, Xpoint, ViewWidth, 150);

            Ypoint = 150 + 5;

            CatsCollectionView.Frame = new CGRect(Xpoint, Ypoint, ViewWidth, ViewHieght - (150 + _margin));
        }


        private void SetTopSliderImages()
        {
            float newX = 0;

            if (categoryViewModel.SliderImages != null)
            {
                pagingScrollOfSetList = new List<float>();

                for (int i = 0; i < categoryViewModel.SliderImages.Count; i++)
                {
                    UIImageView imge = new UIImageView();
                    newX = (float)(catsTopSlider.Frame.Width * i);
                    pagingScrollOfSetList.Add(newX);
                    imge.Frame = new CGRect(newX, 0, catsTopSlider.Frame.Width, catsTopSlider.Frame.Height);
                    ImageLoader.LoadImage(categoryViewModel.SliderImages[i].Src, imge);
                    catsTopSlider.AddSubview(imge);
                }
                float scrollViewContentWidth = newX + (float)catsTopSlider.Frame.Width;
                catsTopSlider.ContentSize = new CGSize(scrollViewContentWidth, 150);

            }
        }

        private void ChangeTopSliderImage()
        {
            if (categoryViewModel.SliderImages == null || pagingScrollOfSetList == null)
            {
                return;
            }
            Random rnd = new Random();
            int TopPos_ = rnd.Next(0, categoryViewModel.SliderImages.Count);
            float RandomX = pagingScrollOfSetList[TopPos_];

            InvokeOnMainThread(() => catsTopSlider.ContentOffset = new CGPoint(RandomX, 0));

        }
    }
}