using System.Collections.Generic;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services.Requests;

namespace Clinical6SDK.Services
{
    /// <summary>
    /// Clinical6 SSOO ptions service.
    /// </summary>
    public class Clinical6SSOOptionsService : JsonApiHttpService, IClinical6SSOOptionsService
    {
        /// <summary>
        /// Gets the SSOP roviders async.
        /// </summary>
        /// <returns>The SSOP roviders async.</returns>
        public async Task<List<SsoOptions>> GetSSOProvidersAsync()
        {
            var path = Constants.SSOProviders.SSOOPIONS;

            var options = new Options { Url = path };

            return await Get<List<SsoOptions>>(options);
        }
    }
}
