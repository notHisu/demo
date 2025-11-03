using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinical6SDK.Common.Exceptions;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services
{
    //http://clinical6-docs.s3-website-us-east-1.amazonaws.com/apidoc/v3consentstrategies/index.html
    public class Clinical6EConsentService: JsonApiHttpService, IClinical6EConsentService
    {
        public async Task<string> GetConsentStrategiesStatus(User user)
        {
            if (user == null)
                throw new Exception("User is required for GetConsentStrategiesStatus");
            if (user.SiteMember == null || user.SiteMember.Id <= 0)
                throw new Exception("Site Member is required for GetConsentStrategiesStatus");

            var siteMember = await Get<SiteMember>(user.SiteMember);

            var consentStatus = string.IsNullOrEmpty(siteMember.ConsentStatus) ? "consent_initial" : siteMember.ConsentStatus;

            return consentStatus;
        }

    }
}
