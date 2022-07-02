using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using Ayadi.Core.ViewModel;
using MvvmCross.Core.ViewModels;
using Tomoor.IOS.Utility;
using CoreGraphics;
using CoreAnimation;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.IOS.Views
{
    public partial class HomeView : MvxTabBarViewController<HomeViewModel>
    {
        private int _tabsCreatedSoFar;

        protected UIButton _cartBtn;
        protected UIButton _searchBtn;
        protected UIButton _favouriteBtn;

        protected float ViewWidth = (float)UIScreen.MainScreen.Bounds.Width;
        protected float ViewHieght = (float)UIScreen.MainScreen.Bounds.Height;

        public static float HeaderViewHeight = 50;
        public static float TabbarHeight = 50;

        public HomeView(IntPtr handle) : base(handle)
        {
        }
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            
            CreateHomeBinding();
            CreateTabs();
        }

        public override void ItemSelected(UITabBar tabbar, UITabBarItem item)
        {
            Title = item.Title;
        }

        private void CreateTabs()
        {
            //var wefwef = CreateTab("SubHome", "ic-settings-", ViewModel.SubHomeViewModel);
            var viewControllers = new UIViewController[]
            {
                CreateTab(null, "ic-settings-", ViewModel.SettingsViewModel),
                CreateTab(null, "ic-user-", ViewModel.UserAccountViewModel),
                 CreateTab(null, "ic-store-", ViewModel.StorsViewModel),
                   CreateTab(null, "ic-cats-", ViewModel.CategoryViewModel),
                CreateTab(null, "ic-home-", ViewModel.SubHomeViewModel)
            };

            ViewControllers = viewControllers;

            SelectedViewController = ViewControllers[4];
            //Title = SelectedViewController.Title;

            //NavigationItem.Title = SelectedViewController.Title;

            //ViewControllerSelected += (o, e) =>
            //{
            //    NavigationItem.Title = TabBar.SelectedItem.Title;
            //};
        }

        private UIViewController CreateTab
            (string title, string imageName, IMvxViewModel viewModel)
        {
            var viewController =
                this.CreateViewControllerFor(viewModel) as UIViewController;
            viewModel.Start();

            UpdateTabBar(viewController, title, imageName);

            return viewController;
        }

        private void UpdateTabBar(UIViewController viewController, string title, string imageName)
        {
           // viewController.Title = title;

            viewController.TabBarItem = new UITabBarItem(
                title,
                UIImage.FromBundle(imageName + "normal.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal),
                _tabsCreatedSoFar)
            {
                SelectedImage = UIImage.FromBundle(imageName + "active.png")
                    .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
            };

            //viewController.TabBarItem.si
            viewController.TabBarItem.ImageInsets = new UIEdgeInsets(4, 0, -4, 0);
           
            //var font = UIFont.FromName("Helvetica", 10);

            //viewController.TabBarItem.SetTitleTextAttributes(
            //    new UITextAttributes { TextColor = Colorizer.DarkTextColor, Font = font },
            //    UIControlState.Normal);

            //viewController.TabBarItem.SetTitleTextAttributes(
            //    new UITextAttributes { TextColor = Colorizer.AccentColor, Font = font },
            //    UIControlState.Selected);

            _tabsCreatedSoFar++;
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
           ViewWidth = (float)UIScreen.MainScreen.Bounds.Width;
            ViewHieght = (float)UIScreen.MainScreen.Bounds.Height;
            AddHeaderView();

            TabbarHeight = (float)TabBar.Frame.Height;
        }

        private void AddHeaderView()
        {
            try
            {
                UIView headerView = new UIView(new CGRect(0, 0, ViewWidth, HeaderViewHeight));
                CGColor dd = new CGColor(239f, 239f, 239f);
                CAGradientLayer gradient = new CAGradientLayer();
                gradient.Frame = headerView.Bounds;
                gradient.Colors = new CGColor[] { UIColor.Black.CGColor, dd };
                headerView.Layer.InsertSublayer(gradient, 0);

                _favouriteBtn = new UIButton(UIButtonType.Custom);
                _favouriteBtn.SetImage(UIImage.FromFile("Images/Favourites.png"), UIControlState.Normal);
                _favouriteBtn.Frame = new CGRect(10, 20, 30, 30);
                headerView.Add(_favouriteBtn);

                UIImageView _logoImageView = new UIImageView(UIImage.FromFile("Images/HLogo.png"));
                _logoImageView.Frame = new CGRect((ViewWidth / 2 - 30), 20, 60, 30);
                headerView.Add(_logoImageView);

                _searchBtn = new UIButton(UIButtonType.Custom);
                _searchBtn.SetImage(UIImage.FromFile("Images/search.png"), UIControlState.Normal);
                _searchBtn.Frame = new CGRect(ViewWidth - 80, 20, 30, 30);
                headerView.Add(_searchBtn);

                _cartBtn = new UIButton(UIButtonType.Custom);
                _cartBtn.SetImage(UIImage.FromFile("Images/cart.png"), UIControlState.Normal);
                _cartBtn.Frame = new CGRect(ViewWidth - 35, 20, 30, 30);
                headerView.Add(_cartBtn);

                View.AddSubview(headerView);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateHomeBinding()
        {
            try
            {
                var set =this.CreateBindingSet<HomeView, HomeViewModel>();

                set.Bind(_favouriteBtn).To(vm => vm.ShowFavouriteViewCommand);
                set.Bind(_searchBtn).To(vm => vm.ShowSearchViewCommand);
                set.Bind(_cartBtn).To(vm => vm.ShowCartViewCommand);

                set.Apply();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}