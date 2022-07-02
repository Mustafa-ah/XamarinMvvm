using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using Ayadi.Core.ViewModel;
using MvvmCross.Droid.Support.V4;
using Tomoor.Droid.Adapters;
using Android.Support.V4.View;
using MvvmCross.Binding.Droid.BindingContext;
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel), Resource.Id.content_frame, false)]
    [Register("Tomoor.Droid.Views.UserAccountView")]
    public class UserAccountView : MvxFragment<UserAccountViewModel>
    {

        BindableProgressBar _bindableProgressBar;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.fragment_user_account, null);
            try
            {
               
                _bindableProgressBar = new BindableProgressBar(Activity);
                var set = this.CreateBindingSet<UserAccountView, UserAccountViewModel>();
                set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
                set.Apply();

                //Button bt = fragView.FindViewById<Button>(Resource.Id.buttonLogout);
                //bt.Click += (s, e) => { StartActivity(new Intent(Activity, typeof(UserDataView))); };
            }
            catch (Java.Lang.OutOfMemoryError ex)
            {

            }
            catch (Exception)
            {

                //throw;//x
            }
            return fragView;
        }
    }
}