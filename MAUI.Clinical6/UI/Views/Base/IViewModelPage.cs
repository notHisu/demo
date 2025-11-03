using System;
using Xamarin.Forms.Clinical6.Core.ViewModels;


namespace Xamarin.Forms.Clinical6.UI.Views
{
    public interface IViewModelPage<BVM> where BVM : BaseViewModel
    {
        BVM ViewModel { get; set; } 
    }
}
