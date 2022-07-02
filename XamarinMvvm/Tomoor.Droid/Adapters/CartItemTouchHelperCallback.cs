using System;
using System.Linq;
using System.Collections.Generic;
using Android.Support.V7.Widget.Helper;
using Android.Support.V7.Widget;
using Ayadi.Core.ViewModel;
using Android.Graphics;
using Android.Util;
using Android.Support.Design.Widget;
using Ayadi.Core.Model;
using Android.Content;
using Android.Widget;

namespace Tomoor.Droid.Adapters
{
    class CartItemTouchHelperCallback : ItemTouchHelper.Callback
    {

        //https://stackoverflow.com/questions/37152965/remove-a-item-in-a-recyclerview-with-swipe-in-mvvmcross
        //http://smstuebe.de/2016/05/22/mvvmcross-swipe2dismiss/
        private readonly RecyclerView.Adapter _adapter;
        CartViewModel CartModel;

        //  Context _ctx;

        RecyclerView _view;

       // ShoppingCart _shop;

        // public static float ALPHA_FULL = 1.0f;

        public CartItemTouchHelperCallback(RecyclerView.Adapter adapter, CartViewModel viewModel,
            Context ctx, RecyclerView view)
        {
            _adapter = adapter;
            CartModel = viewModel;
            //     _ctx = ctx;
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
            int swipeFlags = 0;
            int dragFlags = 0;
            if (CartModel.CartItems[viewHolder.AdapterPosition] is Product)
            {
                 swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;
            }
            else
            {
                 dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            }

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
                Product pr_ = CartModel.CartItems[pos_] as Product;
                // Product St_ = CartModel.CartItems[pos_] as Product;
                Store Store_ = null;

                ShoppingCart _shop = CartModel.ShoppingList.Find(p => p.Product_id.ToString() == pr_.Id);
                // string Id_ = _shop.Id;
                CartModel.ShoppingList.Remove(_shop);

                // check if ther other product for that vendor
                int storeProducts = CartModel.ShoppingList.FindAll(p => p.Product.Vendor_id == pr_.Vendor_id).Count;
                if (storeProducts == 0)
                {
                     Store_ = CartModel.CartItems[pos_ - 1] as Store;
                    CartModel.CartItems.RemoveAt(pos_ - 1);
                    CartModel.CartItems.RemoveAt(pos_ - 1);
                }
                else
                {
                    CartModel.CartItems.RemoveAt(pos_);
                }
                

               
                CartModel.InitializeTotal();
                showSnackbar(_shop, pos_, Store_);
                // _adapter.NotifyItemRemoved(viewHolder.AdapterPosition);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;//x
            }

        }

        private void showSnackbar(ShoppingCart shop, int position, Store lastProductForStoer)
        {
            try
            {
                //https://blog.xamarin.com/add-beautiful-material-design-with-the-android-support-design-library/
                Snackbar dd = Snackbar.Make(_view, CartModel.DeleteMsgString, Snackbar.LengthLong)
                   .SetAction(CartModel.UndoString, (v) =>
                   {
                       if (lastProductForStoer != null)
                       {
                           CartModel.CartItems.Insert(position - 1, lastProductForStoer);
                       }
                       CartModel.CartItems.Insert(position, shop.Product);
                       CartModel.ShoppingList.Add(shop);
                       CartModel.InitializeTotal();
                   });
                callingDismiss dis = new callingDismiss();
                dd.AddCallback(dis);
                dis.SnackDissmesd +=  (s, a) =>
                {
                    CartModel.RemoveProductFromCart(shop.Id);
                };
                dd.SetActionTextColor(Color.Yellow);
                dd.View.FindViewById<TextView>(Resource.Id.snackbar_text).Gravity = Android.Views.GravityFlags.Left;
                dd.View.FindViewById<TextView>(Resource.Id.snackbar_text).SetTextColor(Color.White);
                dd.Show();
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

    }

    
}