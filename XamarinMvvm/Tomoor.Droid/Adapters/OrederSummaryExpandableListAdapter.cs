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
using Java.Lang;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Ayadi.Core.ViewModel;
using MvvmCross.Droid.Support.V7.RecyclerView;
using Tomoor.Droid.Views;
using MvvmCross.Binding.BindingContext;
using Ayadi.Core.Model;
using FFImageLoading;
using FFImageLoading.Views;

namespace Tomoor.Droid.Adapters
{
    public class OrederSummaryExpandableListAdapter : BaseExpandableListAdapter
    {

        readonly CheckoutSummaryView context;
        protected List<Order> DataList { get; set; }
        LayoutInflater inflater;

       // CheckoutSummaryViewModel _viewModel;

        public OrederSummaryExpandableListAdapter(CheckoutSummaryView newContext, List<Order> newList) : base()
        {
            context = newContext;
            DataList = newList;
            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

          //  _viewModel = viewModel;
        }

        public override int GroupCount
        {
            get
            {
                return DataList.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return (long)childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return DataList[groupPosition].Order_items.Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View child = convertView;
            Order order_ = DataList[groupPosition];

            if (order_.Order_status == "ProductsState")
            {
                Product pro = DataList[groupPosition].Order_items[childPosition].Product;
                child = inflater.Inflate(Resource.Layout.list_item_expandable_child_products, null);

                child.FindViewById<TextView>(Resource.Id.textViewPprice).Text = pro.Price + " SAR ";
                child.FindViewById<TextView>(Resource.Id.textViewQuantity).Text = pro.Quantity.ToString();
                child.FindViewById<TextView>(Resource.Id.textViewPname).Text = pro.Name;

                ImageViewAsync imgP = child.FindViewById<ImageViewAsync>(Resource.Id.imageViewPro);
                ImageService.Instance.LoadUrl(pro.ProductImage).Into(imgP);
            }
            else if (order_.Order_status == "PaymentState")
            {
                child = inflater.Inflate(Resource.Layout.list_item_expandable_child_payment, null);

                child.FindViewById<TextView>(Resource.Id.textViewPay).Text = order_.Paymet_Method.FriendlyName;
                child.FindViewById<TextView>(Resource.Id.textViewDec).Text = order_.Paymet_Method.PaymentMethodDescription;
                ImageViewAsync imgPLogo = child.FindViewById<ImageViewAsync>(Resource.Id.imageViewPlogo);
                ImageService.Instance.LoadUrl(order_.Paymet_Method.LogoUrl).Into(imgPLogo);
            }
            else if (order_.Order_status == "ShippingState")
            {
                //id textViewShiptextViewDec  imageViewPlogo
                child = inflater.Inflate(Resource.Layout.list_item_expandable_child_payment, null);
            }
            else if (order_.Order_status == "BillingState")
            {
                //textViewBillingAdress
                child = inflater.Inflate(Resource.Layout.list_item_expandable_child_billing, null);

                // billing
                string adress1 = order_.Billing_address.Address1 + ", " + order_.Billing_address.Address;
                string adress2 = order_.Billing_address.City + ", " + order_.Billing_address.Phone_number + ", " + order_.Billing_address.Company;
                child.FindViewById<TextView>(Resource.Id.textViewBillingSrt).Text = adress1;
                child.FindViewById<TextView>(Resource.Id.textViewBillingAdress).Text = adress2;

                child.FindViewById<TextView>(Resource.Id.textViewBillingAd).Text = context.ViewModel.BillingAdressString;

                // Shipping
                string adress3 = order_.Shipping_address.Address1 + ", " + order_.Shipping_address.Address;
                string adress4 = order_.Shipping_address.City + ", " + order_.Shipping_address.Phone_number + ", " + order_.Shipping_address.Company;
                child.FindViewById<TextView>(Resource.Id.textViewShippingSrt).Text = adress3;
                child.FindViewById<TextView>(Resource.Id.textViewShippingAdress).Text = adress4;

                child.FindViewById<TextView>(Resource.Id.textViewShippingAd).Text = context.ViewModel.ShippingAdresslString;
            }

            //var recyclerView = child.FindViewById<MvxRecyclerView>(Resource.Id.ProductsList);
            //var set = context.CreateBindingSet<CheckoutSummaryView, CheckoutSummaryViewModel>();
            //set.Bind(recyclerView)
            //    .For(v => v.ItemsSource)
            //    .To(vm => vm.Products);
            //set.Apply();
            //ProductsList
            return child;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            Order order_ = DataList[groupPosition];

            if (order_.Order_status == "ProductsState")
            {
                header = inflater.Inflate(Resource.Layout.list_item_expanable_header_products, null);
                header.FindViewById<TextView>(Resource.Id.textViewSubTotal).Text =
                    order_.Order_Subtotal.ToString() + " SAR ";

                header.FindViewById<TextView>(Resource.Id.textViewProducts).Text = context.ViewModel.ProductsString;
                header.FindViewById<TextView>(Resource.Id.textViewSub).Text = context.ViewModel.SubtotalString;
            }
            else if (order_.Order_status == "PaymentState")
            {
                header = inflater.Inflate(Resource.Layout.list_item_expanable_header_payment, null);
                header.FindViewById<TextView>(Resource.Id.textViewPay).Text = context.ViewModel.PaymentString;
            }
            else if (order_.Order_status == "ShippingState")
            {
                header = inflater.Inflate(Resource.Layout.list_item_expanable_header_shippingAdress, null);
                header.FindViewById<TextView>(Resource.Id.textViewShip).Text = context.ViewModel.ShippingString;
            }
            else if (order_.Order_status == "BillingState")
            {
                header = inflater.Inflate(Resource.Layout.list_item_expanable_header_billing, null);
                header.FindViewById<TextView>(Resource.Id.textViewAddresses).Text = context.ViewModel.AdressString;
            }

            return header;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}