using Ayadi.Core.Contracts;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Messages;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.ViewModel
{
    public class HomeViewModel : BaseViewModel, IHomeViewModel
    {
        private readonly IUserDataService _userDataService;

        private readonly Lazy<SettingViewModel> _settingsViewModel;
        private readonly Lazy<SubHomeViewModel> _subHomeViewModel;
        private readonly Lazy<CategoryViewModel> _categoryViewModel;
        private readonly Lazy<StorsViewModel> _storsViewModel;
        private readonly Lazy<UserAccountViewModel> _userAccountViewModel;

        public SubHomeViewModel SubHomeViewModel => _subHomeViewModel.Value;
        public CategoryViewModel CategoryViewModel => _categoryViewModel.Value;
        public StorsViewModel StorsViewModel => _storsViewModel.Value;
        public UserAccountViewModel UserAccountViewModel => _userAccountViewModel.Value;

        //private readonly Lazy<SortingViewModel> _sortingViewModel;
        //public SortingViewModel SortingViewModel => _sortingViewModel.Value;

        private readonly IHomeActivityUiService _homeActivityUiService;

        public SettingViewModel SettingsViewModel => _settingsViewModel.Value;

        private readonly MvxSubscriptionToken _token;

        public string HitBack { get; set; }

        public HomeViewModel(IMvxMessenger messenger, 
            IHomeActivityUiService homeActivityUiService,
            IUserDataService userDataService) : base(messenger)
        {
            _settingsViewModel = new Lazy<SettingViewModel>(Mvx.IocConstruct<SettingViewModel>);
            _subHomeViewModel = new Lazy<SubHomeViewModel>(Mvx.IocConstruct<SubHomeViewModel>);
            _categoryViewModel = new Lazy<CategoryViewModel>(Mvx.IocConstruct<CategoryViewModel>);
            _storsViewModel = new Lazy<StorsViewModel>(Mvx.IocConstruct<StorsViewModel>);
            _userAccountViewModel = new Lazy<UserAccountViewModel>(Mvx.IocConstruct<UserAccountViewModel>);

            _homeActivityUiService = homeActivityUiService;

            _userDataService = userDataService;

            _token = Messenger.Subscribe<HomeUiMessage>(OnCatsShow);

            HitBack = TextSource.GetText("hitBackToExit_");


        }

        private void OnCatsShow(HomeUiMessage obj)
        {
            _homeActivityUiService.SetCatsFoucs();
        }

      //  private User _AppUser;

        //public  async void InitializeUserAsync()
        //{
        //    //if (_connectionService.CheckOnline())
        //    _AppUser = await _userDataService.GetSavedUser();
        //    if (_AppUser.AccessToken == null)
        //    {
        //        _AppUser.AccessToken = await _userDataService.GetUserAccessToken();
        //        User userGuestId = await _userDataService.LoginAsGuest(_AppUser.AccessToken);
        //        _AppUser.Id = userGuestId.Id;
        //        _AppUser.IsGuestUser = true;
        //        bool IsSaved = await _userDataService.SaveUserToLocal(_AppUser);
        //    }



        //    //if (_AppUser.Id == 0)
        //    //{
        //    //    User userGuestId = await _userDataService.LoginAsGuest(_AppUser.AccessToken);
        //    //    _AppUser.Id = userGuestId.Id;
        //    //    _AppUser.IsGuestUser = true;
        //    //    bool IsSaved = await _userDataService.SaveUserToLocal(_AppUser);

        //    //    User test_ = await _userDataService.GetSavedUser();
        //    //    int dww = test_.Id;
        //    //}

        //    //else
        //    //{
        //    //    await _dialogService.ShowAlertAsync("No internet available", "Ayadi says...", "OK");
        //    //    // maybe we can navigate to a start page here, for you to add to this code base!
        //    //}
        //}

        public void ShowSubHome()
        {
            ShowViewModel<SubHomeViewModel>();
        }

        //public void ShowSortView
        //{
        //    ShowViewModel<SortingViewModel>();
        //}

        // commands
        public MvxCommand ShowCategoryViewCommand
        {
            get
            { return new MvxCommand(() =>
                {
                 _homeActivityUiService.SetCatsFoucs();
                 ShowViewModel<CategoryViewModel>();
                 }
        ); }
        }

        public MvxCommand ShowSettingViewCommand
        { get { return new MvxCommand(() => { ShowViewModel<SettingViewModel>();
            _homeActivityUiService.SetSettingFoucs(); }); } }

        public MvxCommand ShowHomeViewCommand
        { get { return new MvxCommand(() => { ShowViewModel<SubHomeViewModel>();
            _homeActivityUiService.SetHomeFoucs();
        }); } }

        public MvxCommand ShowUserViewCommand
        { get { return new MvxCommand(() => OpenUserAccount()); } }

        //public MvxCommand ShowUserViewCommand
        //{ get { return new MvxCommand(() => ShowViewModel<UserAccountViewModel>()); } }

        public MvxCommand ShowStorsViewCommand
        { get { return new MvxCommand(() => { ShowViewModel<StorsViewModel>();
            _homeActivityUiService.SetStoreFoucs();
        }); } }

        public MvxCommand ShowCartViewCommand
        { get { return new MvxCommand(() => ShowViewModel<CartViewModel>()); } }

        public MvxCommand ShowSearchViewCommand
        { get { return new MvxCommand(() => ShowViewModel<SearchViewModel>()); } }

        public MvxCommand ShowFavouriteViewCommand
        { get { return new MvxCommand(() => ShowViewModel<FavouriteViewModel>()); } }

        private async void OpenUserAccount()
        {
            try
            {
                User user_ = await _userDataService.GetSavedUser();
                if (user_.IsGuestUser || user_.Id == 0)
                {
                    ShowViewModel<LoginViewModel>();
                }
                else
                {
                    ShowViewModel<UserAccountViewModel>();
                    _homeActivityUiService.SetAccountFoucs();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
