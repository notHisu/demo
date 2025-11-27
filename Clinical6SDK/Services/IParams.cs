using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clinical6SDK.Services
{
    public interface IParams
    {
        string MobileApplicationKey { get; set; }

        string Id { get; set; }
    }
    
    public class ErrorsResonse
    {
        public string title { get; set; }

        public string detail { get; set; }
        
        public string status { get; set; }
    }

    public class Params : IParams
    {
        public string MobileApplicationKey { get; set; }

        public string Id { get; set; }
    }

    public class Error
    {
        [JsonProperty ("title")]
        public string Title { get; set; }
        //[JsonProperty ("detail")]
        public string Detail
        {
            get
            {
                return DetailField.ToString();
            }
        }

        public List<string> DetailErros
        {
            get
            {
                try
                {
                    var jsonContainer = DetailField as JContainer;
                    return jsonContainer.ToObject<List<string>>();
                }
                catch
                {

                }
                return null;
            }
        }

        [JsonProperty("detail")]
        public object DetailField { get; set; }

        [JsonProperty ("status")]
        public string Status { get; set; }
    }
    
    public class ErrorResponse
    {
        [JsonProperty ("errors")]
        public List<Error> Errors { get; set; }
    }
}
