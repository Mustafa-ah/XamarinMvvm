using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;

namespace Ayadi.Core.ViewModel
{
    public class WriteNewPasswordViewModel : BaseViewModel
    {
        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        public string Email { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; RaisePropertyChanged(() => Code); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; RaisePropertyChanged(() => NewPassword); }
        }


        public WriteNewPasswordViewModel(IMvxMessenger messenger,
       IUserDataService userDataService,
        IConnectionService connectionService,
        IDialogService dialogService):base(messenger)
        {
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
        }

        public void Init(string email)
        {
            Email = email;
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
        { get { return new MvxCommand(() => { Close(this); ShowViewModel<RecoverAccountViewModel>(new { email = Email}); }); } }

        public MvxCommand RecoverAccountCommand
        { get { return new MvxCommand(() => Recover()); } }

        private async void Recover()
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

                else if (string.IsNullOrEmpty(Code))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterCode"),
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

                UserPassword password_ = new UserPassword();
                password_.CustomerId = _AppUser.Id;
                password_.Token = Code;
                password_.New_password = NewPassword;
                password_.Email = Email;

                IsBusy = true;
                Response crecoverd_ = await _userDataService.RecoverUserPassword(_AppUser, password_);
                IsBusy = false;
                if (crecoverd_.Ok)
                {
                    _dialogService.ShowToast(TextSource.GetText("recoverdMsg"));
                    Close(this);
                    ShowViewModel<LoginViewModel>(new { email = Email, password = NewPassword });
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("notRecoverdMsg") + crecoverd_.Message,
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
