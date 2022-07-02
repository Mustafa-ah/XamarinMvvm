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
    [Activity(Label = "WriteNewPasswordView")]
    public class WriteNewPasswordView : MvxActivity<WriteNewPasswordViewModel>
    {
        BindableProgressBar _bianableProgressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_WriteNewPassword);

            _bianableProgressBar = new BindableProgressBar(this);

            var set = this.CreateBindingSet<WriteNewPasswordView, WriteNewPasswordViewModel>();
            set.Bind(_bianableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();
        }
    }
}