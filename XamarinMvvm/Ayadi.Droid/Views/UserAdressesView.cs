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

namespace Ayadi.Droid.Views
{
    [Activity(Label = "UserAdressesView")]
    public class UserAdressesView : MvxActivity<UserAdressesViewModel>
    {
        BindableProgressBar _bindableProgressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Activity_UserAdresses);

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<UserAdressesView, UserAdressesViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();
        }
    }
}