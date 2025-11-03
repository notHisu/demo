using Xamarin.Forms;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    /// <summary>
    /// A very basic cross-platform ellipse which can be filled or strokes with solid colors
    /// </summary>
    /// <remarks>
    /// Modified from https://github.com/chrispellett/Xamarin-Forms-Shape
    /// </remarks>
    public class Ellipse : View
    {
        public BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(Ellipse), Colors.Transparent);

        public Color FillColor
        {
            get => (Color) GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }


        public BindableProperty StrokeColorProperty = BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(Ellipse), Colors.Transparent);

        public Color StrokeColor
        {
            get => (Color) GetValue(StrokeColorProperty);
            set => SetValue(StrokeColorProperty, value);
        }


        public BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(Ellipse), 0.0);

        public double StrokeWidth
        {
            get => (double) GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }
    }
}