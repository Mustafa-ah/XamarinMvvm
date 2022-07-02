using Ayadi.Core.Contracts.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Ayadi.Core.Extensions;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using Ayadi.Core.Contracts.Services;
using System.Collections.ObjectModel;
using Ayadi.Core.Model;
using MvvmCross.Core.ViewModels;
using Ayadi.Core.Messages;

namespace Ayadi.Core.ViewModel
{
    public class FillteringViewModel : BaseViewModel, IFillteringViewModel
    {
        private readonly ICategoryDataService _categoryDataService;
        private readonly IConnectionService _connectionService;
        private readonly IUserDataService _userDataService;

        public event EventHandler ViewModelInitialized;

        private User _AppUser;

        private ObservableCollection<Category> _Categories;
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

        public FillteringViewModel(IMvxMessenger messenger,
            IUserDataService userDataService,
            ICategoryDataService categoryDataService,
             IConnectionService connectionService):base(messenger)
        {
            _categoryDataService = categoryDataService;
            _connectionService = connectionService;
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
                Categories = (await _categoryDataService.GetAllCategories(_AppUser)).ToObservableCollection();
            }
            ViewModelInitialized?.Invoke(this, new EventArgs());
        }

        private void filter(Category cat)
        {
            Messenger.Publish(new FilterMesaage(this) { Cat = cat });
        }

        // commands
        /*
          ShowViewModel<ProductsViewModel>
                    (new { CatId = selectedCat.Id });
             */
        public MvxCommand<Category> ShowProductsViewCommand
        {
            get
            {
                return new MvxCommand<Category>(selectedCat =>
                {
                    filter(selectedCat);
                    SetSelection(selectedCat);
                });
            }
        }

        private void SetSelection(Category cat)
        {
            ObservableCollection<Category> TempCategories = new ObservableCollection<Category>(Categories);
            foreach (Category _cat in Categories)
            {
                _cat.IsSelected = false;
            }
            cat.IsSelected = true;
            //Categories.Clear();
            Categories = TempCategories;
        }
    }
}
