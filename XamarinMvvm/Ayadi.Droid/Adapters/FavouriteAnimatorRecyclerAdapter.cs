using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V7.Widget;
using Android.Views.Animations;
using Ayadi.Core.Model;
using Android.Support.V4.Content;
using Ayadi.Core.ViewModel;
using System.Collections.Generic;

namespace Ayadi.Droid.Adapters
{
    public class FavouriteAnimatorRecyclerAdapter : MvxRecyclerAdapter
    {
        Context _ctx;
        BaseViewModel _viewModel;
        //ImageView likeImag;
       // RecyclerView.ViewHolder _holdel;
       // List<Product> _products;

        public FavouriteAnimatorRecyclerAdapter(IMvxAndroidBindingContext bindingContext, Context ctx, BaseViewModel viewModel)
          : base(bindingContext)
        {
            _ctx = ctx;
            _viewModel = viewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);

         //   _holdel = holder;
            
            ImageView likeImag = holder.ItemView.FindViewById<ImageView>(Resource.Id.imageViewP_like);
            //likeImag.Click -= LikeImag_Click;
            //likeImag.Click += LikeImag_Click;


            ImageView shareImage = holder.ItemView.FindViewById<ImageView>(Resource.Id.imageViewP_Share);
            // shareImage.Click -= ShareImage_Click;
            //  shareImage.Click += ShareImage_Click;
            if (!shareImage.HasOnClickListeners)
            {
                shareImage.Click += (s, a) =>
                {
                    shareImage.Visibility = ViewStates.Gone;
                   
                    //Product pro = new Product();
                    //if (_viewModel is SubHomeViewModel)
                    //{
                    //    SubHomeViewModel mod = _viewModel as SubHomeViewModel;
                    //    pro = mod.Products[holder.AdapterPosition];
                    //}
                    //else if (_viewModel is ProductsViewModel)
                    //{
                    //    ProductsViewModel mod = _viewModel as ProductsViewModel;
                    //    pro = mod.Products[holder.AdapterPosition];
                    //}
                   // Product pro = _products[holder.AdapterPosition];
                    string content_ = Constants.BaseShareArUrl + _viewModel.MainProducts[holder.AdapterPosition].Se_name;
                    Utility.ShareIt.Share(_ctx, "Ayadi", content_);

                    shareImage.Visibility = ViewStates.Visible;
                   
                };
            }


            if (!likeImag.HasOnClickListeners)
            {
                likeImag.Click += async (s, a) => 
                {
                    Product pro = _viewModel.MainProducts[holder.AdapterPosition];
                    SetAnimation(likeImag);
                    if (pro.ISInFavourite)
                    {
                        likeImag.SetImageResource(Resource.Drawable.like);
                        bool dele = await _viewModel.RemoveFavouritesAsync(pro);
                    }
                    else
                    {
                        likeImag.SetImageResource(Resource.Drawable.Liked);
                        bool add = await _viewModel.PostToFavouritesAsync(pro);
                    }
                };
            }
            //likeImag.Click += (s, e) =>
            //{

            //    if (likeImag.Drawable.GetConstantState() == ContextCompat.GetDrawable(_ctx, Resource.Drawable.like).GetConstantState())
            //    {
            //        likeImag.SetImageResource(Resource.Drawable.Liked);
            //    }
            //    else
            //    {
            //        likeImag.SetImageResource(Resource.Drawable.like);
            //    }
            //    SetAnimation(likeImag);
            //};
            //holder.ItemView.Click += (s, e) =>
            //{
            //   // likeImag.SetImageResource(Resource.Drawable.Liked);
            //    SetAnimation(holder.ItemView);
            //};
        }

        //private void ShareImage_Click(object sender, EventArgs e)
        //{
        //    Product pro = _products[_holdel.AdapterPosition];
        //    string content_ = "http://ayadi.local/ar/" + pro.Se_name;

        //    Utility.ShareIt.Share(_ctx, "Ayadi", content_, pro.ProductImage);
        //}

        private void LikeImag_Click(object sender, EventArgs e)
        {
            try
            {
                ImageView likeImag = sender as ImageView;
                if (likeImag.Drawable.GetConstantState() == ContextCompat.GetDrawable(_ctx, Resource.Drawable.like).GetConstantState())
                {
                    likeImag.SetImageResource(Resource.Drawable.Liked);
                }
                else
                {
                    likeImag.SetImageResource(Resource.Drawable.like);
                }
                SetAnimation(likeImag);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        void SetAnimation(View viewToAnimate)
        {
            ScaleAnimation anim = new ScaleAnimation(0.0f, 1.0f, 0.0f, 1.0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            anim.Duration = 500;
            viewToAnimate.StartAnimation(anim);
        }


    }
}