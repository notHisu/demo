using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace Xamarin.Forms.Clinical6.UI.Converters
{
    /// <summary>
    /// Some ImageSource urls would not load without direct conversion from the converter
    /// </summary>
    public class ImageSourceConverter : IValueConverter
    {
        static WebClient _client = new WebClient();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = value?.ToString();

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var byteArray = _client.DownloadData(url);
                    return ImageSource.FromStream(() => new MemoryStream(byteArray));
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ImageSourceConverter Exception: {exc} Url: {url}");
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}