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

namespace Tomoor.Droid.Views
{
    [Activity(Label = "CheckoutShippingView")]
    public class CheckoutShippingView : MvxActivity<CheckoutShippingViewModel>
    {
        public new CheckoutShippingViewModel ViewModel
        {
            get { return (CheckoutShippingViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_CheckoutShipping);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (ViewModel.ShouldClose)
            {
                Finish();
            }
        }
    }
}