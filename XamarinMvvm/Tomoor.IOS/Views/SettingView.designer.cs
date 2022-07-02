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
    [Register ("SettingView")]
    partial class SettingView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton hellowBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LangLap { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (hellowBtn != null) {
                hellowBtn.Dispose ();
                hellowBtn = null;
            }

            if (LangLap != null) {
                LangLap.Dispose ();
                LangLap = null;
            }
        }
    }
}