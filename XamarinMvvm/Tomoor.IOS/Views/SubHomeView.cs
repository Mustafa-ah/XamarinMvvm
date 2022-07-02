
using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreGraphics;
using Ayadi.Core.ViewModel;
using Ayadi.Core.Model;
using Tomoor.IOS.Utility;
using System.Collections.Generic;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.BindingContext;
using Tomoor.IOS.Adapters;
using MvvmCross.Localization;

namespace Tomoor.IOS.Views
{
    public partial class SubHomeView : BaseTabView
    {
        System.Timers.Timer TopSliderTimer;

        List<float> pagingScrollOfSetList;
        List<float> SponsersScrollOfSetList;

        private float _topSliderHeight = 150;
        private float _productsCollectionHeight = 170;
        private float _catsCollectionHeight = 110;
        private float _sponserSliderHeight = 100;

        public SubHomeView(IntPtr handle) : base(handle)
        {
        }

        protected SubHomeViewModel subHomeViewModel
           => ViewModel as SubHomeViewModel;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle
        protected override void CreateBindings()
        {
            UICollectionViewFlowLayout ProductCollectionlayout = new UICollectionViewFlowLayout();
            ProductCollectionlayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            ProductCollectionlayout.ItemSize = new CGSize(110, 160);
            ///layout.SectionInset = new UIEdgeInsets(50, 50, 50, 50);
            ///ProductsCollectionView.CollectionViewLayout = layout;
            ProductsCollectionView.SetCollectionViewLayout(ProductCollectionlayout, true);
            ///if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            ///    EdgesForExtendedLayout = UIRectEdge.None;

            ///ProductsCollectionView.RegisterNibForCell(homeProductsCollectionViewCell.Nib, homeProductsCollectionViewCell.Identifier);
            var source = new MvxCollectionViewSource(ProductsCollectionView, homeProductsCollectionViewCell.Identifier);
           /// HomeProductsCollectionViewSource newSource = new HomeProductsCollectionViewSource(ProductsCollectionView);
            ProductsCollectionView.Source = source;



            //cats collection
            UICollectionViewFlowLayout CatsCollectionlayout = new UICollectionViewFlowLayout();
            CatsCollectionlayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            CatsCollectionlayout.ItemSize = new CGSize(130, 100);

            categoryCollectionView.SetCollectionViewLayout(CatsCollectionlayout, true);

            MvxCollectionViewSource CatsSource = new MvxCollectionViewSource(categoryCollectionView, HomeCatsCollectionViewCell.Identifier);
            categoryCollectionView.Source = CatsSource;

            LoadingOverlay loading_ = new LoadingOverlay(View);

            var set = this.CreateBindingSet<SubHomeView, SubHomeViewModel>();
            set.Bind(loading_).For(v => v.Visable).To(vm => vm.IsBusy);
            set.Bind(source).To(vm => vm.Products);

            set.Bind(CatsSource).To(vm => vm.Categories);


            set.Bind(source)
               .For(cv => cv.SelectionChangedCommand)
               .To(vm => vm.ShowProductViewCommand);

            //Translations
            set.Bind(lapProucts).To(vm => vm.TextSource)
                .WithConversion(new MvxLanguageConverter(),
                    "prodcuts");

            this.BindLanguage(lapCats, nameof(lapCats.Text), "categores");

           

            set.Apply();

            ProductsCollectionView.ReloadData();
            categoryCollectionView.ReloadData();


            TopSliderTimer = new System.Timers.Timer(3000);

            ScrollViewTopSlider.ShowsHorizontalScrollIndicator = false;
            ScrollViewSponsers.ShowsHorizontalScrollIndicator = false;

        }
       

        private void SubHomeViewModel_ViewModelInitialized(object sender, EventArgs e)
        {
            try
            {
                TopSliderTimer.Elapsed -= TopSliderTimer_Elapsed;
                TopSliderTimer.Elapsed += TopSliderTimer_Elapsed;

                SetTopSliderImages();
                SetSponsersSliderImages();

                TopSliderTimer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SetTopSliderImages()
        {
            float newX = 0;
           
            if (subHomeViewModel.SliderImages != null)
            {
                pagingScrollOfSetList = new List<float>();

                for (int i = 0; i < subHomeViewModel.SliderImages.Count; i++)
                {
                    UIImageView imge = new UIImageView();
                    newX = (float)(ScrollViewTopSlider.Frame.Width * i);
                    pagingScrollOfSetList.Add(newX);
                    imge.Frame = new CGRect(newX, 0, ScrollViewTopSlider.Frame.Width, ScrollViewTopSlider.Frame.Height);
                    ImageLoader.LoadImage(subHomeViewModel.SliderImages[i].Src, imge);
                    ScrollViewTopSlider.AddSubview(imge);
                }
                float scrollViewContentWidth = newX + (float)ScrollViewTopSlider.Frame.Width;
                ScrollViewTopSlider.ContentSize = new CGSize(scrollViewContentWidth, _topSliderHeight);

            }
        }

        private void SetSponsersSliderImages()
        {
            float newX = 0;

            float _margin = 30;
            float _imageWidth = 100;
           

            if (subHomeViewModel.SliderImages != null)
            {
                SponsersScrollOfSetList = new List<float>();

                
                for (int i = 0; i < subHomeViewModel.Sponsers.Count; i++)
                {
                    UIImageView imge = new UIImageView();
                   // imge.ContentMode = UIViewContentMode.Center;
                    newX = (float)((ScrollViewSponsers.Frame.Width * i) / 3) + _margin;// 30 => starting margin
                    SponsersScrollOfSetList.Add(newX - _margin);
                    imge.Frame = new CGRect(newX , 0, _imageWidth, ScrollViewSponsers.Frame.Height - _margin);
                    ImageLoader.LoadImage(subHomeViewModel.Sponsers[i].FilePath, imge);
                    ScrollViewSponsers.AddSubview(imge);

                }
                float scrollViewContentWidth = newX + _imageWidth + _margin;
                ScrollViewSponsers.ContentSize = new CGSize(scrollViewContentWidth, _topSliderHeight);

            }
        }

        private void TopSliderTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                ChangeTopSliderImage();
                ChangeSponsersSliderImage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ChangeTopSliderImage()
        {
            if (subHomeViewModel.SliderImages == null || pagingScrollOfSetList == null)
            {
                return;
            }
            Random rnd = new Random();
            int TopPos_ = rnd.Next(0, subHomeViewModel.SliderImages.Count);
            float RandomX = pagingScrollOfSetList[TopPos_];

            InvokeOnMainThread(() => ScrollViewTopSlider.ContentOffset = new CGPoint(RandomX, 0));

        }

        private void ChangeSponsersSliderImage()
        {
            if (subHomeViewModel.Sponsers == null || SponsersScrollOfSetList == null)
            {
                return;
            }
            Random rnd = new Random();
            int SponserPos_ = rnd.Next(0, subHomeViewModel.Sponsers.Count -1);
            float SponserRandomX = SponsersScrollOfSetList[SponserPos_];

            InvokeOnMainThread(() => ScrollViewSponsers.ContentOffset = new CGPoint(SponserRandomX, 0));

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

            subHomeViewModel.ViewModelInitialized += SubHomeViewModel_ViewModelInitialized;
        }

        private void SetPositions()
        {
            try
            {
                float Ypoint = 0;
                float Xpoint = 0;

                float _margin = 1;


                // root scroll view
                HomeScrollView.Frame = new CGRect(0, 0, ViewWidth, ViewHieght);

                // top slider

                float letfHeight = ViewHieght - (_productsCollectionHeight + _catsCollectionHeight + 30 + 30 + 30);

                _topSliderHeight = 150;// letfHeight / 2;
                _sponserSliderHeight = 100;// letfHeight / 2;

                CGRect topSliderFram = new CGRect(Xpoint, Ypoint, ViewWidth, _topSliderHeight);
                ScrollViewTopSlider.Frame = topSliderFram;

                // 5 = margin
                Ypoint = Ypoint + _topSliderHeight + _margin;

                // products label 
                CGRect productLabelFram = new CGRect(Xpoint, Ypoint, ViewWidth, 30);
                showAllProductsView.Frame = productLabelFram;

                lapProucts.Frame = new CGRect(ViewWidth - 100, 5, 100, 20);

                Ypoint = Ypoint + (float)productLabelFram.Height + _margin;

                // products collectionView
                CGRect productcollectionViewlFram = new CGRect(Xpoint, Ypoint, ViewWidth, _productsCollectionHeight);
                ProductsCollectionView.Frame = productcollectionViewlFram;

                Ypoint = Ypoint + _productsCollectionHeight + _margin;


                _margin = 5;

                // cats label 
                CGRect catsLabelFram = new CGRect(Xpoint, Ypoint, ViewWidth, 30);
                catsView.Frame = catsLabelFram;

                lapCats.Frame = new CGRect(ViewWidth - 100, 5, 100, 20);

                Ypoint = Ypoint + (float)catsLabelFram.Height + _margin;

                // cats collectionView
                CGRect catscollectionViewlFram = new CGRect(Xpoint, Ypoint, ViewWidth, _catsCollectionHeight);
                categoryCollectionView.Frame = catscollectionViewlFram;

                //Ypoint = ViewHieght + 30 - _sponserSliderHeight  - _margin;
                Ypoint = (float)catscollectionViewlFram.Y + (float)catscollectionViewlFram.Height + _margin;

                // sponser lap
                CGRect sponserLabFram = new CGRect(Xpoint, Ypoint, ViewWidth, 20);
                LapSponsers.Frame = sponserLabFram;

                Ypoint = (float)sponserLabFram.Y + (float)sponserLabFram.Height + _margin;

                // sponsers slider
                CGRect sponserFram = new CGRect(Xpoint, Ypoint, ViewWidth, _sponserSliderHeight);
                ScrollViewSponsers.Frame = sponserFram;

                Ypoint = (float)sponserFram.Y + (float)sponserFram.Height + _margin;


                //set content lenth form root home scroll
                HomeScrollView.ContentSize = new CGSize(ViewWidth, Ypoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}