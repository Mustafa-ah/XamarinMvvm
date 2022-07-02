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
using Ayadi.Core.Model;

namespace Ayadi.Droid.Adapters
{
    public class CartMvxTemplateSelector : IMvxTemplateSelector
    {
        //http://smstuebe.de/2016/06/12/mvvmcross-recycler-templates/

        private readonly Dictionary<Type, int> _typeMapping;

        public CartMvxTemplateSelector()
        {
            _typeMapping = new Dictionary<Type, int>
        {
            {typeof(Product), Resource.Layout.list_item_cart},
            {typeof(Store), Resource.Layout.list_item_cart_header}
        };
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            return _typeMapping[forItemObject.GetType()];
        }
    }
}