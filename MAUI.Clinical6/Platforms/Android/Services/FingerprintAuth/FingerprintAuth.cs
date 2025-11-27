using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using AndroidX.Biometric;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using FaceIDTest.Droid.Services.FingerprintAuth;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Android;
using Xamarin.Forms.Clinical6.Helpers;

//[assembly: Dependency(typeof(FaceIDTest.Droid.Services.FingerprintAuth.FingerprintAuth))]
namespace FaceIDTest.Droid.Services.FingerprintAuth
{
    public class FingerprintAuth : IFingerprintAuth
    {
        static readonly string _DIALOG_FRAGMENT_TAG = "fingerprint_auth_fragment";

        FingerprintManagerApiDialogFragment _dialogFrag;
        BiometricManager _biometricManager;
        Context _context;

        FingerprintAuthenticatedResult _result;
        TaskCompletionSource<FingerprintAuthenticatedResult> _taskCompletionSource;

        public FingerprintAuth()
        {
            //_context = Android.App.Application.Context;
            _context = Platform.AppContext ?? Android.App.Application.Context;
            _biometricManager = BiometricManager.From(_context);
        }

        public async Task<bool> CanAuthenticate(bool devicePassword = false)
        {
            if (_context == null)
                return false;

            // For Android < Q, check legacy fingerprint permission
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Q)
            {
                var permissionResult = ContextCompat.CheckSelfPermission(_context, Manifest.Permission.UseFingerprint);
                if (permissionResult != Permission.Granted)
                    return false;
            }

            // Check if device can authenticate with strong biometrics or device credential
            var canAuthenticate = _biometricManager.CanAuthenticate(
                BiometricManager.Authenticators.BiometricStrong |
                BiometricManager.Authenticators.DeviceCredential);

            return canAuthenticate == BiometricManager.BiometricSuccess;
        }

        public async Task<FingerprintAuthenticatedResult> GetAuthenticateAsync(string reason, bool devicePassword = false, CancellationToken cancellationToken = default)
        {
            _taskCompletionSource = new TaskCompletionSource<FingerprintAuthenticatedResult>();

            Authenticate(reason, cancellationToken);

            _result = await _taskCompletionSource.Task;

            return _result;
        }

        public void SetResult(FingerprintAuthenticatedResult result)
        {
            _taskCompletionSource.TrySetResult(result);
        }

        private void Authenticate(string reason, CancellationToken cancellationToken)
        {
            try
            {
                _result = new FingerprintAuthenticatedResult();

                // Use BiometricManager to create dialog fragment
                _dialogFrag = FingerprintManagerApiDialogFragment.NewInstance(_result, _biometricManager, cancellationToken, this);
                _dialogFrag.Init();

                //var currentActivity = Controls.Instance; // should be your FragmentActivity
                //var currentActivity = Controls.Instance as AndroidX.Fragment.App.FragmentActivity;
                var currentActivity = Platform.CurrentActivity as FragmentActivity;
                //_dialogFrag.Show(currentActivity.FragmentManager, _DIALOG_FRAGMENT_TAG);
                _dialogFrag.Show(currentActivity.SupportFragmentManager, _DIALOG_FRAGMENT_TAG);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"FingerprintAuth Authenticate exception: {ex}");
            }
        }
    }
}
