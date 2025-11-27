// Platforms/Android/Services/BiometricsService.cs
using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Biometric;
using AndroidX.Core.Content;
using Xamarin.Forms.Clinical6.Helpers;
using Xamarin.Forms.Clinical6.Services;

namespace MAUI.Clinical6.Android.Services
{
    // Note: no Xamarin.Forms Dependency attribute here. Use MAUI DI registration instead.
    public class BiometricsService : IBiometricsService
    {
        private readonly Context _context;
        private BiometricManager _biometricManager;
        private readonly IFingerprintAuth _fingerprintAuth; // optional, inject via DI

        // Parameterless ctor (works if you don't inject IFingerprintAuth).
        // If you plan to authenticate (BiometricPrompt), register and inject IFingerprintAuth via DI.
        //public BiometricsService()
        //{
        //    // Use MAUI Platform.AppContext if available; fallback to Android application context
        //    try
        //    {
        //        // Platform.AppContext is available in MAUI; we don't refer to Microsoft.Maui.* types here to avoid extra deps
        //        //_context = global::Android.App.Application.Context;
        //        _context = Platform.AppContext ?? global::Android.App.Application.Context;
        //    }
        //    catch
        //    {
        //        //_context = global::Android.App.Application.Context;
        //        _context = Platform.AppContext ?? global::Android.App.Application.Context;
        //    }

        //    _biometricManager = BiometricManager.From(_context);
        //}

        public BiometricsService()
        {
            _context = Platform.CurrentActivity ?? Platform.AppContext ?? global::Android.App.Application.Context;

            global::Android.Util.Log.Debug("BiometricsService", $"Platform.AppContext is {(Platform.AppContext == null ? "null" : "not null")}");
            global::Android.Util.Log.Debug("BiometricsService", $"Platform.CurrentActivity is {(Platform.CurrentActivity == null ? "null" : "not null")}");

            _biometricManager = BiometricManager.From(_context);
            global::Android.Util.Log.Debug("BiometricsService", $"Context type: {_context.GetType().FullName}");
        }

        // Constructor that supports DI for IFingerprintAuth (recommended)
        public BiometricsService(IFingerprintAuth fingerprintAuth) : this()
        {
            _fingerprintAuth = fingerprintAuth;
        }

        /// <summary>
        /// Checks whether biometric or device credential auth is available.
        /// </summary>
        /// <param name="devicePassword">If true, treat device credential (PIN/pattern/password) as acceptable.</param>
        //public async Task<bool> CanAuthenticate(bool devicePassword = false)
        //{
        //    if (_context == null)
        //        return false;

        //    // Legacy permission check for Android < Q (API 29)
        //    if (Build.VERSION.SdkInt < BuildVersionCodes.Q)
        //    {
        //        var perm = ContextCompat.CheckSelfPermission(_context, Manifest.Permission.UseFingerprint);
        //        if (perm != global::Android.Content.PM.Permission.Granted)
        //            return false;
        //    }

        //    _biometricManager = BiometricManager.From(_context);

        //    var authenticators = BiometricManager.Authenticators.BiometricStrong
        //                         | BiometricManager.Authenticators.DeviceCredential;

        //    int canAuthenticate = _biometricManager.CanAuthenticate(authenticators);
        //    global::Android.Util.Log.Debug("BiometricsService", $"CanAuthenticate returned: {canAuthenticate}");

        //    // Debug logging suggestion:
        //    // Android.Util.Log.Debug("BiometricsService", $"CanAuthenticate returned: {canAuthenticate}");

        //    if (canAuthenticate == BiometricManager.BiometricSuccess)
        //        return true;

        //    // If device credential is allowed and device keyguard is secure — treat as true when requested
        //    try
        //    {
        //        var keyguardManager = (KeyguardManager)_context.GetSystemService(Context.KeyguardService);
        //        bool isSecure = keyguardManager?.IsKeyguardSecure ?? false;
        //        if (isSecure && devicePassword)
        //        {
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        // swallow any platform-specific exceptions and continue returning false
        //    }

        //    return false;
        //}

        public async Task<bool> CanAuthenticate(bool devicePassword = false)
        {
            if (_context == null)
            {
                global::Android.Util.Log.Debug("BiometricsService", "Context is null");
                return false;
            }

            if (Build.VERSION.SdkInt < BuildVersionCodes.Q)
            {
                var perm = ContextCompat.CheckSelfPermission(_context, Manifest.Permission.UseFingerprint);
                if (perm != global::Android.Content.PM.Permission.Granted)
                {
                    global::Android.Util.Log.Debug("BiometricsService", "USE_FINGERPRINT permission not granted");
                    return false;
                }
            }

            _biometricManager = BiometricManager.From(_context);

            // Log device info
            global::Android.Util.Log.Debug("BiometricsService", $"Device: {Build.Manufacturer} {Build.Model}, API: {Build.VERSION.SdkInt}");

            var authenticators = new[]
            {
                BiometricManager.Authenticators.BiometricStrong,
                BiometricManager.Authenticators.BiometricWeak,
                BiometricManager.Authenticators.DeviceCredential,
                BiometricManager.Authenticators.BiometricStrong | BiometricManager.Authenticators.DeviceCredential,
                BiometricManager.Authenticators.BiometricWeak | BiometricManager.Authenticators.DeviceCredential
            };

            foreach (var auth in authenticators)
            {
                int result = _biometricManager.CanAuthenticate(auth);
                global::Android.Util.Log.Debug("BiometricsService", $"CanAuthenticate({auth}) returned: {result}");

                if (result == BiometricManager.BiometricSuccess)
                {
                    global::Android.Util.Log.Debug("BiometricsService", $"Supported: {auth}");
                    return true;
                }
            }

            var keyguardManager = (KeyguardManager)_context.GetSystemService(Context.KeyguardService);
            bool isSecure = keyguardManager?.IsKeyguardSecure ?? false;
            global::Android.Util.Log.Debug("BiometricsService", $"Keyguard secure: {isSecure}");

            if (isSecure && devicePassword)
                return true;

            return false;
        }


        /// <summary>
        /// Keep this signature to match your existing IBiometricsService.
        /// This method delegates actual prompt/auth to IFingerprintAuth when available.
        /// </summary>
        public async void AuthenticateUser(Action<bool, string> OnAuthComplete)
        {
            try
            {
                if (_fingerprintAuth != null)
                {
                    var resp = await _fingerprintAuth.GetAuthenticateAsync("Authenticate");
                    if (resp != null && resp.isAutheticated)
                        OnAuthComplete?.Invoke(true, string.Empty);
                    else
                        OnAuthComplete?.Invoke(false, resp?.ErrorMessage ?? "Authentication failed");
                }
                else
                {
                    // No IFingerprintAuth registered - return a failure message
                    OnAuthComplete?.Invoke(false, "Fingerprint auth implementation not registered. Register IFingerprintAuth in MAUI DI.");
                }
            }
            catch (Exception ex)
            {
                OnAuthComplete?.Invoke(false, ex.Message);
            }
        }

        /// <summary>
        /// Returns which local authentication method is available (None/Pin/Fingerprint etc.)
        /// </summary>
        public LocalAuthenticationType CanAuthenticationWith()
        {
            LocalAuthenticationType authType = LocalAuthenticationType.None;
            var context = _context ?? global::Android.App.Application.Context;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var biometricManager = BiometricManager.From(context);
                int canAuthenticate = biometricManager.CanAuthenticate(
                    BiometricManager.Authenticators.BiometricStrong |
                    BiometricManager.Authenticators.DeviceCredential);

                switch (canAuthenticate)
                {
                    case BiometricManager.BiometricSuccess:
                        authType = LocalAuthenticationType.BiometryTouchID_Or_Fingerprint;
                        break;
                    case BiometricManager.BiometricErrorNoHardware:
                    case BiometricManager.BiometricErrorNoneEnrolled:
                    case BiometricManager.BiometricErrorHwUnavailable:
                    default:
                        authType = LocalAuthenticationType.None;
                        break;
                }

                // Check if keyguard (PIN/pattern/password) is set
                try
                {
                    var keyguardManager = (KeyguardManager)context.GetSystemService(Context.KeyguardService);
                    if (keyguardManager?.IsKeyguardSecure ?? false)
                    {
                        if (authType == LocalAuthenticationType.None)
                            authType = LocalAuthenticationType.Pin;
                    }
                }
                catch
                {
                    // ignore
                }

                // legacy permission check (older Android)
                var permissionResult = ContextCompat.CheckSelfPermission(context, Manifest.Permission.UseFingerprint);
                if (permissionResult == global::Android.Content.PM.Permission.Granted)
                {
                    if (authType == LocalAuthenticationType.None)
                        authType = LocalAuthenticationType.BiometryTouchID_Or_Fingerprint;
                }
            }

            return authType;
        }
    }
}
