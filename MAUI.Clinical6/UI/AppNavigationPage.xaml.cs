using Xamarin.Forms.Clinical6.Core.Helpers;

namespace Xamarin.Forms.Clinical6.UI
{
    public partial class AppNavigationPage : NavigationPage
    {
        public AppNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = AppHelpers.GetResource<Color>("LoginNavBackground");
            BarTextColor = AppHelpers.GetResource<Color>("LoginNavText");

            InitializeComponent();
        }
    }
}
