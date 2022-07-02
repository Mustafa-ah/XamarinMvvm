using System;
using MvvmCross.Platform.Converters;
using System.Globalization;

namespace Tomoor.Droid.Converters
{
    public class SelectedImageValueConverter : MvxValueConverter<bool, int>
    {
        protected override int Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = value ? Resource.Drawable.selected : Resource.Drawable.un_selected;
            return result;
        }
    }
}