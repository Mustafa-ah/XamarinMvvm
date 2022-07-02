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
using Ayadi.Droid.Adapters;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V4.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Binding.BindingContext;
using Ayadi.Droid.Utility;
using Java.Lang;

namespace Ayadi.Droid.Views
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

        private string DisplayStyle;

        ImageView _imageSorting;
        ImageView _imagefiltering;
        MvxGridView gridView;
        MvxRecyclerView recyclerView;
        FavouriteAnimatorGridViewAdapter _gridViewAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
         

            try
            {
                base.OnCreate(savedInstanceState);
                //
                SetContentView(Resource.Layout.Activity_Products);

                _Db = new XmlDb(this);
                DisplayStyle = _Db.GetProductsStyle();


                gridView = FindViewById<MvxGridView>(Resource.Id.Products_Grid);
                recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.Products_List);


                _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
                _drawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow_light, (int)GravityFlags.Start);

                _imageSorting = FindViewById<ImageView>(Resource.Id.imageViewSorting);
                _imagefiltering = FindViewById<ImageView>(Resource.Id.imageViewFilter);

                if (DisplayStyle == "List")
                {
                    gridView.Visibility = ViewStates.Gone;
                    recyclerView.Visibility = ViewStates.Visible;
                }
                    recyclerView.Adapter = new FavouriteAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext,
                   this, ViewModel);

                    _gridViewAdapter = new FavouriteAnimatorGridViewAdapter(this, ViewModel,(IMvxAndroidBindingContext)BindingContext);
                    gridView.Adapter = _gridViewAdapter;
               

                _bindableProgressBar = new BindableProgressBar(this);
                var set = this.CreateBindingSet<ProductsView, ProductsViewModel>();
                set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
                set.Apply();


                _imageSorting.Click += _imageSorting_Click;
                _imagefiltering.Click += _imagefiltering_Click;
                ViewModel.ProductsSorted += _drawerLayout_Click;
                ViewModel.ProductsViewChanged += ViewModel_ProductsViewChanged;
                ViewModel.CategoryChosed += ViewModel_CategoryChosed;

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

        private void ViewModel_CategoryChosed(object sender, EventArgs e)
        {
            _drawerLayout.CloseDrawer((int)GravityFlags.End, true);
            Android.Util.Log.Debug("eeeeeeeee", "............................closed...............");
        }

        private void ViewModel_ProductsViewChanged(object sender, EventArgs e)
        {
            DisplayStyle = sender as string;
            SetListOrGrid(DisplayStyle);
        }


        private void SetListOrGrid(string type_)
        {
            try
            {
                if (type_ == "List")
                {
                  //  recyclerView.Adapter = new FavouriteAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext,
                  //this, ViewModel);
                    gridView.Visibility = ViewStates.Gone;
                    recyclerView.Visibility = ViewStates.Visible;
                   // gridView.Adapter.Dispose();
                }
                else
                {
                    //_gridViewAdapter = new FavouriteAnimatorGridViewAdapter(this, (IMvxAndroidBindingContext)BindingContext);
                    //gridView.Adapter = _gridViewAdapter;
                    gridView.Visibility = ViewStates.Visible;
                    recyclerView.Visibility = ViewStates.Gone;
                   // recyclerView.Dispose();
                }
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
            //gridView.NumColumns = 1;
            //gridView.SetColumnWidth(300);
            //_gridViewAdapter.NotifyDataSetChanged();
            //  recyclerView.Visibility = ViewStates.Visible;
            //  gridView.Visibility = ViewStates.Gone;
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