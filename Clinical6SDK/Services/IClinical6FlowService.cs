using System.Collections.Generic;
using System.Threading.Tasks;

using Clinical6SDK.Models;
using Clinical6SDK.Services.Requests;
using Clinical6SDK.Services.Responses;

namespace Clinical6SDK.Services
{
    public interface IClinical6FlowService : IClinical6Service
    {
        Task<bool> CollectAsync(Flow flow, List<string> options);
        Task<CollectFieldsResponse> CollectFieldsAsync(Flow flow, List<FlowField> fields = null, List<string> options = null);


        Task<Flow> GetFlowAsync(string id, uint page = 1, uint perPage = 0);
        Task<GetInputDataByFlowResponse> GetInputDataByFlowAsync(Flow flow);
        Task<string> GetInputDataByIdAsync(string inputId);
        Task<bool> TransitionAsync(Flow flow, string transition, Dictionary<string, string> options = null);



        Task<T> Collect<T>(IParams parameters, string Json = null,  Options options = null);
        Task<T> CollectFields<T>(string jsonPar = null, string fields = null,  Options options = null);
        Task<T> InsertKeyValue<T>(IParams parameters, string Json = null,  Options options = null);
    }
}
