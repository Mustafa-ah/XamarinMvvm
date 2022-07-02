using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.ViewModel
{
    public class ChangPasswordViewModel : BaseViewModel
    {
        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _oldPaswword;
        public string OldPassword
        {
            get { return _oldPaswword; }
            set { _oldPaswword = value; RaisePropertyChanged(() => OldPassword); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; RaisePropertyChanged(() => NewPassword); }
        }

        private string _rPassword;
        public string RPassword
        {
            get { return _rPassword; }
            set { _rPassword = value; RaisePropertyChanged(() => RPassword); }
        }

        public ChangPasswordViewModel(IMvxMessenger messenger,
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

            _AppUser = await _userDataService.GetSavedUser();

        }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() =>  Close(this)); } }

        public MvxCommand ChangePasswordCommand
        { get { return new MvxCommand(() => ChangePassword()); } }

        private async void ChangePassword()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }
                if (!_connectionService.CheckOnline())
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(OldPassword))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterOldPaswword"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(NewPassword))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterNewPassword"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (NewPassword.Length < 6)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("shortPasswordMsg_"),
                       TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (NewPassword != RPassword)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("passwordNotMatchMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                UserPassword password_ = new UserPassword();
                password_.CustomerId = _AppUser.Id;
                password_.Old_password = OldPassword;
                password_.New_password = NewPassword;

                IsBusy = true;
                Response changed_ = await _userDataService.ChangeUserPassword(_AppUser, password_);
                IsBusy = false;
                if (changed_.Ok)
                {
                     _dialogService.ShowToast(TextSource.GetText("passwordChanged"));
                    Close(this);
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("passwordNotChanged") + changed_.Message,
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                }
              
            }
            catch (Exception)
            {
                IsBusy = false;
                //throw;//x
            }

        }

    }
}
