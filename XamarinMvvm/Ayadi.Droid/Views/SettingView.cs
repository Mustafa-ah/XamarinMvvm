using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using Ayadi.Core.ViewModel;
using MvvmCross.Droid.Support.V4;
using Ayadi.Droid.Adapters;
using Android.Support.V4.View;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Content;

namespace Ayadi.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel), Resource.Id.content_frame, false)]
    [Register("Ayadi.Droid.Views.SettingView")]
    public class SettingView : MvxFragment<SettingViewModel>
    {
        XmlDb _Db;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _Db = new XmlDb(Activity);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.Fragment_Setting, null);

            SetLangChange(fragView);
            return fragView;
        }

        private void SetLangChange(View fragView)
        {
            try
            {
                string _currentLang = _Db.getSavedLangId();

                if (_currentLang == "0")
                {
                    if (Java.Util.Locale.Default.Language == "ar")
                    {
                        fragView.FindViewById<ImageView>(Resource.Id.imageViewAr)
                    .SetImageResource(Resource.Drawable.RadioChecked);

                        fragView.FindViewById<ImageView>(Resource.Id.imageViewEn)
                        .SetImageResource(Resource.Drawable.RadioUnChecked);
                    }
                    else
                    {
                        fragView.FindViewById<ImageView>(Resource.Id.imageViewAr)
                    .SetImageResource(Resource.Drawable.RadioUnChecked);

                        fragView.FindViewById<ImageView>(Resource.Id.imageViewEn)
                        .SetImageResource(Resource.Drawable.RadioChecked);
                    }
                }
                else
                {
                    if (_currentLang == "ar-SA")
                    {
                        fragView.FindViewById<ImageView>(Resource.Id.imageViewAr)
                    .SetImageResource(Resource.Drawable.RadioChecked);

                        fragView.FindViewById<ImageView>(Resource.Id.imageViewEn)
                        .SetImageResource(Resource.Drawable.RadioUnChecked);
                    }
                    else
                    {
                        fragView.FindViewById<ImageView>(Resource.Id.imageViewAr)
                    .SetImageResource(Resource.Drawable.RadioUnChecked);

                        fragView.FindViewById<ImageView>(Resource.Id.imageViewEn)
                        .SetImageResource(Resource.Drawable.RadioChecked);
                    }
                }

                LinearLayout langLayoutAr = fragView.FindViewById<LinearLayout>(Resource.Id.linearLayoutAr);
                LinearLayout langLayoutEn = fragView.FindViewById<LinearLayout>(Resource.Id.linearLayoutEn);

                langLayoutAr.Click += delegate {
                    //fragView.FindViewById<ImageView>(Resource.Id.imageViewAr)
                    //.SetImageResource(Resource.Drawable.RadioChecked);

                    //fragView.FindViewById<ImageView>(Resource.Id.imageViewEn)
                    //.SetImageResource(Resource.Drawable.RadioUnChecked);

                    _Db.SaveLangId("ar-SA");

                    Restart();
                };

                langLayoutEn.Click += delegate {
                    //fragView.FindViewById<ImageView>(Resource.Id.imageViewAr)
                    //.SetImageResource(Resource.Drawable.RadioUnChecked);

                    //fragView.FindViewById<ImageView>(Resource.Id.imageViewEn)
                    //.SetImageResource(Resource.Drawable.RadioChecked);

                    _Db.SaveLangId("en-US");

                    Restart();
                };
            }
            catch (Exception ex)
            {
                Log.Error("Setting fragment", ex.Message);
            }
        }

        private void Restart()
        {
            try
            {
                Intent restart = Activity.BaseContext.PackageManager.GetLaunchIntentForPackage(
                       Activity.BaseContext.PackageName);
                restart.AddFlags(ActivityFlags.ClearTop);
                restart.AddFlags(ActivityFlags.NewTask);
                StartActivity(restart);
                //Activity.Finish();
                Java.Lang.JavaSystem.Exit(0);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
    }
}