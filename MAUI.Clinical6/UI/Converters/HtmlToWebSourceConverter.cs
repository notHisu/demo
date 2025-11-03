using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.Forms.Clinical6.UI.Converters
{
    public class HtmlToWebSourceConverter : IValueConverter
    {
        private const string HtmlTemplate = "<html>" +
                                            "<body>" +
                                            "{0}" +
                                            "</body>" +
                                            "</html>";
                                            
                                            
        private const string HtmlTemplate2  = @"<html>
                <head>
                      <meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/>
                </head>
                <body height='100%'>{0}</body>
           </html>";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var html = value as string ?? string.Empty;
            return new HtmlWebViewSource {Html = string.Format(HtmlTemplate, HtmlDecode(html)) };
        }

        public string HtmlDecode(string html)
        {
            if (html == null) return null;
            // For now, just strip all html tags
            var tagPattern = new System.Text.RegularExpressions.Regex(@"<\s*\/?\w+(\s*\w+\s*=\s*""[^""]+""\s*)*\s*\/?>");

            var stripped = tagPattern.Replace(html, string.Empty);
            return System.Net.WebUtility.HtmlDecode(stripped);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}