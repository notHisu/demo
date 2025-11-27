using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Clinical6.Helpers
{
    public enum LocalAuthenticationType
    {
        None,
        Pin,
        BiometryTouchID,
        BiometryFaceID,
        BiometryTouchID_Or_Fingerprint
    }

    public interface IFingerprintAuth
    {
        Task<bool> CanAuthenticate(bool devicePassword = false);

        Task<FingerprintAuthenticatedResult> GetAuthenticateAsync(string reason, bool devicePassword = false, CancellationToken cancellationToken = default(CancellationToken));
    }

    public class FingerprintAuthenticatedResult
    {
        public FingerprintAuthenticatedResult()
        {
            isAutheticated = false;
            ErrorCode = -1;
            ErrorMessage = "";
        }
        public bool isAutheticated { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public bool IsSuccess { get; private set; } = false;
        public void SetSuccess() => IsSuccess = true;
        public void SetFailed() => IsSuccess = false;

    }
}
