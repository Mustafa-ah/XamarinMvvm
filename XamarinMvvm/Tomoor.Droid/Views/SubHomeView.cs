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
using Tomoor.Droid.Adapters;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Android.Support.V7.Widget;
using System.Timers;
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel), Resource.Id.content_frame,false)]
    [Register("Tomoor.Droid.Views.SubHomeView")]
    public class SubHomeView : MvxFragment<SubHomeViewModel>
    {
        BindableProgressBar _bindableProgressBar;
        ViewPager _MainPager;
        //HomeViewPagerAdapter _homeViewPagerAdapter;
        //private bool _sliding = true;
        private bool _Sponsersliding = true;

        bool CanTopPagerChange;
        bool IsViewModelFirstInitialize;

        private int _interval;

        ImageView[] dotImages;

        MvxRecyclerView recyclerView;

        MvxRecyclerView _SponsersRecyclerView;
        ImageView _newxtImg;
        ImageView _PretImg;

        LinearLayout indicatorContainer;

        Timer SponsersTimer;
        Timer TopSliderTimer;
        public new SubHomeViewModel ViewModel
        {
            get { return (SubHomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        private int _sponsersCount;
        private int _TopSliderImagesCount;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ViewModel.ViewModelInitialized += ViewModel_ViewModelInitialized;
            // Create your fragment here
        }

        private async void ChangeTopPage()
        {
            try
            {
                for (int i = 0; i < double.MaxValue; i++)
                {
                    if (CanTopPagerChange)
                    {

                        if (_TopSliderImagesCount == -1)
                        {
                            _TopSliderImagesCount = ViewModel.SliderImages.Count;
                        }
                        Activity.RunOnUiThread(() => _MainPager.SetCurrentItem(_TopSliderImagesCount, true));
                        _TopSliderImagesCount--;
                        await System.Threading.Tasks.Task.Run(() => { System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5)); });
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("cat timer", ex.Message);
                //throw;//x
            }
        }

        private void ViewModel_ViewModelInitialized(object sender, EventArgs e)
        {
            _MainPager.Adapter = new HomeViewPagerAdapter(Activity, ViewModel.SliderImages);
            if (ViewModel.Sponsers != null)
            {
                if (SponsersTimer == null)
                {
                    _sponsersCount = ViewModel.Sponsers.Count;
                    _interval = ViewModel.sponserSlider[0].Interval * 1000;
                    SponsersTimer = new Timer(_interval);
                    SponsersTimer.Elapsed += Timer_Elapsed;
                    SponsersTimer.Start();
                }
                
            }


            // main slider
            if (!IsViewModelFirstInitialize)
            {
                
                _TopSliderImagesCount = ViewModel.SliderImages.Count;
                CanTopPagerChange = true;
                ChangeTopPage();
                IsViewModelFirstInitialize = true;
            }
            //if (ViewModel.SliderImages != null)
            //{
            //    if (TopSliderTimer == null)
            //    {
            //        TopSliderTimer = new Timer(7000);
            //        TopSliderTimer.Elapsed += TopSliderTimer_Elapsed;
            //        TopSliderTimer.Start();
            //        _TopSliderImagesCount = ViewModel.SliderImages.Count;
            //        _MainPager.Adapter = new ProductViewPagerAdapter(Activity, ViewModel.SliderImages);
            //    }


            //    //TimerHomeState st_ = new TimerHomeState();
            //    //System.Threading.TimerCallback timerDelegate = new System.Threading.TimerCallback(CheckStatus);
            //    //System.Threading.Timer tim = new System.Threading.Timer(timerDelegate, st_, 1000, 1000);
            //    //st_.tmr = tim;

            //}

            // makeIndication();
            // StartSponserSlider();

        }

        //static void CheckStatus(Object state)
        //{
        //    TimerHomeState s = (TimerHomeState)state;
        //    s.counter++;
        //    Console.WriteLine("{0} Checking Status {1}.", DateTime.Now.TimeOfDay, s.counter);
        //    if (s.counter == 5)
        //    {
        //        // Shorten the period. Wait 10 seconds to restart the timer.
        //        (s.tmr).Change(10000, 100);
        //        Console.WriteLine("changed...");
        //    }
        //    if (s.counter == 10)
        //    {
        //        Console.WriteLine("disposing of timer...");
        //        s.tmr.Dispose();
        //        s.tmr = null;
        //    }
        //}

        private void TopSliderTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (ViewModel.SliderImages == null)
                {
                    return; 
                }
                if (_TopSliderImagesCount < 0)
                {
                    _TopSliderImagesCount = ViewModel.SliderImages.Count;
                }
                Activity.RunOnUiThread(()=> _MainPager.SetCurrentItem(_TopSliderImagesCount, true));
                _TopSliderImagesCount--;
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("home timer", ex.Message);
                TopSliderTimer?.Dispose();
                ////throw;//x
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (ViewModel.Sponsers == null)
                {
                    return;
                }
                if (_sponsersCount < 1)
                {
                    _sponsersCount = ViewModel.Sponsers.Count;
                }
                Activity.RunOnUiThread(() => ScrollLisTo(_sponsersCount));
                // _SponsersRecyclerView.SmoothScrollToPosition(_sponsersCount);

                if (_sponsersCount > 0)
                {
                    _sponsersCount--;
                }
                
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("home timer", ex.Message);
                ////throw;//x
            }
        }

        private void ScrollLisTo(int position)
        {
            if (position > -1)
            {
                _SponsersRecyclerView.SmoothScrollToPosition(position);
            }
        }

        public override void OnStop()
        {
            base.OnStop();
            CanTopPagerChange = false;
            SponsersTimer?.Stop();
        }

        public override void OnResume()
        {
            base.OnResume();
            if (ViewModel.SliderImages != null)
            {
                if (!CanTopPagerChange)
                {
                    CanTopPagerChange = true;
                    ChangeTopPage();
                }

            }
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

            _MainPager = fragView.FindViewById<ViewPager>(Resource.Id.viewpager);
            //ViewPagerData.MainCatalog pagerData = new ViewPagerData.MainCatalog();

            //_homeViewPagerAdapter = new HomeViewPagerAdapter(Activity, pagerData);
            //_MainPager.Adapter = _homeViewPagerAdapter;

            _newxtImg = fragView.FindViewById<ImageView>(Resource.Id.imageNext);
            _PretImg = fragView.FindViewById<ImageView>(Resource.Id.imagePre);

         //   indicatorContainer = fragView.FindViewById<LinearLayout>(Resource.Id.IndicatorLayout);

            _SponsersRecyclerView = fragView.FindViewById<MvxRecyclerView>(Resource.Id.sponsors_recycler_view);
            recyclerView = fragView.FindViewById<MvxRecyclerView>(Resource.Id.products_recycler_view);
            //recyclerView.Adapter = new FavouriteAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext,
            //    Activity, ViewModel);

            //TextView ShowAllCatsText = fragView.FindViewById<TextView>(Resource.Id.textViewShowAllCats);
            //ShowAllCatsText.Click += ShowAllCatsText_Click;

            _bindableProgressBar = new BindableProgressBar(Activity);
            var set = this.CreateBindingSet<SubHomeView, SubHomeViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

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


        //private async void PageScrroling()
        //{
        //    int _count = _homeViewPagerAdapter.Count;
        //    while (_sliding)
        //    {
        //        if (_count < 0)
        //        {
        //            _count = _homeViewPagerAdapter.Count;
        //        }

        //        _MainPager.SetCurrentItem(_count, true);
        //        await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(3000));

        //        _count--;
        //    }

        //    //for (int i = 0; i < _homeViewPagerAdapter.Count; i++)
        //    //{
        //    //    _MainPager.SetCurrentItem(i, true);
        //    // await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(2000));
        //    //    //await System.Threading.Thread.Sleep(1000);
        //    //}
        //}

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

    //class TimerHomeState
    //{
    //    public int counter = 0;
    //    public System.Threading.Timer tmr;
    //}
}