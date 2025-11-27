using System;
namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public class LocalHtmlWebViewer : WebView
    {
        public static readonly BindableProperty HtmlProperty = BindableProperty.Create(propertyName: nameof(Html), returnType: typeof(string), declaringType: typeof(LocalHtmlWebViewer), defaultValue: default(string), propertyChanged: HtmlpropertyChanged);

        private static void HtmlpropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var newButton = newValue as string;
            if (newButton == null) return;

            var form = (LocalHtmlWebViewer)bindable;

            if (string.IsNullOrEmpty(form.Html))
            {
                return;
            }
        }

        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }

        public event EventHandler UrlLoaded;

        public void OnUrlLoaded()
        {
            UrlLoaded?.Invoke(this, null);
        }
    }
}
