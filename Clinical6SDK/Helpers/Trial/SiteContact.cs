using System;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services;

namespace Clinical6SDK.Helpers
{
    public class SiteContact : SiteContactModel
    {
        public async Task<SiteContact> Save()
        {
            var service = new JsonApiHttpService();
            return Id != null && Id > 0
                ? await service.Update<SiteContact>(this)
                : await service.Insert<SiteContact>(this);
        }
    }
}
