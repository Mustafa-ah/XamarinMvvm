using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using Ayadi.Core.ViewModel;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Binding.Droid.BindingContext;

namespace Ayadi.Droid.Views
{
    [MvxFragment(typeof(HomeViewModel),Resource.Id.content_frame, false)]
    [Register("Ayadi.Droid.Views.StorsView")]
    public class StorsView : MvxFragment<StorsViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            View fragView = this.BindingInflate(Resource.Layout.fragment_stors, null);

            return fragView;

           // return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}