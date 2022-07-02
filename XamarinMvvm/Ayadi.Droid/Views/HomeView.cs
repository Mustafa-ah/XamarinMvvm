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
using MvvmCross.Droid.Support.V7.AppCompat;
using Ayadi.Core.ViewModel;
using Android.Support.V4.View;
using Ayadi.Droid.Adapters;
using Android.Support.V4.App;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Support.V4;

namespace Ayadi.Droid.Views
{
    [Activity(Label = "HomeView")]
    public class HomeView : MvxCachingFragmentCompatActivity<HomeViewModel>
    {
        //ViewPager _MainPager;
        //HomeViewPagerAdapter _homeViewPagerAdapter;

      //  private Android.App.FragmentManager _fragmentManager;


        static HomeView instance = new HomeView();

        public static HomeView CurrentActivity => instance;

        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        LinearLayout _homeLayout;
        //LinearLayout _CatsLayout;
        //LinearLayout _StorsLayout;
        //LinearLayout _UserLayout;
        //LinearLayout _SettingLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_Home);

            ViewModel.ShowSubHome();
           // ViewModel.ShowSortView();
         //   _fragmentManager = FragmentManager;

            //_MainPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            //ViewPagerData.MainCatalog pagerData = new ViewPagerData.MainCatalog();
            //_homeViewPagerAdapter = new HomeViewPagerAdapter(this, pagerData);
            //_MainPager.Adapter = _homeViewPagerAdapter;

            //PageScrroling();

            _homeLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Home);
            //_CatsLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Cats);
            //_StorsLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Stors);
            //_UserLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Account);
            //_SettingLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutF_Setting);

           // _homeLayout.Click += _homeLayout_Click;
           // _CatsLayout.Click += _CatsLayout_Click;
            //_StorsLayout.Click += _StorsLayout_Click;
            //_UserLayout.Click += _UserLayout_Click;
            //_SettingLayout.Click += _SettingLayout_Click;

            _homeLayout.SetBackgroundColor(Android.Graphics.Color.Rgb(141, 8, 146));
        }
        /*
        private void _SettingLayout_Click(object sender, EventArgs e)
        {
            _homeLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _CatsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _StorsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _UserLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _SettingLayout.SetBackgroundColor(Android.Graphics.Color.CadetBlue);
        }

        private void _UserLayout_Click(object sender, EventArgs e)
        {
            _homeLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _CatsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _StorsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _UserLayout.SetBackgroundColor(Android.Graphics.Color.CadetBlue);
            _SettingLayout.SetBackgroundColor(Android.Graphics.Color.White);
        }

        private void _StorsLayout_Click(object sender, EventArgs e)
        {
            _homeLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _CatsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _StorsLayout.SetBackgroundColor(Android.Graphics.Color.CadetBlue);
            _UserLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _SettingLayout.SetBackgroundColor(Android.Graphics.Color.White);
        }

        private void _CatsLayout_Click(object sender, EventArgs e)
        {
            CatsLayoutClick();
        }

        // case we gona call it from subHome Fragment
        public void CatsLayoutClick()
        {
            _homeLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _CatsLayout.SetBackgroundColor(Android.Graphics.Color.CadetBlue);
            _StorsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _UserLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _SettingLayout.SetBackgroundColor(Android.Graphics.Color.White);
        }

        private void _homeLayout_Click(object sender, EventArgs e)
        {
            _homeLayout.SetBackgroundColor(Android.Graphics.Color.CadetBlue);
            _CatsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _StorsLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _UserLayout.SetBackgroundColor(Android.Graphics.Color.White);
            _SettingLayout.SetBackgroundColor(Android.Graphics.Color.White);
        }
        */
        //public override void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo, Android.Support.V4.App.FragmentTransaction transaction)
        //{
        //    var currentFrag = SupportFragmentManager.FindFragmentById(Resource.Id.content_frame) as MvxFragment;

        //    //if (currentFrag != null && fragmentInfo.ViewModelType != typeof(MenuViewModel)
        //    //    && currentFrag.FindAssociatedViewModelType() != fragmentInfo.ViewModelType)
        //    //    fragmentInfo.AddToBackStack = true;
        //    base.OnBeforeFragmentChanging(fragmentInfo, transaction);
        //}

        //private async void PageScrroling()
        //{
        //    int _count = _homeViewPagerAdapter.Count;
        //    while (true)
        //    {
        //        if (_count < 0)
        //        {
        //            _count= _homeViewPagerAdapter.Count;
        //        }

        //      _MainPager.SetCurrentItem(_count, true);
        //     await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(2000));

        //        _count--;
        //    }

        //    //for (int i = 0; i < _homeViewPagerAdapter.Count; i++)
        //    //{
        //    //    _MainPager.SetCurrentItem(i, true);
        //    // await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(2000));
        //    //    //await System.Threading.Thread.Sleep(1000);
        //    //}
        //}
    }
}