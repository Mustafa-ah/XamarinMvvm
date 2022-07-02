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
using Tomoor.Droid.Adapters;
using Android.Support.V4.View;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Content;
using Android.App;
using System.Threading.Tasks;
using Ayadi.Core.Repositories;
using Ayadi.Core.Contracts.Repository;
using MvvmCross.Platform;

namespace Tomoor.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel), Resource.Id.content_frame, false)]
    [Register("Tomoor.Droid.Views.SettingView")]
    public class SettingView : MvxFragment<SettingViewModel>
    {
        XmlDb _Db;

        ImageView _langAr;
        ImageView _langEn;

        LinearLayout langLayoutAr;
        LinearLayout langLayoutEn;

        public new SettingViewModel ViewModel
        {
            get { return (SettingViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _Db = new XmlDb(Activity);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.Fragment_Setting, null);

            _langAr = fragView.FindViewById<ImageView>(Resource.Id.imageViewAr);
            _langEn = fragView.FindViewById<ImageView>(Resource.Id.imageViewEn);

            langLayoutAr = fragView.FindViewById<LinearLayout>(Resource.Id.linearLayoutAr);
            langLayoutEn = fragView.FindViewById<LinearLayout>(Resource.Id.linearLayoutEn);

            langLayoutAr.Click += delegate { ChangLangAr(); };

            langLayoutEn.Click += delegate { ChangLangEn(); };

            SetLangChange();
            return fragView;
        }

        private void SetLangChange()
        {
            try
            {
                string _currentLang = _Db.getSavedLangId();

                if (_currentLang == "0")
                {
                    if (Java.Util.Locale.Default.Language == "ar")
                    {
                        _langAr.SetImageResource(Resource.Drawable.RadioChecked);
                        _langEn.SetImageResource(Resource.Drawable.RadioUnChecked);
                        langLayoutAr.Enabled = false;
                    }
                    else
                    {
                        _langAr.SetImageResource(Resource.Drawable.RadioUnChecked);
                        _langEn.SetImageResource(Resource.Drawable.RadioChecked);
                        langLayoutEn.Enabled = false;
                    }
                }
                else
                {
                    if (_currentLang == "ar-SA")
                    {
                        _langAr.SetImageResource(Resource.Drawable.RadioChecked);

                        _langEn.SetImageResource(Resource.Drawable.RadioUnChecked);
                        langLayoutAr.Enabled = false;
                    }
                    else
                    {
                        _langAr.SetImageResource(Resource.Drawable.RadioUnChecked);

                        _langEn.SetImageResource(Resource.Drawable.RadioChecked);
                        langLayoutEn.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error("Setting fragment", ex.Message);
            }
        }

        private void ChangLangAr()
        {
            try
            {
               
                //_langAr.SetImageResource(Resource.Drawable.RadioChecked);
                //_langEn.SetImageResource(Resource.Drawable.RadioUnChecked);
                _Db.SaveLangId("ar-SA");

                var builder = new AlertDialog.Builder(Activity);
                builder.SetIconAttribute
                    (Android.Resource.Attribute.AlertDialogIcon);
                builder.SetTitle(ViewModel.Tomoor);
                builder.SetMessage(ViewModel.RestatMsg);
                builder.SetPositiveButton(ViewModel.Ok,
                    delegate {
                        Intent restart = Activity.BaseContext.PackageManager.GetLaunchIntentForPackage(
                        Activity.BaseContext.PackageName);
                        restart.AddFlags(ActivityFlags.ClearTop);
                        restart.AddFlags(ActivityFlags.NewTask);
                        StartActivity(restart);
                        //Activity.Finish();
                        Java.Lang.JavaSystem.Exit(0);
                    });
                builder.SetNegativeButton(ViewModel.Cancel,
                    delegate {
                        _Db.SaveLangId("en-US");
                        ViewModel.saveLangId(Ayadi.Core.Model.Constants.LangIdEn);
                    });
                builder.Create().Show();

                Task.Factory.StartNew(() =>
                {
                    ViewModel.ClearCashedData();
                    ViewModel.saveLangId(Ayadi.Core.Model.Constants.LangIdAr);
                });

            }
            catch (Exception ex)
            {
                // Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                //throw;//x
            }
        }

        private void ChangLangEn()
        {
            try
            {
                //_langAr.SetImageResource(Resource.Drawable.RadioChecked);
                //_langEn.SetImageResource(Resource.Drawable.RadioUnChecked);
                _Db.SaveLangId("en-US");

                var builder = new AlertDialog.Builder(Activity);
                builder.SetIconAttribute
                    (Android.Resource.Attribute.AlertDialogIcon);
                builder.SetTitle(ViewModel.Tomoor);
                builder.SetMessage(ViewModel.RestatMsg);
                builder.SetPositiveButton(ViewModel.Ok,
                    delegate {
                        Intent restart = Activity.BaseContext.PackageManager.GetLaunchIntentForPackage(
                        Activity.BaseContext.PackageName);
                        restart.AddFlags(ActivityFlags.ClearTop);
                        restart.AddFlags(ActivityFlags.NewTask);
                        StartActivity(restart);
                        //Activity.Finish();
                        Java.Lang.JavaSystem.Exit(0);
                    });
                builder.SetNegativeButton(ViewModel.Cancel,
                    delegate {
                        _Db.SaveLangId("ar-SA");
                        ViewModel.saveLangId(Ayadi.Core.Model.Constants.LangIdAr);
                    });
                builder.Create().Show();

                Task.Factory.StartNew(async () =>
                {
                    ViewModel.ClearCashedData();
                    ViewModel.saveLangId(Ayadi.Core.Model.Constants.LangIdEn);

                    ICartRepository cartRepo = Mvx.Resolve<ICartRepository>();
                    var shopingList = await cartRepo.GetShoppingCartItemsFromAPI(ViewModel._user);
                    cartRepo.SetActiveShoppingList(shopingList);
                    //await cartRepo.SaveShopingCartToLocal(shopingList);
                });

            }
            catch 
            {
                //Toast.MakeText(Activity, ex.Message, ToastLength.Long).Show();
                //throw;//x
            }
        }
        /*
        private void Restart( string langID)
        {
            try
            {
                ViewModel.ClearCashedData();
                var builder = new AlertDialog.Builder(Activity);
                builder.SetIconAttribute
                    (Android.Resource.Attribute.AlertDialogIcon);
                builder.SetTitle(ViewModel.Tomoor);
                builder.SetMessage(ViewModel.RestatMsg);
                builder.SetPositiveButton(ViewModel.Ok,
                   async delegate {
                       await clearData(langID);
                       Intent restart = Activity.BaseContext.PackageManager.GetLaunchIntentForPackage(
                       Activity.BaseContext.PackageName);
                        restart.AddFlags(ActivityFlags.ClearTop);
                        restart.AddFlags(ActivityFlags.NewTask);
                       StartActivity(restart);
                       //Activity.Finish();
                       Java.Lang.JavaSystem.Exit(0);
                    });
                builder.SetNegativeButton(ViewModel.Cancel,
                    delegate { });
                builder.Create().Show();
               
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
        private async Task clearData(string langID)
        {
            await Task.Run(() =>
            {
                ViewModel.ClearCashedData();
                ViewModel.saveLangId(langID);
            });
            
        }
        */
    }
}