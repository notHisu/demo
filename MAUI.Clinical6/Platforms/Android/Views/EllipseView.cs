using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Paint = Android.Graphics.Paint;

namespace Xamarin.Forms.Clinical6.Android
{
    /// <summary>
    /// Ellipse view.
    /// </summary>
    public class EllipseView : global::Android.Views.View
    {
        public EllipseView(Context context)
            : base(context) { }

        public EllipseView(Context context, IAttributeSet attrs)
            : base(context, attrs) { }

        public EllipseView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr) { }

        public EllipseView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes) { }


        public global::Android.Graphics.Color FillColor { get; set; }
        public global::Android.Graphics.Color StrokeColor { get; set; }
        public double StrokeWidth { get; set; }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            var fillPaint = new Paint();
            fillPaint.Flags = PaintFlags.AntiAlias;
            fillPaint.SetStyle(Paint.Style.Fill);
            fillPaint.Color = FillColor;

            var strokePaint = new Paint();
            strokePaint.Flags = PaintFlags.AntiAlias;
            strokePaint.SetStyle(Paint.Style.Stroke);
            strokePaint.Color = StrokeColor;
            strokePaint.StrokeWidth = (float)StrokeWidth;

            var strokeTotal = StrokeWidth * 2;
            var centerX = GetX() + Width / 2;
            var centerY = base.GetY() + Height / 2;
            var radius = ((float)Math.Min(Width - strokeTotal, Height - strokeTotal)) / 2;
            canvas.DrawCircle(centerX, centerY, radius, fillPaint);
            canvas.DrawCircle(centerX, centerY, radius, strokePaint);
        }
    }
}
