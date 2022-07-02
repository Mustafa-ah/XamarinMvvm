using Ayadi.Core.Model;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using System;
using UIKit;

namespace Tomoor.IOS
{
    public partial class HomeCatsCollectionViewCell : MvxCollectionViewCell
    {
        public HomeCatsCollectionViewCell (IntPtr handle) : base (handle)
        {
        }

        private MvxImageViewLoader _imageViewLoader;

        internal static NSString Identifier = new NSString("HomeCatsCell");

        private void CreateBindings()
        {
            
            _imageViewLoader = new MvxImageViewLoader(() => catImage);
            var set = this.CreateBindingSet<HomeCatsCollectionViewCell, Category>();

            set.Bind(lapCatName)
                .To(vm => vm.Name);

            set.Bind(LapProductsCount)
               .To(vm => vm.ProductCount);
            //set.Bind(LapPrice)
            //    .To(vm => vm.Price)
            //    .WithConversion(new CurrencyToStringConverter());

            //set.Bind(btnLike).To(vm => vm.AddToFavouritCommand);

            ////my own touch ^_*
            //set.Bind(imageLike).For(im => im.Image).To(vm => vm.ISInFavourite)
            //    .WithConversion(new FavouriteImageValueConverter());

            set.Bind(_imageViewLoader).To(pro => pro.Image.Src);

            set.Apply();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CreateBindings();
        }

    }
}