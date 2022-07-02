// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Tomoor.IOS.Cells
{
    [Register ("CatsCollectionViewCell")]
    partial class CatsCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CatImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView FrameImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabCatName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labPcount { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CatImage != null) {
                CatImage.Dispose ();
                CatImage = null;
            }

            if (FrameImage != null) {
                FrameImage.Dispose ();
                FrameImage = null;
            }

            if (imageCount != null) {
                imageCount.Dispose ();
                imageCount = null;
            }

            if (LabCatName != null) {
                LabCatName.Dispose ();
                LabCatName = null;
            }

            if (labPcount != null) {
                labPcount.Dispose ();
                labPcount = null;
            }
        }
    }
}