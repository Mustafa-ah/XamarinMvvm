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
using Android.Support.V4.View;
using Java.Lang;
using Tomoor.Droid.ViewPagerData;
using Ayadi.Core.Model;
using FFImageLoading.Views;
using FFImageLoading;
using Android.Graphics.Drawables;

namespace Tomoor.Droid.Adapters
{
    class HomeViewPagerAdapter : PagerAdapter
    {
        Context _context;
        List<Imager> _imagesUrlList;
        //https://developer.xamarin.com/guides/android/user_interface/viewpager/#adapter
        public HomeViewPagerAdapter(Context context, List<Imager> ImagesUrlList)
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
            // imageViewAsync.SetImageResource(Resource.Drawable.TomoorBg);
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
            ImageViewAsync imageViewAsync = view as ImageViewAsync;
            BitmapDrawable bmpDrawable = (BitmapDrawable)imageViewAsync.Drawable;
            if (bmpDrawable != null && bmpDrawable.Bitmap != null)
            {
                // This is the important part
                bmpDrawable.Bitmap.Recycle();

            }
            imageViewAsync.SetImageBitmap(null);
            imageViewAsync.Dispose();
            container.RemoveView(imageViewAsync);
            GC.Collect();
            view = null;
        }
    }
}