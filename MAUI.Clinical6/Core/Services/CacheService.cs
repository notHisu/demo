using Akavache;
using Clinical6SDK.Models;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Core.Models;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface ICacheService
    {
        /// <summary>
        /// For saving sensitive data - like credentials.
        /// </summary>
        ISecureBlobCache Secure { get; set; }

        /// <summary>
        /// A database, kept in memory. The data is stored for the lifetime of the app and cleared on app restart
        /// </summary>
        ISecureBlobCache InMemory { get; set; }

        /// <summary>
        /// Local cached app data that can get deleted automatically if running out of space and may get deleted without notification.
        /// </summary>
        IBlobCache LocalData { get; set; }

        /// <summary>
        /// Primarily for user settings and can get automatically backed up to the cloud. Don't save sensitive data here.
        /// </summary>
        IBlobCache UserAccount { get; set; }

        /// <summary>
        /// Clears all cached data including Secure, InMemory, LocalData, and UserAccount
        /// </summary>
        /// <returns></returns>
        Task ClearAll();
        Task ClearSecure();
        Task ClearInMemory();
        Task ClearLocalData();
        Task ClearUserAccount();

        /// <summary>
        /// Critical to the integrity of your Akavache cache and must be called when your application shuts down
        /// </summary>
        /// <returns></returns>
        Task Shutdown();
    }

    public class CacheService : ICacheService
    {
        public ISecureBlobCache Secure
        {
            get => BlobCache.Secure;
            set => BlobCache.Secure = value;
        }

        public ISecureBlobCache InMemory
        {
            get => BlobCache.InMemory;
            set => BlobCache.InMemory = value;
        }

        public IBlobCache LocalData
        {
            get => BlobCache.LocalMachine;
            set => BlobCache.LocalMachine = value;
        }

        public IBlobCache UserAccount
        {
            get => BlobCache.UserAccount;
            set => BlobCache.UserAccount = value;
        }

        public async Task ClearAll()
        {
            var cachesCleared = Secure.InvalidateAll().Merge(InMemory.InvalidateAll()).Merge(LocalData.InvalidateAll()).Merge(UserAccount.InvalidateAll());
            await cachesCleared;
        }

        public async Task ClearSecure()
        {
            await Secure.InvalidateAll();
        }

        public async Task ClearInMemory()
        {
            await InMemory.InvalidateAll();
        }

        public async Task ClearLocalData()
        {
            await LocalData.InvalidateAll();
        }

        public async Task ClearUserAccount()
        {
            await UserAccount.InvalidateAll();
        }

        public async Task Shutdown()
        {
            await BlobCache.Shutdown();
        }
    }

    public static class CacheExtensions
    {
        public static async Task SaveAuthToken(this ICacheService cacheService, string authKey)
        {
            await cacheService.Secure.InsertObject(CacheKey.AuthToken, authKey);
        }

        public static async Task ClearAuthToken(this ICacheService cacheService)
        {
            await cacheService.Secure.InsertObject<string>(CacheKey.AuthToken, null);
        }

        public static async Task SaveDeviceIdAndAuthToken(this ICacheService cacheService, DataModel<DevicesResponse> devicesResponse)
        {
            await cacheService.SaveAuthToken(devicesResponse.Attributes.AccessToken);
            await cacheService.SaveDeviceId(devicesResponse.Id);
        }

        public static async Task SaveDeviceId(this ICacheService cacheService, int deviceId)
        {
            await cacheService.Secure.InsertObject(CacheKey.DeviceId, deviceId);
        }

        public static async Task SaveRegisteredEmail(this ICacheService cacheService, string email)
        {
            await cacheService.Secure.InsertObject(CacheKey.RegisteredEmail, email);
        }

        public static async Task SaveNewEmail(this ICacheService cacheService, string email)
        {
            await cacheService.Secure.InsertObject(CacheKey.NewMail, email);
        }

        public static async Task SaveNumberOfExpectedFlowProcesses(this ICacheService cacheService, int numberOfFlowProcesses)
        {
            await cacheService.LocalData.InsertObject(CacheKey.NumberOfExpectedFlowProcesses, numberOfFlowProcesses);
        }

        public static async Task SaveHasPin(this ICacheService cacheService, bool hasPin)
        {
            await cacheService.Secure.InsertObject(CacheKey.UserHasPin, hasPin);
        }

        public static async Task SaveTermsOfUseAccepted(this ICacheService cacheService, bool accepted)
        {
            await cacheService.Secure.InsertObject(CacheKey.TermsOfUseAccepted, accepted);
        }

        public static async Task SaveVerificationCode(this ICacheService cacheService, string code)
        {
            await cacheService.Secure.InsertObject(CacheKey.VerificationCode, code);
        }

        public static async Task SaveUserId(this ICacheService cacheService, int? id)
        {
            await cacheService.Secure.InsertObject(CacheKey.UserId, id);
        }

        public static async Task SaveSiteMemberId(this ICacheService cacheService, int? id)
        {
            await cacheService.Secure.InsertObject(CacheKey.SiteMemberId, id);
        }

        public static async Task SaveStudySettings(this ICacheService cacheService, List<AppSettings> studySettings)
        {
            await cacheService.Secure.InsertObject(CacheKey.StudySettings, studySettings);
        }

        public static async Task SaveStudyLogo(this ICacheService cacheService, string url)
        {
            await cacheService.Secure.InsertObject(CacheKey.StudyLogo, url);
        }

        public static async Task SaveAPIStudySettings(this ICacheService cacheService, List<AppSettings> studySettings)
        {
            await cacheService.Secure.InsertObject(CacheKey.APIStudySettings, studySettings);
        }

        public static async Task SavePushToken(this ICacheService cacheService, string pushToken)
        {
            await cacheService.Secure.InsertObject(CacheKey.PushToken, pushToken);
        }

        public static async Task SaveVideoSession(this ICacheService cacheService, string VideoSessionIdentifier)
        {
            await cacheService.Secure.InsertObject(CacheKey.VideoSession, VideoSessionIdentifier);
        }

        public static async Task SaveDateVideoSession(this ICacheService cacheService, string VideoSessionStartAt)
        {
            await cacheService.Secure.InsertObject(CacheKey.VideoSessionStartAt, VideoSessionStartAt);
        }

        public static async Task SaveUploadedPushToken(this ICacheService cacheService, string pushToken)
        {
            await cacheService.Secure.InsertObject(CacheKey.UploadedPushToken, pushToken);
        }

        public static async Task SaveUserCulture(this ICacheService cacheService, string culture)
        {
            await cacheService.Secure.InsertObject(CacheKey.UserCulture, culture);
        }

        public static async Task SaveCurrentLanguage(this ICacheService cacheService, Language language)
        {
            await cacheService.UserAccount.InsertObject(CacheKey.CurrentLanguage, language);
        }

        public static async Task SaveSupportedLanguages(this ICacheService cacheService, IList<Language> languages)
        {
            await cacheService.UserAccount.InsertObject(CacheKey.SupportedLanguages, languages);
        }

        public static async Task SaveTranslations(this ICacheService cacheService, IDictionary<string, string> translations)
        {
            await cacheService.UserAccount.InsertObject(CacheKey.Translations, translations);
        }

        public static async Task<int> GetDeviceId(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject(CacheKey.DeviceId, () => 0);
        }

        public static async Task<List<AppSettings>> GetStudySettings(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject(CacheKey.StudySettings, () => new List<AppSettings>());
        }

        public static async Task<List<AppSettings>> GetAPIStudySettings(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject(CacheKey.APIStudySettings, () => new List<AppSettings>());
        }

        public static async Task<string> GetStudyLogo(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject(CacheKey.StudyLogo, () => string.Empty);
        }

        public static async Task<string> GetAuthToken(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.AuthToken, () => null);
        }

        public static async Task<int> NumberOfExpectedFlowProcesses(this ICacheService cacheService)
        {
            return await cacheService.LocalData.GetOrCreateObject(CacheKey.NumberOfExpectedFlowProcesses, () => 27);
        }

        public static async Task<string> GetPushToken(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.PushToken, () => null);
        }

        public static async Task<string> GetUploadedPushToken(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.UploadedPushToken, () => null);
        }

        public static async Task<string> GetRegisteredEmail(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.RegisteredEmail, () => null);
        }

        public static async Task<string> GetNewEmail(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.NewMail, () => null);
        }

        public static async Task<string> GetUserCulture(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.UserCulture, () => null);
        }

        public static async Task<Language> GetCurrentLanguage(this ICacheService cacheService)
        {
            return await cacheService.UserAccount.GetOrCreateObject<Language>(CacheKey.CurrentLanguage, () => null);
        }

        public static async Task<List<Language>> GetSupportedLanguages(this ICacheService cacheService)
        {
            return await cacheService.UserAccount.GetOrCreateObject<List<Language>>(CacheKey.SupportedLanguages, () => null);
        }

        public static async Task<Dictionary<string, string>> GetTranslations(this ICacheService cacheService)
        {
            return await cacheService.UserAccount.GetOrCreateObject<Dictionary<string, string>>(CacheKey.Translations, () => null);
        }

        public static async Task<string> GetVerificationCode(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.VerificationCode, () => null);
        }

        public static async Task<bool> GetHasPin(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<bool>(CacheKey.UserHasPin, () => false);
        }

        public static async Task<int?> GetUserId(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<int?>(CacheKey.UserId, () => null);
        }

        public static async Task<int?> GetSiteMemberId(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<int?>(CacheKey.SiteMemberId, () => null);
        }

        public static async Task<bool> GetTermsOfUseAccepted(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<bool>(CacheKey.TermsOfUseAccepted, () => false);
        }

        public static async Task<string> GetVideoConsultationSessionIdentifier(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.VideoSession, () => string.Empty);
        }

        public static async Task<string> GetVideoSessionStartAt(this ICacheService cacheService)
        {
            return await cacheService.Secure.GetOrCreateObject<string>(CacheKey.VideoSessionStartAt, () => string.Empty);
        }
    }


    public static class CacheKey
    {
        public const string AuthToken = "AuthToken";
        public const string DeviceId = "DeviceId";
        public const string RegisteredEmail = "RegisteredEmail";
        public const string NumberOfExpectedFlowProcesses = "ExpectedFlowProcesses";
        public const string PushToken = "PushToken";
        public const string UploadedPushToken = "UploadedPushToken";
        public const string UserHasPin = "UserHasPin";
        public const string UserId = "UserId";
        public const string SiteMemberId = "SiteMemberId";
        public const string VerificationCode = "VerificationCode";
        public const string TermsOfUseAccepted = "TermsOfUseAccepted";
        public const string NewMail = "NewMail";
        public const string UserLanguage = "UserLanguage";
        public const string StudySettings = "StudySettings";
        public const string StudyLogo = "StudyLogo";
        public const string APIStudySettings = "APIStudySettings";
        public const string VideoSession = "VideoSession";
        public const string VideoSessionStartAt = "VideoSessionStartAt";
        public const string UserCulture = "UserCulture";
        public const string CurrentLanguage = "CurrentLanguage";
        public const string SupportedLanguages = "SupportedLanguages";
        public const string Translations = "Translations";
    }
}