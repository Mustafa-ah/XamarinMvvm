using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Ayadi.Core.ViewModel;

namespace Ayadi.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object hint = null)
        {
            ShowViewModel<HomeViewModel>();
        }
    }
}
