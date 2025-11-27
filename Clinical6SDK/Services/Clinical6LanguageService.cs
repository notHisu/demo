using Clinical6SDK.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Clinical6SDK.Services
{
    public class Clinical6LanguageService : JsonApiHttpService, IClinical6LanguageService
    {
        /// <summary>
        /// Gets the translations v2.
        /// </summary>
        /// <returns>The translations v2.</returns>
        /// <param name="language">Language, needs valid Iso</param>
        /// <param name="iso">Iso.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <example>
        /// // Inject the handler or client into your service code
        /// var service = new Clinical6LanguageService();
        /// service._httpMessageHandler = mockHttp;
        /// service.BaseUrl = "https://validation.clinical6.com";
        /// var response = await service.GetTranslationsV2<Dictionary<string, string>>( new Language { Iso = "en" });
        /// 
        /// string backButtonText = response["BACK"];
        /// </example>
        public async Task<T> GetTranslationsV2<T>(Language language)
        {
            string _iso = language?.Iso;

            var path = string.Format("{0}/{1}", Constants.ApiRoutes.Languages.LANGUAGES_V2, _iso);

            var response = await Send(path, (content) =>
            {
                var translations = JObject.Parse(content);
                var test = translations.GetValue("translations");
                var testString = test.ToString(Formatting.None);
                return JsonConvert.DeserializeObject<T>(testString);
            },
            (errorMessage) => JObject.Parse(errorMessage));

            return response.Data;
        }
    }
}