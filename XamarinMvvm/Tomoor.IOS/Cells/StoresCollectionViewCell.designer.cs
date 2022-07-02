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
    [Register ("StoresCollectionViewCell")]
    partial class StoresCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView FramImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labStoreName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView StoreImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FramImage != null) {
                FramImage.Dispose ();
                FramImage = null;
            }

            if (labStoreName != null) {
                labStoreName.Dispose ();
                labStoreName = null;
            }

            if (StoreImage != null) {
                StoreImage.Dispose ();
                StoreImage = null;
            }
        }
    }
}