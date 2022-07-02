using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views;
using Ayadi.Core.ViewModel;
using Android.Views.Animations;
using MvvmCross.Binding.Droid.Views;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "StoreView")]
    public class StoreView : MvxActivity<StoreViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Store);
           
            //imgCover.SetColorFilter(Android.Graphics.Color.Rgb(123, 123, 123), Android.Graphics.PorterDuff.Mode.Multiply);
        }

        protected override void OnResume()
        {
            base.OnResume();
            MvxImageView img = FindViewById<MvxImageView>(Resource.Id.imageViewS_logo);
            MvxImageView imgCover = FindViewById<MvxImageView>(Resource.Id.imageViewS_cover);
            imgCover.SetColorFilter(Android.Graphics.Color.Rgb(100, 100, 100), Android.Graphics.PorterDuff.Mode.Darken);
            SetAnimation(img);
        }
        void SetAnimation(View viewToAnimate)
        {
            ScaleAnimation anim = new ScaleAnimation(0.0f, 1.0f, 0.0f, 1.0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            anim.Duration = 500;
            viewToAnimate.StartAnimation(anim);
        }
    }
}