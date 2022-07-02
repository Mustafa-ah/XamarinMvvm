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

namespace Tomoor.Droid.ViewPagerData
{
    //https://developer.xamarin.com/guides/android/user_interface/viewpager/#adapter
    public class MainViewPagerData
    {
        // Image ID for this tree image:
        public int imageId;

        // Caption text for this image:
        public string caption;

        // Returns the ID of the image:
        public int ImageID { get { return imageId; } }

        // Returns the caption text for the image:
        public string Caption { get { return caption; } }
    }
    public class MainCatalog
    {
        // Built-in tree catalog (could be replaced with a database)
        static MainViewPagerData[] treeBuiltInCatalog = {
            new MainViewPagerData { imageId = Resource.Drawable.Logo,
                           caption = "No.1: The Larch" },
            new MainViewPagerData { imageId = Resource.Drawable.Logo,
                           caption = "No.2: Maple" },
            new MainViewPagerData { imageId = Resource.Drawable.Logo,
                           caption = "No.3: Birch" },
            new MainViewPagerData { imageId = Resource.Drawable.Logo,
                           caption = "No.4: Coconut" },
            new MainViewPagerData { imageId = Resource.Drawable.Logo,
                           caption = "No.5: Oak" }
        };

        // Array of tree pages that make up the catalog:
        private MainViewPagerData[] treePages;

        // Create an instance copy of the built-in tree catalog:
        public MainCatalog() { treePages = treeBuiltInCatalog; }

        // Indexer (read only) for accessing a tree page:
        public MainViewPagerData this[int i] { get { return treePages[i]; } }

        // Returns the number of tree pages in the catalog:
        public int NumTrees { get { return treePages.Length; } }
    }

}