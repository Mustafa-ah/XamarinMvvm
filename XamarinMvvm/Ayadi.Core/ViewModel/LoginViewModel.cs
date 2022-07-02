using Ayadi.Core.Contracts.Repository;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using Ayadi.Core.Repositories;
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
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(() => UserName); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(() => Password); }
        }

        public LoginViewModel(IMvxMessenger messenger,
        IUserDataService userDataService,
         IConnectionService connectionService,
         IDialogService dialogService):base(messenger)
        {
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
        }

        public void Init(string email, string password)
        {
            UserName = email;
            Password = password;
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        //commands 
        public MvxCommand ShowSignupViewCommand
        { get { return new MvxCommand(() => { ShowViewModel<SignupViewModel>(); Close(this); }); } }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }

        public MvxCommand RecoverAccountCommand
        { get { return new MvxCommand(() => { ShowViewModel<RecoverAccountViewModel>(new { email = this.UserName }); Close(this); }); } }

        public MvxCommand LoginCommand
        { get { return new MvxCommand(() => Login()); } }

       
        private async void Login()
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

                else if (string.IsNullOrEmpty(UserName))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterEmailMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (!Utility.HelperTools.ValidateEmail(UserName))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterValidEmailMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(Password))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterPasswordMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (Password.Length < 6)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("shortPasswordMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                IsBusy = true;

                User user_ = await _userDataService.GetSavedUser();
                user_.Password = Password;
                user_.Username = UserName;
                user_.Email = UserName;
                if (user_.AccessToken == null)
                {
                    user_.AccessToken = await _userDataService.GetUserAccessToken();
                }
                User Responds = await _userDataService.Login(user_.AccessToken,user_);
               
                if (Responds.Id != 0)
                {
                    user_.Id = Responds.Id;
                    user_.IsGuestUser = false;
                    await _userDataService.SaveUserToLocal(user_);
                    ICartRepository cartRepo = Mvx.Resolve<ICartRepository>();
                    var shopingList = await cartRepo.GetShoppingCartItemsFromAPI(user_);
                    //await cartRepo.SaveShopingCartToLocal(shopingList);
                    cartRepo.SetActiveShoppingList(shopingList);
                    Messenger.Publish(new Messages.LoginMessage(this));
                    Close(this);
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("loginFaildMsg_") + Responds.RespondsMessage,
                     TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                }
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                //throw;//x
            }
        }

    }
}
