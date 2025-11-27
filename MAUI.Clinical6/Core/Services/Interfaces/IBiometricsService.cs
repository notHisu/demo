using System;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Helpers;

namespace Xamarin.Forms.Clinical6.Services
{
    public interface IBiometricsService
    {
        /// <summary>
        /// Can Authentication With
        /// </summary>
        /// <returns></returns>
        LocalAuthenticationType CanAuthenticationWith();

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="OnAuthComplete"></param>
        void AuthenticateUser(Action<bool, string> OnAuthComplete);

        /// <summary>
        /// Indicate is the device can sue Biometrics
        /// </summary>
        /// <param name="devicePassword"></param>
        /// <returns></returns>
        Task<bool> CanAuthenticate(bool devicePassword = false);
    }
}
