using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        public const string ApiRoot = "ApiRoot";
        public const string MobileApplicationKey = "MobileApplicationKey";
        public const string IsLoginIn = "IsLoginIn";
        public const string LoginDate = "LoginDate";
        public const string SleepTime = "SleepTime";
        public const string ConsetStatus = "ConsetStatus";
        public const string PushDevicetoken = "PushDevicetoken";
        public const string LastConsentedAt = "LastConsentedAt";
        public const string AutomaticLogin = "AutomaticLogin";
        public const string UserPin = "UserPin";
        public const string UserLanguage = "UserLanguage";
        public const string UserLanguageSelected = "UserLanguageSelected";
        public const string CurrentSeesionDeviceId = "CurrentSeesionDeviceId";
        public const string SignatureComplete = "SignatureComplete";
        public const string HomeLocation = "HomeLocation";
        public const string IsBiometricsActive = "isBiometricsActive";
        public const string IsBiometricsAlreadySetup = "isBiometricsAlreadySetup";
        public const string IsVideoSessionActiove = "IsVideoSessionActiove";
        public const string IsBiometricsFirstSetup = "isBiometricsFirstSetup";
        public const string IncludeCountryInSiteAddress = "includeCountryInSiteAddress";
        public const string UseNameForActivityHistory = "useNameForActivityHistory";
        public const string UseNameForFlow = "useNameForFlow";
        public const string PinResetEmail = "PinResetEmail";
        public const string SiteMember = "SiteMember";
        public const string MagicLinkStart = "MagicLinkStart";
        public const string PermanentLink = "PermanentLink";
        public const string ResetPassword = "ResetPassword";
        public const string StartDate = "StartDate";
        public const string PreviousNavColor = "PreviousNavColor";
        public const string SiteDisplayId = "SiteId";
        public const string SiteId = "Site.Id";
        public const string SiteName = "SiteName";
        public const string NotAvailable = "N/A";
        public const string KCCQ = "KCCQ";

        public const string StudyWeekTotal = "StudyWeekTotal";
        public const string StudyWeeksStartAtOne = "StudyWeeksStartAtOne";

        public const string EnvironmentMappedLogins = "EnvironmentMappedLogins";

        public const string NumericPrecision = "NumericPrecision"; // Support for value of 1 or 2 (after decimal)

        //NOTE: Secure settings
        public const string AppPin = "AppPin";
        public const string BiometricsEmail = "BiometricsEmail";
        public const string BiometricsPin = "BiometricsPin";
        public const string NewSavedEmail = "NewSavedEmail";

        public const string Reconnect = "Reconnect";
        public const string ReconnectId = "ReconnectId";

        #region Migration Settings
        private const string PinFlagMigrationCompleteKey = EnvironmentConfig.PinFlagMigrationCompleteKey;

        private const bool PinFlagMigrationCompleteDefault = false;
        public static bool PinFlagMigrationComplete
        {
            get => Preferences.Get(PinFlagMigrationCompleteKey, PinFlagMigrationCompleteDefault);
            set => Preferences.Set(PinFlagMigrationCompleteKey, value);
        }
        #endregion

        public static string GetProperty(string name)
        {
            if (Preferences.ContainsKey(name))
                return Preferences.Get(name, string.Empty);

            //if (Application.Current.Properties.ContainsKey(name))
            //    return (string)Application.Current.Properties[name];

            return null;
        }

        public async static void SetProperty(string name, string value)
        {
            Preferences.Set(name, value);

            //if (Application.Current == null)
            //    return;

            //if (Application.Current.Properties.ContainsKey(name))
            //    Application.Current.Properties[name] = value;
            //else
            //    Application.Current.Properties.Add(name, value);

            //await Application.Current.SavePropertiesAsync();
        }

        public static bool GetBoleanProperty(string name)
        {
            if (Application.Current == null)
                return false;

            //if (Application.Current.Properties.ContainsKey(name))
            //    return (bool)Application.Current.Properties[name];

            if (Preferences.ContainsKey(name))
                return Preferences.Get(name, false);

            return false;
        }

        public async static void SetBoleanProperty(string name, bool value)
        {
            Preferences.Set(name, value);

            //if (Application.Current.Properties.ContainsKey(name))
            //    Application.Current.Properties[name] = value;
            //else
            //    Application.Current.Properties.Add(name, value);

            //await Application.Current.SavePropertiesAsync();
        }

        public static int? GetIntProperty(string name)
        {
            if (Preferences.ContainsKey(name))
                return Preferences.Get(name, defaultValue: 0);

            //if (Application.Current == null)
            //    return null;

            //if (Application.Current.Properties.ContainsKey(name))
            //    return (int)Application.Current.Properties[name];

            return null;
        }

        public async static void SetIntProperty(string name, int value)
        {
            Preferences.Set(name, value);

            //if (Application.Current.Properties.ContainsKey(name))
            //    Application.Current.Properties[name] = value;
            //else
            //    Application.Current.Properties.Add(name, value);

            //await Application.Current.SavePropertiesAsync();
        }

        public static Dictionary<string, string> GetDictProperty(string name)
        {
            if (!Preferences.ContainsKey(name))
                return null;

            var dictAsJson = Preferences.Get(name, string.Empty);
            if (string.IsNullOrEmpty(dictAsJson))
                return null;

            //if (!Application.Current.Properties.ContainsKey(name)) return null;

            //var dictAsJson = (string) Application.Current.Properties[name];

            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(dictAsJson);
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }

        public static async void SetDictProperty(string name, Dictionary<string, string> value)
        {
            var dictAsJson = JsonConvert.SerializeObject(value);
            Preferences.Set(name, dictAsJson);

            //if (Application.Current.Properties.ContainsKey(name))
            //    Application.Current.Properties[name] = dictAsJson;
            //else
            //    Application.Current.Properties.Add(name, dictAsJson);

            //await Application.Current.SavePropertiesAsync();
        }
    }
}