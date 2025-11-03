using Foundation;
using UIKit;

namespace Inventiva
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        //public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        //{
        //    // Set status bar style (light or dark content)
        //    UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;

        //    return base.FinishedLaunching(app, options);
        //}
    }
}
