using Ayadi.Core.Converters;
using Ayadi.Core.Model;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using System;
using Tomoor.IOS.Converters;
using UIKit;

namespace Tomoor.IOS
{
    public partial class homeProductsCollectionViewCell : MvxCollectionViewCell
    {
        public homeProductsCollectionViewCell (IntPtr handle) : base (handle)
        {
           
        }
        private  MvxImageViewLoader _imageViewLoader;

        internal static NSString Identifier = new NSString("HomeProductCell");
       // public static readonly UINib Nib = UINib.FromName("HomeProductCell", NSBundle.MainBundle);
        private void CreateBindings()
        {
            _imageViewLoader = new MvxImageViewLoader(() => ProductImage);
            var set = this.CreateBindingSet<homeProductsCollectionViewCell, Product>();

            set.Bind(ProductName)
                .To(vm => vm.Name);


            set.Bind(LapPrice)
                .To(vm => vm.Price)
                .WithConversion(new CurrencyToStringConverter());

            set.Bind(btnLike).To(vm => vm.AddToFavouritCommand);

            //my own touch ^_*
            set.Bind(imageLike).For(im => im.Image).To(vm => vm.ISInFavourite)
                .WithConversion(new FavouriteImageValueConverter());

            set.Bind(_imageViewLoader).To(pro => pro.ProductImage);

            set.Apply();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CreateBindings();
        }

    }
}