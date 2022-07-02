using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Ayadi.Core.Contracts.Services;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tomoor.IOS.Utility;

namespace Tomoor.IOS.Services
{
    public class LoadingService : ILoadingDataService
    {
        //https://forums.xamarin.com/discussion/24689/how-to-acces-the-current-view-uiviewcontroller-from-an-external-service
        //UIWindow window = UIApplication.SharedApplication.KeyWindow;
        //UIViewController vc;
        LoadingOverlay loading;
        public LoadingService()
        {
            //vc = window.RootViewController;
            //if (vc.PresentedViewController != null)
            //{
            //    vc = vc.PresentedViewController;
            //    loading = new LoadingOverlay(vc.View);
            //}
        }
        public void HideFragmentLoading()
        {
            HideLoading();
        }

        public void HideLoading()
        {
            if (loading != null)
            {
                loading.Hide();
            }
        }

        public Task ShowFragmentLoading()
        {
            return ShowLoading();
        }

        public void ShowLFragmentLoading()
        {
            ShowLoading();
        }

        public Task ShowLoading()
        {
            try
            {
                UIWindow window = UIApplication.SharedApplication.KeyWindow;
                UIViewController vc;
                vc = window.RootViewController;
                return Task.Run(() => UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    if (vc.PresentedViewController != null)
                    {
                        vc = vc.PresentedViewController;
                        loading = new LoadingOverlay(vc.View);
                        vc.View.Add(loading);
                    }

                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.FromResult(0);
            }
            
        }
    }
}