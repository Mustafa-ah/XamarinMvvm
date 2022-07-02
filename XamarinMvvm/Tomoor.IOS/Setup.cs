using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.iOS.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters;
using Ayadi.Core;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using Ayadi.Core.Contracts.Services;
using Tomoor.IOS.Services;

namespace Tomoor.IOS
{
    class Setup : MvxIosSetup
    {
        private MvxApplicationDelegate _applicationDelegate;
        UIWindow _window;

        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter) : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            App app = new App();
            app.Lang = "en-US";
            return app;
        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();
            Mvx.RegisterSingleton<IDialogService>(() => new DialogService());
            Mvx.RegisterSingleton<IHomeActivityUiService>(() => new HomeActivityUiService());
            Mvx.RegisterSingleton<ILoadingDataService>(() => new LoadingService());
        }

        protected override IMvxIosViewsContainer CreateIosViewsContainer()
        {
            return new StoryBoardContainer();
        }
    }
}