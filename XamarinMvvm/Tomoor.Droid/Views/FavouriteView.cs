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
using MvvmCross.Droid.Views;
using Ayadi.Core.ViewModel;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Android.Support.V7.Widget.Helper;
using Tomoor.Droid.Adapters;
using Android.Support.V7.Widget;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "FavouriteView",Theme = "@style/Theme.AppCompat")]
    public class FavouriteView : MvxActivity<FavouriteViewModel>
    {

        public new FavouriteViewModel ViewModel
        {
            get { return (FavouriteViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Favourites);

            MvxRecyclerView recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.favtList);

            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(this);
                recyclerView.SetLayoutManager(layoutManager);

                ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(
                    recyclerView.GetAdapter(), ViewModel, this, recyclerView);

                ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
                itemTouchHelper.AttachToRecyclerView(recyclerView);
               // recyclerView.SetItemAnimator(new DefaultItemAnimator());
            }
        }
    }
}