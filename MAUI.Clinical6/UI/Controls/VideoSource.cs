using System;
using System.ComponentModel;
using Xamarin.Forms.Clinical6.UI.Converters;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    [TypeConverter(typeof(VideoSourceConverter))]
    public abstract class VideoSource : Element
    {
        public static VideoSource FromUri(string uri)
        {
            return new UriVideoSource() { Uri = uri };
        }
    }
}
