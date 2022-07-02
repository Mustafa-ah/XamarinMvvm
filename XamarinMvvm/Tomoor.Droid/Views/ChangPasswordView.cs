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
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;
using Ayadi.Core.ViewModel;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "ChangPasswordView")]
    public class ChangPasswordView : MvxActivity<ChangPasswordViewModel>
    {
        private BindableProgressBar _bindableProgressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_ChangePassword);

            _bindableProgressBar = new BindableProgressBar(this);

            var set = this.CreateBindingSet<ChangPasswordView, ChangPasswordViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

        }
    }
}