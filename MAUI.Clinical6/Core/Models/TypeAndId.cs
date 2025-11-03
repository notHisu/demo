using Newtonsoft.Json;

namespace Xamarin.Forms.Clinical6.Core.Models
{
    public class TypeAndId
    {
        [JsonProperty("type")]
        public string TypeName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}