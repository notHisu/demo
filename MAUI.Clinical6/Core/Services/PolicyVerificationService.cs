using Clinical6SDK.Helpers;
using Clinical6SDK.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface IPolicyVerificationService
    {
        Task<bool> VerifyPolicyDocuments(User mobileUser);
    }

    public class PolicyVerificationService : IPolicyVerificationService
    {
        Clinical6SDK.Services.IContentService _contentService = new ContentService();

        public async Task<bool> VerifyPolicyDocuments(User mobileUser)
        {
            if (mobileUser == null)
                return false;

            var languageService = LanguageService.Instance;
            var result = await _contentService.GetPublicContents(languageService.CurrentLanguage?.Iso?.ToLower());
            if (!result.Any())
                return false;

            bool policiesValid = false;
            var privacyPolices = result.Where(x => x.ContentType.PermanentLink == "policy");
            foreach (var policy in privacyPolices)
            {
                policiesValid = IsUserAcceptedDateValid(policy, mobileUser.PrivacyPolicyAcceptedAt);
                if (policiesValid == false)
                    break;
            }

            return policiesValid;
        }

        private bool IsUserAcceptedDateValid(Content dynamicContent, DateTime? userAcceptedDate)
        {
            if (dynamicContent == null)
                return false;

            if (!userAcceptedDate.HasValue)
                return false;

            var documentDate = DateTime.Parse(dynamicContent["updated_at"].ToString());
            var localDateTimeUserAcceptanceDate = userAcceptedDate.Value.ToLocalTime();

            if (localDateTimeUserAcceptanceDate > documentDate)
                return true;

            return false;
        }
    }
}
