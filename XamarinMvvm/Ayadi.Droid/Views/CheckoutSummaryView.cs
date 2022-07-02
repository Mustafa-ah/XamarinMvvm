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
using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Ayadi.Core.Model;

namespace Ayadi.Droid.Views
{
    [Activity(Label = "CheckoutSummaryView")]
    public class CheckoutSummaryView : MvxActivity<CheckoutSummaryViewModel>
    {
        // MvxRecyclerView recycler;

        public new CheckoutSummaryViewModel ViewModel
        {
            get { return (CheckoutSummaryViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        ExpandableListView _summryList;
        Adapters.OrederSummaryExpandableListAdapter ItemsAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Activity_CheckoutSummary);

          //  ViewModel.ViewModelReady += ViewModel_ViewModelReady;
            //recycler = FindViewById<MvxRecyclerView>(Resource.Id.ProductsList);

            // LinearLayoutManager manager =
            //   new LinearLayoutManager(this, LinearLayoutManager.Vertical, false) { AutoMeasureEnabled = true };

            //recycler.SetLayoutManager(manager);

            
        }

        private void ViewModel_ViewModelReady(object sender, EventArgs e)
        {
            try
            {
                List<Order> orders_ = new List<Order>();

                Order Porder = new Order();
                Porder.Order_status = "ProductsState";
                Porder.Order_items = ViewModel.CurrentOrder.Order_items;
                orders_.Add(Porder);

                Order PaymentOrder = new Order();
                PaymentOrder.Order_status = "PaymentState";
                PaymentOrder.Order_items = new List<OrderItems>() { new OrderItems() };// jsut one child
                PaymentOrder.Payment_method_system_name = ViewModel.CurrentOrder.Payment_method_system_name;
                orders_.Add(PaymentOrder);

                Order ShippOrder = new Order();
                ShippOrder.Order_status = "ShippingState";
                ShippOrder.Order_items = new List<OrderItems>() { new OrderItems() };// jsut one child
                ShippOrder.Shipping_method = ViewModel.CurrentOrder.Shipping_method;
                orders_.Add(ShippOrder);

                Order BillOrder = new Order();
                BillOrder.Order_status = "BillingState";
                BillOrder.Order_items = new List<OrderItems>() { new OrderItems() };// jsut one child
                BillOrder.Billing_address = ViewModel.CurrentOrder.Billing_address;
                orders_.Add(BillOrder);

                ItemsAdapter = new Adapters.OrederSummaryExpandableListAdapter(this, orders_);

                _summryList = FindViewById<ExpandableListView>(Resource.Id.SummaryExpandableListview);
                _summryList.SetAdapter(ItemsAdapter);

                // advanced way =>
                //https://stackoverflow.com/questions/6873345/expand-all-children-in-expandable-list-view
                _summryList.ExpandGroup(1, true);
                _summryList.ExpandGroup(2, true);
                _summryList.ExpandGroup(3, true);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }


        //protected override void OnViewModelSet()
        //{
        //    base.OnViewModelSet();

        //    List<Order> orders_ = new List<Order>();
        //    ViewModel.CurrentOrder.Order_status = "ProductsState";
        //    orders_.Add(ViewModel.CurrentOrder);



        //    ItemsAdapter = new Adapters.OrederSummaryExpandableListAdapter(this, orders_);

        //    _summryList = FindViewById<ExpandableListView>(Resource.Id.SummaryExpandableListview);
        //   // _summryList.SetAdapter(ItemsAdapter);

        //}
    }
}