using System;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.UI.Views;
using Xamarin.Forms.Clinical6.ViewModels;

namespace Xamarin.Forms.Clinical6.Login
{
    public partial class EmailPMAPage : BaseContentPage<LoginPMAViewModel>
    {
        public EmailPMAPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async delegate
            {
                await new DeviceInfoService().CheckServerVersionAsync();
            });

            Task.Run(async delegate
            {
                await ViewModel.RefreshScreen();
            });

            ViewModel.CallBackDisplayEndpoints += HandlerCallBackDisplayEndpoints;
            if(BindingContext is LoginPMAViewModel vm)
            {
                await vm.InitializeAsync();
            }
        }

        private void HandlerCallBackDisplayEndpoints()
        {
            pickerEndpoints.IsVisible = true;
            pickerEndpoints.Focus();
        }
    }
}