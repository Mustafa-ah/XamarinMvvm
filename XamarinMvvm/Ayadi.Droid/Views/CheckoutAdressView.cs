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
using Ayadi.Droid.Utility;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Android.Support.V7.Widget;
using Ayadi.Droid.Adapters;
using MvvmCross.Binding.Droid.BindingContext;

namespace Ayadi.Droid.Views
{
    [Activity(Label = "CheckoutAdressView")]
    public class CheckoutAdressView : MvxActivity<CheckoutAdressViewModel>
    {
        BindableProgressBar _bindableProgressBar;
        MvxRecyclerView _BillingRecycler;
        MvxRecyclerView _ShippingRecycler;

        CheckBox _ShippingCheck;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_CheckoutAdress);

            _ShippingCheck = FindViewById<CheckBox>(Resource.Id.ShippingCheckBox);

            _BillingRecycler = FindViewById<MvxRecyclerView>(Resource.Id.BillingAdressList);
            _ShippingRecycler = FindViewById<MvxRecyclerView>(Resource.Id.ShippingAdressList);

            LinearLayoutManager BillingManager =
              new LinearLayoutManager(this, LinearLayoutManager.Vertical, false) { AutoMeasureEnabled = true };

            LinearLayoutManager ShippinggManager =
             new LinearLayoutManager(this, LinearLayoutManager.Vertical, false) { AutoMeasureEnabled = true };

            _BillingRecycler.SetLayoutManager(BillingManager);
            _ShippingRecycler.SetLayoutManager(ShippinggManager);

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<CheckoutAdressView, CheckoutAdressViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

            _BillingRecycler.Adapter = new AdressAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext);
            _ShippingRecycler.Adapter = new AdressAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext);

            _ShippingCheck.CheckedChange += _ShippingCheck_CheckedChange;
        }

        private void _ShippingCheck_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (!e.IsChecked)
            {
                FindViewById<TextView>(Resource.Id.textViewSelectShipping).Visibility = ViewStates.Visible;
                _ShippingRecycler.Visibility = ViewStates.Visible;
            }
            else
            {
                _ShippingRecycler.Visibility = ViewStates.Gone;
                FindViewById<TextView>(Resource.Id.textViewSelectShipping).Visibility = ViewStates.Gone;
            }
        }
    }
}