using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V7.Widget;
using Android.Views.Animations;
using Tomoor.Droid.Views;
using Ayadi.Core.Model;
using Android.Views;

namespace Tomoor.Droid.Adapters
{
    public class AdressAnimatorRecyclerAdapter : MvxRecyclerAdapter
    {
        View PreviousView;
        public AdressAnimatorRecyclerAdapter(IMvxAndroidBindingContext bindingContext)
          : base(bindingContext)
        {
            
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            base.OnBindViewHolder(holder, position);
            try
            {
                holder.ItemView.Click += (s, e) =>
                {
                    SetAnimation(holder.ItemView);
                };
            }
            catch (System.Exception)
            {

                //throw;//x
            }
        }

        void SetAnimation(View viewToAnimate)
        {
            if (PreviousView != null)
            {
                PreviousView.SetBackgroundResource(Android.Resource.Drawable.EditBoxBackground);
            }
            PreviousView = viewToAnimate;
            viewToAnimate.SetBackgroundColor(Android.Graphics.Color.Rgb(255, 170, 85));
            //ScaleAnimation anim = new ScaleAnimation(0.0f, 1.0f, 0.0f, 1.0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            //anim.Duration = 500;
            //viewToAnimate.StartAnimation(anim);@android:drawable/editbox_background
        }
    }
}