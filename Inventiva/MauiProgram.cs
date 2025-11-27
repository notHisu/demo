using Autofac;
using Autofac.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;   // <- critical for .Host
using FFImageLoading.Maui;      // For FFImageLoading.Maui.Helpers.Init()
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using NLog.Extensions.Logging;  // For .AddNLog()
using SkiaSharp.Views.Maui.Controls.Hosting;
//using Xamarin.Forms.Clinical6.Android.Renderers;
using Xamarin.Forms.Clinical6.UI.Controls;
using Xamarin.Forms.Clinical6.UI.Effects;
using Xamarin.Forms.Clinical6.UI.Views;
using ZXing.Net.Maui.Controls;           // For .UseZXingNetMaui()
using Xamarin.Forms.Clinical6.Views;






#if ANDROID
//using Xamarin.Forms.Clinical6.Android.Renderers;
//using MAUI.Clinical6.Platforms.Android.Renders;
using Xamarin.Forms.Clinical6.Handlers;
using MAUI.Clinical6.Platforms.Android.Controls;
//using MAUI.Clinical6.Platforms.Android.Handlers;
//using MAUI.Clinical6.Platforms.Android.PageRenderer;
using clinical6Ui.sample.Platforms.Android.Handlers;
using Xamarin.Forms.Clinical6.Services;
using MAUI.Clinical6.Android.Services;
//using MAUI.Clinical6.Android.Services;
using Xamarin.Forms.Clinical6.Android.Effects;
using Xamarin.Forms.Clinical6.Maui.Handlers;
using Xamarin.Forms.Clinical6.Helpers;
using FaceIDTest.Droid.Services.FingerprintAuth;
#endif

namespace Inventiva
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader() // ZXing.Net.Maui
                .UseSkiaSharp() // SkiaSharp.Views.Maui.Controls
                .UseFFImageLoading()
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory()) // swap container
                //.ConfigureContainer<ContainerBuilder>(containerBuilder =>
                //{
                //    // Call your Autofac registrations here
                //    Xamarin.Forms.Clinical6.UI.DependencyConfig.Configure(containerBuilder);
                //})
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Bold.ttf", "Roboto-Bold");
                    fonts.AddFont("Roboto-Light.ttf", "Roboto-Light");
                    fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                });

            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                // Remove default Frame/BorderHandler mapping if necessary
                handlers.AddHandler<MaterialFrame, MaterialFrameHandler>();
#endif
            });

            // Register your custom handler here:
            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                //Controls
                handlers.AddHandler(typeof(MaterialTextbox), typeof(MaterialTextboxHandler));
                handlers.AddHandler(typeof(Ellipse), typeof(EllipseHandler));
                //handlers.AddHandler(typeof(HtmlLabel), typeof(HtmlLabelHandler));
                ////handlers.AddHandler(typeof(MaterialFrame), typeof(MaterialFrameHandler));
                //handlers.AddHandler(typeof(ExtendedCheckBox), typeof(ExtendedCheckBoxHandler));
                //handlers.AddHandler(typeof(VideoPlayer), typeof(VideoPlayerHandler));
                //handlers.AddHandler(typeof(CircleView), typeof(CircleViewHandler));
                //handlers.AddHandler(typeof(AccessibilityLabel), typeof(AccessibilityLabelHandler));
                //handlers.AddHandler(typeof(BorderLessDatePicker), typeof(BorderLessDatePickerHandler));
                //handlers.AddHandler(typeof(BorderLessTimePicker), typeof(BorderLessTimePickerHandler));
                //handlers.AddHandler(typeof(StepSlider), typeof(StepSliderHandler));
                //handlers.AddHandler(typeof(VideoConsultRoomPage), typeof(VideoConsultRoomPageHandler));
                //handlers.AddHandler(typeof(VerticalStepSlider), typeof(VerticalStepSliderHandler));
                //handlers.AddHandler(typeof(DashboardTabPage), typeof(DashboardTabbedHandler));

                ////PageRenderer
                handlers.AddHandler(typeof(SwipeableListView), typeof(SwipeableListViewHandler));
                //handlers.AddHandler(typeof(FlyoutPage), typeof(DashboardFlyoutPageHandler));
                //handlers.AddHandler(typeof(NavigationPage), typeof(ExtendedNavigationPageHandler));
                //handlers.AddHandler(typeof(CustomWebView), typeof(CustomWebViewHandler));

                ////Renders
                //handlers.AddHandler(typeof(BorderLessPicker), typeof(BorderLessPickerHandler));
                handlers.AddHandler(typeof(LocalHtmlWebViewer), typeof(LocalHtmlWebViewerHandler));
                //handlers.AddHandler(typeof(PdfWebViewer), typeof(PdfWebViewerHandler));
                //handlers.AddHandler(typeof(BorderLessEntry), typeof(BorderLessEntryHandler));
#endif
            });

            builder.ConfigureEffects(effects =>
            {
#if ANDROID
                effects.Add<ScrollReporterEffect, ScrollReporterEffectPlatform>();
#endif
            });

            // Autofac initialization if replacing default DI container (optional)
            // Akavache initialization (early in startup)
            Akavache.Registrations.Start("inventiva");

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Logging.AddNLog();

#if ANDROID
        builder.Services.AddSingleton<IFingerprintAuth, FingerprintAuth>();
        builder.Services.AddSingleton<IBiometricsService, BiometricsService>();
#endif
            return builder.Build();
        }
    }
}
