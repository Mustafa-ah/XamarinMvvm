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
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using Ayadi.Core.ViewModel;
using Android.Graphics;
using Android.Util;
using Android.Support.Design.Widget;
using Ayadi.Core.Model;

namespace Ayadi.Droid.Adapters
{
    public class SimpleItemTouchHelperCallback : ItemTouchHelper.Callback
    {

        //https://stackoverflow.com/questions/37152965/remove-a-item-in-a-recyclerview-with-swipe-in-mvvmcross
        //http://smstuebe.de/2016/05/22/mvvmcross-swipe2dismiss/
        private readonly RecyclerView.Adapter _adapter;
        FavouriteViewModel faveModel;

        Context _ctx;

        RecyclerView _view;

        ShoppingCart _shop;

       // public static float ALPHA_FULL = 1.0f;

        public SimpleItemTouchHelperCallback(RecyclerView.Adapter adapter, FavouriteViewModel viewModel,
            Context ctx, RecyclerView view)
        {
            _adapter = adapter;
            faveModel = viewModel;
            _ctx = ctx;
            _view = view;
        }
       
        //public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        //{
        //    //https://stackoverflow.com/questions/30820806/adding-a-colored-background-with-text-icon-under-swiped-row-when-using-androids
        //    if (actionState == ItemTouchHelper.ActionStateSwipe)
        //    {
        //        View itemView = viewHolder.ItemView;
        //        Paint p = new Paint();
        //        Bitmap icon;
        //        if (dX > 0)
        //        {
        //            icon = BitmapFactory.DecodeResource(_ctx.Resources, Resource.Drawable.delete_icon);

        //            /* Set your color for positive displacement */
        //            p.SetARGB(0, 255, 251, 255);

        //            // Draw Rect with varying right side, equal to displacement dX
        //            c.DrawRect((float)itemView.Left, (float)itemView.Top, dX,
        //                    (float)itemView.Bottom, p);
        //            float pdpp = (float)itemView.Top + ((float)itemView.Bottom - (float)itemView.Top - icon.Height) / 2;
        //            float dfd = (float)itemView.Left + (float)convertDpToPx(16);
        //            // Set the image icon for Right swipe
        //            c.DrawBitmap(icon,
        //                    pdpp,
        //                    dfd,
        //                    p);
        //        }
        //        else
        //        {
        //            icon = BitmapFactory.DecodeResource(_ctx.Resources, Resource.Drawable.delete_icon);

        //            /* Set your color for negative displacement */
        //            p.SetARGB(0, 255, 251, 255);

        //            // Draw Rect with varying left side, equal to the item's right side
        //            // plus negative displacement dX
        //            c.DrawRect((float)itemView.Right + dX, (float)itemView.Top,
        //                    (float)itemView.Right, (float)itemView.Bottom, p);

        //            //Set the image icon for Left swipe
        //            c.DrawBitmap(icon,
        //                    (float)itemView.Right - (float)convertDpToPx(16) - icon.Width,
        //                    (float)itemView.Top + ((float)itemView.Bottom - (float)itemView.Top - icon.Height) / 2,
        //                    p);
        //        }
        //        // Fade out the view as it is swiped out of the parent's bounds
        //        float alpha = ALPHA_FULL - Math.Abs(dX) / (float)viewHolder.ItemView.Width;
        //        viewHolder.ItemView.Alpha = alpha;
        //        viewHolder.ItemView.TranslationX = dX;// SetTranslationX(dX);


        //    }
        //    else
        //    {
        //        base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
        //    }
        //}

        //private double convertDpToPx(int dp)
        //{
        //    return Math.Round(dp * _ctx.Resources.DisplayMetrics.Xdpi / (float)DisplayMetricsDensity.Default);
        //}
      
        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            int swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;
            return MakeMovementFlags(dragFlags, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            _adapter.NotifyItemMoved(viewHolder.AdapterPosition, target.AdapterPosition);
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            try
            {
                int pos_ = viewHolder.AdapterPosition;
                _shop = faveModel.WishList[pos_];
               // string Id_ = _shop.Id;

                faveModel.FavItems.RemoveAt(pos_);

                showSnackbar(_shop, pos_);
                // _adapter.NotifyItemRemoved(viewHolder.AdapterPosition);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;//x
            }

        }

        private void showSnackbar(ShoppingCart shop, int position)
        {
            try
            {
                //https://blog.xamarin.com/add-beautiful-material-design-with-the-android-support-design-library/
                Snackbar dd = Snackbar.Make(_view, "Un do delete", Snackbar.LengthLong)
                   .SetAction("Un Do", (v) => 
                   {
                       faveModel.FavItems.Insert(position, shop.Product);
                   });
                callingDismiss dis = new callingDismiss();
                dd.AddCallback(dis);
                dis.SnackDissmesd += async (s, a) =>
                {
                    bool deleted = await faveModel.RemoveProductFromFavourite(shop.Id);
                };
                dd.SetActionTextColor(Color.Yellow);
                dd.Show();
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

    }

    public class callingDismiss: Snackbar.Callback
    {
        public event EventHandler SnackDissmesd;
        public override void OnDismissed(Snackbar transientBottomBar, int @event)
        {
            base.OnDismissed(transientBottomBar, @event);
            if (@event == DismissEventTimeout || @event == DismissEventConsecutive || @event == DismissEventManual)
            {
                SnackDissmesd?.Invoke(this, new EventArgs());
            }
        }
    }
}