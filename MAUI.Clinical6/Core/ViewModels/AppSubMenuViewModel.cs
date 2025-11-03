using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Core.Services;
using Xamarin.Forms.Clinical6.Core.ViewModels;
using Xamarin.Forms.Clinical6.Helpers;

namespace Xamarin.Forms.Clinical6.ViewModels
{
    public class AppSubMenuViewModel : BaseViewModel<AppSubMenuViewModel.InitValues>
    {
        private AppMenu _selectedItem;
        private AppMenu parentMenu;
        private string _pageTitle;

        public readonly ICacheService _cacheService;
        public readonly IDeviceInfoService _deviceService;

        public List<AppMenu> _menus;

        public AppSubMenuViewModel(INavigationService navigationService, ICacheService cacheService, IDeviceInfoService deviceService) : base(navigationService, cacheService, deviceService)
        {
            _cacheService = cacheService;
            _deviceService = deviceService;
        }

        public AppMenu SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value && value != null)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    _selectedItem = null;
                    ViewResourcesTask(value);
                }
            }
        }

        public List<AppMenu> Menus
        {
            get
            {
                return _menus;
            }
            set
            {
                if (_menus != value)
                {
                    _menus = value;
                    OnPropertyChanged(nameof(Menus));
                }
            }
        }

        public new string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                if (_pageTitle != value)
                {
                    _pageTitle = value;
                    OnPropertyChanged(nameof(PageTitle));
                }
            }
        }

        protected override async Task InitImplementation()
        {
            if (InitParam != null)
            {
                var isSubbMenu = InitParam?.IsSubMenu;

                if ((bool)isSubbMenu)
                {
                    Menus = InitParam.Menus;
                    parentMenu = InitParam.ParentMenu;
                    PageTitle = parentMenu.Title;
                    OnPropertyChanged(nameof(PageTitle));
                }
                else
                {
                    await Navigation.Pop();
                }
            }

        }

        public void ViewResourcesTask(AppMenu currentMenu)
        {
            var children = Menus.Where(c => c.Parent == currentMenu).OrderBy(c => c.Position).ToList();

            if (children?.Count > 0)
            {
                this.NavigateSubMenu(currentMenu, children);
                return;
            }

            this.Navigate(currentMenu);
        }

        public async Task LogouTask()
        {
            await _cacheService.ClearAll();
            await _cacheService.ClearAuthToken();

            Settings.SetProperty(Settings.ConsetStatus, string.Empty);
            Settings.SetProperty(Settings.LastConsentedAt, string.Empty);

            //await Navigation.Push<AppInitViewModel>();
        }

        public class InitValues
        {
            public List<AppMenu> Menus;
            public AppMenu ParentMenu;
            public bool IsSubMenu;
        }
    }
}
