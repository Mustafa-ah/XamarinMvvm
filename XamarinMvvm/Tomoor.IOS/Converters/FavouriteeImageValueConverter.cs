using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmCross.Platform.Converters;
using Foundation;
using UIKit;
using System.Globalization;

namespace Tomoor.IOS.Converters
{
    public class FavouriteImageValueConverter : MvxValueConverter<bool, UIImage>
    {
        protected override UIImage Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? UIImage.FromFile("Images/Liked.png") : UIImage.FromFile("Images/like.png");
        }
    }
}