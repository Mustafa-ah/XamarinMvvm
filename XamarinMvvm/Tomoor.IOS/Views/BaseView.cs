using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using CoreGraphics;
using CoreAnimation;

namespace Tomoor.IOS.Views
{
    public class BaseView : MvxViewController, IMvxOverridePresentationAttribute
    {
        protected UIButton btnClose;
        protected UILabel labTitle;

        protected float ViewWidth = (float)UIScreen.MainScreen.Bounds.Width;
        protected float ViewHieght = (float)UIScreen.MainScreen.Bounds.Height;

        public BaseView(IntPtr handle) : base(handle)
        {
        }

        public MvxBasePresentationAttribute PresentationAttribute()
        {
            return new MvxModalPresentationAttribute
            {
                ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
                ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CreateHeaderView();
            CreateBindings();
        }

        protected virtual void CreateBindings()
        {
        }

        private void CreateHeaderView()
        {
            try
            {
                UIView headerView = new UIView(new CGRect(0, 0, ViewWidth, 60));
                CGColor dd = new CGColor(239f, 239f, 239f);
                CAGradientLayer gradient = new CAGradientLayer();
                gradient.Frame = headerView.Bounds;
                gradient.Colors = new CGColor[] { UIColor.Orange.CGColor, dd };
                headerView.Layer.InsertSublayer(gradient, 0);

                CGRect btnCloseFrame = new CGRect(10, 20, 40, 40);
                btnClose = new UIButton(UIButtonType.Custom);
                btnClose.SetImage(UIImage.FromFile("Images/previous_item.png"), UIControlState.Normal);
                btnClose.Frame = btnCloseFrame;
                headerView.AddSubview(btnClose);

                CGRect labTitleFrame = new CGRect((ViewWidth / 2) - 50, 20, 100, 40);
                labTitle = new UILabel(labTitleFrame);
                labTitle.Text = "To be set ...";

                headerView.AddSubview(labTitle);

                View.Add(headerView);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            /*
              try
            {
               

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
             */
        }
    }
}