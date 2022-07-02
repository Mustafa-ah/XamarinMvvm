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
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Tomoor.Droid.Adapters
{
    public class ProductsRecyclerViewItemSelector : IMvxTemplateSelector
    {
        private int _orientation;

        public ProductsRecyclerViewItemSelector(int oriantation)
        {
            _orientation = oriantation;
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            if (_orientation == 0)
            {
                return Resource.Layout.list_item_prodcut_h;
            }
            else
            {
                return Resource.Layout.list_item_prodcut;
            }
        }
    }
}