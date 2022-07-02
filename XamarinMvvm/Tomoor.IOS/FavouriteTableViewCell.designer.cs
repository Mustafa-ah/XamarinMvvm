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
    [Register ("FavouriteTableViewCell")]
    partial class FavouriteTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageFrame { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageProduct { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabProductName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabProductPrice { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ImageFrame != null) {
                ImageFrame.Dispose ();
                ImageFrame = null;
            }

            if (ImageProduct != null) {
                ImageProduct.Dispose ();
                ImageProduct = null;
            }

            if (LabProductName != null) {
                LabProductName.Dispose ();
                LabProductName = null;
            }

            if (LabProductPrice != null) {
                LabProductPrice.Dispose ();
                LabProductPrice = null;
            }
        }
    }
}