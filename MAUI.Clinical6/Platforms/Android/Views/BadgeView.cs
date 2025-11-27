
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using droid = Android;

namespace Xamarin.Forms.Clinical6.Android.Views
{
    public class BadgeView : TextView
    {
        private droid.Views.View target;

        public BadgeView(Context context, droid.Views.View view) : base(context)
        {
            Initialize(context, view);
        }

        public BadgeView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            //Initialize();
        }

        public BadgeView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            //Initialize();
        }

        void Initialize(Context context, droid.Views.View _target)
        {
            this.target = _target;
        }

        public void UpdateTabBadge(int badgeNumber)
        {
            if (badgeNumber > 0)
            {
                target.Visibility = ViewStates.Visible;
                ((TextView)target).SetText(badgeNumber);
            }
            else
            {
                //target.setVisibility(View.GONE);
                target.Visibility = ViewStates.Gone;
            }
        }
    }
}
