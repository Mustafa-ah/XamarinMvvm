using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using MvvmCross.Platform.Converters;

namespace Tomoor.Droid.Converters
{
    public class RateImageValueConverter : MvxValueConverter<int, int>
    {
        protected override int Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 1:
                    return Resource.Drawable.StarOne;
                case 2:
                    return Resource.Drawable.StarTow;
                case 3:
                    return Resource.Drawable.StarThree;
                case 4:
                    return Resource.Drawable.StarFour;
                case 5:
                    return Resource.Drawable.Stars;
                default:
                    return Resource.Drawable.StarZero;
            }
        }
    }
}