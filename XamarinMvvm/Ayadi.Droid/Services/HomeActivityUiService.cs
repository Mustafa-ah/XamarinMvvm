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
using Ayadi.Core.Contracts.Services;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform;
using Android.Graphics;

namespace Ayadi.Droid.Services
{
    public class HomeActivityUiService : IHomeActivityUiService
    {
        protected Activity CurrentActivity =>
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        LinearLayout _homeLayout;
        LinearLayout _CatsLayout;
        LinearLayout _StorsLayout;
        LinearLayout _UserLayout;
        LinearLayout _SettingLayout;

        ImageView _homeImage;
        ImageView _CatsImage;
        ImageView _StorsImage;
        ImageView _UserImage;
        ImageView _SettingImage;


        TextView _homeTxt;
        TextView _CatsTxt;
        TextView _StorsTxt;
        TextView _UserTxt;
        TextView _SettingTxt;

        public void HomeActivityUiServiceInit()
        {
            _homeLayout = CurrentActivity.FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Home);
            _CatsLayout = CurrentActivity.FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Cats);
            _StorsLayout = CurrentActivity.FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Stors);
            _UserLayout = CurrentActivity.FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Account);
            _SettingLayout = CurrentActivity.FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Setting);

            _homeImage = CurrentActivity.FindViewById<ImageView>(Resource.Id.imageViewHome);
            _CatsImage = CurrentActivity.FindViewById<ImageView>(Resource.Id.imageViewCats);
            _StorsImage = CurrentActivity.FindViewById<ImageView>(Resource.Id.imageViewStors);
            _UserImage = CurrentActivity.FindViewById<ImageView>(Resource.Id.imageViewAccount);
            _SettingImage = CurrentActivity.FindViewById<ImageView>(Resource.Id.imageViewSettings);

            _homeTxt = CurrentActivity.FindViewById<TextView>(Resource.Id.textViewF_Home);
            _CatsTxt = CurrentActivity.FindViewById<TextView>(Resource.Id.textViewF_Cats);
            _StorsTxt = CurrentActivity.FindViewById<TextView>(Resource.Id.textViewF_Stors);
            _UserTxt = CurrentActivity.FindViewById<TextView>(Resource.Id.textViewF_Account);
            _SettingTxt = CurrentActivity.FindViewById<TextView>(Resource.Id.textViewF_Setting);
        }
        public void SetAccountFoucs()
        {
            HomeActivityUiServiceInit();
            Application.SynchronizationContext.Post(ignored =>
            {
                _homeLayout.SetBackgroundColor(Color.White);
                _CatsLayout.SetBackgroundColor(Color.White);
                _StorsLayout.SetBackgroundColor(Color.White);
                _UserLayout.SetBackgroundColor(Color.Rgb(141, 8, 146));
                _SettingLayout.SetBackgroundColor(Color.White);

                _homeImage.SetImageResource(Resource.Drawable.Home);
                _CatsImage.SetImageResource(Resource.Drawable.tabs);
                _StorsImage.SetImageResource(Resource.Drawable.Stor_icon);
                _UserImage.SetImageResource(Resource.Drawable.user_white);
                _SettingImage.SetImageResource(Resource.Drawable.setting);

                _homeTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _CatsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _StorsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _UserTxt.SetTextColor(Color.White);
                _SettingTxt.SetTextColor(Color.Rgb(141, 8, 146));

            }, null);
        }

        public void SetCatsFoucs()
        {
            HomeActivityUiServiceInit();
            Application.SynchronizationContext.Post(ignored =>
            {
                _homeLayout.SetBackgroundColor(Color.White);
                _CatsLayout.SetBackgroundColor(Color.Rgb(141, 8, 146));
                _StorsLayout.SetBackgroundColor(Color.White);
                _UserLayout.SetBackgroundColor(Color.White);
                _SettingLayout.SetBackgroundColor(Color.White);

                _homeImage.SetImageResource(Resource.Drawable.Home);
                _CatsImage.SetImageResource(Resource.Drawable.tabs_white);
                _StorsImage.SetImageResource(Resource.Drawable.Stor_icon);
                _UserImage.SetImageResource(Resource.Drawable.user);
                _SettingImage.SetImageResource(Resource.Drawable.setting);

                _homeTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _CatsTxt.SetTextColor(Color.White);
                _StorsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _UserTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _SettingTxt.SetTextColor(Color.Rgb(141, 8, 146));

            }, null);
        }

        public void SetHomeFoucs()
        {
            HomeActivityUiServiceInit();
            Application.SynchronizationContext.Post(ignored =>
            {
                _homeLayout.SetBackgroundColor(Color.Rgb(141, 8, 146));
                _CatsLayout.SetBackgroundColor(Color.White);
                _StorsLayout.SetBackgroundColor(Color.White);
                _UserLayout.SetBackgroundColor(Color.White);
                _SettingLayout.SetBackgroundColor(Color.White);

                _homeImage.SetImageResource(Resource.Drawable.homet_white);
                _CatsImage.SetImageResource(Resource.Drawable.tabs);
                _StorsImage.SetImageResource(Resource.Drawable.Stor_icon);
                _UserImage.SetImageResource(Resource.Drawable.user);
                _SettingImage.SetImageResource(Resource.Drawable.setting);

                _homeTxt.SetTextColor(Color.White);
                _CatsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _StorsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _UserTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _SettingTxt.SetTextColor(Color.Rgb(141, 8, 146));

            }, null);
        }

        public void SetSettingFoucs()
        {
            HomeActivityUiServiceInit();
            Application.SynchronizationContext.Post(ignored =>
            {
                _homeLayout.SetBackgroundColor(Color.White);
                _CatsLayout.SetBackgroundColor(Color.White);
                _StorsLayout.SetBackgroundColor(Color.White);
                _UserLayout.SetBackgroundColor(Color.White);
                _SettingLayout.SetBackgroundColor(Color.Rgb(141, 8, 146));

                _homeImage.SetImageResource(Resource.Drawable.Home);
                _CatsImage.SetImageResource(Resource.Drawable.tabs);
                _StorsImage.SetImageResource(Resource.Drawable.Stor_icon);
                _UserImage.SetImageResource(Resource.Drawable.user);
                _SettingImage.SetImageResource(Resource.Drawable.Setting_white);

                _homeTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _CatsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _StorsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _UserTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _SettingTxt.SetTextColor(Color.White);
            }, null);
        }

        public void SetStoreFoucs()
        {
            HomeActivityUiServiceInit();
            Application.SynchronizationContext.Post(ignored =>
            {
                _homeLayout.SetBackgroundColor(Color.White);
                _CatsLayout.SetBackgroundColor(Color.White);
                _StorsLayout.SetBackgroundColor(Color.Rgb(141, 8, 146));
                _UserLayout.SetBackgroundColor(Color.White);
                _SettingLayout.SetBackgroundColor(Color.White);

                _homeImage.SetImageResource(Resource.Drawable.Home);
                _CatsImage.SetImageResource(Resource.Drawable.tabs);
                _StorsImage.SetImageResource(Resource.Drawable.store_white);
                _UserImage.SetImageResource(Resource.Drawable.user);
                _SettingImage.SetImageResource(Resource.Drawable.setting);

                _homeTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _CatsTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _StorsTxt.SetTextColor(Color.White);
                _UserTxt.SetTextColor(Color.Rgb(141, 8, 146));
                _SettingTxt.SetTextColor(Color.Rgb(141, 8, 146));
            }, null);
        }
    }
}