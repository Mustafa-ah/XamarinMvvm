﻿// WARNING
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
    [Register ("StorsView")]
    partial class StorsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView StoresCollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (StoresCollectionView != null) {
                StoresCollectionView.Dispose ();
                StoresCollectionView = null;
            }
        }
    }
}