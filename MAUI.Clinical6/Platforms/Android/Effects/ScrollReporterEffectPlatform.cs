using System;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using Microsoft.Maui.Controls.Platform;
using Xamarin.Forms;
using ListView = Android.Widget.ListView;
using PlatformEffects = Xamarin.Forms.Clinical6.Android.Effects;
using RoutingEffects = Xamarin.Forms.Clinical6.UI.Effects;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(PlatformEffects.ScrollReporterEffectPlatform), "ScrollReporterEffect")]
namespace Xamarin.Forms.Clinical6.Android.Effects
{
    /// <summary>
    /// Scroll reporter effect.
    /// </summary>
    /// <see cref="https://www.youtube.com/watch?v=xMHP5EfXdDg"/>
    public class ScrollReporterEffectPlatform : PlatformEffect
    {
        private RoutingEffects.ScrollReporterEffect effect;
        private global::Android.Widget.ListView nativeControl;
        float density;
        private Dictionary<Int32, Int32> listViewItemHeights = new Dictionary<Int32, Int32>();
        private double CellHeight = 0;

        protected override void OnAttached()
        {
            if (Element is ListView == false)
                return;

            // find our effect
            effect = (RoutingEffects.ScrollReporterEffect)Element.Effects.FirstOrDefault(e => e is RoutingEffects.ScrollReporterEffect);

            // implement effect
            nativeControl = (global::Android.Widget.ListView) Control;
            nativeControl.Scroll += NativeControl_Scroll;

            density = nativeControl.Context.Resources.DisplayMetrics.Density;
        }

        private void NativeControl_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            double pos = GetListViewYPosition();
            pos = pos / density;
            effect.OnScrollChanged(Element, new RoutingEffects.ScrollReporterEffect.ScrollEventArgs(pos));
        }

        private int GetListViewYPosition()
        {
            var listView = nativeControl;

            if (listView != null)
            {
                var c = listView.GetChildAt(0); //this is the first visible row
                if (c != null)
                {
                    int scrollY = -c.Top;
                    if (listViewItemHeights.ContainsKey(listView.FirstVisiblePosition) == false)
                    {
                        CellHeight = c.Height;
                        listViewItemHeights.Add(listView.FirstVisiblePosition, c.Height);
                    }
                    for (int i = 0; i < listView.FirstVisiblePosition; ++i)
                    {
                        if (listViewItemHeights.ContainsKey(i) && listViewItemHeights[i] != 0)
                            scrollY += listViewItemHeights[i];
                    }
                    return scrollY;
                }
            }
            return 0;
        }

        protected override void OnDetached()
        {
            nativeControl.Scroll -= NativeControl_Scroll;
        }
    }
}
