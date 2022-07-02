using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Ayadi.Core.Converters
{
    public class FavouriteeImageValueConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value)
            {
                return "res:like";
            }
            else
            {
                return "res:Liked";
            }
        }
        //protected override bool ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return value == 2130837606;
        //}

        /*
       <Mvx.MvxImageView
         android:src="@drawable/like"
         android:layout_width="wrap_content"
         android:layout_height="match_parent"
         android:id="@+id/imageViewP_like"
         android:padding="5dp"
         local:MvxBind="ImageUrl ISInFavourite, Converter=FavouriteImage; Click AddToFavouritesCommand" />
       */
    }
}
