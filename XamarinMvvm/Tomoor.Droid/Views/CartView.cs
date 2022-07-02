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
using Tomoor.Droid.Adapters;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "CartView")]
    public class CartView : MvxActivity<CartViewModel>
    {
        private MvxRecyclerView _cartListView;

        public new CartViewModel ViewModel
        {
            get { return (CartViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activiy_Cart);

            _cartListView = FindViewById<MvxRecyclerView>(Resource.Id.CartList);
           
            if (_cartListView != null)
            {
                _cartListView.ItemTemplateSelector = new CartMvxTemplateSelector();

                _cartListView.Adapter = new CartAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext, this);

                _cartListView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(this);
                _cartListView.SetLayoutManager(layoutManager);

                ItemTouchHelper.Callback callback = new CartItemTouchHelperCallback(
                    _cartListView.GetAdapter(), ViewModel, this, _cartListView);

                ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
                itemTouchHelper.AttachToRecyclerView(_cartListView);
                // recyclerView.SetItemAnimator(new DefaultItemAnimator());
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (ViewModel.ShouldClose)
            {
                Finish();
            }
        }
    }
}