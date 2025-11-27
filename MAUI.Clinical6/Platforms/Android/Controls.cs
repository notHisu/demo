using System;
using Android.App;
using Droid = Android;

namespace Xamarin.Forms.Clinical6.Android
{
    /// <summary>
    /// Controls.
    /// </summary>
    public static class Controls
    {
        internal static Activity Instance { get; private set; }

        public static global::Android.Views.View CustomLayout { get; set; }

        /// <summary>
        /// Init the specified mainactitivy.
        /// </summary>
        /// <param name="mainactitivy">Mainactitivy.</param>
        public static void Init(Activity mainactitivy)
        {
            Instance = mainactitivy;

            //set default layout
            CustomLayout = Instance.LayoutInflater.Inflate(MAUI.Clinical6.Resource.Layout.RoomLayout, null); 
            
        }
    }
}
