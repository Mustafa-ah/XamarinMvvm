using System;

using Foundation;
using UIKit;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.BindingContext;
using Ayadi.Core.Model;
using Tomoor.IOS.Views;
using CoreGraphics;

namespace Tomoor.IOS.Cells
{
    public partial class CatsCollectionViewCell : MvxCollectionViewCell
    {
        //public static readonly NSString Key = new NSString("CatsCollectionViewCell");
        //public static readonly UINib Nib;

        //static CatsCollectionViewCell()
        //{
        //    Nib = UINib.FromName("CatsCollectionViewCell", NSBundle.MainBundle);
        //}

       // protected float ViewWidth = (float)UIScreen.MainScreen.Bounds.Width;
       // protected float ViewHieght = (float)UIScreen.MainScreen.Bounds.Height;

        protected CatsCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        private MvxImageViewLoader _imageViewLoader;

        internal static readonly NSString Identifier = new NSString("catsCollectionCell");

        private void CreateBindings()
        {

            _imageViewLoader = new MvxImageViewLoader(() => CatImage);
            var set = this.CreateBindingSet<CatsCollectionViewCell, Category>();

            set.Bind(LabCatName)
                .To(vm => vm.Name);

            set.Bind(labPcount).To(vm => vm.ProductCount);
           

            set.Bind(_imageViewLoader).To(pro => pro.Image.Src);

            set.Apply();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CreateBindings();
            SetPositions();
        }

        private void SetPositions()
        {
            float _cellWidth = CategoryView.CellWidth;
            float _cellHeight = 100;

            FrameImage.Frame = new CGRect(0, 0, _cellWidth, _cellHeight);

            CatImage.Frame = new CGRect(5, 5, _cellWidth - 10, _cellHeight -10);

            LabCatName.Frame = new CGRect(_cellWidth - 110, _cellHeight - 30, 100, 21);
        }
    }
}