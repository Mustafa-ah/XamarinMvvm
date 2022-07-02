using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Threading.Tasks;

namespace Ayadi.Core.ViewModel
{
    public class RecoverAccountViewModel : BaseViewModel
    {
        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; RaisePropertyChanged(() => Email); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }
        public RecoverAccountViewModel(IMvxMessenger messenger,
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
            this.Email = email;
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
        { get { return new MvxCommand(() => { Close(this); ShowViewModel<LoginViewModel>(); }); } }

        public MvxCommand RecoverAccountCommand
        { get { return new MvxCommand(() => RecoverAccount()); } }

        private async void RecoverAccount()
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

                else if (string.IsNullOrEmpty(Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterEmailMsg_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (!Utility.HelperTools.ValidateEmail(Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterValidEmailMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                IsBusy = true;
                _AppUser.Email = Email;
                Response recoverd = await _userDataService.RecoverUserPassword(_AppUser);
                IsBusy = false;

                if (recoverd.Ok)
                {
                    _dialogService.ShowToast(TextSource.GetText("recoverdMsg"));
                    Close(this);
                    ShowViewModel<WriteNewPasswordViewModel>(new { email = Email });
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("unRecoverdMsg") + recoverd.Message,
                     TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
    }
}
