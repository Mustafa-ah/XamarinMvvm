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
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Tomoor.Droid.Adapters;
using MvvmCross.Binding.Droid.BindingContext;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "UserAdressesView")]
    public class UserAdressesView : MvxActivity<UserAdressesViewModel>
    {
        BindableProgressBar _bindableProgressBar;

        private MvxRecyclerView _addressesListView;

        public new UserAdressesViewModel ViewModel
        {
            get { return (UserAdressesViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Activity_UserAdresses);


            _addressesListView = FindViewById<MvxRecyclerView>(Resource.Id.AdressesList);

            _addressesListView.Adapter = new UserAddressesRecyclerAdapter((IMvxAndroidBindingContext)BindingContext, this);


            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<UserAdressesView, UserAdressesViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();
        }
    }
}