using Android.Webkit;
using Android.Views;
using Microsoft.Maui.Handlers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.UI.Controls;
using WebView = Android.Webkit.WebView;

namespace clinical6Ui.sample.Platforms.Android.Handlers
{
    public class LocalHtmlWebViewerHandler : ViewHandler<LocalHtmlWebViewer, global::Android.Webkit.WebView>
    {
        private ExtendedWebViewClient _webClient;

        public static IPropertyMapper<LocalHtmlWebViewer, LocalHtmlWebViewerHandler> Mapper =
            new PropertyMapper<LocalHtmlWebViewer, LocalHtmlWebViewerHandler>(ViewHandler.ViewMapper)
            {
                [nameof(LocalHtmlWebViewer.Html)] = MapHtml
            };

        public LocalHtmlWebViewerHandler() : base(Mapper) { }

        protected override global::Android.Webkit.WebView CreatePlatformView()
        {
            var webView = new global::Android.Webkit.WebView(Context);
            webView.Settings.JavaScriptEnabled = true;
            webView.LayoutParameters = new ViewGroup.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent
            );
            return webView;
        }

        protected override void ConnectHandler(global::Android.Webkit.WebView platformView)
        {
            base.ConnectHandler(platformView);

            if (VirtualView == null || PlatformView == null)
                return;

            // Set WebViewClient first
            _webClient = new ExtendedWebViewClient(VirtualView);
            PlatformView.SetWebViewClient(_webClient);

            LoadHtml();
        }

        private static void MapHtml(LocalHtmlWebViewerHandler handler, LocalHtmlWebViewer view)
        {
            handler?.LoadHtml();
        }

        private void LoadHtml()
        {
            if (VirtualView == null || PlatformView == null)
                return;

            if (string.IsNullOrWhiteSpace(VirtualView.Html))
            {
                VirtualView.IsVisible = false;
                return;
            }

            VirtualView.IsVisible = true;

            // Add viewport meta tag if missing
            string html = VirtualView.Html;
            if (!html.Contains("<meta name=\"viewport\""))
            {
                html = "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" + html;
            }

            // Load HTML directly into platform WebView
            PlatformView.LoadDataWithBaseURL(null, html, "text/html", "UTF-8", null);
        }

        internal class ExtendedWebViewClient : WebViewClient
        {
            private readonly LocalHtmlWebViewer _control;

            public ExtendedWebViewClient(LocalHtmlWebViewer control)
            {
                _control = control;
            }

            public override async void OnPageFinished(global::Android.Webkit.WebView view, string url)
            {
                if (_control == null || view == null)
                    return;

                int retries = 20;
                while ((view.ContentHeight == 0) && retries-- > 0)
                {
                    await Task.Delay(100);
                }

                try
                {
                    // Set the VirtualView HeightRequest to match content
                    _control.HeightRequest = view.ContentHeight;

                    // Force native Android layout update
                    view.RequestLayout();

                    // Fire event
                    _control.OnUrlLoaded();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"WebView resize error: {ex}");
                }

                base.OnPageFinished(view, url);
            }
        }
    }
}
