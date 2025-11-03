using System;
using System.Globalization;
using Xamarin.Forms.Clinical6.Helpers;

namespace Xamarin.Forms.Clinical6.UI.Converters
{
    public class StripHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var html = value as string;
            if (html == null)
                return null;

            return html.StripHtml();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}