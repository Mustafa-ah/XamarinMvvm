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
using MvvmCross.Droid.Support.V7.RecyclerView;
using Tomoor.Droid.Adapters;
using MvvmCross.Binding.Droid.BindingContext;
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "CheckoutPaymentView")]
    public class CheckoutPaymentView : MvxActivity<CheckoutPaymentViewModel>
    {
        MvxRecyclerView _BillingRecycler;
        BindableProgressBar _bindableProgressBar;
        public new CheckoutPaymentViewModel ViewModel
        {
            get { return (CheckoutPaymentViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_CheckoutPayment);

            _BillingRecycler = FindViewById<MvxRecyclerView>(Resource.Id.PaymentMethodsList);
            _BillingRecycler.Adapter = new PaymentMethodAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext);

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<CheckoutPaymentView, CheckoutPaymentViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();
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