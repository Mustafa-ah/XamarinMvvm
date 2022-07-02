using Ayadi.Core.Contracts.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;
using Ayadi.Core.Messages;
using Ayadi.Core.Contracts.Services;

namespace Ayadi.Core.ViewModel
{
    public class SortingViewModel : BaseViewModel, ISortingViewModel
    {

        public SortingViewModel(IMvxMessenger messenger) : base(messenger)
        {
            //highlight selected item
        }

        public event EventHandler ViewChanged;
        public event EventHandler Sorted;

        private void Sorting(string sortBy)
        {
            Messenger.Publish(new SortMessage(this) { SortBy = sortBy });
            Sorted?.Invoke(sortBy, new EventArgs());
        }

        private void ChangeView(string view)
        {
            Messenger.Publish(new ProductsViewMessage(this) { ViewType = view });
            ViewChanged?.Invoke(view, new EventArgs());
        }

        //commands
        public MvxCommand SortByNameAtoZCommand
        { get { return new MvxCommand(() => Sorting("ByNameAtoZ")); } }

        public MvxCommand SortByNameZtoACommand
        { get { return new MvxCommand(() => Sorting("ByNameZtoA")); } }

        public MvxCommand SortByPriceMaxToMinCommand
        { get { return new MvxCommand(() => Sorting("ByPriceMaxToMin")); } }

        public MvxCommand SortByPriceMinToMaxCommand
        { get { return new MvxCommand(() => Sorting("ByPriceMinToMax")); } }

        public MvxCommand SortByDefaultPlaceCommand
        { get { return new MvxCommand(() => Sorting("ByDefaultPlace")); } }

        public MvxCommand SortByLastUpdateCommand
        { get { return new MvxCommand(() => Sorting("ByLastUpdate")); } }

        public MvxCommand GridViewCommand
        { get { return new MvxCommand(() => ChangeView("Grid")); } }

        public MvxCommand ListViewCommand
        { get { return new MvxCommand(() => ChangeView("List")); } }
    }
}
