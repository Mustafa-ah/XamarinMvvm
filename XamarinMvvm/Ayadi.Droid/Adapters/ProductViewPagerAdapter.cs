using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Java.Lang;
using Ayadi.Droid.ViewPagerData;
using System;
using System.Collections.Generic;
using Ayadi.Core.Model;
using Ayadi.Droid.Utility;
using FFImageLoading.Views;
using FFImageLoading;

namespace Ayadi.Droid.Adapters
{
    public class ProductViewPagerAdapter : PagerAdapter
    {
        Context _context;
        List<Imager> _imagesUrlList;
        //https://developer.xamarin.com/guides/android/user_interface/viewpager/#adapter
        public ProductViewPagerAdapter(Context context, List<Imager> ImagesUrlList)
        {
            this._context = context;
            this._imagesUrlList = ImagesUrlList;
        }

        public override int Count
        {
            get
            {
                return _imagesUrlList.Count;
            }
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
           // var imageView = new ImageView(_context);
            ImageViewAsync imageViewAsync = new ImageViewAsync(_context);
            ImageService.Instance.LoadUrl(_imagesUrlList[position].Src).Into(imageViewAsync);
            // ImageLoader.LoadImage(_context, _imagesUrlList[position].Src, imageView, 1);
            //imageView.SetImageResource(treeCatalog[position].imageId);
            imageViewAsync.SetScaleType(ImageView.ScaleType.FitXy);
            // var viewPager = container.JavaCast<ViewPager>();
            // viewPager.AddView(imageView);
            container.AddView(imageViewAsync);
            return imageViewAsync;
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object view)
        {
           // var viewPager = container.JavaCast<ViewPager>();
            container.RemoveView(view as View);
        }
    }
}