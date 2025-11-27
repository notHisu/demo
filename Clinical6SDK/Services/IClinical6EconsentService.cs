using System.Threading.Tasks;
using Clinical6SDK.Helpers;

namespace Clinical6SDK.Services
{
    public interface IClinical6EConsentService : IClinical6Service
    {
        Task<string> GetConsentStrategiesStatus(User user);
    }
}
