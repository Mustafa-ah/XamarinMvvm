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
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V7.Widget;
using Android.Views.Animations;
using Ayadi.Droid.Views;
using Ayadi.Core.Model;

namespace Ayadi.Droid.Adapters
{
    public class CartAnimatorRecyclerAdapter : MvxRecyclerAdapter
    {
        //CartList
        CartView _ctx;

        public CartAnimatorRecyclerAdapter(IMvxAndroidBindingContext bindingContext, CartView ctx)
          : base(bindingContext)
        {
            _ctx = ctx;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);
            try
            {

                ImageView minus = holder.ItemView.FindViewById<ImageView>(Resource.Id.imageViewMinus);
                if (minus != null)
                {
                    //  minus.Click -= Minus_Click;
                    //  minus.Click += Minus_Click;
                    if (!minus.HasOnClickListeners)
                    {
                        minus.Click += async (s, e) =>
                        {
                            TextView _quantityTxt = holder.ItemView.FindViewById<TextView>(Resource.Id.textViewQuantity);
                            int quant = int.Parse(_quantityTxt.Text);
                            if (quant == 1)
                            {
                                string[] words = _ctx.ViewModel.GetMessageWords();
                                AlertDialog.Builder alert = new AlertDialog.Builder(_ctx);
                                alert.SetTitle(words[0]);
                                alert.SetMessage(words[1]);
                                alert.SetPositiveButton(words[2], (senderAlert, args) => {
                                    Toast.MakeText(_ctx, words[4], ToastLength.Short).Show();
                                    var position_ = holder.AdapterPosition;
                                    _ctx.ViewModel.CartItems.RemoveAt(position_);
                                    string id_ = _ctx.ViewModel.ShoppingList[position_].Id;
                                    _ctx.ViewModel.RemoveProductFromCart(id_);
                                    // ((CartView)_ctx).ViewModel.SaveCartList();
                                    _ctx.ViewModel.InitializeTotal();
                                });

                                alert.SetNegativeButton(words[3], (senderAlert, args) => {
                                    Toast.MakeText(_ctx, words[5], ToastLength.Short).Show();
                                });

                                Dialog dialog = alert.Create();
                                dialog.Show();

                            }
                            else
                            {
                                var position_ = holder.AdapterPosition;
                                ShoppingCart cartForUp = _ctx.ViewModel.ShoppingList[position_];
                                quant = quant - 1;
                                cartForUp.Quantity = cartForUp.Quantity - 1;
                                _quantityTxt.Text = quant.ToString();
                                SetAnimation(_quantityTxt);
                                // ((CartView)_ctx).ViewModel.SaveCartList();
                                ShoppingCart cart_ = await _ctx.ViewModel.PutCart(cartForUp);
                                _ctx.ViewModel.InitializeTotal();//PutCart
                            }
                        };
                    }
                    
                }

                ImageView plus = holder.ItemView.FindViewById<ImageView>(Resource.Id.imageViewPlus);
                if (plus != null)
                {
                    //  minus.Click -= Minus_Click;
                    //  minus.Click += Minus_Click;
                    if (!plus.HasOnClickListeners)
                    {
                        plus.Click += async (s, e) =>
                        {
                            TextView _quantityTxt = holder.ItemView.FindViewById<TextView>(Resource.Id.textViewQuantity);
                            int quant = int.Parse(_quantityTxt.Text);
                            var position_ = holder.AdapterPosition;
                            ShoppingCart cartForUp = _ctx.ViewModel.ShoppingList[position_];
                            cartForUp.Quantity = cartForUp.Quantity + 1;
                            quant = quant + 1;
                            _quantityTxt.Text = quant.ToString();
                            SetAnimation(_quantityTxt);
                            ShoppingCart cart_ = await _ctx.ViewModel.PutCart(cartForUp);
                            _ctx.ViewModel.InitializeTotal();
                            //((CartView)_ctx).ViewModel.SaveCartList();
                        };
                    }
                        
                }
            }
            catch 
            {

                ////throw;//x
            }
            

        }

        //private void Minus_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_quantity == null)
        //        {
        //            return;
        //        }
        //        ImageView MinusImag = sender as ImageView;
        //        int quant = int.Parse(_quantity.Text);
        //        quant = quant - 1;
        //        _quantity.Text = quant.ToString();
        //        ((CartView)_ctx).ViewModel.InitializeTotal();
        //        SetAnimation(_quantity);
        //    }
        //    catch (Exception)
        //    {

        //        //throw;//x
        //    }
        //}

        void SetAnimation(View viewToAnimate)
        {
            ScaleAnimation anim = new ScaleAnimation(0.0f, 1.0f, 0.0f, 1.0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            anim.Duration = 500;
            viewToAnimate.StartAnimation(anim);
        }
    }
}