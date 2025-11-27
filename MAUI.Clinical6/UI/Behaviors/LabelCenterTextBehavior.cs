using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;

#if IOS
using UIKit;
using Foundation;
#endif

#if ANDROID
using Android.Views;
#endif

namespace MAUI.Clinical6.UI.Behaviors
{
    public class LabelCenterTextBehavior : Behavior<Label>
    {
        protected override void OnAttachedTo(Label bindable)
        {
            base.OnAttachedTo(bindable);

            // Explicitly use MAUI alignment
            bindable.HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center;
            bindable.VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center;

            LabelHandler.Mapper.AppendToMapping("CenterText", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Gravity = GravityFlags.Center;
#elif IOS
                handler.PlatformView.TextAlignment = UITextAlignment.Center;
#endif
            });
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);

            // Reset alignment
            bindable.HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Start;
            bindable.VerticalTextAlignment = Microsoft.Maui.TextAlignment.Start;
        }
    }
}
