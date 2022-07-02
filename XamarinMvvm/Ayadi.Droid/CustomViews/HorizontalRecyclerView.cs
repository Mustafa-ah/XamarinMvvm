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
using MvvmCross.Droid.Support.V7.RecyclerView;
using Android.Util;
using Android.Support.V7.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView.AttributeHelpers;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Ayadi.Droid.CustomViews
{
    [Register("Ayadi.Droid.CustomViews.HorizontalRecyclerView")]
    public class HorizontalRecyclerView : MvxRecyclerView
    {
        public HorizontalRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
        }

        public HorizontalRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0, new MvxRecyclerAdapter())
    {
        }

        public HorizontalRecyclerView(Context context, IAttributeSet attrs, int defStyle) : this(context, attrs, defStyle, new MvxRecyclerAdapter())
    {
        }

        public HorizontalRecyclerView(Context context, IAttributeSet attrs, int defStyle, IMvxRecyclerAdapter adapter) : base(context, attrs, defStyle, adapter)
    {
            if (adapter == null)
                return;

            var layoutManager = new LinearLayoutManager(context) { Orientation = Horizontal };
            SetLayoutManager(layoutManager);

            var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
            var itemTemplateSelector = MvxRecyclerViewAttributeExtensions.BuildItemTemplateSelector(context, attrs);

            adapter.ItemTemplateSelector = itemTemplateSelector;
            Adapter = adapter;

            if (itemTemplateSelector.GetType() == typeof(MvxDefaultTemplateSelector))
                ItemTemplateId = itemTemplateId;
        }
    }
}
