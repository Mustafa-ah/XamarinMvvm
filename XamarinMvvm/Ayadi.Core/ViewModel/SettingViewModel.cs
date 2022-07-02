using Akavache;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.WebBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.ViewModel
{
    public class SettingViewModel : BaseViewModel, ISettingViewModel
    {
        public User _user;
        ISettingDataService _stingDataService;
        private readonly IUserDataService _userDataService;
        private readonly IMvxWebBrowserTask _webBrowser;

        public string RestatMsg { get; set; }
        public string Tomoor { get; set; }
        public string Ok { get; set; }
        public string Cancel { get; set; }

        public SettingViewModel(IMvxMessenger messenger,
            IUserDataService userDataService,
            ISettingDataService stingDataService
            , IMvxWebBrowserTask webBrowser) : base(messenger)
        {
            _stingDataService = stingDataService;
            _userDataService = userDataService;
            _webBrowser = webBrowser;

            RestatMsg = TextSource.GetText("restartApp");
            Tomoor = TextSource.GetText("tomoor_");
            Ok = TextSource.GetText("ok_");
            Cancel = TextSource.GetText("cancel_");
        }


        public override async void Start()
        {
            base.Start();

            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            _user = await _userDataService.GetSavedUser();
        }

        //public MvxCommand ChangeToArabicCommand
        //{ get { return new MvxCommand(() => _stingDataService.setArLang()); } }

        //public MvxCommand ChangeToEnglishCommand
        //{ get { return new MvxCommand(() => _stingDataService.setEnLang()); } }

        public MvxCommand AboutUsCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _webBrowser.ShowWebPage(Constants.AboutUsUrl);
                });
            }
        }

        public MvxCommand TermsOfUseCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _webBrowser.ShowWebPage(Constants.TermsOfUseUrl);
                });
            }
        }

        public MvxCommand ContactUsCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _webBrowser.ShowWebPage(Constants.ContactUsUrl);
                });
            }
        }

        public void ClearCashedData()
        {
            _stingDataService.ClearSavedData();
        }

        public async void saveLangId(string langID)
        {
             await _stingDataService.SavedLangId(langID);
        }
    }
}
