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
    public class ProductsReviewViewModel : BaseViewModel, IProductsReviewViewModel
    {
        private readonly IProductsDataService _productsDataService;
        private readonly IConnectionService _connectionService;
        private readonly IUserDataService _userDataService;
        private readonly IDialogService _dialogService;

        private ReviewItems _reviewItems;

        public ReviewItems ReviewItems
        {
            get { return _reviewItems; }
            set { _reviewItems = value; RaisePropertyChanged(() => ReviewItems); }
        }

        #region review State
        public string Bad { get; set; }
        public string NotGodd { get; set; }
        public string Good { get; set; }
        public string ILikeit { get; set; }
        public string Iloveit { get; set; }
        #endregion

        private User _AppUser;

        private int _ProductId;

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public ProductsReviewViewModel(IMvxMessenger messenger,
           IProductsDataService productsDataService,
            IConnectionService connectionService,
            IDialogService dialogService,
            IUserDataService userDataService) : base(messenger)
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
                ReviewItems = new ReviewItems();
                IsBusy = false;

                #region rev Sattes
                Bad = TextSource.GetText("bad");
                NotGodd = TextSource.GetText("notGoog");
                Good = TextSource.GetText("good");
                ILikeit = TextSource.GetText("iLike");
                Iloveit = TextSource.GetText("iLove");
                #endregion
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

        public MvxCommand RatingCommand
        { get { return new MvxCommand(() => Rating()); } }

        private async void Rating()
        {
            try
            {
                if (ReviewItems.Rating == 0)
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("makeRate"),
                         TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                    return;
                }
                //else if(string.IsNullOrEmpty(ReviewItems.ReviewText))
                //{
                //    await _dialogService.ShowAlertAsync("Enter Comment",
                //        TextSource.GetText("tomoor_"), TextSource.GetText("ok_"));
                //    return;
                //}
                ReviewItems.CustomerId = _AppUser.Id;
             //   ReviewItems.CustomerName = "name";// _AppUser.First_name;
                ReviewItems.WrittenOnStr = DateTime.Now.ToString();
                ReviewItems.StoreId = 1;
              //  ReviewItems.Title = "Title";
                IsBusy = true;
                bool rated = await _productsDataService.PostProductReviews(_AppUser, _ProductId, ReviewItems);
                IsBusy = false;
                if (rated)
                {
                     _dialogService.ShowToast(TextSource.GetText("done"));
                    Close(this);
                }
                else
                {
                    await _dialogService.ShowAlertAsync(TextSource.GetText("fail"),
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
