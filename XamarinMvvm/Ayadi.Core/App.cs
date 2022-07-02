using Akavache;
using System.Reactive.Linq;
using Ayadi.Core.Utility;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Localization;
using Ayadi.Core.Contracts.Repository;
using Ayadi.Core.Model;
using Ayadi.Core.Repositories;
using Ayadi.Core.Contracts.Services;
using MvvmCross.Plugins.Messenger;

namespace Ayadi.Core
{
    public class App : MvxApplication
    {
        public string Lang { get; set; }
        public override void Initialize()
        {
           
            base.Initialize();
            // so important :
            // LinkerPleaseInclude
            //MvxSubscriptionToken
           
            //GetLang();
            //if (Lang == null)
            //{
            //    Lang = "ar-SA";
            //}

            CreatableTypes()
               .EndingWith("Repository")
               .AsInterfaces()
               .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.RegisterSingleton<IMvxTextProvider>
                (new ResxTextProvider(Strings.ResourceManager,Lang));

            BlobCache.ApplicationName = "Ayadi";

            RegisterAppStart(new AppStart());

            InitializeUserAsync();
        }

        //private async void GetLang()
        //{
        //    Lang =  await BlobCache.LocalMachine.GetObject<string>("Lang");
        //}

        private async void InitializeUserAsync()
        {
            IUserRepository userRepo = Mvx.Resolve<IUserRepository>();
               IConnectionService _connectionService = Mvx.Resolve<IConnectionService>();
            bool _connected = _connectionService.CheckOnline();
            User _AppUser = await userRepo.GetSavedUser();
            if (_AppUser.LangID == null)
            {
                _AppUser.LangID = Lang == "ar-SA" ? "3" : "1";
                bool IsSaved = await userRepo.SaveUserToLocal(_AppUser);
            }
            if (_connected)
            {
                if (_AppUser.AccessToken == null)
                {
                    _AppUser.AccessToken = await userRepo.GetUserAccessToken();
                    bool IsSaved = await userRepo.SaveUserToLocal(_AppUser);
                }
                _AppUser = await userRepo.GetSavedUser();
                if (_AppUser.Id == 0)
                {
                    User userGuestId = await userRepo.LoginAsGuest(_AppUser.AccessToken);
                    _AppUser.Id = userGuestId.Id;
                    _AppUser.IsGuestUser = true;
                    bool IsSaved_ = await userRepo.SaveUserToLocal(_AppUser);
                }
                else
                {
                    ICartRepository cartRepo = Mvx.Resolve<ICartRepository>();
                    var shopingList = await cartRepo.GetShoppingCartItemsFromAPI(_AppUser);
                    //get new guest user
                    if (_AppUser.IsGuestUser && shopingList.Count == 0)
                    {
                        User userGuestId = await userRepo.LoginAsGuest(_AppUser.AccessToken);
                        _AppUser.Id = userGuestId.Id;
                        _AppUser.IsGuestUser = true;
                        bool IsSaved_ = await userRepo.SaveUserToLocal(_AppUser);
                    }
                    //await cartRepo.SaveShopingCartToLocal(shopingList);
                    cartRepo.SetActiveShoppingList(shopingList);
                    SendMessge(shopingList);
                }
            }
            
        }

        private void SendMessge (List<ShoppingCart> shopList)
        {
            IMvxMessenger messaenger = Mvx.Resolve<IMvxMessenger>();
            messaenger.Publish(new Messages.ShoppingListMessage(this, shopList));
        }
    }
}
