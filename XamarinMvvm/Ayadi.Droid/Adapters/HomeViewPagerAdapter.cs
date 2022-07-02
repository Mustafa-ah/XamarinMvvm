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
using Ayadi.Droid.ViewPagerData;

namespace Ayadi.Droid.Adapters
{
    class HomeViewPagerAdapter : PagerAdapter
    {
        Context context;
        MainCatalog treeCatalog;
        //https://developer.xamarin.com/guides/android/user_interface/viewpager/#adapter
        public HomeViewPagerAdapter(Context context, MainCatalog treeCatalog)
        {
            this.context = context;
            this.treeCatalog = treeCatalog;
        }

        public override int Count
        {
            get
            {
               return treeCatalog.NumTrees; 
            }
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var imageView = new ImageView(context);
            imageView.SetImageResource(treeCatalog[position].imageId);
            imageView.SetScaleType(ImageView.ScaleType.FitXy);
            //var viewPager = container.JavaCast<ViewPager>();
            // viewPager.AddView(imageView);
            container.AddView(imageView);
            return imageView;
        }


        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object view)
        {
           // var viewPager = container.JavaCast<ViewPager>();
            container.RemoveView(view as View);
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
        }
    }
}