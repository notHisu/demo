using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;


#if ANDROID
using Android.Graphics.Drawables;
using Android.Content.Res;
#elif IOS
using UIKit;
using CoreGraphics;
#endif

namespace MAUI.Clinical6.UI.Behaviors
{
    public class BorderEditorBehavior : Behavior<Editor>
    {
        public Color BorderColor { get; set; } = Colors.Gray;
        public float BorderThickness { get; set; } = 2f;
        public float CornerRadius { get; set; } = 8f;

        protected override void OnAttachedTo(Editor bindable)
        {
            base.OnAttachedTo(bindable);

            EditorHandler.Mapper.AppendToMapping("BorderEditor", (handler, view) =>
            {
#if ANDROID
                var gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                gd.SetStroke((int)BorderThickness, BorderColor.ToPlatform());
                gd.SetCornerRadius(CornerRadius);

                handler.PlatformView.SetBackground(gd);
#elif IOS
                handler.PlatformView.Layer.CornerRadius = CornerRadius;
                handler.PlatformView.Layer.BorderWidth = BorderThickness;
                handler.PlatformView.Layer.BorderColor = BorderColor.ToCGColor();
                handler.PlatformView.ClipsToBounds = true;
#endif
            });
        }

        protected override void OnDetachingFrom(Editor bindable)
        {
            base.OnDetachingFrom(bindable);

            EditorHandler.Mapper.ModifyMapping("BorderEditor", null);
        }
    }
}

