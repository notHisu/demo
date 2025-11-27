using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services
{
    public interface IClinical6SSOOptionsService : IClinical6Service
    {
        Task<List<SsoOptions>> GetSSOProvidersAsync();
    }
}
