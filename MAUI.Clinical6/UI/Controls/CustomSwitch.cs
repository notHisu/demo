namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public class CustomSwitch : Switch
    {
        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbTintColor), typeof(Color), typeof(CustomSwitch), Color.FromHex("#005db5"));
        public static readonly BindableProperty OnTintColorProperty = BindableProperty.Create(nameof(OnTintColor), typeof(Color), typeof(CustomSwitch), Color.FromHex("#b3d7ef"));

        public Color ThumbTintColor
        {
            get { return (Color)GetValue(ThumbColorProperty); }
            set { SetValue(ThumbColorProperty, value); }
        }

        public Color OnTintColor
        {
            get { return (Color)GetValue(OnTintColorProperty); }
            set { SetValue(OnTintColorProperty, value); }
        }
    }
}
