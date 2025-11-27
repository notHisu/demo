using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Java.Lang;
using System.Threading;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Helpers;
using Res = Android.Resource;
using AndroidX.Biometric;


namespace FaceIDTest.Droid.Services.FingerprintAuth
{
    /// <summary>
    ///     This DialogFragment is displayed when the app is scanning for fingerprints.
    /// </summary>
    /// <remarks>
    ///     This DialogFragment doesn't perform any checks to see if the device
    ///     is actually eligible for fingerprint authentication. All of those checks are performed by the
    ///     Activity.
    /// </remarks>
    public class FingerprintManagerApiDialogFragment : AndroidX.Fragment.App.DialogFragment
    {
        static readonly string _TAG = "X:" + typeof(FingerprintManagerApiDialogFragment).Name;

        FingerprintAuthenticatedResult _result;
        Android.Widget.Button _cancelButton;
        CancellationSignal _cancellationSignal;
        CancellationToken _cancellationToken;
        BiometricManager _biometricManager;
        BiometricPrompt _biometricPrompt;

        CryptoObjectHelper _CryptObjectHelper { get; set; }

        bool _IsScanningForFingerprints => _cancellationSignal != null;

        public bool ScanForFingerprintsInOnResume { get; set; } = true;
        public bool UserCancelledScan { get; set; }
        public FingerprintAuth Called;

        public static FingerprintManagerApiDialogFragment NewInstance(
            FingerprintAuthenticatedResult result,
            BiometricManager biometricManager,
            CancellationToken cancellationToken,
            FingerprintAuth called)
        {
            return new FingerprintManagerApiDialogFragment
            {
                _biometricManager = biometricManager,
                _result = result,
                _cancellationToken = cancellationToken,
                Called = called
            };
        }

        public void Init(bool startScanning = true)
        {
            ScanForFingerprintsInOnResume = startScanning;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
            _CryptObjectHelper = new CryptoObjectHelper();
            //SetStyle(DialogFragmentStyle.Normal, Res.Style.ThemeMaterialLightDialog);
            const int STYLE_NORMAL = 0;
            SetStyle(STYLE_NORMAL, Res.Style.ThemeMaterialLightDialog);
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var biometricTouchIDTitle = string.Empty;
            var biometricSignTitle = string.Empty;

            if (!Settings.GetBoleanProperty(Settings.IsBiometricsFirstSetup))
            {
                biometricTouchIDTitle = "BiometricTouchIDFirtsTimeTitle".Localized();
                biometricSignTitle = string.Format("BiometricSignFingerPrintTitle".Localized(), AppInfo.Name);
            }
            else
            {
                biometricTouchIDTitle = "BiometricTouchIDTitle".Localized();
                biometricSignTitle = string.Format("BiometricSignTitle".Localized(), AppInfo.Name);
            }

            Dialog.SetTitle(biometricSignTitle);

            var v = inflater.Inflate(MAUI.Clinical6.Resource.Layout.dialog_scanning_for_fingerprint, container, false);

            var titleMessage = v.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.fingerprint_description);
            titleMessage.Text = biometricTouchIDTitle;

            var hintMessage = v.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.fingerprint_status);
            hintMessage.Text = "BiometricTouchIDHint".Localized();

            _cancelButton = v.FindViewById<Android.Widget.Button>(MAUI.Clinical6.Resource.Id.cancel_button);
            _cancelButton.Text = "DialogCancel".Localized();
            _cancelButton.Click += (sender, args) =>
            {
                UserCancelledScan = true;
                StopListeningForFingerprints();
                Dismiss();
            };

            return v;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (!ScanForFingerprintsInOnResume)
                return;

            UserCancelledScan = false;

            _cancellationSignal = new CancellationSignal();
            _cancellationToken.Register(() => _cancellationSignal.Cancel());

            var executor = ContextCompat.GetMainExecutor(Context);

            // Use Activity property to get the hosting FragmentActivity
            var activity = Activity;

            if (activity == null)
            {
                // Activity not attached yet
                return;
            }

            // Create BiometricPrompt using FragmentActivity and callback
            _biometricPrompt = new BiometricPrompt(
                this,  // pass FragmentActivity here
                executor,
                new BiometricAuthCallback(_result, this)
            );

            // Build the prompt info
            var promptInfo = new BiometricPrompt.PromptInfo.Builder()
                .SetTitle("Biometric Authentication")
                .SetSubtitle("Place your finger on the sensor")
                .SetNegativeButtonText("Cancel")
                .Build();

            _biometricPrompt.Authenticate(promptInfo);
        }


        public override void OnPause()
        {
            base.OnPause();
            if (_IsScanningForFingerprints)
                StopListeningForFingerprints(true);
        }

        void StopListeningForFingerprints(bool butStartListeningAgainInOnResume = false)
        {
            if (_cancellationSignal != null && !_cancellationSignal.IsCanceled)
            {
                _cancellationSignal.Cancel();
                _cancellationSignal = null;
            }
            ScanForFingerprintsInOnResume = butStartListeningAgainInOnResume;
        }

        public override void OnDestroyView()
        {
            Called.SetResult(_result);

            // Workaround for Android bug https://code.google.com/p/android/issues/detail?id=17423
            if (Dialog != null && RetainInstance)
            {
                Dialog.SetDismissMessage(null);
            }

            base.OnDestroyView();
        }

        // New authentication callback for BiometricPrompt
        class BiometricAuthCallback : BiometricPrompt.AuthenticationCallback
        {
            readonly FingerprintAuthenticatedResult _result;
            readonly FingerprintManagerApiDialogFragment _fragment;

            public BiometricAuthCallback(FingerprintAuthenticatedResult result, FingerprintManagerApiDialogFragment fragment)
            {
                _result = result;
                _fragment = fragment;
            }

            public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
            {
                base.OnAuthenticationSucceeded(result);
                // Handle success, update _result and notify fragment
                //_result.IsSuccess = true;
                _result.SetSuccess();
                _fragment.Dismiss();
            }

            public override void OnAuthenticationFailed()
            {
                base.OnAuthenticationFailed();
                // Optionally update UI for failure feedback
            }

            public override void OnAuthenticationError(int errorCode, ICharSequence? errString)
            {
                base.OnAuthenticationError(errorCode, errString);

                // User canceled or fatal error
                if (errorCode == BiometricPrompt.ErrorUserCanceled ||
                        errorCode == BiometricPrompt.ErrorCanceled)
                {
                    _fragment.UserCancelledScan = true;
                    _fragment.Dismiss();
                }
                else
                {
                    // Handle other errors if needed
                }
            }
        }
    }
}
