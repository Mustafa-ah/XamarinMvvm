using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ayadi.Core.Extensions;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Contracts.ViewModel;
using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;
using Ayadi.Core.Model;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Messages;
using System.Threading;

namespace Ayadi.Core.ViewModel
{
    public class UserAdressesViewModel : BaseViewModel, IUserAdressesViewModel
    {

        private readonly IUserDataService _userDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;

        private readonly MvxSubscriptionToken _token;
        //   private readonly MvxSubscriptionToken _deleteMsgToken;
        //  private readonly MvxSubscriptionToken _UpdateMsgToken;

        public bool IsAppeared { get; set; }

        private User _AppUser;

        private UserAdress _updatedAdress;

        private ObservableCollection<UserAdress> _adresses;

        public ObservableCollection<UserAdress> Adresses
        {
            get { return _adresses; }
            set { _adresses = value; RaisePropertyChanged(() => Adresses); }
        }

        //private User _currentUser;

        //public User CurrentUser
        //{
        //    get { return _currentUser; }
        //    set { _currentUser = value; RaisePropertyChanged(() => CurrentUser); }
        //}


        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public UserAdressesViewModel(IMvxMessenger messenger,
           IUserDataService userDataService,
            IConnectionService connectionService,
            IDialogService dialogService):base(messenger)
        {
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;

            _token = Messenger.Subscribe<AdresessUpdatedMessage>(OnAdressesUpdated);
            //   _deleteMsgToken = Messenger.Subscribe<DeleteAdressMessage>(OnAdressesDeleted);
            // _UpdateMsgToken = Messenger.Subscribe<UpdateAdressMessage>(OnUpdateAdress);
            IsAppeared = true;
        }

        // calling this method from UserAddressesRecyclerAdapter
        public void updateAddress(UserAdress ads)
        {
            try
            {
                _AppUser.Billing_address = ads;
                _updatedAdress = ads;
                ShowViewModel<UserAdressViewModel>(new { AdressId = ads.Id });
                Task.Factory.StartNew(() => _userDataService.SaveUserToLocal(_AppUser));

            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private void OnUpdateAdress(UpdateAdressMessage obj)
        {
            try
            {
                _AppUser.Billing_address = obj.Adress;
                _updatedAdress = obj.Adress;
                ShowViewModel<UserAdressViewModel>(new { AdressId = obj.Adress.Id });
                Task.Factory.StartNew(() => _userDataService.SaveUserToLocal(_AppUser));
                
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private UserAdress _deletedAdress;


        public async void OnAdressesDeleted(DeleteAdressMessage obj)
        {
            try
            {
                bool Delete_ = await _dialogService.ShowDecisionAlertAsync(TextSource.GetText("deleteAdresses"),
                  TextSource.GetText("tomoor_"), TextSource.GetText("ok_"), TextSource.GetText("cancel_"));

                _dialogService.AlertCliked -= _dialogService_AlertCliked;
                _dialogService.AlertCliked += _dialogService_AlertCliked;
                _deletedAdress = obj.Adress;
            }
            catch (Exception)
            {

                //throw;//x
            }
        }
        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            IsAppeared = false;
        }
        //public override void ViewDestroy(bool viewFinishing = true)
        //{
        //    base.ViewDestroy(viewFinishing);
        //    IsAppeared = false;
        //}
        //public override void ViewDestroy()
        //{
        //    base.ViewDestroy();
        //    IsAppeared = false;
        //}

        private async void _dialogService_AlertCliked(object sender, EventArgs e)
        {
            try
            {
                // when AlertCliked fires from different location
                if (!IsAppeared)
                {
                    return;
                }
                bool delet_ = (bool)sender;
                if (delet_)
                {
                    IsBusy = true;
                    bool isDelted_ = await _userDataService.DeleteUserAddress(_AppUser, _deletedAdress.Id);
                    if (isDelted_)
                    {
                        Adresses.Remove(_deletedAdress);
                    }
                    else
                    {
                        await _dialogService.ShowAlertAsync(TextSource.GetText("adressesNotDel"),
                                 TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    }
                    IsBusy = false;

                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                //throw;//x
            }
        }

        private void OnAdressesUpdated(AdresessUpdatedMessage obj)
        {
            try
            {
                Adresses.Add(obj.NewAdress);
                if (_updatedAdress != null)
                {
                    Adresses.Remove(_updatedAdress);
                }
                //Adresses = obj.NewAdressesList.ToObservableCollection();
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        public async void DeletedAddress(UserAdress ads_)
        {
            try
            {

                bool Delete_ = await _dialogService.ShowDecisionAlertAsync(TextSource.GetText("deleteAdresses"),
                  TextSource.GetText("tomoor_"), TextSource.GetText("ok_"), TextSource.GetText("cancel_"));

                _dialogService.AlertCliked -= _dialogService_AlertCliked;
                _dialogService.AlertCliked += _dialogService_AlertCliked;
                _deletedAdress = ads_;
            }
            catch (Exception)
            {

                //throw;//x
            }
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
                IsBusy = true;
                _AppUser = await _userDataService.GetSavedUser();
                Adresses = (await _userDataService.GetUserAdresses(_AppUser)).ToObservableCollection();
                IsBusy = false;
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                  TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                // maybe we can navigate to a start page here, for you to add to this code base!
            }
        }

        // commands
        public MvxCommand ShowAdressViewCommand
        { get { return new MvxCommand(() => 
            {
                ShowViewModel<UserAdressViewModel>();
                _updatedAdress = new UserAdress();
            }); } }

        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }
    }
}
