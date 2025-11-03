using System;

using Xamarin.Forms;

namespace Xamarin.Forms.Clinical6.UI.Views
{
    public class BasePage : ContentPage
    {
        /// <summary>
        /// Gets or Sets the Back button click overriden custom action
        /// </summary>
        public Action CustomBackButtonAction { get; set; }

        public static readonly BindableProperty EnableBackButtonOverrideProperty = BindableProperty.Create(
           nameof(EnableBackButtonOverride), typeof(bool), typeof(BasePage), false);

        /// <summary>
        /// Gets or Sets Custom Back button overriding state
        /// </summary>
        public bool EnableBackButtonOverride
        {
            get
            {
                return (bool)GetValue(EnableBackButtonOverrideProperty);
            }
            set
            {
                SetValue(EnableBackButtonOverrideProperty, value);
            }
        }

//        public BasePage()
//        {
//            // remove any default padding
//            Padding = 0;

//            // hide the navigation bar by default for all pages inheriting from BasePage
//            NavigationPage.SetHasNavigationBar(this, false);

//            // disable the iOS safe area so no blank space appears at top/bottom
//#if __IOS__
//            Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, false);
//#endif
//        }
    }
}

