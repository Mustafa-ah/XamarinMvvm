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
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace Tomoor.Droid
{
    [Activity(
        MainLauncher = true,
        Label = "@string/ApplicationName",
        Theme = "@style/SplashTheme",
        NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity()
            : base(Resource.Layout.Activity_Splash)
        {
           // StartService(new Intent(this, typeof(UpdateDataService)));
        }
    }
}