using Android.Content;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Xamarin.Forms.Clinical6.UI.Controls;

namespace Xamarin.Forms.Clinical6.Handlers
{
    public class EllipseHandler : ViewHandler<Ellipse, Android.EllipseView>
    {
        public EllipseHandler() : base(PropertyMapper)
        {
        }

        public static IPropertyMapper<Ellipse, EllipseHandler> PropertyMapper =
            new PropertyMapper<Ellipse, EllipseHandler>(ViewHandler.ViewMapper)
            {
                [nameof(Ellipse.FillColor)] = MapFillColor,
                [nameof(Ellipse.StrokeColor)] = MapStrokeColor,
                [nameof(Ellipse.StrokeWidth)] = MapStrokeWidth,
            };

        protected override Android.EllipseView CreatePlatformView()
        {
            return new Android.EllipseView(Context)
            {
                FillColor = VirtualView.FillColor.ToPlatform(),
                StrokeColor = VirtualView.StrokeColor.ToPlatform(),
                StrokeWidth = VirtualView.StrokeWidth
            };
        }

        private static void MapFillColor(EllipseHandler handler, Ellipse ellipse)
        {
            handler.PlatformView.FillColor = ellipse.FillColor.ToPlatform();
            handler.PlatformView.Invalidate();
        }

        private static void MapStrokeColor(EllipseHandler handler, Ellipse ellipse)
        {
            handler.PlatformView.StrokeColor = ellipse.StrokeColor.ToPlatform();
            handler.PlatformView.Invalidate();
        }

        private static void MapStrokeWidth(EllipseHandler handler, Ellipse ellipse)
        {
            handler.PlatformView.StrokeWidth = ellipse.StrokeWidth;
            handler.PlatformView.Invalidate();
        }
    }
}

//using Android.Content;
//using Microsoft.Maui.Controls.Compatibility;
//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
//using Microsoft.Maui.Controls.Platform;
//using Microsoft.Maui.Handlers;
//using Microsoft.Maui.Platform;
//using Xamarin.Forms.Clinical6.UI.Controls;

//[assembly: ExportRenderer(typeof(Ellipse), typeof(Xamarin.Forms.Clinical6.Droid.Renderers.EllipseRenderer))]
//namespace Xamarin.Forms.Clinical6.Droid.Renderers
//{
//    /// <summary>
//    /// Ellipse renderer.
//    /// </summary>
//    public class EllipseRenderer : ViewRenderer<Ellipse, Android.EllipseView>
//    {
//        public EllipseRenderer(Context context) : base(context) { }

//        /// <summary>
//        /// Ons the element changed.
//        /// </summary>
//        /// <param name="e">E.</param>
//        protected override void OnElementChanged(ElementChangedEventArgs<Ellipse> e)
//        {
//            base.OnElementChanged(e);

//            if (Control == null)
//            {
//                SetNativeControl(new Android.EllipseView(Context)
//                {
//                    FillColor = Element.FillColor.ToAndroid(),
//                    StrokeColor = Element.StrokeColor.ToAndroid(),
//                    StrokeWidth = Element.StrokeWidth
//                });
//            }
//        }

//        /// <summary>
//        /// Ons the element property changed.
//        /// </summary>
//        /// <param name="sender">Sender.</param>
//        /// <param name="e">E.</param>
//        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);

//            if (e.PropertyName == nameof(Element.FillColor))
//            {
//                Control.FillColor = Element.FillColor.ToAndroid();
//                Control.Invalidate();
//            }
//            else if (e.PropertyName == nameof(Element.StrokeColor))
//            {
//                Control.FillColor = Element.StrokeColor.ToAndroid();
//                Control.Invalidate();
//            }
//            else if (e.PropertyName == nameof(Element.StrokeWidth))
//            {
//                Control.StrokeWidth = Element.StrokeWidth;
//                Control.Invalidate();
//            }
//        }
//    }
//}