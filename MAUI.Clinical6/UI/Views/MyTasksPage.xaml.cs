using System;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.UI.Effects;
using Xamarin.Forms.Clinical6.UI.Views;

namespace Xamarin.Forms.Clinical6.Views
{
    public partial class MyTasksPage : BaseContentPage<MyTasksViewModel>
    {
        public MyTasksPage()
        {
            Title = "TasksText".Localized();
            IconImageSource = "gui_tab_home";
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ViewModel.AutoRefreshEnable = true;
            ViewModel.InitAutoRefresh();

            // Start off invisible since we do not want it to appear blinking off.
            //x_stkRemainingActivities.IsVisible = false;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.AutoRefreshEnable)
            {
                ViewModel.AutoRefreshEnable = true;
                ViewModel.InitAutoRefresh();
            }

            // attach parallax effect
            var parallaxEffect = new ScrollReporterEffect();
            MyTasksListView.Effects.Add(parallaxEffect);
            parallaxEffect.ScrollChanged += ParallaxEffect_ScrollChanged;


            Console.WriteLine(string.Format("<<<<<<<<<<<< UpcomingAppoiment {0} >>>>>>>>>>>>>>", Xamarin.Forms.Clinical6.Core.Helpers.Settings.GetBoleanProperty("UpcomingAppoiment")));

            if (!Settings.GetBoleanProperty(Settings.IsLoginIn))
            {
                return;
            }

            if (Xamarin.Forms.Clinical6.Core.Helpers.Settings.GetBoleanProperty("UpcomingAppoiment"))
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                 //if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }

                //await ViewModel.Navigation.Push<VideoConsultListViewModel>();
                //Xamarin.Forms.Clinical6.Core.Helpers.Settings.SetBoleanProperty("UpcomingAppoiment", false);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.AutoRefreshEnable = false;
        }

        /// <summary>
        /// Parallax Effect - scrolling
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        /// <see cref="https://www.youtube.com/watch?v=xMHP5EfXdDg"/>
        private void ParallaxEffect_ScrollChanged(object sender, ScrollReporterEffect.ScrollEventArgs e)
        {
            var scrollValue = e.ScrollY;
            var parallaxScaleNormal = 2.5;
            var parallaxScaleFast = 1.5;
            if ((scrollValue / parallaxScaleNormal) > 0)
            {
                HeaderImage.TranslationY = -scrollValue / parallaxScaleNormal;
            }
            HeaderTextStackLayout.TranslationY = -scrollValue / parallaxScaleFast;
        }
    }
}
