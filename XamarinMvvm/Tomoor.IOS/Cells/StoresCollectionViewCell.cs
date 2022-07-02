using System;

using Foundation;
using UIKit;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.BindingContext;
using Ayadi.Core.Model;

namespace Tomoor.IOS.Cells
{
    public partial class StoresCollectionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Identifier = new NSString("StorsCollectionCell");

        private MvxImageViewLoader _imageViewLoader;

        protected StoresCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        private void CreatBinding()
        {
            try
            {
                _imageViewLoader = new MvxImageViewLoader(() => StoreImage);

                var set = this.CreateBindingSet<StoresCollectionViewCell, Store>();

                set.Bind(_imageViewLoader).To(vm => vm.Image.Src);
                set.Bind(labStoreName).To(vm => vm.Name);

                set.Apply();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CreatBinding();
        }
    }
}