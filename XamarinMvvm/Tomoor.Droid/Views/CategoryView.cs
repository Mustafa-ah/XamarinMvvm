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
using System.Threading;
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;

namespace Tomoor.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel), Resource.Id.content_frame, false)]
    [Register("Tomoor.Droid.Views.CategoryView")]
    public class CategoryView : MvxFragment<CategoryViewModel>
    {
        BindableProgressBar _bindableProgressBar;
        ViewPager _MainPager;
        //HomeViewPagerAdapter _homeViewPagerAdapter;

        private int _TopSliderImagesCount;
        private Timer TopSliderTimer;
        public new CategoryViewModel ViewModel
        {
            get { return (CategoryViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        bool CanChange;
        bool IsViewModelFirstInitialize;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.Fragment_Category, null);
            _MainPager = fragView.FindViewById<ViewPager>(Resource.Id.viewpager);

            _bindableProgressBar = new BindableProgressBar(Activity);
            var set = this.CreateBindingSet<CategoryView, CategoryViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

            ViewModel.ViewModelInitialized += ViewModel_ViewModelInitialized;
           
            return fragView;
        }

        private void ViewModel_ViewModelInitialized(object sender, EventArgs e)
        {
            try
            {
                _MainPager.Adapter = new HomeViewPagerAdapter(Activity, ViewModel.SliderImages);
                // TopSliderTimer = new Timer(x => TopSliderTimer_Elapsed(), null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
                _TopSliderImagesCount = ViewModel.SliderImages.Count;
                if (!IsViewModelFirstInitialize)
                {
                    CanChange = true;
                    ChangePage();
                    IsViewModelFirstInitialize = true;
                    
                }

            }
            catch (Exception)
            {

                //throw;//x
            }
        }
        public override void OnStop()
        {
            base.OnStop();
            //TopSliderTimer?.Dispose();
            CanChange = false;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (ViewModel.SliderImages != null)
            {
                if (!CanChange )
                {
                    CanChange = true;
                    ChangePage();
                }
                
            }
        }
       

        private async void ChangePage()
        {
            try
            {
                
                for (int i = 0; i < double.MaxValue; i++)
                {
                    if (CanChange)
                    {

                        if (_TopSliderImagesCount == -1)
                        {
                            _TopSliderImagesCount = ViewModel.SliderImages.Count;
                        }
                        Activity.RunOnUiThread(() => _MainPager.SetCurrentItem(_TopSliderImagesCount, true));
                        _TopSliderImagesCount--;
                        await System.Threading.Tasks.Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(5)); });
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
    }
}