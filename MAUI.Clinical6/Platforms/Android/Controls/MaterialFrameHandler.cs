using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using Xamarin.Forms.Clinical6.UI.Controls;

namespace MAUI.Clinical6.Platforms.Android.Controls
{
    public class MaterialFrameHandler : ViewHandler<MaterialFrame, ContentViewGroup>
    {
        // Mapper specifically for MaterialFrame
        public static IPropertyMapper<MaterialFrame, MaterialFrameHandler> Mapper =
            new PropertyMapper<MaterialFrame, MaterialFrameHandler>(ViewHandler.ViewMapper)
            {
                [nameof(MaterialFrame.CornerRadius)] = MapBackground,
                [nameof(MaterialFrame.BackgroundColor)] = MapBackground,
                [nameof(MaterialFrame.HasShadow)] = MapElevation,
                [nameof(MaterialFrame.Elevation)] = MapElevation
            };

        public MaterialFrameHandler() : base(Mapper)
        {
        }

        protected override ContentViewGroup CreatePlatformView()
        {
            // ContentViewGroup is what MAUI uses internally for ContentView/Frame content
            return new ContentViewGroup(Context);
        }

        private static void MapBackground(MaterialFrameHandler handler, MaterialFrame frame)
        {
            if (handler.PlatformView == null || frame == null)
                return;

            try
            {
                var color = frame.BackgroundColor.ToPlatform();
                int[] colors = { color, color };

                var gradientDrawable = new GradientDrawable(GradientDrawable.Orientation.LeftRight, colors);

                float cornerRadiusPx = handler.Context.ToPixels((float)frame.CornerRadius * 2f);
                gradientDrawable.SetCornerRadius(cornerRadiusPx);

                handler.PlatformView.SetBackground(gradientDrawable);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    handler.PlatformView.OutlineProvider = new RoundRectOutlineProvider(cornerRadiusPx);
                    handler.PlatformView.ClipToOutline = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MaterialFrameHandler] MapBackground exception: {ex}");
            }
        }

        private static void MapElevation(MaterialFrameHandler handler, MaterialFrame frame)
        {
            if (handler.PlatformView == null || frame == null)
                return;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                // Use Elevation property if set; fall back to HasShadow
                var elevation = frame.HasShadow ? frame.Elevation : 0f;
                handler.PlatformView.Elevation = elevation;
            }
        }

        // OutlineProvider used for rounded clipping
        private class RoundRectOutlineProvider : ViewOutlineProvider
        {
            readonly float _radius;

            public RoundRectOutlineProvider(float radius)
            {
                _radius = radius;
            }

            public override void GetOutline(global::Android.Views.View view, Outline outline)
            {
                try
                {
                    int w = view.Width;
                    int h = view.Height;
                    if (w <= 0 || h <= 0)
                        return;

                    outline.SetRoundRect(0, 0, w, h, _radius);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[RoundRectOutlineProvider] GetOutline exception: {ex}");
                }
            }
        }
    }
}
