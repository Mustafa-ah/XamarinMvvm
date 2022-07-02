using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MvvmCross.Localization;
using MvvmCross.Platform;

namespace Ayadi.Core.Converters
{
    public class CurrencyToStringConverter : MvxValueConverter<decimal, string>
    {
        //public IMvxLanguageBinder TextSource =>
        //  new MvxLanguageBinder("", GetType().Name);

        private IMvxTextProvider _textProvider;
        private IMvxTextProvider TextProvider
        {
            get
            {
                _textProvider = _textProvider ?? Mvx.Resolve<IMvxTextProvider>();
                return _textProvider;
            }
        }
        protected override string Convert(decimal value, Type targetType, object parameter, CultureInfo culture)
        {

           // string currency = TextProvider.GetText(null, null, "currency");
            return value+ " " + TextProvider.GetText(null, null, "currency") ;
        }
    }
}
