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

namespace Ayadi.Droid.Views
{
    [Activity(Label = "SignupView")]
    public class SignupView : MvxActivity<SignupViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_SignUp);
        }
    }
}