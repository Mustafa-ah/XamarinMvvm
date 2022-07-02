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
    public class FavouriteImageValueConverter : MvxValueConverter<bool, int>
    {
        protected override int Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = value ? Resource.Drawable.Liked : Resource.Drawable.like;
            return result;
        }
        //protected override bool ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return value == 2130837606;
        //}
      
    }
}