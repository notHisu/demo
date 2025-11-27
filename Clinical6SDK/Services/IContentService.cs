using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Clinical6SDK.Common.Converters;
using Clinical6SDK.Models;
using Clinical6SDK.Helpers;
using Clinical6SDK.Services.Requests;
using JsonApiSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Clinical6SDK.Services
{
    public interface IContentService : IClinical6Service
    {
        T DeserializeResponse<T>(string content);
        T DeserializeResponseError<T>(string content);
        JObject ModelToJObject(IJsonApiModel obj);
        Task<List<Content>> Get(string contentType);
        Task<List<Content>> GetPublicContents(string locale);
    }
}
