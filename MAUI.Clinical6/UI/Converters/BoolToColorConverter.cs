using System;
using System.Globalization;

namespace Xamarin.Forms.Clinical6.UI.Converters
{
    public class BoolToColorConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty TrueColorProperty = BindableProperty.Create(nameof(TrueColor), typeof(Color), typeof(BoolToColorConverter), Colors.Transparent);

        public Color TrueColor
        {
            get => (Color) GetValue(TrueColorProperty);
            set => SetValue(TrueColorProperty, value);
        }


        public static readonly BindableProperty FalseColorProperty = BindableProperty.Create(nameof(FalseColor), typeof(Color), typeof(BoolToColorConverter), Colors.Transparent);

        public Color FalseColor
        {
            get => (Color) GetValue(FalseColorProperty);
            set => SetValue(FalseColorProperty, value);
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return value;

            var boolValue = (bool) value;
            var brush = boolValue ? TrueColor : FalseColor;

            return brush;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}