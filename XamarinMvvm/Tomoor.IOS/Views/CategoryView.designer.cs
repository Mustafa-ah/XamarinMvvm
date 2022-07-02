// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Tomoor.IOS.Views
{
    [Register ("CategoryView")]
    partial class CategoryView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView CatsCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView catsTopSlider { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CatsCollectionView != null) {
                CatsCollectionView.Dispose ();
                CatsCollectionView = null;
            }

            if (catsTopSlider != null) {
                catsTopSlider.Dispose ();
                catsTopSlider = null;
            }
        }
    }
}