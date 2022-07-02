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
    [Register ("FavouriteView")]
    partial class FavouriteView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView TabelViewFavourite { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TabelViewFavourite != null) {
                TabelViewFavourite.Dispose ();
                TabelViewFavourite = null;
            }
        }
    }
}