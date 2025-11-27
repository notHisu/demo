namespace Xamarin.Forms.Clinical6.Views
{
    public partial class DashboardMasterPage : FlyoutPage
    {
        public DashboardMasterPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IsGestureEnabled = false;
        }
    }
}