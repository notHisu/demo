using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms.Clinical6.Core.Resources;
using Xamarin.Forms.Clinical6.Core.Services;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    public static class AppHelpers
    {
        public static object GetResource(string keyName)
        {
            // Search all dictionaries
            if (!Application.Current.Resources.TryGetValue(keyName, out var retVal))
            {
                Console.WriteLine($"Resource not found: {keyName}");
                Debugger.Break();
            }

            return retVal;
        }

        public static T GetResource<T>(string keyName)
        {
            // Search all dictionaries
            if (!Application.Current.Resources.TryGetValue(keyName, out var retVal))
            {
                Console.WriteLine($"Resource not found: {keyName}");
                Debugger.Break();
            }

            if (retVal != null)
            {
                return (T)retVal;
            }
            else
            {
                return default;
            }
        }

        public static string Localized(this string key)
        {
            var languageService = LanguageService.Instance;

            try
            {
                //NOTE: try to get translations from BE cached values
                var remoteTranslation = languageService?.Translate(key);
                if (!string.IsNullOrEmpty(remoteTranslation))
                    return remoteTranslation;
                else
                    Debug.WriteLine($"Missing Remote Translation Key: {key} Language: {languageService?.CurrentLanguage?.Name}, {languageService?.CurrentLanguage?.Iso}");

                var cultureInfo = GetCultureInfoSafely();

                var mainResourceManager = MainService.Instance.AppResourceManager;
                if (mainResourceManager != null)
                {
                    var resourceManagerValue = mainResourceManager.GetString(key, cultureInfo);
                    if (!string.IsNullOrEmpty(resourceManagerValue))
                        return resourceManagerValue;
                    else
                        Debug.WriteLine($"Missing Consuming App Resx Translation Key: {key} Language: {languageService?.CurrentLanguage?.Name}, {languageService?.CurrentLanguage?.Iso}");
                }

                //NOTE: Defaults to resx files if BE translation not available
                var appResourceTranslation = AppResources.ResourceManager.GetString(key, cultureInfo);
                if (!string.IsNullOrEmpty(appResourceTranslation))
                    return appResourceTranslation;
                else
                    Debug.WriteLine($"Missing Local Resx Translation Key: {key} Language: {AppResources.Culture?.DisplayName}, {AppResources.Culture?.Name}");

            }
            catch (CultureNotFoundException exc)
            {
                Debug.WriteLine($"Culture not supported by environment Key: {key} Language: {languageService?.CurrentLanguage?.Name}, {languageService?.CurrentLanguage?.Iso} {exc}");
            }

            return key;
        }

        /**
         * Returns a <c>CultureInfo</c> instance. If it cannot be created (such as due to a non-supported locale such
         * as es-419), will return the <c>CultureInfo.CurrentCulture</c> instead.
         */
        public static CultureInfo GetCultureInfoSafely()
        {
            try
            {
                var languageService = LanguageService.Instance;

                if (languageService?.CurrentLanguage?.Iso is string iso && !string.IsNullOrWhiteSpace(iso))
                {
                    try
                    {
                        return new CultureInfo(iso);
                    }
                    catch (CultureNotFoundException exc)
                    {
                        Debug.WriteLine($"Culture not supported: {iso}. {exc}");
                    }
                }

                // Fallback if LanguageService or CurrentLanguage is null, or ISO invalid
                return CultureInfo.CurrentCulture;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetCultureInfoSafely] Unexpected error: {ex}");
                return CultureInfo.InvariantCulture;
            }
        }

    }
}