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
using Ayadi.Droid.Adapters;
using MvvmCross.Binding.Droid.BindingContext;

namespace Ayadi.Droid.Views
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

            _cartListView.ItemTemplateSelector = new CartMvxTemplateSelector();

            _cartListView.Adapter = new CartAnimatorRecyclerAdapter((IMvxAndroidBindingContext)BindingContext, this);

           // _cartListView.sccr
        }
    }
}