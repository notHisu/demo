namespace Xamarin.Forms.Clinical6.Core
{
    /// <summary>
    /// Environment config.
    /// </summary>
    public static class EnvironmentConfig
    {
        /// <summary>
        /// Gets or sets the API root.
        /// </summary>
        /// <value>The API root.</value>
        public static string ApiRoot { get; set; }

        /// <summary>
        /// Gets or sets the mobile application key.
        /// </summary>
        /// <value>The mobile application key.</value>
        public static string MobileApplicationKey { get; set; }

        public static string ContactEmail = "EUAPlague@ppdi.com";
        public const string HockeyAppIdiOS = "";
        public const string HockeyAppIdAndroid = "";
        public const bool HockeyAppReporting = false;
        public const bool HockeyAppUpdates = false;
        public const bool HockeyAppTracking = false;
        public const string PinFlagMigrationCompleteKey = "PinFlagMigrationComplete";
        public static bool IsPiApp = true;
        public static bool IsConsent = false;
        public static bool IsVideoConsent = true;
        public static int TimeOutSession = 15;
    }
}