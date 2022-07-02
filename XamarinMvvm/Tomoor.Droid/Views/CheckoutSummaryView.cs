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
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;
using Android.Support.V4.Content;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "CheckoutSummaryView")]
    public class CheckoutSummaryView : MvxActivity<CheckoutSummaryViewModel>
    {
        // MvxRecyclerView recycler;
        BindableProgressBar _bindableProgressBar;

        System.Timers.Timer timer;
        int lastExpandedPosition = -1;
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

            timer = new System.Timers.Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            //recycler = FindViewById<MvxRecyclerView>(Resource.Id.ProductsList);

            // LinearLayoutManager manager =
            //   new LinearLayoutManager(this, LinearLayoutManager.Vertical, false) { AutoMeasureEnabled = true };
          //  FindViewById<Button>(Resource.Id.buttonCheckout).Click += (s, a) => { FinishAffinity();StartActivity(new Intent(this, typeof(HomeView))); };
            //recycler.SetLayoutManager(manager);
            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<CheckoutSummaryView, CheckoutSummaryViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            try
            {
                if (ViewModel.ViewModelInitilized)
                {
                    List<Order> orders_ = new List<Order>();

                    Order Porder = new Order();
                    Porder.Order_status = "ProductsState";
                    Porder.Order_items = ViewModel.CurrentOrder.Order_items;
                    Porder.Order_Subtotal = ViewModel.CurrentOrder.Order_Subtotal;
                    orders_.Add(Porder);

                    Order PaymentOrder = new Order();
                    PaymentOrder.Order_status = "PaymentState";
                    PaymentOrder.Order_items = new List<OrderItems>() { new OrderItems() };// jsut one child
                    PaymentOrder.Payment_method_system_name = ViewModel.CurrentOrder.Payment_method_system_name;
                    PaymentOrder.Paymet_Method = ViewModel.CurrentOrder.Paymet_Method;
                    orders_.Add(PaymentOrder);

                    //Order ShippOrder = new Order();
                    //ShippOrder.Order_status = "ShippingState";
                    //ShippOrder.Order_items = new List<OrderItems>() { new OrderItems() };// jsut one child
                    //ShippOrder.Shipping_method = ViewModel.CurrentOrder.Shipping_method;
                    //orders_.Add(ShippOrder);

                    Order BillOrder = new Order();
                    BillOrder.Order_status = "BillingState";
                    BillOrder.Order_items = new List<OrderItems>() { new OrderItems() };// jsut one child
                    BillOrder.Billing_address = ViewModel.CurrentOrder.Billing_address;
                    BillOrder.Shipping_address = ViewModel.CurrentOrder.Shipping_address;
                    orders_.Add(BillOrder);



                    ItemsAdapter = new Adapters.OrederSummaryExpandableListAdapter(this, orders_);

                    _summryList = FindViewById<ExpandableListView>(Resource.Id.SummaryExpandableListview);
                    _summryList.GroupExpand += _summryList_GroupExpand;
                  //  _summryList.SetGroupIndicator(ContextCompat.GetDrawable(this, Resource.Drawable.ExpandableListIndicator));
                    RunOnUiThread(() =>
                    {
                        _summryList.SetAdapter(ItemsAdapter);
                       // _summryList.ExpandGroup(3, true);
                    });
                    // advanced way =>
                    //https://stackoverflow.com/questions/6873345/expand-all-children-in-expandable-list-view
                    //  _summryList.ExpandGroup(1, true);
                    //  _summryList.ExpandGroup(2, true);

                    ViewModel.ViewModelInitilized = false;
                    timer.Dispose();
                    // timer.Dispose();
                }
                //if (ViewModel.OrderPosted)
                //{
                //    Toast.MakeText(this, "Order Completed ", ToastLength.Short).Show();
                //    timer.Dispose();
                //    ViewModel.OrderPosted = false;
                //    FinishAffinity();
                //    Intent homeIntent = new Intent(this, typeof(HomeView));
                //    //homeIntent.SetFlags(ActivityFlags.ClearTop);
                //    StartActivity(homeIntent);
                //}
            }
            catch (Exception)
            {
                timer.Dispose();
                //throw;//x
            }
        }

        private void _summryList_GroupExpand(object sender, ExpandableListView.GroupExpandEventArgs e)
        {
            if (lastExpandedPosition != -1
                    && e.GroupPosition != lastExpandedPosition)
            {
                _summryList.CollapseGroup(lastExpandedPosition);
            }
            lastExpandedPosition = e.GroupPosition;
           
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