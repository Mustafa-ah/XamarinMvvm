﻿// WARNING
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
    [Register ("HomeCatsCollectionViewCell")]
    partial class HomeCatsCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView catImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageFrame { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lapCatName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LapProductsCount { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (catImage != null) {
                catImage.Dispose ();
                catImage = null;
            }

            if (imageFrame != null) {
                imageFrame.Dispose ();
                imageFrame = null;
            }

            if (lapCatName != null) {
                lapCatName.Dispose ();
                lapCatName = null;
            }

            if (LapProductsCount != null) {
                LapProductsCount.Dispose ();
                LapProductsCount = null;
            }
        }
    }
}