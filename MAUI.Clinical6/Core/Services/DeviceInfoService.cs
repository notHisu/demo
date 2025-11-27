using Clinical6SDK.Models;
using Clinical6SDK.Services;
using Clinical6SDK.Services.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Services;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface IDeviceInfoService
    {
        Technology DeviceTechnology { get; }

        string AppVersion { get; }

        bool IsFirstLaunchForBuild { get; }

        Task<bool> CheckServerVersionAsync();
    }

    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly Lazy<Technology> _deviceTechnology;
        public Technology DeviceTechnology => _deviceTechnology.Value;

        private readonly Lazy<string> _appVersion;
        public string AppVersion => _appVersion.Value;

        private readonly Lazy<string> _appBuild;
        public string AppBuild => _appBuild.Value;

        public bool IsFirstLaunchForBuild => VersionTracking.IsFirstLaunchForCurrentBuild;

        public DeviceInfoService()
        {
            _deviceTechnology = new Lazy<Technology>(GetDeviceTechnology);
            _appVersion = new Lazy<string>(() => VersionTracking.CurrentVersion);
            _appBuild = new Lazy<string>(() => VersionTracking.CurrentBuild);
        }

        private Technology GetDeviceTechnology()
        {
            return DeviceInfo.Platform == DevicePlatform.iOS
                ? Technology.Ios
                : Technology.Andriod;
        }

        private async Task<List<AppSettings>> GetStudySettings()
        {
            try
            {
                var ServiceSDK = new Clinical6BaseService();
                ServiceSDK.BaseUrl = EnvironmentConfig.ApiRoot;

                var service = new JsonApiHttpService
                {
                    BaseUrl = EnvironmentConfig.ApiRoot,
                    AuthToken = await new CacheService().GetAuthToken()
                };

                var options = new Options { Url = "/v3/public_settings" };

                var response = await ServiceSDK.Get<List<AppSettings>>(options);

                return response;
            }
            catch
            {

            }

            return new List<AppSettings>();
        }


        public async Task<bool> CheckServerVersionAsync()
        {
            try
            {
#if DEBUG
                return false;
#endif

                var response = await GetStudySettings();

                if (response != null)
                {
                    var serverVersion = "0";
                    var urlStored = string.Empty;
                    var key = "allowed_ios_build";
                    var keyUrlStored = "ios_store_url";

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        key = "allowed_android_build";
                        keyUrlStored = "play_store_url";
                    }

                    var serverBuild = response.Where(c => c.Id == key).FirstOrDefault();

                    if (serverBuild != null)
                    {
                        serverVersion = serverBuild?.SettingsAttributes?.Value?.ToString();
                    }

                    var serverUrl = response.Where(c => c.Id == keyUrlStored).FirstOrDefault();

                    if (serverUrl != null)
                    {
                        urlStored = serverUrl?.SettingsAttributes?.Value?.ToString();
                    }

                    if (string.IsNullOrEmpty(urlStored) || string.IsNullOrEmpty(serverVersion))
                    {
                        return false;
                    }

                    var build = DependencyService.Get<IManagerAppVersion>().GetBuildNumber();

                    var appName = DependencyService.Get<IManagerAppVersion>().AppVersionName();

                    if (build.IndexOf(".") <= 0)
                    {
                        build = string.Format("{0}.0.0", build);
                    }

                    if (serverVersion.IndexOf(".") <= 0)
                    {
                        serverVersion = string.Format("{0}.0.0", serverVersion);
                    }

                    var versionApp = new Version(build);
                    var versionServer = new Version(serverVersion);

                    var result = versionServer.CompareTo(versionApp);

                    if (result > 0)
                    {
                        Device.BeginInvokeOnMainThread(async delegate
                        {
                            await Application.Current.MainPage.DisplayAlert("NewVersionAvailableTitle".Localized(), "NewVersionAvailableMessage".Localized(), "DownloadTitle".Localized());
                            DependencyService.Get<IManagerAppVersion>().OpenLink(urlStored);
                        });

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }
    }
}