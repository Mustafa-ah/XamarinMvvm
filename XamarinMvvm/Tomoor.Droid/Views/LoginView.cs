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

namespace Tomoor.Droid.Views
{
    [Activity(Label = "LoginView")]
    public class LoginView : MvxActivity<LoginViewModel>
    {
        BindableProgressBar _bindableProgressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Login);
            this.Window.SetSoftInputMode(SoftInput.StateHidden);

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

        }
    }
}