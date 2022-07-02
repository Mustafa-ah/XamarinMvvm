using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.ViewModel;
using Ayadi.Core.Extensions;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Messages;

namespace Ayadi.Core.ViewModel
{
    public class CategoryViewModel : BaseViewModel, ICategoryViewModel
    {
        private ObservableCollection<Category> _Categories;

        private readonly ICategoryDataService _categoryDataService;
      //  private readonly ILoadingDataService _loadingDataService;
        private readonly IConnectionService _connectionService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;

        private User _AppUser;

        public event EventHandler ViewModelInitialized;

        private readonly MvxSubscriptionToken _topSliderMsgToken;

        public List<Imager> SliderImages { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public ObservableCollection<Category> Categories
        {
            get
            {
                return _Categories;
            }
            set
            {
                _Categories = value;
                RaisePropertyChanged(() => Categories);
            }
        }

          public CategoryViewModel(IMvxMessenger messenger,
              ICategoryDataService categoryDataService,
              IConnectionService connectionService,
               IUserDataService userDataService,
              IDialogService dialogService):base(messenger)
        {
            _categoryDataService = categoryDataService;
           // _loadingDataService = loadingDataService;
            _userDataService = userDataService;
            _connectionService = connectionService;
            _dialogService = dialogService;

            _topSliderMsgToken = Messenger.Subscribe<TopHomeSliderMessage>(OnTopSliderImagesReady);
        }

        private void OnTopSliderImagesReady(TopHomeSliderMessage obj)
        {
            SliderImages = obj.ImgList;
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
                //await _loadingDataService.ShowFragmentLoading();
                IsBusy = true;
                _AppUser = await _userDataService.GetSavedUser();

                Categories = (await _categoryDataService.GetAllCategories(_AppUser)).ToObservableCollection();
                // _loadingDataService.HideFragmentLoading();
                IsBusy = false;
                //temp
                if (SliderImages == null)
                {
                    SliderImages = new List<Imager>()
                     {
                        new Imager() {Src = "http://a.up-00.com/2017/11/151206634618921.jpg" },
                        new Imager() {Src = "http://a.up-00.com/2017/11/151206634628292.jpg" },
                        new Imager() {Src = "http://a.up-00.com/2017/11/151206634648183.jpg" },
                        new Imager() {Src = "http://a.up-00.com/2017/11/151206634686254.jpg" }
                     };
                }
                
            }
            else
            {
                // await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                //TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                
            }


            ViewModelInitialized?.Invoke(this, new EventArgs());
        }

        // commands

        public MvxCommand<Category> ShowProductsViewCommand
        {
            get
            {
                return new MvxCommand<Category>(selectedCat =>
                {
                    ShowViewModel<ProductsViewModel>
                    (new { CatId = selectedCat.Id, title = selectedCat.Name });
                });
            }
        }
    }
}
