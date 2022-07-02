using Ayadi.Core.Converters;
using Ayadi.Core.Model;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using System;
using UIKit;

namespace Tomoor.IOS
{
    public partial class FavouriteTableViewCell : MvxTableViewCell
    {
        internal static NSString Identifier = new NSString("FavouriteCell");
        public FavouriteTableViewCell (IntPtr handle) : base (handle)
        {
        }

        private MvxImageViewLoader _imageViewLoader;

        private void CreateBindings()
        {
            _imageViewLoader = new MvxImageViewLoader(() => ImageProduct);
            var set = this.CreateBindingSet<FavouriteTableViewCell, Product>();

            set.Bind(_imageViewLoader)
                .To(vm => vm.ProductImage);

            set.Bind(LabProductName)
                .To(vm => vm.Name);

            set.Bind(LabProductPrice)
                .To(vm => vm.Price)
                .WithConversion(new CurrencyToStringConverter());

            set.Apply();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CreateBindings();
        }
    }
}