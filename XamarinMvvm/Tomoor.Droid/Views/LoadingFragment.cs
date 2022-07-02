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

namespace Tomoor.Droid.Views
{
    public class LoadingFragment : DialogFragment
    {
        int width = 0;
        int height = 0;
        DisplayMetrics metrics;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            metrics = Activity.Resources.DisplayMetrics;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
             return inflater.Inflate(Resource.Layout.Fragment_Loading, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            // remove title
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            Dialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            Dialog.SetCanceledOnTouchOutside(false);
            Dialog.SetCancelable(false);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Android.Resource.Style.AnimationDialog;
        }


        public override void OnStart()
        {
            base.OnStart();
            try
            {
                // set screen size
                width = metrics.WidthPixels - (metrics.WidthPixels / 5);
                height = metrics.HeightPixels - (metrics.HeightPixels / 3);
                this.Dialog?.Window.SetLayout(width, height);

            }
            catch (Exception)
            {

                //throw;//x
            }
           
        }
    }
}