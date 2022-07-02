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
    public class FavouriteTableViewSource : MvxTableViewSource
    {
        public FavouriteTableViewSource(UITableView tableView) : 
            base(tableView)
        {
        }

        public FavouriteTableViewSource(IntPtr handle) : base(handle)
        {
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ItemsSource.Count();
        }


        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = (FavouriteTableViewCell)
                tableView.DequeueReusableCell(FavouriteTableViewCell.Identifier);
            return cell;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ItemsSource?.ElementAt(indexPath.Row);
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 100;
        }
    }
}