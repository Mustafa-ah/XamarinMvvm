using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using System.Threading.Tasks;
using System.Threading;
using Ayadi.Core.Contracts.Services;
using Ayadi.Core.Contracts.Repository;
using Ayadi.Core.Model;
using Ayadi.Core.Repositories;

namespace Tomoor.Droid
{
    [Service]
    public class UpdateDataService : Service
    {
        CancellationTokenSource _cts;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            _cts = new CancellationTokenSource();
            Task.Run(() => { UpdateData();}, _cts.Token);
            
            return StartCommandResult.NotSticky;
        }

        private async void UpdateData()
        {
            try
            {
                Thread.Sleep(30000);

                IConnectionService _connectionService = Mvx.Resolve<IConnectionService>();
                bool _connected = _connectionService.CheckOnline();
                if (_connected)
                {
                    IUserRepository userRepo = Mvx.Resolve<IUserRepository>();
                    User user = await userRepo.GetSavedUser();

                    IProductRepository producting = Mvx.Resolve<IProductRepository>();
                    var Pls = await producting.GetHomeProductsFromAPI(user);

                    ICategoryRepository catReop = Mvx.Resolve<ICategoryRepository>();
                    var ls = await catReop.GetHomeCategoriesFromAPI(user);
                    var allLs = await catReop.GetAllCategoriesFromAPI(user);

                    SponserRepository spoRepo = new SponserRepository();
                    var spoList = await spoRepo.GetAllSponsersFromApi(user);

                    StoreRepository storRepo = new StoreRepository();
                    var storsList = storRepo.GetAllStorsFromAPI(user);
                }

                StopSelf();
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}