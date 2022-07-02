using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using Ayadi.Core.ViewModel;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Binding.Droid.BindingContext;

namespace Ayadi.Droid.Views
{
    [MvxFragment(typeof(ProductsViewModel), Resource.Id.right_drawer, false)]
    [Register("Ayadi.Droid.Views.FillteringView")]
    public class FillteringView : MvxFragment<FillteringViewModel>
    {

        public new FillteringViewModel ViewModel
        {
            get { return (FillteringViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        ProgressBar loder;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.Fragment_Filtring, null);
            loder = fragView.FindViewById<ProgressBar>(Resource.Id.progressBarCats);
            ViewModel.ViewModelInitialized += ViewModel_ViewModelInitialized;
            return fragView;
        }

        private void ViewModel_ViewModelInitialized(object sender, System.EventArgs e)
        {
            loder.Visibility = ViewStates.Gone;
        }
    }
}