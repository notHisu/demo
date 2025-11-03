using System.Threading.Tasks;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services
{
    public interface IExternalOAuthService : IClinical6Service
    {
        Task<AuthorizationTokenModel> GetAuthorizationUrl();
        Task<AuthorizationTokenModel> GetVerification();
    }
}
