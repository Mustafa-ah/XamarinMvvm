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
using MvvmCross.Droid.Support.V4;
using Ayadi.Core.ViewModel;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V4.View;
using Ayadi.Droid.Adapters;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Android.Support.V7.Widget;
using System.Timers;

namespace Ayadi.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel), Resource.Id.content_frame,false)]
    [Register("Ayadi.Droid.Views.SubHomeView")]
    public class SubHomeView : MvxFragment<SubHomeViewModel>
    {
        ViewPager _MainPager;
        HomeViewPagerAdapter _homeViewPagerAdapter;
        private bool _sliding = true;
        private bool _Sponsersliding = true;

        private int _interval;

        ImageView[] dotImages;

        MvxRecyclerView recyclerView;

        MvxRecyclerView _SponsersRecyclerView;
        ImageView _newxtImg;
        ImageView _PretImg;

        LinearLayout indicatorContainer;

        Timer timer;

        public new SubHomeViewModel ViewModel
        {
            get { return (SubHomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        private int _sponsersCount;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewModel.ViewModelInitialized += ViewModel_ViewModelInitialized;
            // Create your fragment here

            
        }

        private void ViewModel_ViewModelInitialized(object sender, EventArgs e)
        {
            _sponsersCount = ViewModel.Sponsers.Count;
            _interval = ViewModel.sponserSlider[0].Interval * 1000;
            timer = new Timer(_interval);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
           // makeIndication();
           // StartSponserSlider();
           
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_sponsersCount == 0)
                {
                    _sponsersCount = ViewModel.Sponsers.Count;
                }
                Activity.RunOnUiThread(() => _SponsersRecyclerView.SmoothScrollToPosition(_sponsersCount));
                // _SponsersRecyclerView.SmoothScrollToPosition(_sponsersCount);

                if (_sponsersCount > 0)
                {
                    _sponsersCount--;
                }
                
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        //public override void OnResume()
        //{
        //    base.OnResume();
        //    _sliding = true;
        //    _Sponsersliding = true;
        //}
        //public override void OnStop()
        //{
        //    base.OnStop();
        //    _sliding = false;
        //    _Sponsersliding = false;
        //}

        public override void OnStop()
        {
            base.OnStop();
            timer.Stop();
        }

        private async void StartSponserSlider()
        {
            try
            {

                int _ScrollSize = _SponsersRecyclerView.ScrollBarSize;
               // int _scrollBy = 70;
                while (_Sponsersliding)
                {
                    if (_sponsersCount < 0)
                    {
                        _sponsersCount = ViewModel.Sponsers.Count;
                    }
                     
                    // _SponsersRecyclerView.SmoothScrollBy(200, 0);
                    _SponsersRecyclerView.SmoothScrollToPosition(_sponsersCount);
                    //for (int i = 0; i < _sponsersCount; i++)
                    //{
                    //    dotImages[i].SetImageResource(Resource.Drawable.doted);
                    //    //if (i == _sponsersCount -1)
                    //    //{
                    //    //    dotImages[i].SetImageResource(Resource.Drawable.doted);
                    //    //}
                    //}
                    //if (_sponsersCount != 0)
                    //{
                    //    dotImages[_sponsersCount - 1].SetImageResource(Resource.Drawable.dimmed);
                    //}
                    /*
                     <LinearLayout
            android:orientation="horizontal"
            android:layout_width="match_parent"
            android:layout_height="10dp"
            android:gravity="center"
            android:id="@+id/IndicatorLayout"/>  
                     */
                    // dotImages[_sponsersCount - 2].SetImageResource(Resource.Drawable.dimmed);

                    await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(_interval));

                    //_ScrollSize = _ScrollSize - 5;

                    //if (_ScrollSize < 0)
                    //{
                    //    _SponsersRecyclerView.SmoothScrollToPosition(1);
                    //    _ScrollSize = _SponsersRecyclerView.ScrollBarSize;
                    //}
                    _sponsersCount--;
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.Fargment_home, null);

            //_MainPager = fragView.FindViewById<ViewPager>(Resource.Id.viewpager);
            //ViewPagerData.MainCatalog pagerData = new ViewPagerData.MainCatalog();

            //_homeViewPagerAdapter = new HomeViewPagerAdapter(Activity, pagerData);
            //_MainPager.Adapter = _homeViewPagerAdapter;

            _newxtImg = fragView.FindViewById<ImageView>(Resource.Id.imageNext);
            _PretImg = fragView.FindViewById<ImageView>(Resource.Id.imagePre);

         //   indicatorContainer = fragView.FindViewById<LinearLayout>(Resource.Id.IndicatorLayout);

            _SponsersRecyclerView = fragView.FindViewById<MvxRecyclerView>(Resource.Id.sponsors_recycler_view);
            recyclerView = fragView.FindViewById<MvxRecyclerView>(Resource.Id.products_recycler_view);
            recyclerView.Adapter = new FavouriteAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext,
                Activity, ViewModel);

            //TextView ShowAllCatsText = fragView.FindViewById<TextView>(Resource.Id.textViewShowAllCats);
            //ShowAllCatsText.Click += ShowAllCatsText_Click;

            _newxtImg.Click += _newxtImg_Click;
            _PretImg.Click += _PretImg_Click;

            //PageScrroling();
            return fragView;
            // return base.OnCreateView(inflater, container, savedInstanceState);
        }

      
        private void _PretImg_Click(object sender, EventArgs e)
        {
            try
            {
                _Sponsersliding = false;
                _SponsersRecyclerView.SmoothScrollBy(80, 0);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private void _newxtImg_Click(object sender, EventArgs e)
        {
            try
            {
                // mRecyclerView.getLayoutManager().scrollToPosition(linearLayoutManager.findLastVisibleItemPosition() + 1);
                //  _SponsersRecyclerView.GetLayoutManager().ScrollHorizontallyBy(5,_SponsersRecyclerView,MvxRecyclerView.)
                // _SponsersRecyclerView.GetLayoutManager().ScrollToPosition(MvxGuardedLinearLayoutManager.Horizontal);
                //  _SponsersRecyclerView.GetLayoutManager().ScrollToPosition(LinearLayoutManager.);
                // _SponsersRecyclerView.SmoothScrollToPosition(2);
                _Sponsersliding = false;
                _SponsersRecyclerView.SmoothScrollBy(-80, 0);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }


        private async void PageScrroling()
        {
            int _count = _homeViewPagerAdapter.Count;
            while (_sliding)
            {
                if (_count < 0)
                {
                    _count = _homeViewPagerAdapter.Count;
                }

                _MainPager.SetCurrentItem(_count, true);
                await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(3000));

                _count--;
            }

            //for (int i = 0; i < _homeViewPagerAdapter.Count; i++)
            //{
            //    _MainPager.SetCurrentItem(i, true);
            // await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(2000));
            //    //await System.Threading.Thread.Sleep(1000);
            //}
        }

        private void makeIndication()
        {
            dotImages = new ImageView[_sponsersCount];
            for (int i = 0; i < _sponsersCount; i++)
            {
                ImageView img = new ImageView(Activity);
                //LinearLayout.LayoutParams paramz = new LinearLayout.LayoutParams(10, 10);
                //img.LayoutParameters = paramz;
                if (i == _sponsersCount - 1)
                {
                    img.SetImageResource(Resource.Drawable.doted);
                }
                else
                {
                    img.SetImageResource(Resource.Drawable.dimmed);
                }
                dotImages[i] = img;
                indicatorContainer.AddView(img);
            }
        }
    }
}