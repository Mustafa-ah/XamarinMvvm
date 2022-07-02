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
using Tomoor.Droid.Utility;
using MvvmCross.Droid.Views;
using Ayadi.Core.ViewModel;
using MvvmCross.Binding.BindingContext;


namespace Tomoor.Droid.Views
{
    [Activity(Label = "ProductsReviewView")]
    public class ProductsReviewView : MvxActivity<ProductsReviewViewModel>
    {
        int rate = 0;
        TextView ratingStat;
        ImageView img1;
        ImageView img2;
        ImageView img3;
        ImageView img4;
        ImageView img5;

        BindableProgressBar _bindableProgressBar;

        public new ProductsReviewViewModel ViewModel
        {
            get { return (ProductsReviewViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_ProductReview);
            this.Window.SetSoftInputMode(SoftInput.StateHidden);

            ratingStat = FindViewById<TextView>(Resource.Id.textStarState);

            img1 = FindViewById<ImageView>(Resource.Id.imageStar1);
            img2 = FindViewById<ImageView>(Resource.Id.imageStar2);
            img3 = FindViewById<ImageView>(Resource.Id.imageStar3);
            img4 = FindViewById<ImageView>(Resource.Id.imageStar4);
            img5 = FindViewById<ImageView>(Resource.Id.imageStar5);

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<ProductsReviewView, ProductsReviewViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();


            img1.Click += Img1_Click;
            img2.Click += Img2_Click;
            img3.Click += Img3_Click;
            img4.Click += Img4_Click;
            img5.Click += Img5_Click;

        }


        private void Img5_Click(object sender, EventArgs e)
        {
            rate = 5;
            ratingStat.Text = ViewModel.Iloveit;// " i loved it";
            img5.SetImageResource(Resource.Drawable.starRate);
            img4.SetImageResource(Resource.Drawable.starRate);
            img3.SetImageResource(Resource.Drawable.starRate);
            img2.SetImageResource(Resource.Drawable.starRate);
            img1.SetImageResource(Resource.Drawable.starRate);
            ViewModel.ReviewItems.Rating = rate;
            
        }


        private void Img4_Click(object sender, EventArgs e)
        {
            rate = 4;
            ratingStat.Text = ViewModel.ILikeit;//"i like it";
            img5.SetImageResource(Resource.Drawable.outlinedStar);
            img4.SetImageResource(Resource.Drawable.starRate);
            img3.SetImageResource(Resource.Drawable.starRate);
            img2.SetImageResource(Resource.Drawable.starRate);
            img1.SetImageResource(Resource.Drawable.starRate);
            ViewModel.ReviewItems.Rating = rate;
        }

        private void Img3_Click(object sender, EventArgs e)
        {
            rate = 3;
            ratingStat.Text = ViewModel.Good;//"Good";
            img5.SetImageResource(Resource.Drawable.outlinedStar);
            img4.SetImageResource(Resource.Drawable.outlinedStar);
            img3.SetImageResource(Resource.Drawable.starRate);
            img2.SetImageResource(Resource.Drawable.starRate);
            img1.SetImageResource(Resource.Drawable.starRate);
            ViewModel.ReviewItems.Rating = rate;
        }

        private void Img2_Click(object sender, EventArgs e)
        {
            rate = 2;
            ratingStat.Text = ViewModel.NotGodd;//"Not Good";
            img5.SetImageResource(Resource.Drawable.outlinedStar);
            img4.SetImageResource(Resource.Drawable.outlinedStar);
            img3.SetImageResource(Resource.Drawable.outlinedStar);
            img2.SetImageResource(Resource.Drawable.starRate);
            img1.SetImageResource(Resource.Drawable.starRate);
            ViewModel.ReviewItems.Rating = rate;
        }

        private void Img1_Click(object sender, EventArgs e)
        {
            rate = 1;
            ratingStat.Text = ViewModel.Bad;//"Bad";
            img5.SetImageResource(Resource.Drawable.outlinedStar);
            img4.SetImageResource(Resource.Drawable.outlinedStar);
            img3.SetImageResource(Resource.Drawable.outlinedStar);
            img2.SetImageResource(Resource.Drawable.outlinedStar);
            img1.SetImageResource(Resource.Drawable.starRate);
            ViewModel.ReviewItems.Rating = rate;
        }
    }
}