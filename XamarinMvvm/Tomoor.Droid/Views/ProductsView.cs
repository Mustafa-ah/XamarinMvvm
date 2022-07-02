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
using MvvmCross.Droid.Views;
using Ayadi.Core.ViewModel;
using MvvmCross.Binding.Droid.Views;
using Tomoor.Droid.Adapters;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V4.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Binding.BindingContext;
using Tomoor.Droid.Utility;
using Java.Lang;
using Android.Util;
using Android.Support.V4.View;
using Android.Support.V7.Widget;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "ProductsView")]
    public class ProductsView : MvxCachingFragmentCompatActivity<ProductsViewModel> 
    {
        BindableProgressBar _bindableProgressBar;
        private DrawerLayout _drawerLayout;
        //private MvxActionBarDrawerToggle _drawerToggle;
        XmlDb _Db;
        public new ProductsViewModel ViewModel
        {
            get { return (ProductsViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        private int _currentPage = 1;
        private bool _isStartPreparePage;
        private bool _canLoadMore;
        private bool _canLoadPrevious;
        private string DisplayStyle;

        private int _scrollUpCounter;

        ImageView _imageSorting;
        ImageView _imagefiltering;
      //  MvxGridView gridView;
        MvxRecyclerView recyclerView;
       // FavouriteAnimatorGridViewAdapter _gridViewAdapter;

        ProgressBar _loder;

        private readonly object _scrollLockObject = new object();
        private const int LoadNextItemsThreshold = 9;


        protected override void OnCreate(Bundle savedInstanceState)
        {
         
            try
            {
                base.OnCreate(savedInstanceState);
                //
                SetContentView(Resource.Layout.Activity_Products);

                _Db = new XmlDb(this);
                DisplayStyle = _Db.GetProductsStyle();



               // gridView = FindViewById<MvxGridView>(Resource.Id.Products_Grid);
                recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.Products_List);

                _loder = FindViewById<ProgressBar>(Resource.Id.Loder_);

                _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
                _drawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow_light, (int)GravityFlags.Start);

                _imageSorting = FindViewById<ImageView>(Resource.Id.imageViewSorting);
                _imagefiltering = FindViewById<ImageView>(Resource.Id.imageViewFilter);


                SetListOrGridView(DisplayStyle);

                // recyclerView.Adapter = new FavouriteAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext,
                //this, ViewModel);

                //_gridViewAdapter = new FavouriteAnimatorGridViewAdapter(this, ViewModel,(IMvxAndroidBindingContext)BindingContext);
                //gridView.Adapter = _gridViewAdapter;



                _bindableProgressBar = new BindableProgressBar(this);
                var set = this.CreateBindingSet<ProductsView, ProductsViewModel>();
                set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
                set.Apply();


                _imageSorting.Click += _imageSorting_Click;
                _imagefiltering.Click += _imagefiltering_Click;
                ViewModel.ProductsSorted += _drawerLayout_Click;
                ViewModel.ProductsViewChanged += ViewModel_ProductsViewChanged;
                ViewModel.CategoryChosed += ViewModel_CategoryChosed;

                //https://www.codeproject.com/Articles/1082055/Xamarin-Android-EventHandlers-for-ScrollView-for-E
                //gridView.ViewTreeObserver.AddOnScrollChangedListener(this);
                 //  gridView.Scroll += GridView_Scroll;
                ViewModel.PagePrepared += ViewModel_PagePrepared;
                // gridView.Touch += GridView_Touch;
               // gridView.ScrollStateChanged += GridView_ScrollStateChanged;
               // recyclerView.

                ViewModel.ShowSortView();
                ViewModel.ShowFillterView();
            }
            catch (OutOfMemoryError ex)
            {
                string ed = ex.Message;
               // throw;
            }

            catch(System.Exception ex)
            {
                string jkj = ex.Message;
            }
            //  SetListOrGrid(DisplayStyle);
            

           
        }

        private void ViewModel_PagePrepared(object sender, EventArgs e)
        {
            try
            {
                _loder.Visibility = ViewStates.Gone;
               
            }
            catch (System.Exception)
            {

                //throw;//x
            }
        }

      

        //private void GridView_ScrollChange(object sender, View.ScrollChangeEventArgs e)
        //{
        //    //Toast.MakeText(this, "X: " + e.ScrollX + " Y: " + e.ScrollY, ToastLength.Short).Show();
        //    Android.Util.Log.Debug("...........scrolling.......", "........... X: " + e.ScrollX + " Y: " + e.ScrollY);
        //}

        private void ViewModel_CategoryChosed(object sender, EventArgs e)
        {
            _drawerLayout.CloseDrawer((int)GravityFlags.End, true);
           // Android.Util.Log.Debug("eeeeeeeee", "............................closed...............");
        }

        private void ViewModel_ProductsViewChanged(object sender, EventArgs e)
        {
            DisplayStyle = sender as string;
            SetListOrGrid(DisplayStyle);
        }

        private void SetListOrGridView(string type_)
        {
            LinearLayoutManager Mgr;
            if (DisplayStyle == "List")
            {
                recyclerView.ItemTemplateSelector = new ProductsRecyclerViewItemSelector(0);
                var linearLayoutManager = new LinearLayoutManager(this);

                recyclerView.SetLayoutManager(linearLayoutManager);

                Mgr = linearLayoutManager;
            }
            else
            {
                recyclerView.ItemTemplateSelector = new ProductsRecyclerViewItemSelector(1);
                var gridLayoutManager = new GridLayoutManager(this, 3);

                recyclerView.SetLayoutManager(gridLayoutManager);

                Mgr = gridLayoutManager;

            }

            var onScrollListener = new ProductsRecyclerViewOnScrollListener(Mgr);
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) => {
                if (_loder.Visibility != ViewStates.Visible)
                {
                    if (ViewModel.CanLoadMore)
                    {
                        _loder.Visibility = ViewStates.Visible;
                        _currentPage++;
                        ViewModel.LoadProductsPage(_currentPage);
                    }
                }
               
            };

            onScrollListener.CancelLoadMoreEvent += (object sender, EventArgs e) => {
                if (_loder.Visibility == ViewStates.Visible)
                {
                    _loder.Visibility = ViewStates.Gone;
                }
            };

            recyclerView.AddOnScrollListener(onScrollListener);
        }

        private void SetListOrGrid(string type_)
        {
            try
            {
                SetListOrGridView(type_);
                _Db.SaveProductsStyle(type_);
                _drawerLayout.CloseDrawer((int)GravityFlags.Start, true);
            }
            catch (System.Exception ex)
            {

                //throw;//x
            }
        }

        private void _imagefiltering_Click(object sender, EventArgs e)
        {
            _drawerLayout.OpenDrawer((int)GravityFlags.End, true);
        }

        private void _drawerLayout_Click(object sender, EventArgs e)
        {
            _drawerLayout.CloseDrawer((int)GravityFlags.Start, true);
        }

        private void _imageSorting_Click(object sender, EventArgs e)
        {
            _drawerLayout.OpenDrawer((int)GravityFlags.Start, true);
        }

     
    }
}