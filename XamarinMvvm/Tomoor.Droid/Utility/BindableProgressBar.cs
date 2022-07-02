using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tomoor.Droid.Views;

namespace Tomoor.Droid.Utility
{
    public class BindableProgressBar
    {

        //http://slodge.blogspot.com.eg/2013/07/n34-showing-progress-isbusy-display-n1.html
        //private ProgressBar _Progress;
        Activity _ctx;

        private LoadingFragment loadingFragment;
        FragmentManager fm;
        public BindableProgressBar(Activity ctx)
        {
            _ctx = ctx;
            fm = _ctx.FragmentManager;
        }

        public bool Visable
        {
            get { return loadingFragment != null; }
            set
            {
                if (value == Visable)
                {
                    return;
                }

                if (value)
                {
                    loadingFragment = new LoadingFragment();
                    loadingFragment.Cancelable = false;
                    loadingFragment.Show(fm, "Laoding");
                }
                else
                {
                    loadingFragment.Dismiss();
                    loadingFragment = null;
                }
            }
        }
    }

    /*
     binable progress dailog
       public class BindableProgress
        {
            private ProgressDialog _dailoge;
            Context _ctx;
            public BindableProgress(Context ctx)
            {
                _ctx = ctx;
            }

            public bool Visable
            {
                get { return _dailoge != null; }
                set {
                    if (value == Visable)
                    {
                        return;
                    }

                    if (value)
                    {
                        _dailoge = new ProgressDialog(_ctx, Android.Resource.Style.ThemeBlack);
                        _dailoge.SetMessage("Loading...");
                        _dailoge.Show();
                    }
                    else
                    {
                        _dailoge.Hide();
                        _dailoge = null;
                    }
                }
            }

        }
     */
}