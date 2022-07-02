using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.ExtensionMethods;

namespace Tomoor.IOS.Adapters
{
    public class HomeProductsCollectionViewSource : MvxCollectionViewSource
    {
        public HomeProductsCollectionViewSource(UICollectionView collectionView):base(collectionView)
        {

        }

        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
        {
            return (homeProductsCollectionViewCell)collectionView.DequeueReusableCell(homeProductsCollectionViewCell.Identifier, indexPath);
        }

  
        //protected override object GetItemAt(NSIndexPath indexPath)
        //{
        //    if (indexPath.Item == ItemsSource.Count()) return null;
        //    var item = base.GetItemAt(indexPath);
        //    if (item is Item realItem)
        //    {
        //        realItem.RemoveCommand = (IMvxCommand)this.RemoveCommand;
        //        realItem.Index = indexPath.Row + 1;
        //    }

        //    return item;
        //}

        //private NSString GetCellKey(NSIndexPath indexPath)
        //{
        //    if (indexPath.Item == ItemsSource.Count()) return AddItemCell.Key;

        //    return ItemCell.Key;
        //}

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ItemsSource.Count();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ItemsSource?.ElementAt(indexPath.Row);
        }
    }
}