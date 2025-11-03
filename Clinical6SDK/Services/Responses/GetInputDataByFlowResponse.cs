using System.Collections.Generic;
using Newtonsoft.Json;
using Clinical6SDK.Models;


namespace Clinical6SDK.Services.Responses
{
    public class GetInputDataByFlowResponse
    {
        public Dictionary<string, string> CapturedValues { get; set; }
    }
}
