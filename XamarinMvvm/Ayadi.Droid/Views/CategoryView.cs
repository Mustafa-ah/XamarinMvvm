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
using Ayadi.Droid.Adapters;
using Android.Support.V4.View;
using MvvmCross.Binding.Droid.BindingContext;

namespace Ayadi.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel), Resource.Id.content_frame, false)]
    [Register("Ayadi.Droid.Views.CategoryView")]
    public class CategoryView : MvxFragment<CategoryViewModel>
    {
        ViewPager _MainPager;
        HomeViewPagerAdapter _homeViewPagerAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.Fragment_Category, null);
            //_MainPager = fragView.FindViewById<ViewPager>(Resource.Id.viewpager);
            //ViewPagerData.MainCatalog pagerData = new ViewPagerData.MainCatalog();
            //_homeViewPagerAdapter = new HomeViewPagerAdapter(Activity, pagerData);
           // _MainPager.Adapter = _homeViewPagerAdapter;

         //   PageScrroling();
            return fragView;
        }

        private async void PageScrroling()
        {
            int _count = _homeViewPagerAdapter.Count;
            while (true)
            {
                if (_count < 0)
                {
                    _count = _homeViewPagerAdapter.Count;
                }

                _MainPager.SetCurrentItem(_count, true);
                await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(2000));

                _count--;
            }

            //for (int i = 0; i < _homeViewPagerAdapter.Count; i++)
            //{
            //    _MainPager.SetCurrentItem(i, true);
            // await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(2000));
            //    //await System.Threading.Thread.Sleep(1000);
            //}
        }
    }
}