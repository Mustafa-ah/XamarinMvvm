using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Ayadi.Core.Contracts.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Ayadi.Droid.Views;

namespace Ayadi.Droid.Services
{
    class LoadingService : ILoadingDataService
    {
        protected Activity CurrentActivity =>
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        ViewGroup layout;
        ProgressBar pb;

        LoadingFragment loadingFragment;

        public Task ShowLoading()
        {
            return Task.Run(() =>
            {
                MakeLoading();
            });
        }

        public void HideLoading()
        {
            try
            {
                if (layout != null && pb != null) 
                {
                    layout.RemoveView(pb);
                }
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("loading service ", ex.Message);
            }
        }

        private void MakeLoading()
        {
            try
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    layout = (ViewGroup)CurrentActivity.FindViewById(Android.Resource.Id.Content).RootView;
                    pb = new ProgressBar(CurrentActivity, null, Android.Resource.Attribute.ProgressBarStyleLargeInverse);

                    pb.Indeterminate = true;
                    pb.Visibility = ViewStates.Visible;

                    RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(layout.LayoutParameters);

                    param.AddRule(LayoutRules.CenterInParent);
                    pb.LayoutParameters = param;
                    layout.AddView(pb);
                }, null);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("loading service ", ex.Message);
            }
        }

        public void ShowLFragmentLoading()
        {
            MakeFragmentLoading();
            //return Task.Run(() =>
            //{
            //    MakeFragmentLoading();
            //});
        }

        private void MakeFragmentLoading()
        {
            try
            {
                loadingFragment = new LoadingFragment();
                loadingFragment.Cancelable = false;
                FragmentTransaction transaction = CurrentActivity.FragmentManager.BeginTransaction();
                loadingFragment.Show(transaction, "Laoding");
                //Application.SynchronizationContext.Post(ignored =>
                //{
                //    loadingFragment = new LoadingFragment();
                //    loadingFragment.Cancelable = false;
                //    //Bundle arguments = new Bundle();
                //    //arguments.PutInt("userAddressId", 0);
                //    //arguments.PutString("resIdList", "0");
                //    //loadingFragment.Arguments = arguments;
                //    FragmentTransaction transaction = CurrentActivity.FragmentManager.BeginTransaction();
                //    loadingFragment.Show(transaction, "Laoding");
                //}, null);
                

            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("loading service ", ex.Message);
                //throw;//x
            }
        }

        public void HideFragmentLoading()
        {
            try
            {
                loadingFragment.Dismiss();
                Android.Util.Log.Error("loading service ",".............................dissmis loder..........................");
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("loading service ", ex.Message);
                //throw;//x
            }
        }

        public Task ShowFragmentLoading()
        {
            return Task.Run(() =>
            {
                MakeFragmentLoading();
            });
        }
    }
}