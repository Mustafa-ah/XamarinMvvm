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
    [Register ("SubHomeView")]
    partial class SubHomeView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnShowAllCats { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnShowAllProducts { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView categoryCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView catsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView HomeScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lapCats { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lapProucts { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LapSponsers { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView ProductsCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollViewSponsers { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollViewTopSlider { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView showAllProductsView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnShowAllCats != null) {
                btnShowAllCats.Dispose ();
                btnShowAllCats = null;
            }

            if (btnShowAllProducts != null) {
                btnShowAllProducts.Dispose ();
                btnShowAllProducts = null;
            }

            if (categoryCollectionView != null) {
                categoryCollectionView.Dispose ();
                categoryCollectionView = null;
            }

            if (catsView != null) {
                catsView.Dispose ();
                catsView = null;
            }

            if (HomeScrollView != null) {
                HomeScrollView.Dispose ();
                HomeScrollView = null;
            }

            if (lapCats != null) {
                lapCats.Dispose ();
                lapCats = null;
            }

            if (lapProucts != null) {
                lapProucts.Dispose ();
                lapProucts = null;
            }

            if (LapSponsers != null) {
                LapSponsers.Dispose ();
                LapSponsers = null;
            }

            if (ProductsCollectionView != null) {
                ProductsCollectionView.Dispose ();
                ProductsCollectionView = null;
            }

            if (ScrollViewSponsers != null) {
                ScrollViewSponsers.Dispose ();
                ScrollViewSponsers = null;
            }

            if (ScrollViewTopSlider != null) {
                ScrollViewTopSlider.Dispose ();
                ScrollViewTopSlider = null;
            }

            if (showAllProductsView != null) {
                showAllProductsView.Dispose ();
                showAllProductsView = null;
            }
        }
    }
}