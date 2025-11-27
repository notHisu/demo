using System;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services;

namespace Clinical6SDK.Helpers
{
    public class Device : DeviceModel
    {
        public async Task<Device> Save()
        {
            var service = new JsonApiHttpService();
            return Id != null && Id > 0
                ? await service.Update<Device>(this)
                : await service.Insert<Device>(this);
        }
    }
}
