using Clinical6SDK.Models;
using Clinical6SDK.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface ILanguageService
    {
        Language CurrentLanguage { get; }
        Task<IList<Language>> GetLanguages();
        Task<IDictionary<string, string>> GetTranslations(Language language);
        string Translate(string key);
    }

    public class LanguageService : ILanguageService
    {
        readonly Clinical6LanguageService _clinical6LanguageService;
        readonly ICacheService _cacheService;

        Dictionary<string, string> _translations;

        public static LanguageService Instance { get; private set; }

        public Language CurrentLanguage { get; private set; }

        public LanguageService(IClinical6LanguageService clinical6LanguageService, ICacheService cacheService)
        {
            _clinical6LanguageService = (Clinical6LanguageService)clinical6LanguageService;
            _cacheService = cacheService;

            Instance = this;
        }

        public async Task<IList<Language>> GetLanguages()
        {
            var languages = await _clinical6LanguageService.Get<List<Language>>();
            if (languages != null)
            {
                await _cacheService.SaveSupportedLanguages(languages);
                return languages;
            }
            else
            {
                languages = await _cacheService.GetSupportedLanguages();
                if (languages != null)
                {
                    return languages;
                }
            }

            return null;
        }

        public async Task<IDictionary<string, string>> GetTranslations(Language language)
        {
            _translations = await _clinical6LanguageService.GetTranslationsV2<Dictionary<string, string>>(language);
            if (_translations != null)
            {
                await _cacheService.SaveTranslations(_translations);
                SetCurrentLanguage(language);
                return _translations;
            }
            else
            {
                _translations = await _cacheService.GetTranslations();
                if (_translations != null)
                {
                    CurrentLanguage = await _cacheService.GetCurrentLanguage();
                    return _translations;
                }
                else
                {
                    SetCurrentLanguage(language);
                    return null;
                }
            }
        }

        public string Translate(string key)
        {
            if (_translations != null)
            {
                string translation;
                if (_translations.TryGetValue(key, out translation))
                {
                    return translation;
                }
            }

            return string.Empty;
        }

        public async void SetCurrentLanguage(Language language)
        {
            CurrentLanguage = language;
            await _cacheService.SaveCurrentLanguage(language);
        }
    }
}