using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Clinical6.Core.Helpers;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface IDialogService
    {
        Task ShowAlert(string title, string message, string dismiss = null);
        Task<bool> ShowDialog(string title, string message, string dismiss = null, string accept = null);
        Task<string> ShowChoices(string title, string cancel, string destruction, params string[] buttons);
    }


    public class DialogService : IDialogService
    {
        private Page CurrentPage => Application.Current.MainPage;


        public async Task ShowAlert(string title, string message, string dismiss = null)
        {
            var page = CurrentPage;
            if (page == null) return;

            await page.DisplayAlert(title, message, dismiss ?? "DialogOk".Localized());
        }


        public async Task<bool> ShowDialog(string title, string message, string dismiss = null, string accept = null)
        {
            var page = CurrentPage;
            if (page == null) return false;

            return await page.DisplayAlert(title, message, accept ?? "DialogOk".Localized(), dismiss ?? "DialogCancel".Localized());
        }


        public async Task<string> ShowChoices(string title, string cancel, string destruction, params string[] buttons)
        {
            var page = CurrentPage;
            return await page.DisplayActionSheet(title, cancel ?? "DialogCancel".Localized(), destruction, buttons);
        }
    }
}