using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Newtonsoft.Json;

namespace Clinical6SDK.Services
{
    public class VerificationCodeService
    {
        public async Task<VerificationCodeModel> GetHostname(String code)
        {
            var vc = new VerificationCodeModel
            {
                Code = code
            };

            var json = JsonConvert.SerializeObject(vc);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = new Uri(ClientSingleton.Instance.VerificationCodeUrl);
            var response = await ClientSingleton.Instance.HttpClient.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                var content2 = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<VerificationCodeModel>(content2);
            }
            else
            {
                return null;
            }
        }
    }
}
