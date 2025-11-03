using System;
using System.Globalization;
//using System.Windows.Data;
using Humanizer;
//using NuGet.ProjectManagement;

namespace Xamarin.Forms.Clinical6.UI.Converters
{
    public class DateTimeHumanizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.Humanize(false, null, culture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}