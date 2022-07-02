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
using MvvmCross.Droid.Views;
using Ayadi.Core.ViewModel;
using Android.Support.V4.View;
using Tomoor.Droid.Adapters;
using Android.Views.Animations;
using Tomoor.Droid.Utility;
using MvvmCross.Binding.BindingContext;
using Android.Support.V4.Content;
using Ayadi.Core.Model;
using Android.Support.Design.Widget;

namespace Tomoor.Droid.Views
{
    [Activity(Label = "ProductView")]
    public class ProductView : MvxActivity<ProductViewModel>
    {
        ViewPager _productPager;
        ProductViewPagerAdapter _productViewPagerAdapter;

        public new ProductViewModel ViewModel
        {
            get { return (ProductViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        Button AddBtn;
        LinearLayout PlusMinusLayout;
        ImageView PlusImage;
        ImageView MinusImage;
        TextView quantityText;

        BindableProgressBar _bindableProgressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_Product);


            #region share & like

            //ImageView likeImag = FindViewById<ImageView>(Resource.Id.imageViewP_like);
            //likeImag.Click += LikeImag_Click;


            //ImageView shareImage = FindViewById<ImageView>(Resource.Id.imageViewP_Share);
            //if (!shareImage.HasOnClickListeners)
            //{
            //    shareImage.Click += (s, a) =>
            //    {
            //        shareImage.Visibility = ViewStates.Gone;
            //        string content_ = Constants.BaseShareArUrl + ViewModel.SelectedProduct.Se_name;
            //        ShareIt.Share(this, "Tomoor", content_);

            //        shareImage.Visibility = ViewStates.Visible;

            //    };
            //}
            #endregion

            _bindableProgressBar = new BindableProgressBar(this);
            var set = this.CreateBindingSet<ProductView, ProductViewModel>();
            set.Bind(_bindableProgressBar).For(p => p.Visable).To(vm => vm.IsBusy);
            set.Apply();

            #region view Pager
            try
            {
                _productPager = FindViewById<ViewPager>(Resource.Id.viewpager);

                AddBtn = FindViewById<Button>(Resource.Id.add_btn);
                PlusMinusLayout = FindViewById<LinearLayout>(Resource.Id.linearLayoutPlusMinus);
                PlusImage = FindViewById<ImageView>(Resource.Id.imageViewPlus);
                MinusImage = FindViewById<ImageView>(Resource.Id.imageViewMinus);
                quantityText = FindViewById<TextView>(Resource.Id.textViewQuantity);

                AddBtn.Click += AddBtn_Click;
                PlusImage.Click += PlusImage_Click;
                MinusImage.Click += MinusImage_Click;

                ViewModel.CartListReady += delegate
                {
                    if (ViewModel.SelectedProduct.Images != null)
                    {
                        _productViewPagerAdapter = new ProductViewPagerAdapter(this, ViewModel.SelectedProduct.Images);
                        _productPager.Adapter = _productViewPagerAdapter;

                        //http://xleon.net/xamarin/android/a-simple-page-indicator-for-your-android-viewpager.html
                        var dots = FindViewById<TabLayout>(Resource.Id.dots);

                        if (ViewModel.SelectedProduct.Images.Count > 1)
                        {
                            dots.SetupWithViewPager(_productPager, true); // <- magic here
                        }
                        else
                        {
                            dots.Visibility = ViewStates.Gone;
                        }
                    }
                    if (ViewModel.IsProductInShoppingList)
                    {
                        AddBtn_Click(this, new EventArgs());
                    }
                    quantityText.Text = ViewModel.SelectedProduct.Quantity.ToString();
                };

                
            }
            catch(Exception ex)
            {
                Android.Util.Log.Error("ProductView.OnCreate", ex.Message);
            }
            #endregion
        }

        private void LikeImag_Click(object sender, EventArgs e)
        {
            try
            {
                ImageView likeImag = sender as ImageView;
                if (likeImag.Drawable.GetConstantState() == ContextCompat.GetDrawable(this, Resource.Drawable.like).GetConstantState())
                {
                    likeImag.SetImageResource(Resource.Drawable.Liked);
                }
                else
                {
                    likeImag.SetImageResource(Resource.Drawable.like);
                }
                SetAnimation(likeImag);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }


        private async void MinusImage_Click(object sender, EventArgs e)
        {
            try
            {
                int quant = int.Parse(quantityText.Text);
                if (quant == 1)
                {
                    string[] words = ViewModel.GetMessageWords();
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle(words[0]);
                    alert.SetMessage(words[1]);
                    alert.SetPositiveButton(words[2], (senderAlert, args) => {
                        Toast.MakeText(this, words[4], ToastLength.Short).Show();
                        // ViewModel.Cartls.Remove(ViewModel.SelectedProduct);
                        ViewModel.DeletProduct();
                        AddBtn.Visibility = ViewStates.Visible;
                        PlusMinusLayout.Visibility = ViewStates.Gone;
                        // ViewModel.SaveCartList();
                    });

                    alert.SetNegativeButton(words[3], (senderAlert, args) => {
                        Toast.MakeText(this, words[5], ToastLength.Short).Show();
                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                    
                }
                else
                {
                    quant = quant - 1;
                    quantityText.Text = quant.ToString();
                    SetAnimation(quantityText);
                    ViewModel.SelectedProduct.Quantity--;
                    ShoppingCart dd = await ViewModel.PutCart();
                    //ViewModel.SaveCartList();
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private async void PlusImage_Click(object sender, EventArgs e)
        {
            try
            {
                int quant = int.Parse(quantityText.Text);
                quant = quant + 1;
                quantityText.Text = quant.ToString();
                SetAnimation(quantityText);
                ViewModel.SelectedProduct.Quantity++;
               ShoppingCart dd = await ViewModel.PutCart();
                //ViewModel.SaveCartList();
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddBtn.Visibility = ViewStates.Gone;
            PlusMinusLayout.Visibility = ViewStates.Visible;
        }


        void SetAnimation(View viewToAnimate)
        {
            ScaleAnimation anim = new ScaleAnimation(0.0f, 1.0f, 0.0f, 1.0f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
            anim.Duration = 500;
            viewToAnimate.StartAnimation(anim);
        }
    }
}