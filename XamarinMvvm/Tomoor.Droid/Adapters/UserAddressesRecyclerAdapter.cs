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
using Tomoor.Droid.Views;
using Ayadi.Core.Model;


namespace Tomoor.Droid.Adapters
{
    public class UserAddressesRecyclerAdapter : MvxRecyclerAdapter
    {
        UserAdressesView _ctx;

        public UserAddressesRecyclerAdapter(IMvxAndroidBindingContext bindingContext, UserAdressesView ctx)
          : base(bindingContext)
        {
            _ctx = ctx;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);
            try
            {
                int pos_ = holder.AdapterPosition;
                ImageView UpdateImage = holder.ItemView.FindViewById<ImageView>(Resource.Id.imageViewUp);
                if (UpdateImage != null)
                {

                    if (!UpdateImage.HasOnClickListeners)
                    {
                        UpdateImage.Click += (s, e) =>
                        {
                            UpdateImage.Enabled = false;
                            UserAdress ads_ = _ctx.ViewModel.Adresses[pos_];
                            _ctx.ViewModel.updateAddress(ads_);
                            UpdateImage.Enabled = true;
                        };
                    }

                }

                ImageView DeleteImg = holder.ItemView.FindViewById<ImageView>(Resource.Id.imageViewDel);
                if (DeleteImg != null)
                {
                    //  minus.Click -= Minus_Click;
                    //  minus.Click += Minus_Click; 
                    if (!DeleteImg.HasOnClickListeners)
                    {
                        DeleteImg.Click +=  (s, e) =>
                        {
                            DeleteImg.Enabled = false;
                           // Android.Util.Log.Error("loading service ", $".............................{pos_}..........................");
                            UserAdress ads_ = _ctx.ViewModel.Adresses[pos_];
                            _ctx.ViewModel.DeletedAddress(ads_);
                            DeleteImg.Enabled = true;
                        };
                    }

                }
            }
            catch (Exception)
            {

                //throw;//x
            }


        }

        void SetAnimation(View viewToAnimate)
        {
            ScaleAnimation anim = new ScaleAnimation(0.0f, 1.0f, 0.0f, 1.0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            anim.Duration = 500;
            viewToAnimate.StartAnimation(anim);
        }
    }
}