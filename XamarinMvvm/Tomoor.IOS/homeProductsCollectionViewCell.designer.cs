// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tomoor.IOS
{
    [Register ("homeProductsCollectionViewCell")]
    partial class homeProductsCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnLike { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageFrame { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageLike { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LapPrice { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProductImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProductName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnLike != null) {
                btnLike.Dispose ();
                btnLike = null;
            }

            if (imageFrame != null) {
                imageFrame.Dispose ();
                imageFrame = null;
            }

            if (imageLike != null) {
                imageLike.Dispose ();
                imageLike = null;
            }

            if (LapPrice != null) {
                LapPrice.Dispose ();
                LapPrice = null;
            }

            if (ProductImage != null) {
                ProductImage.Dispose ();
                ProductImage = null;
            }

            if (ProductName != null) {
                ProductName.Dispose ();
                ProductName = null;
            }
        }
    }
}