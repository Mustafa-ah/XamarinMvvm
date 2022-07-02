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
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.BindingContext;
using Android.Support.V4.Content;
using Android.Views.Animations;
using Ayadi.Core.ViewModel;
using Ayadi.Core.Model;

namespace Ayadi.Droid.Adapters
{
    public class FavouriteAnimatorGridViewAdapter : MvxAdapter
    {
        Context _ctx;
        BaseViewModel _viewModel;
        IMvxAndroidBindingContext _bindingContext;
       // LayoutInflater inflater;
        //ImageView likeImag;

        public FavouriteAnimatorGridViewAdapter(Context ctx, BaseViewModel viewModel, IMvxAndroidBindingContext bindingContext)
          : base(ctx,bindingContext)
        {
            _ctx = ctx;
            _viewModel = viewModel;
            _bindingContext = bindingContext;
           // inflater = (LayoutInflater)_ctx.GetSystemService(Context.LayoutInflaterService);
        }

        //protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        //{
        //    base.BindBindableView(source, viewToUse);
        //    ImageView likeImag = viewToUse.Content.FindViewById<ImageView>(Resource.Id.imageViewP_like);
        //    likeImag.Click -= LikeImag_Click;
        //    likeImag.Click += LikeImag_Click;
        //}

        //protected override View GetSimpleView(View convertView, object dataContext)
        //{
        //    if (convertView == null)
        //    {
        //        convertView = CreateSimpleView(dataContext);
        //    }
        //    else
        //    {
        //        BindSimpleView(convertView, dataContext);
        //    }
        //    ImageView likeImag = convertView.FindViewById<ImageView>(Resource.Id.imageViewP_like);
        //    likeImag.Click -= LikeImag_Click;
        //    likeImag.Click += LikeImag_Click;
        //    return convertView;

        //   // return base.GetSimpleView(convertView, dataContext);
        //}

        //protected override View CreateSimpleView(object dataContext)
        //{
        //    // note - this could technically be a non-binding inflate - but the overhead is minial
        //    var view = _bindingContext.BindingInflate(SimpleViewLayoutId, null);
        //    BindSimpleView(view, dataContext);
        //    ImageView likeImag = view.FindViewById<ImageView>(Resource.Id.imageViewP_like);
        //    likeImag.Click -= LikeImag_Click;
        //    likeImag.Click += LikeImag_Click;
        //    return view;
        //}

        protected override IMvxListItemView CreateBindableView(object dataContext, ViewGroup parent, int templateId)
        {
             MvxListItemView listItemView = new MvxListItemView(_ctx, _bindingContext.LayoutInflaterHolder, dataContext, parent, templateId);

            
            ImageView likeImag = listItemView.Content.FindViewById<ImageView>(Resource.Id.imageViewP_like);
            //likeImag.Click -= LikeImag_Click;
            //likeImag.Click += LikeImag_Click;

            ImageView shareImage = listItemView.Content.FindViewById<ImageView>(Resource.Id.imageViewP_Share);
            // shareImage.Click -= ShareImage_Click;
            //  shareImage.Click += ShareImage_Click;
            if (!shareImage.HasOnClickListeners)
            {
                shareImage.Click += (s, a) =>
                {
                    Product pro = dataContext as Product;
                    shareImage.Visibility = ViewStates.Gone;
                    string content_ = Constants.BaseShareArUrl + pro.Se_name;
                    Utility.ShareIt.Share(_ctx, "Ayadi", content_);

                    shareImage.Visibility = ViewStates.Visible;
                };
            }


            if (!likeImag.HasOnClickListeners)
            {
                likeImag.Click += async (s, a) =>
                {
                    Product pro = dataContext as Product;
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

            return listItemView;
        }
        //protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        //{
        //    if (convertView == null)
        //    {
        //        convertView = inflater.Inflate(Resource.Layout.list_item_prodcut, null);
        //    }
        //    ImageView likeImag = convertView.FindViewById<ImageView>(Resource.Id.imageViewP_like);
        //    likeImag.Click -= LikeImag_Click;
        //    likeImag.Click += LikeImag_Click;
        //    return base.GetBindableView(convertView, dataContext, parent, templateId);
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
            catch 
            {

                ////throw;//x
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