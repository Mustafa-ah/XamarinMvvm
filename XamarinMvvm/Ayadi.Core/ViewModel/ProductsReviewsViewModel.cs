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
using Ayadi.Core.Extensions;

namespace Ayadi.Core.ViewModel
{
    public class ProductsReviewsViewModel : BaseViewModel, IProductsReviewsViewModel
    {
        private readonly IProductsDataService _productsDataService;
        private readonly IConnectionService _connectionService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;


        private User _AppUser;

        private int _ProductId;

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private string _noReviews;

        public string NoReviews
        {
            get { return _noReviews; }
            set { _noReviews = value; RaisePropertyChanged(() => NoReviews); }
        }


        private ObservableCollection<Review> _reviews;

        public ObservableCollection<Review> Reviews
        {
            get { return _reviews; }
            set { _reviews = value; RaisePropertyChanged(() => Reviews); }
        }


        public ProductsReviewsViewModel(IMvxMessenger messenger,
           IProductsDataService productsDataService,
            IConnectionService connectionService,
            IDialogService dialogService,
            IUserDataService userDataService) :base(messenger)
        {
            _productsDataService = productsDataService;
            _connectionService = connectionService;
            _userDataService = userDataService;
            _dialogService = dialogService;

        }

        public void Init(int ProductId)
        {
            _ProductId = ProductId;
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            // StorName = "new Stor";
            if (_connectionService.CheckOnline())
            {
                IsBusy = true;
                _AppUser = await _userDataService.GetSavedUser();
                Reviews = (await _productsDataService.GetProductReviews( _AppUser, _ProductId)).ToObservableCollection();

                if (Reviews.Count == 0)
                {
                    NoReviews = TextSource.GetText("noReviewsYet");
                }
                IsBusy = false;
                //ReviewItems itm = new ReviewItems()
                //{
                //    Rating = 3,
                //    ReviewText = "my new comment bla bla bla ..... dwdwdwd w jwkfjwklf ",
                //    WrittenOnStr = " 1 / 2 / 2013",
                //    CustomerName = "Mustafa 1"
                //};
                //ReviewItems itm2 = new ReviewItems()
                //{
                //    Rating = 5,
                //    ReviewText = "my new comment bla bla bla ..... dwdwdwd w jwkfjwklf ",
                //    WrittenOnStr = " 1 / 2 / 2013",
                //    CustomerName = "Mustafa 2 "
                //};
                //ReviewItems itm3 = new ReviewItems()
                //{
                //    ReviewText = "my new comment bla bla bla ..... dwdwdwd w jwkfjwklf ",
                //    WrittenOnStr = " 1 / 2 / 2013",
                //    CustomerName = "Mustafa 3 "
                //};
                //IsBusy = false;

                //Review rev = new Review() { Items = itm };
                //Reviews.Add(rev);

                //Review rev2 = new Review() { Items = itm2 };
                //Reviews.Add(rev2);

                //Review rev3 = new Review() { Items = itm3 };
                //Reviews.Add(rev3);
            }
            else
            {
                await _dialogService.ShowAlertAsync(TextSource.GetText("noInterner_"),
                 TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                // maybe we can navigate to a start page here, for you to add to this code base!
            }
        }


        // commands
        public MvxCommand CloseViewCommand
        { get { return new MvxCommand(() => Close(this)); } }
    }
}
