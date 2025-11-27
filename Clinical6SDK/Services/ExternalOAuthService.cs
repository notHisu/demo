using System.Threading.Tasks;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;
using Clinical6SDK.Services.Requests;

namespace Clinical6SDK.Services
{
    public class ExternalOAuthService : JsonApiHttpService, IExternalOAuthService
    {
        public string ExternalSystemName { get; set; } = "";

        /// <summary>
        /// Gets the Authorization Url
        /// </summary>
        /// <returns></returns>
        public async Task<AuthorizationTokenModel> GetAuthorizationUrl()
        {
            Options options = new Options { Url = string.Format(Constants.ExternalOAuth.AUTHORIZE, ExternalSystemName) };
            return await Get<AuthorizationTokenModel>(options);
        }

        /// <summary>
        /// Get the Verification (if the refresh token exists)
        /// </summary>
        /// <returns></returns>
        public async Task<AuthorizationTokenModel> GetVerification()
        {
            Options options = new Options { Url = string.Format(Constants.ExternalOAuth.VERIFY, ExternalSystemName) };
            return await Get<AuthorizationTokenModel>(options);
        }
    }
}
