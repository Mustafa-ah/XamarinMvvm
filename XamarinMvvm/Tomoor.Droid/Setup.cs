using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Ayadi.Core;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Shared.Presenter;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using Tomoor.Droid.Services;
using Ayadi.Core.Contracts.Services;

namespace Tomoor.Droid
{
    class Setup : MvxAndroidSetup
    {
        XmlDb _Db;
        public Setup(Context applicationContext) : base(applicationContext)
        {
            _Db = new XmlDb(applicationContext);
           
        }

        protected override IMvxApplication CreateApp()
        {
            string Lang = _Db.getSavedLangId();
            if (Lang == "0")
            {
                if (Java.Util.Locale.Default.Language == "ar")
                {
                    Lang = "ar-SA";
                }
                else
                {
                    Lang = "en-US";
                }
            }

            Ayadi.Core.App app = new Ayadi.Core.App();
            app.Lang = Lang;
            return app;
           // return new Core.App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var mvxFragmentsPresenter =
                new MvxFragmentsPresenter(AndroidViewAssemblies);
            Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(mvxFragmentsPresenter);
            return mvxFragmentsPresenter;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();
            Mvx.RegisterSingleton<ILoadingDataService>(() => new LoadingService());
            Mvx.RegisterSingleton<IDialogService>(() => new DialogService());
            Mvx.RegisterSingleton<IHomeActivityUiService>(() => new HomeActivityUiService());
        }
    }
}