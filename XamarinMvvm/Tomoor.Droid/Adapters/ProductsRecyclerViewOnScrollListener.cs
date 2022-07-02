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
using Android.Support.V7.Widget;

namespace Tomoor.Droid.Adapters
{
    public class ProductsRecyclerViewOnScrollListener : RecyclerView.OnScrollListener
    {
        //https://gist.github.com/martijn00/a45a238c5452a273e602#file-xamarinrecyclerviewonscrolllistener-cs
        public delegate void LoadMoreEventHandler(object sender, EventArgs e);
        public event LoadMoreEventHandler LoadMoreEvent;
        public event LoadMoreEventHandler CancelLoadMoreEvent;

        private LinearLayoutManager LayoutManager;

        public ProductsRecyclerViewOnScrollListener(LinearLayoutManager layoutManager)
        {
            LayoutManager = layoutManager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var pastVisiblesItems = LayoutManager.FindFirstVisibleItemPosition();

            if ((visibleItemCount + pastVisiblesItems) >= totalItemCount)
            {
                LoadMoreEvent(this, null);
            }
            else
            {
                CancelLoadMoreEvent(this, null);
            }
        }
    }
}