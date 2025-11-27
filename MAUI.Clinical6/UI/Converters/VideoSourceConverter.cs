using System;
using System.Globalization;
using Xamarin.Forms.Clinical6.UI.Controls;

namespace Xamarin.Forms.Clinical6.UI.Converters
{
    public class VideoSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = value?.ToString();

            try
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    return VideoSource.FromUri(url);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"VideoSourceConverter Exception: {exc} Url: {url}");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}