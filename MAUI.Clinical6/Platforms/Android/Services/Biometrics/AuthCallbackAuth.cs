using System;
using Android.Hardware.Biometrics;
using Android.Runtime;
using Android.Util;
using Java.Lang;
using Javax.Crypto;

namespace Xamarin.Forms.Clinical6.Android.Services
{
    public class AuthCallbackAuth : BiometricPrompt.AuthenticationCallback
    {
        // Can be any byte array, keep unique to application.
        static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        // The TAG can be any string, this one is for demonstration.
        static readonly string TAG = "X:" + typeof(AuthCallbackAuth).Name;

        private Action<bool, string> _onAuthComplete = null;

        public AuthCallbackAuth(Action<bool, string> OnAuthComplete)
        {
            _onAuthComplete = OnAuthComplete;
        }

        public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
        {
            if (result.CryptoObject.Cipher != null)
            {
                try
                {
                    // Calling DoFinal on the Cipher ensures that the encryption worked.
                    byte[] doFinalResult = result.CryptoObject.Cipher.DoFinal(SECRET_BYTES);

                    // No errors occurred, trust the results.

                    _onAuthComplete?.Invoke(true, null);
                }
                catch (BadPaddingException bpe)
                {
                    // Can't really trust the results.
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + bpe);

                    _onAuthComplete?.Invoke(false, "Failed to encrypt the data with the generated key." + bpe);
                }
                catch (IllegalBlockSizeException ibse)
                {
                    // Can't really trust the results.
                    Log.Error(TAG, "Failed to encrypt the data with the generated key." + ibse);

                    _onAuthComplete?.Invoke(false, "Failed to encrypt the data with the generated key." + ibse);
                }
            }
            else
            {
                // No cipher used, assume that everything went well and trust the results.
                _onAuthComplete?.Invoke(true, null);
            }
        }

        //public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
        //{
        //    // Report the error to the user. Note that if the user canceled the scan,
        //    // this method will be called and the errMsgId will be FingerprintState.ErrorCanceled.

        //    _onAuthComplete?.Invoke(false, errString?.ToString());
        //}

        public override void OnAuthenticationFailed()
        {
            // Tell the user that the fingerprint was not recognized.
            _onAuthComplete?.Invoke(false, "Authrntication Failed");
        }

        //public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
        //{
        //    // Notify the user that the scan failed and display the provided hint.
        //    _onAuthComplete?.Invoke(false, helpString.ToString());
        //}

        public override void OnAuthenticationError([GeneratedEnum] BiometricErrorCode errorCode, ICharSequence? errString)
        {
            //base.OnAuthenticationError(errorCode, errString);
            // Report the error to the user. Note that if the user canceled the scan,
            // this method will be called and the errMsgId will be FingerprintState.ErrorCanceled.

            _onAuthComplete?.Invoke(false, errString?.ToString());
        }

        public override void OnAuthenticationHelp([GeneratedEnum] BiometricAcquiredStatus helpCode, ICharSequence? helpString)
        {
            // Notify the user that the scan failed and display the provided hint.
            _onAuthComplete?.Invoke(false, helpString.ToString());
        }
    }
}
