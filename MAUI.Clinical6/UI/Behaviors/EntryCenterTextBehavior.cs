using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if IOS
using UIKit;
using Foundation;
#endif

#if ANDROID
using Android.Views;
#endif

namespace MAUI.Clinical6.UI.Behaviors
{
    public class EntryCenterTextBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            // Explicitly use MAUI alignment
            bindable.HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center;

            EntryHandler.Mapper.AppendToMapping("CenterText", (handler, view) =>
            {
#if ANDROID
            handler.PlatformView.Gravity = GravityFlags.CenterHorizontal;
#elif IOS
                handler.PlatformView.TextAlignment = UIKit.UITextAlignment.Center;
#endif
            });
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);

            // Reset alignment
            bindable.HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Start;
        }
    }

}
