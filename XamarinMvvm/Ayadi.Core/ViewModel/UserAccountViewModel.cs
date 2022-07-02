using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.ViewModel
{
    public class UserAccountViewModel : BaseViewModel, IUserAccountViewModel
    {
        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; RaisePropertyChanged(() => CurrentUser); }
        }

        public bool IsAppeared { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public UserAccountViewModel(IMvxMessenger messenger,
           IUserDataService userDataService,
            IConnectionService connectionService,
            IDialogService dialogService):base(messenger)
        {
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
        }


        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            if (_connectionService.CheckOnline())
            {
               // IsBusy = true;
                _AppUser = await _userDataService.GetSavedUser();
                //CurrentUser = await _userDataService.GetUserById();//UserId
               // IsBusy = false;
            }
            else
            {
                //await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                //   TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                
            }
            IsAppeared = true;
        }

        public MvxCommand ShowUserDataViewCommand
        { get { return new MvxCommand(() => ShowViewModel<UserDataViewModel>()); } }

        public MvxCommand ShowAdressesViewCommand
        { get { return new MvxCommand(() => ShowViewModel<UserAdressesViewModel>()); } }

        public MvxCommand ShowOrdersViewCommand
        { get { return new MvxCommand(() => ShowViewModel<OrdersViewModel>()); } }

        public MvxCommand ChangePasswordCommand
        { get { return new MvxCommand(() => ShowViewModel<ChangPasswordViewModel>()); } }

        public MvxCommand LogoutCommand
        { get { return new MvxCommand(() => Logout()); } }

        private async void Logout()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                bool Delete_ = await _dialogService.ShowDecisionAlertAsync(TextSource.GetText("logoutMsg"),
                 TextSource.GetText("tomoor_"), TextSource.GetText("ok_"), TextSource.GetText("cancel_"));
                _dialogService.AlertCliked += _dialogService_AlertCliked;
            }
            catch (Exception)
            {

                //throw;//x
            }
           
        }

        private void _dialogService_AlertCliked(object sender, EventArgs e)
        {
            if (!IsAppeared)
            {
                return;
            }
            bool ok = (bool)sender;
            if (ok)
            {
                IsBusy = true;
                _AppUser.Id = 0;
                _userDataService.SaveUserToLocal(_AppUser);
                _userDataService.Logout(_AppUser);
                IsBusy = false;
                ShowViewModel<SubHomeViewModel>();
                IHomeActivityUiService homeActivityUiService = Mvx.Resolve<IHomeActivityUiService>();
                homeActivityUiService.SetHomeFoucs();
            }
            
        }
    }
}
