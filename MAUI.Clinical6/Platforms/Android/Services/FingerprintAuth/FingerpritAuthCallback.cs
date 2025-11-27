using System;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.Hardware.Biometrics;
using Android.Hardware.Fingerprints;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Javax.Crypto;
using Microsoft.Maui.Platform;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms.Clinical6.Helpers;

namespace FaceIDTest.Droid.Services.FingerprintAuth
{
    public class FingerpritAuthCallback : BiometricPrompt.AuthenticationCallback
    {
        FingerprintAuthenticatedResult _result;

        readonly FingerprintManagerApiDialogFragment _fragment;
        // Can be any byte array, keep unique to application.
        static readonly byte[] _SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        // The TAG can be any string, this one is for demonstration.
        static readonly string _TAG = "X:" + typeof(FingerpritAuthCallback).Name;

        public FingerpritAuthCallback(FingerprintManagerApiDialogFragment frag)
        {
            _result = null;
            _fragment = frag;
        }

        public FingerpritAuthCallback(FingerprintAuthenticatedResult authRes, FingerprintManagerApiDialogFragment frag)
        {
            _result = authRes;
            _fragment = frag;
        }

        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
        {
            Log.Debug(_TAG, "OnAuthenticationSucceeded");
            if (result.CryptoObject.Cipher != null)
            {
                try
                {
                    // Calling DoFinal on the Cipher ensures that the encryption worked.
                    byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(_SECRET_BYTES);
                    Log.Debug(_TAG, "Fingerprint authentication succeeded, doFinal results: {0}",
                                  Convert.ToBase64String(doFinalResult));
                    // No errors occurred, trust the results.
                    ReportSuccess();
                }
                catch (BadPaddingException bpe)
                {
                    // Can't really trust the results.
                    Log.Error(_TAG, "Failed to encrypt the data with the generated key." + bpe);
                    ReportAuthenticationFailed();
                }
                catch (IllegalBlockSizeException ibse)
                {
                    // Can't really trust the results.
                    Log.Error(_TAG, "Failed to encrypt the data with the generated key." + ibse);
                    ReportAuthenticationFailed();
                }
            }
            else
            {
                // No cipher used, assume that everything went well and trust the results.
                Log.Debug(_TAG, "Fingerprint authentication succeeded.");
                ReportSuccess();
            }
        }

        //public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
        //{
        //    // Report the error to the user. Note that if the user canceled the scan,
        //    // this method will be called and the errMsgId will be FingerprintState.ErrorCanceled.

        //    // There are some situations where we don't care about the error. For example, 
        //    // if the user cancelled the scan, this will raise errorID #5. We don't want to
        //    // report that, we'll just ignore it as that event is a part of the workflow.
        //    bool reportError = (errMsgId == (int)FingerprintState.ErrorCanceled) &&
        //                !_fragment.ScanForFingerprintsInOnResume;

        //    string debugMsg = string.Format("OnAuthenticationError: {0}:`{1}`.", errMsgId, errString);

        //    if (_fragment.UserCancelledScan)
        //    {
        //        string msg = "User cancelled the scan.";//;
        //        ReportScanFailure(-1, msg);
        //    }
        //    else if (reportError)
        //    {
        //        ReportScanFailure(errMsgId, errString.ToString());
        //        debugMsg += " Reporting the error.";
        //    }
        //    else
        //    {
        //        debugMsg += " Ignoring the error.";
        //        //ReportScanFailure(-1, string.Empty);
        //    }
        //    Log.Debug(_TAG, debugMsg);
        //}

        public override void OnAuthenticationError([GeneratedEnum] BiometricErrorCode errorCode, ICharSequence? errString)
        {
            // Report the error to the user. Note that if the user canceled the scan,
            // this method will be called and the errMsgId will be FingerprintState.ErrorCanceled.

            // There are some situations where we don't care about the error. For example, 
            // if the user cancelled the scan, this will raise errorID #5. We don't want to
            // report that, we'll just ignore it as that event is a part of the workflow.
            bool reportError = (errorCode == BiometricErrorCode.Canceled) &&
                        !_fragment.ScanForFingerprintsInOnResume;

            string debugMsg = string.Format("OnAuthenticationError: {0}:`{1}`.", errorCode, errString);

            if (_fragment.UserCancelledScan)
            {
                string msg = "User cancelled the scan.";//;
                ReportScanFailure(-1, msg);
            }
            else if (reportError)
            {
                ReportScanFailure((int)errorCode, errString.ToString());
                debugMsg += " Reporting the error.";
            }
            else
            {
                debugMsg += " Ignoring the error.";
                //ReportScanFailure(-1, string.Empty);
            }
            Log.Debug(_TAG, debugMsg);
        }

        public override void OnAuthenticationFailed()
        {
            // Tell the user that the fingerprint was not recognized.
            Log.Info(_TAG, "Authentication failed.");
            // ReportAuthenticationFailed();
            ReportScanFailure(1024, "FingerprintNotRecognized".Localized());
        }

        //public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
        //{
        //    // Notify the user that the scan failed and display the provided hint.
        //    Log.Debug(_TAG, "OnAuthenticationHelp: {0}:`{1}`", helpString, helpMsgId);
        //    ReportScanFailure(helpMsgId, helpString.ToString());
        //}

        public override void OnAuthenticationHelp([GeneratedEnum] BiometricAcquiredStatus helpCode, ICharSequence? helpString)
        {
            // Notify the user that the scan failed and display the provided hint.
            Log.Debug(_TAG, "OnAuthenticationHelp: {0}:`{1}`", helpString, helpCode);
            ReportScanFailure((int)helpCode, helpString.ToString());
        }

        async void ReportSuccess()
        {
            if (!Settings.GetBoleanProperty(Settings.IsBiometricsFirstSetup))
            {
                Settings.SetBoleanProperty(Settings.IsBiometricsFirstSetup, true);
            }
            //var titleMessage = _fragment.View.FindViewById<TextView>(Xamarin.Forms.Clinical6.Android.Resource.Id.fingerprint_status);
            var titleMessage = _fragment.View.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.fingerprint_status);
            titleMessage.Text = "BiometricFingerPrintScanRecognized".Localized();
            //titleMessage.SetTextColor(Xamarin.Forms.Color.FromHex("#41BF6A").ToAndroid());
            titleMessage.SetTextColor(Color.FromArgb("#41BF6A").ToPlatform());

            //var imgStatus = _fragment.View.FindViewById<ImageView>(Xamarin.Forms.Clinical6.Android.Resource.Id.fingerprint_icon);
            //imgStatus.SetImageResource(Xamarin.Forms.Clinical6.Android.Resource.Drawable.gui_fill_warning);
            var imgStatus = _fragment.View.FindViewById<ImageView>(MAUI.Clinical6.Resource.Id.fingerprint_icon);
            imgStatus.SetImageResource(MAUI.Clinical6.Resource.Drawable.gui_fill_success);

            await Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(1.2));
                SetResult(true);
            });
        }

        async void ReportScanFailure(int errMsgId, string errorMessage)
        {
            if (errMsgId == -1)
            {
                SetResult(false, errorMessage);
                return;
            }

            var titleMessage = _fragment.View.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.fingerprint_status);
            titleMessage.Text = errorMessage;
            //titleMessage.SetTextColor(Xamarin.Forms.Color.FromHex("#be1717").ToAndroid());
            titleMessage.SetTextColor(Color.FromArgb("#be1717").ToPlatform());

            var imgStatus = _fragment.View.FindViewById<ImageView>(MAUI.Clinical6.Resource.Id.fingerprint_icon);
            imgStatus.SetImageResource(MAUI.Clinical6.Resource.Drawable.gui_fill_warning);

            if (errMsgId == 1024)
            {
                return;
            }

            await Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                SetResult(false, errorMessage);
            });
        }

        void ReportAuthenticationFailed()
        {
            SetResult(false, "Authentication Failed");
        }

        void SetResult(bool auth, string errorMessage = "")
        {
            _result.isAutheticated = auth;
            _result.ErrorMessage = errorMessage;

            _fragment.Dismiss();
        }
    }
}