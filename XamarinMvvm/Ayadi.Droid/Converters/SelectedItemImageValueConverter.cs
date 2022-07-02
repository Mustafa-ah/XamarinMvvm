using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;

namespace Ayadi.Droid.Converters
{
    public class SelectedItemImageValueConverter : MvxValueConverter<bool, int>
    {
        protected override int Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = value ? Resource.Drawable.RadioChecked : Resource.Drawable.RadioUnChecked;
            return result;
        }
    }
}