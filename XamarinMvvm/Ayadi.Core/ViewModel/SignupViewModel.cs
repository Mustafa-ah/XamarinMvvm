using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ayadi.Core.ViewModel
{
    public class SignupViewModel : BaseViewModel, ISignupViewModel
    {

        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; RaisePropertyChanged(()=> CurrentUser); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _rPassword;

        public string RPassword
        {
            get { return _rPassword; }
            set { _rPassword = value; RaisePropertyChanged(() => RPassword); }
        }


        private Gender _selectedGender;

        public Gender SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; RaisePropertyChanged(() => SelectedGender); }
        }

        private int UserId;
        //
        private ObservableCollection<Gender> _genders;

        public ObservableCollection<Gender> Genders
        {
            get { return _genders; }
            set { _genders = value; RaisePropertyChanged(() => Genders); }
        }

        private string _dateOfBirth;

        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; RaisePropertyChanged(() => DateOfBirth); }
        }

        public SignupViewModel(IMvxMessenger messenger,
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
            //if (_connectionService.CheckOnline())
            //{

            CurrentUser = await _userDataService.GetSavedUser();

            Gender gen = new Gender(TextSource.GetText("gender_"),"G");
            gen.IsSelected = true;
            Gender genM = new Gender(TextSource.GetText("male_"), "M");
            Gender genF = new Gender(TextSource.GetText("female_"),"F");
            SelectedGender = gen;

            Genders = new ObservableCollection<Gender>();
            Genders.Add(gen);
            Genders.Add(genM);
            Genders.Add(genF);

        }
        
        //commands

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => { Close(this); ShowViewModel<LoginViewModel>(); }); } }

        public MvxCommand SignupCommand
        { get { return new MvxCommand(() => Signup()); } }

        public MvxCommand<Gender> GenderSelectedCommand
        {
            get
            {
                return new MvxCommand<Gender>((ge) => {
                    foreach (var item in Genders)
                    {
                        item.IsSelected = false;
                    }
                    SelectedGender = ge;
                    ge.IsSelected = true;
                });
            }
        }
        //public MvxAsyncCommand SignupCommand
        //{ get { return new MvxAsyncCommand(async () => UserId = await Signup()); } }


        //public ICommand SignupCommand
        //{
        //    get
        //    {
        //        return new MvxAsyncCommand(Signup);
        //    }
        //}

        private async void Signup()
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
                    return ;
                }
                else if (string.IsNullOrEmpty(CurrentUser.First_name))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterFistNameMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(CurrentUser.Last_name))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterLastNameMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                
                else if (string.IsNullOrEmpty(CurrentUser.Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterEmailMsg_"),
                    TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (!Utility.HelperTools.ValidateEmail(CurrentUser.Email))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterValidEmailMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (SelectedGender.Name == TextSource.GetText("gender_"))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterGenderMsg_"),
                     TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (string.IsNullOrEmpty(CurrentUser.Password))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterPasswordMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                }

                else if (CurrentUser.Password.Length < 6)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("shortPasswordMsg_"),
                       TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }

                else if (CurrentUser.Password != RPassword)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("passwordNotMatchMsg_"),
                      TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                DateTime _dateOfBirth;
                if (!DateTime.TryParse(DateOfBirth,out _dateOfBirth))
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("enterBirthDayMsg_"),
                       TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                else
                {
                    CurrentUser.DateOfBirthDay = _dateOfBirth.Day;
                    CurrentUser.DateOfBirthYear = _dateOfBirth.Year;
                    CurrentUser.DateOfBirthMonth = _dateOfBirth.Month;
                }
                CurrentUser.AcceptPrivacyPolicyEnabled = true;
                CurrentUser.Registered_in_store_id = 1;

               // CurrentUser.PhoneNumber = "01113145562";
                CurrentUser.Username = CurrentUser.Email;

                CurrentUser.Gender = SelectedGender.Id;
                IsBusy = true;
                if (CurrentUser.AccessToken == null)
                {
                    CurrentUser.AccessToken = await _userDataService.GetUserAccessToken();
                }
                User _signedUser = await _userDataService.Signup(CurrentUser.AccessToken , CurrentUser);
                
                if (_signedUser.Id != 0)
                {
                    //CurrentUser.Id = _signedUser.Id;
                    //CurrentUser.IsGuestUser = false;
                    //await _userDataService.SaveUserToLocal(CurrentUser);
                    _dialogService.ShowToast(TextSource.GetText("signupSucsessMsg_"));
                    ShowViewModel<LoginViewModel>(new { email = _signedUser.Email, password = _signedUser.Password });
                    IsBusy = false;
                    Close(this);
                }
                else
                {
                    IsBusy = false;
                    await _dialogService.ShowAlertAsync(TextSource.GetText("signupFaildMsg_") + _signedUser.RespondsMessage,
                  TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                  //  _dialogService.ShowToast(TextSource.GetText("signupFaildMsg_"));
                }
               // return _signedUser.Id;
            }
            catch (Exception)
            {
                IsBusy = false;
                //throw;//x
            }
             
        }
        //public static bool ValidateEmail(string str)
        //{
        //    return System.Text.RegularExpressions.Regex.IsMatch(str, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        //}
    }
}
