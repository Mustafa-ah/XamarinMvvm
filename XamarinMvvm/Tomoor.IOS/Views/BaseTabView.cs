using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using Tomoor.IOS.Utility;
using CoreGraphics;
using CoreAnimation;

namespace Tomoor.IOS.Views
{
    public class BaseTabView : MvxViewController
    {
       protected UIImageView _cartImageView;
       protected UIImageView _searchImageView;
       protected UIButton _favouriteImageBtn;

        protected float ViewWidth = (float)UIScreen.MainScreen.Bounds.Width;
        protected float ViewHieght = (float)UIScreen.MainScreen.Bounds.Height;

        protected float TabBarHeight = 146;

        public BaseTabView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var stringAttributes = new UIStringAttributes();
            stringAttributes.Font = UIFont.SystemFontOfSize(16);
            stringAttributes.ForegroundColor = UIColor.FromRGB(255, 255, 255);
            NavigationController.NavigationBar.BarTintColor = Colorizer.DarkColor;
            NavigationController.NavigationBar.TintColor = UIColor.White;
            NavigationController.NavigationBar.TitleTextAttributes = stringAttributes;
            this.NavigationController.SetNavigationBarHidden(true, true);
            // View.BackgroundColor = Colorizer.AppBackgroundColor;

            CreateBindings();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            ViewWidth = (float)UIScreen.MainScreen.Bounds.Width;
            ViewHieght = (float)UIScreen.MainScreen.Bounds.Height;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }

        protected virtual void CreateBindings()
        {
        }

    }
}