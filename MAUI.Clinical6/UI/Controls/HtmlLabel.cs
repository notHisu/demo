namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public class HtmlLabel : Label
    {
        //public static readonly BindableProperty HtmlTextProperty =
            //BindableProperty.CreateAttached(nameof(HtmlText), typeof(string), typeof(HtmlLabel), string.Empty);

        //public static readonly BindableProperty HtmlAddExtraMaringProperty =
            //BindableProperty.CreateAttached(nameof(HtmlAddExtraMaring), typeof(bool), typeof(HtmlLabel), true);

        public static readonly BindableProperty HtmlTextProperty =
            BindableProperty.Create(nameof(HtmlText), typeof(string), typeof(HtmlLabel), string.Empty);

        public static readonly BindableProperty HtmlAddExtraMaringProperty =
            BindableProperty.Create(nameof(HtmlAddExtraMaring), typeof(bool), typeof(HtmlLabel), true);


        public string HtmlText
        {
            get {  return (string)GetValue(HtmlTextProperty); }
            set {  SetValue(HtmlTextProperty, value); }
        }

        public bool HtmlAddExtraMaring
        {
            get { return (bool)GetValue(HtmlAddExtraMaringProperty); }
            set { SetValue(HtmlAddExtraMaringProperty, value); }
        }

        //public string GetHexString(Color color)
        //{
        //    var red = (int)(color.R * 255);
        //    var green = (int)(color.G * 255);
        //    var blue = (int)(color.B * 255);
        //    var alpha = (int)(color.A * 255);
        //    var hex = $"#{red:X2}{green:X2}{blue:X2}{alpha:X2}";

        //    return hex;
        //}

        public string GetHexString(Color color)
        {
            var red = (int)(color.Red * 255);
            var green = (int)(color.Green * 255);
            var blue = (int)(color.Blue * 255);
            var alpha = (int)(color.Alpha * 255);

            var hex = $"#{red:X2}{green:X2}{blue:X2}{alpha:X2}";
            return hex;
        }

        public string GetStyledHtml(bool withStyle = false)
        {
            string htmlSource;
            if (withStyle)
            {
                htmlSource = string.Format("<meta name=\"viewport\" content=\"initial-scale=1.0\" />" +
                "<!DOCTYPE html><html><head><link href='https://fonts.googleapis.com/css?family=Roboto:400,100,300,100italic,300italic,400italic,500italic,500,700,700italic,900,900italic' rel='stylesheet' type='text/css'></head> " +
                "<style type=\"text/css\"> " +
                    "@font-face {{ " +
                        "font-family: 'Roboto2'; " +
                        "src: url('Fonts/Roboto-Regular.ttf'); " +
                    "}} " +
                    "@font-face {{ " +
                        "font-family: 'Roboto3'; " +
                        "src: url('Roboto-Regular.ttf'); " +
                    "}} " +
                    ".wrapper {{ " +
                        "font-family: 'Roboto', 'Roboto2', 'Roboto3'; " +
                        "font-size: {0}px; " +
                        "color: {1}; " +
                    "}} " +
                    "div {{ " +
                        "font-family: 'Roboto', 'Roboto2', 'Roboto3'; " +
                        "font-size: {0}px; " +
                        "color: {1}; " +
                    "}} " +
                    "span {{ " +
                        "font-family: 'Roboto', 'Roboto2', 'Roboto3'; " +
                        "font-size: {0}px; " +
                        "color: {1}; " +
                    "}} " +
                    "p {{ " +
                        "font-family: 'Roboto', 'Roboto2', 'Roboto3'; " +
                        "font-size: {0}px; " +
                        "color: {1}; " +
                    "}} " +
                "</style> " +
                "<body><div class='wrapper'>{2}</div></body></html>",
                FontSize.ToString(),
                GetHexString(TextColor),
                Text
                );
            }
            else
            {
                string fontLink = "<link href='https://fonts.googleapis.com/css?family=Roboto:400,100,300,100italic,300italic,400italic,500italic,500,700,700italic,900,900italic' rel='stylesheet' type='text/css'>";
                string htmlStyle = string.Format("='color: {0}; font-size: {1}px; font-family: {2} !important'", GetHexString(TextColor), FontSize.ToString(), "'Roboto-Regular', 'Roboto', sans-serif");
                htmlSource = string.Format("<html><head>{0}</head><body><div style=\"{1}\">{2}</div></body></html>", fontLink, htmlStyle, Text);
            }

            return htmlSource;
        }

        public string GetLightStyledHtml()
        {
            string htmlSource;

            string htmlStyle = string.Format("color: {0}; font-size: {1}px; font-family: {2} !important", GetHexString(TextColor), FontSize.ToString(), "'Roboto-Regular', 'Roboto', sans-serif");
            htmlSource = string.Format("<span style=\"{0}\">{1}</span>", htmlStyle, Text);

            return htmlSource;
        }
    }
}