using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Model;
using Ayadi.Core.Extensions;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace Ayadi.Core.ViewModel
{
    public class StorsViewModel : BaseViewModel , IStorsViewModel
    {

        private ObservableCollection<Store> _stors;

        private readonly IStoreDataService _storeDataService;
        private readonly ILoadingDataService _loadingDataService;
        private readonly IConnectionService _connectionService;
        private readonly IDialogService _dialogService;
        private readonly IUserDataService _userDataService;


        private User _AppUser;

        public ObservableCollection<Store> Stors
        {
            get
            {
                return _stors;
            }
            set
            {
                _stors = value;
                RaisePropertyChanged(() => Stors);
            }
        }

        public StorsViewModel(IMvxMessenger messenger, IStoreDataService storeDataService,
              ILoadingDataService loadingDataService,
              IConnectionService connectionService,
              IUserDataService userDataService,
              IDialogService dialogService):base(messenger)
        {
            _storeDataService = storeDataService;
            _loadingDataService = loadingDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;
            _userDataService = userDataService;
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
                _AppUser = await _userDataService.GetSavedUser();
               // await _loadingDataService.ShowFragmentLoading();
                Stors = (await _storeDataService.GetAllStors(_AppUser)).ToObservableCollection();
                // _loadingDataService.HideFragmentLoading();

                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
                Stors.Add(Stors[0]);
            }
            else
            {
                //await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                //TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                
            }

        }

        // commands

        public MvxCommand<Store> ShowStoreViewCommand
        {
            get
            {
                return new MvxCommand<Store>(selectedStore =>
                {
                    ShowViewModel<StoreViewModel>
                    (new { storename = selectedStore.Id });
                });
            }
        }
    }
}
