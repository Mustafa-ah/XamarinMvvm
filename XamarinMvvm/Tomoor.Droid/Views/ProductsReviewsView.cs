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
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "ProductsReviewsView")]
    public class ProductsReviewsView : MvxActivity<ProductsReviewsViewModel>
    {
      //  RecyclerView recyclerView;
       // DividerItemDecoration mDividerItemDecoration;
      //  LinearLayoutManager mLayoutManager;

        BindableProgressBar _bindableProgressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_ProductReviews);

           // recyclerView = FindViewById<RecyclerView>(Resource.Id.ReviewsList);

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<ProductsReviewsView, ProductsReviewsViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

            //Drawable dividerDrawable = ContextCompat.GetDrawable(this,Android.Resource.Drawable.DividerHorizontalBright);

            //recyclerView.addItemDecoration(new DividerItemDecoration(this, dividerDrawable));


            //mDividerItemDecoration = new DividerItemDecoration(recyclerView.Context, DividerItemDecoration.Horizontal);
            //recyclerView.AddItemDecoration(mDividerItemDecoration);
        }
    }
}