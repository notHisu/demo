using System;
using System.Globalization;

namespace Xamarin.Forms.Clinical6.UI.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public CurrencyConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value).ToString("C2", MainService.Instance.IsUSOnlyCulture ? new CultureInfo("en-US") : culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
