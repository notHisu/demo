using System;
namespace Xamarin.Forms.Clinical6.UI.Effects
{
    public class ScrollReporterEffect : RoutingEffect
    {
        public class ScrollEventArgs : EventArgs
        {
            public double ScrollY { get; set; }
            public ScrollEventArgs(double ScrollY)
            {
                this.ScrollY = ScrollY;
            }
        }

        public event Action<Object, ScrollEventArgs> ScrollChanged;


        public ScrollReporterEffect() : base("Xamarin.ScrollReporterEffect") { }

        public void OnScrollChanged(Object sender, ScrollEventArgs e)
        {
            ScrollChanged?.Invoke(sender, e);
        }
    }
}
