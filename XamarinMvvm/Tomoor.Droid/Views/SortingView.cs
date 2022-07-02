using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using Ayadi.Core.ViewModel;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Graphics;

namespace Tomoor.Droid.Views
{
    [MvxFragment(typeof(ProductsViewModel), Resource.Id.left_drawer, false)]
    [Register("Tomoor.Droid.Views.SortingView")]
    public class SortingView : MvxFragment<SortingViewModel>
    {

        public new SortingViewModel ViewModel
        {
            get { return (SortingViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        XmlDb _Db;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _Db = new XmlDb(Activity);
            // Create your fragment here
        }

        LinearLayout gridText;
        LinearLayout Listext;

        //invisible
        TextView txtPotition;

        TextView txtnameA;
        TextView txtNameZ;
        TextView txtPriceH;
        TextView txtPriceL;
        TextView txtUpdate;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.Fragment_sorting, null);
             gridText = fragView.FindViewById<LinearLayout>(Resource.Id.linearLayoutGrid);
             Listext = fragView.FindViewById<LinearLayout>(Resource.Id.linearLayoutList);

            //invisible
            txtPotition = fragView.FindViewById<TextView>(Resource.Id.textViewPosition);

            txtnameA = fragView.FindViewById<TextView>(Resource.Id.textViewName1);
            txtNameZ = fragView.FindViewById<TextView>(Resource.Id.textViewName2);
            txtPriceH = fragView.FindViewById<TextView>(Resource.Id.textViewPrice1);
            txtPriceL = fragView.FindViewById<TextView>(Resource.Id.textViewPrice2);
            txtUpdate = fragView.FindViewById<TextView>(Resource.Id.textViewUpdate);


            string viewType = _Db.GetProductsStyle();
            HightLight(viewType);

            ViewModel.ViewChanged += ViewModel_ViewChanged;
            ViewModel.Sorted += ViewModel_Sorted;

            return fragView;
        }

        private void ViewModel_Sorted(object sender, EventArgs e)
        {
            string sortBy = sender as string;
            try
            {
                switch (sortBy)
                {
                    case "ByNameAtoZ":
                        txtnameA.SetBackgroundColor(Color.Yellow);
                        txtNameZ.SetBackgroundColor(Color.White);
                        txtPriceH.SetBackgroundColor(Color.White);
                        txtPriceL.SetBackgroundColor(Color.White);
                        txtUpdate.SetBackgroundColor(Color.White);
                        txtPotition.SetBackgroundColor(Color.White);
                        break;

                    case "ByNameZtoA":
                        txtnameA.SetBackgroundColor(Color.White);
                        txtNameZ.SetBackgroundColor(Color.Yellow);
                        txtPriceH.SetBackgroundColor(Color.White);
                        txtPriceL.SetBackgroundColor(Color.White);
                        txtUpdate.SetBackgroundColor(Color.White);
                        txtPotition.SetBackgroundColor(Color.White);
                        break;

                    case "ByPriceMaxToMin":
                        txtnameA.SetBackgroundColor(Color.White);
                        txtNameZ.SetBackgroundColor(Color.White);
                        txtPriceH.SetBackgroundColor(Color.Yellow);
                        txtPriceL.SetBackgroundColor(Color.White);
                        txtUpdate.SetBackgroundColor(Color.White);
                        txtPotition.SetBackgroundColor(Color.White);
                        break;

                    case "ByPriceMinToMax":
                        txtnameA.SetBackgroundColor(Color.White);
                        txtNameZ.SetBackgroundColor(Color.White);
                        txtPriceH.SetBackgroundColor(Color.White);
                        txtPriceL.SetBackgroundColor(Color.Yellow);
                        txtUpdate.SetBackgroundColor(Color.White);
                        txtPotition.SetBackgroundColor(Color.White);
                        break;

                    case "ByDefaultPlace":
                        txtnameA.SetBackgroundColor(Color.White);
                        txtNameZ.SetBackgroundColor(Color.White);
                        txtPriceH.SetBackgroundColor(Color.White);
                        txtPriceL.SetBackgroundColor(Color.White);
                        txtUpdate.SetBackgroundColor(Color.White);
                        txtPotition.SetBackgroundColor(Color.Yellow);
                        break;

                    case "ByLastUpdate":
                        txtnameA.SetBackgroundColor(Color.White);
                        txtNameZ.SetBackgroundColor(Color.White);
                        txtPriceH.SetBackgroundColor(Color.White);
                        txtPriceL.SetBackgroundColor(Color.White);
                        txtUpdate.SetBackgroundColor(Color.Yellow);
                        txtPotition.SetBackgroundColor(Color.White);
                        break;

                    default:
                        txtnameA.SetBackgroundColor(Color.White);
                        txtNameZ.SetBackgroundColor(Color.White);
                        txtPriceH.SetBackgroundColor(Color.White);
                        txtPriceL.SetBackgroundColor(Color.White);
                        txtUpdate.SetBackgroundColor(Color.White);
                        txtPotition.SetBackgroundColor(Color.White);
                        break;
                }
            }
            catch
            {

                //throw;//x
            }
        }

        private void ViewModel_ViewChanged(object sender, EventArgs e)
        {
            string view_ = sender as string;
            HightLight(view_);
        }

        private void HightLight(string viewType)
        {
            if (viewType == "Grid")
            {
                gridText.SetBackgroundColor(Color.Yellow);
                Listext.SetBackgroundColor(Color.White);
            }
            else
            {
                gridText.SetBackgroundColor(Color.White);
                Listext.SetBackgroundColor(Color.Yellow);
            }
        }

        
    }
}