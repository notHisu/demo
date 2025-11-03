using Clinical6SDK.Models;
using System.Threading.Tasks;

namespace Clinical6SDK.Services
{
    public interface IClinical6LanguageService : IClinical6Service
    {
        Task<T> GetTranslationsV2<T>(Language language);
    }
}