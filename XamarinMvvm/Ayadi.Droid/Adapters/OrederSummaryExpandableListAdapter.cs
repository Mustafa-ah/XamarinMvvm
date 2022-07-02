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
using Ayadi.Droid.Views;
using MvvmCross.Binding.BindingContext;
using Ayadi.Core.Model;
using FFImageLoading;
using FFImageLoading.Views;

namespace Ayadi.Droid.Adapters
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

                child.FindViewById<TextView>(Resource.Id.textViewPay).Text = order_.Payment_method_system_name;
            }
            else if (order_.Order_status == "ShippingState")
            {
                //id textViewShip
                child = inflater.Inflate(Resource.Layout.list_item_expandable_child_payment, null);
            }
            else if (order_.Order_status == "BillingState")
            {
                //textViewBillingAdress
                child = inflater.Inflate(Resource.Layout.list_item_expandable_child_billing, null);
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
            }
            else if (order_.Order_status == "PaymentState")
            {
                header = inflater.Inflate(Resource.Layout.list_item_expanable_header_payment, null);
            }
            else if (order_.Order_status == "ShippingState")
            {
                header = inflater.Inflate(Resource.Layout.list_item_expanable_header_shippingAdress, null);
            }
            else if (order_.Order_status == "BillingState")
            {
                header = inflater.Inflate(Resource.Layout.list_item_expanable_header_billing, null);
            }

            return header;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
    }
}