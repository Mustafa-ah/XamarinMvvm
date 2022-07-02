using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using CoreGraphics;

namespace Tomoor.IOS.Utility
{
    class LoadingOverlay : UIView
    {
        ///https://developer.xamarin.com/recipes/ios/standard_controls/popovers/display_a_loading_message/
        // control declarations
        UIActivityIndicatorView activitySpinner;
        UILabel loadingLabel;

        UIView _containerView;

        public LoadingOverlay(UIView view) : base(view.Frame)
        {
            // configurable bits
            _containerView = view;
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            // derive the center x and y
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activitySpinner.Frame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - activitySpinner.Frame.Height - 20,
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();

            // create and configure the "Loading Data" label
            loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ));
            loadingLabel.BackgroundColor = UIColor.Clear;
            loadingLabel.TextColor = UIColor.White;
            //string witStr = Foundation.NSBundle.MainBundle.LocalizedString("Load_" + LangId, "Loading Data...");
            loadingLabel.Text = "Loading Data...";
            loadingLabel.TextAlignment = UITextAlignment.Center;
            loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(loadingLabel);
        }

        private bool _visable;
        public bool Visable
        {
            get {return _visable; }
            set
            {
                //if (value == Visable)
                //{
                //    return;
                //}
                _visable = value;
                if (value)
                {
                    _containerView.Add(this);
                }
                else
                {
                    Hide();
                }
            }
        }
        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            UIView.Animate(
                0.5, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }
    }
}