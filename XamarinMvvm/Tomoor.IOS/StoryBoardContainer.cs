using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.iOS.Views;
using MvvmCross.Core.ViewModels;

namespace Tomoor.IOS
{
    class StoryBoardContainer : MvxIosViewsContainer
    {
        protected override IMvxIosView CreateViewOfType(Type viewType, MvxViewModelRequest request)
        {
            return (IMvxIosView)UIStoryboard.FromName("MainStory", null)
               .InstantiateViewController(viewType.Name);
        }
    }
}