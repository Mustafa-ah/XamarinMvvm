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
    [Activity(Label = "RecoverAccountView")]
    public class RecoverAccountView : MvxActivity<RecoverAccountViewModel>
    {
        BindableProgressBar _bindableProgressBar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Activity_RecoverAccount);

            _bindableProgressBar = new BindableProgressBar(this);

            var set = this.CreateBindingSet<RecoverAccountView, RecoverAccountViewModel>();
            set.Bind(_bindableProgressBar).For(b => b.Visable).To(vm => vm.IsBusy);
            set.Apply();

        }
    }
}