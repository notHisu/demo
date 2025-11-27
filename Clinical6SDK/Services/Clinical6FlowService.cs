using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

using Clinical6SDK;
using Clinical6SDK.Services.Responses;
using Clinical6SDK.Services.Requests;
using Clinical6SDK.Models;
using JsonApiSerializer;
using System.Reflection;
using Clinical6SDK.Common.Exceptions;
using Newtonsoft.Json.Linq;

namespace Clinical6SDK.Services
{
    public class Clinical6FlowService : JsonApiHttpService, IClinical6FlowService
    {
        /// <summary>
        /// Collects data from the flow process via a id
        /// </summary>
        /// <returns>The collect.</returns>
        /// <param name="flow">Flow.</param>
        /// <param name="options">Options.</param>
        public async Task<bool> CollectAsync(Flow flow, List<string> options)
        {
            if (!HasToken)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }
            if (flow == null)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWNULL);
            }

            if (flow.Id <= 0)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWIDREQUIRED);
            }

            // What abot fields
            if (flow.Fields == null || flow.Fields.Count == 0)
            {
                throw new ArgumentException("fields are required.");
            }

            return (await CollectFieldsAsync(flow, flow.Fields as List<FlowField>, options)) != null;
        }

        /// <summary>
        /// Collects data outside the flow process.  This is used for per step saves.
        /// </summary>
        /// <returns>The fields.</returns>
        /// <param name="flow">Flow.</param>
        /// <param name="fields">Fields.</param>
        /// <param name="options">Options.</param>
        public async Task<CollectFieldsResponse> CollectFieldsAsync(Flow flow, List<FlowField> fields = null, List<string> options = null)
        {
            if (!HasToken)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }
            if (flow == null)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWNULL);
            }

            if (flow.Id <= 0)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWIDREQUIRED);
            }

            // bool ret = false;

            if (flow.Entry != null) // v3 with entry
            {
                Dictionary<int, string> attributes = new Dictionary<int, string>();
                if (fields != null && fields.Count > 0)
                {
                    foreach (var f in fields)
                    {
                        attributes.Add(f.InputId, f.Value);
                    }
                }

                var path = string.Format(Constants.ApiRoutes.Flow.FLOW_PROCESS_VALUES, flow.Entry.Id);

                var relationships = new CollectFieldsRelationship
                {
                    FlowProcess = new Relationship<FlowProcessRelationshipData>
                    {
                        Data = new FlowProcessRelationshipData
                        {
                            Id = flow.Id
                        }
                    },
                    OwnerFlowProcess = new Relationship<UserelationshipData>
                    {
                        Data = new UserelationshipData()
                        {
                            Id = Convert.ToInt32(flow.Owner)
                        }
                    }
                };

                var postData = new DataAttributesRequest<CollectFieldsRequest, CollectFieldsRelationship>
                {
                    DataAttributes = new DataAttributes<CollectFieldsRequest, CollectFieldsRelationship>
                    {
                        Type = "data_collection__flow_process_values",
                        Attributes = new CollectFieldsRequest // checar
                        {
                            Attributes = attributes
                        },
                        Relationships = relationships
                    }
                };

                var urlOptions = new Options { Url = path };

                return await base.Insert<CollectFieldsResponse>(postData, urlOptions);
            }
            else
            {
                // v2
                // throw new Exception("Flow must have entry for v3");
                return null;
            }
        }

        /// <summary>
        /// Gets the flow process from a id.
        /// </summary>
        /// <returns>The flow.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="page">Page.</param>
        /// <param name="perPage">Per page.</param>
        public async Task<Flow> GetFlowAsync(string id, uint page = 1, uint perPage = 0)
        {
            if (!HasToken)
            {
                throw new Clinical6UnauthorizedException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("permalink, Flow id is required");
            }

            string httpQuery = string.Format("?page={0}&per_page={1}", page, perPage); // ???????
            httpQuery = ""; // ?????????????

            var path = string.Format(Constants.ApiRoutes.Flow.FLOW_PROCESS, id);

            var url = string.Format("{0}{1}", path, httpQuery);

            var response = await Send(
                url,
                DeserializeResponse<Flow>,
                DeserializeResponseError<ResponseError>);


            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Gets input data for a flow.
        /// </summary>
        /// <returns>The input data by flow.</returns>
        /// <param name="flow">Flow.</param>
        public async Task<GetInputDataByFlowResponse> GetInputDataByFlowAsync(Flow flow)
        {
            if (!HasToken)
            {
                throw new Clinical6UnauthorizedException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }
            if (flow == null)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWNULL);
            }
            if (flow.Id <= 0)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWIDREQUIRED);
            }

            string id = flow.PermanentLink;

            Dictionary<string, string> parms = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(flow.Options.Owner))
            {
                parms.Add("owner", flow.Options.Owner);
            }
            if (!string.IsNullOrWhiteSpace(flow.Options.OwnerType))
            {
                parms.Add("owner_type", flow.Options.OwnerType);
            }
            if (!string.IsNullOrWhiteSpace(flow.Options.ExistingId))
            {
                parms.Add("existing_id", flow.Options.ExistingId);
            }
            if (flow.Entry != null) // ??????? //&& flow.Entry.captured_value_group && flow.Entry.captured_value_group.id)
            {
                // ???????
                // parms.Add("captured_value_group_id", flow.Entry.captured_value_group.id);
            }

            var path = string.Format(Constants.ApiRoutes.Flow.FLOW_PROCESS_INPUT_DATA, id);

            var response = await Send(
                path,
                DeserializeResponse<Dictionary<string, string>>,
                DeserializeResponseError<ResponseError>,
                requestData: parms);

            var resp = new GetInputDataByFlowResponse
            {
                CapturedValues = new Dictionary<string, string>()
            };

            if (response != null && response.IsResponseSuccessful && response.Data != null)
            {
                foreach (var c in response.Data)
                {
                    resp.CapturedValues.Add(c.Key, c.Value);
                }
            }

            return resp;
        }

        /// <summary>
        /// Gets the input data for a specific input id.
        /// </summary>
        /// <returns>The input data by identifier.</returns>
        /// <param name="inputId">Input identifier.</param>
        public async Task<string> GetInputDataByIdAsync(string inputId)
        {
            if (!HasToken)
            {
                throw new Clinical6UnauthorizedException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }
            if (!string.IsNullOrWhiteSpace(inputId))
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOW_INPUTID);
            }

            var path = string.Format(Constants.ApiRoutes.Flow.FLOW_PROCESS_INPUT_DATA_BYID, inputId);

            var response = await Send<string, ResponseError>(
                path,
                DeserializeResponse<string>,
                DeserializeResponseError<ResponseError>);


            return response.IsResponseSuccessful ? response.Data : string.Empty;
        }

        /// <summary>
        /// Transitions flow from one status to another.
        /// </summary>
        /// <returns>The transition.</returns>
        /// <param name="flow">Flow.</param>
        /// <param name="transition">Transition.</param>
        /// <param name="options">Options.</param>
        public async Task<bool> TransitionAsync(Flow flow, string transition, Dictionary<string, string> options = null)
        {
            if (!HasToken)
            {
                throw new Clinical6UnauthorizedException("No token or token must be valid");
            }
            if (flow == null)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWNULL);
            }

            if (flow.Id <= 0)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWIDREQUIRED);
            }
            if (string.IsNullOrWhiteSpace(transition))
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOW_TRANSITIONNULL);
            }


            var path = string.Format(Constants.ApiRoutes.Flow.FLOW_PROCESS_TRANSITION, flow.PermanentLink);
            var url = new Uri(string.Format("{0}{1}", BaseUrl, path));


            var transitionRequestData = new TransitionRequest
            {
                OptionsTransition = new OptionsTransition
                {
                    Options = new Dictionary<string, string>()
                },
                Object = flow.PermanentLink,
                ObjectType = "data_collection/flow_process",
                Transition = transition
            };


            if (options != null)
            {
                foreach (var opt in options)
                {
                    transitionRequestData.OptionsTransition.Options.Add(opt.Key, opt.Value);
                }
            }


            var urlOptions = new Options { Url = path };

            return await Insert<bool>(transitionRequestData, urlOptions);
        }





        public async Task<T> Collect<T>(IParams parameters, string Json = null, Options options = null)
        {
            if (!HasToken)
            {
                throw new Clinical6UnauthorizedException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }

            AreParamsValid(new List<string>() { "id", "fields" }, parameters, Json);

            string jsonPar = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                jsonPar = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings());
                //_parameters = parameters.GetType().GetRuntimeProperties().AsDictionary(parameters);
                _parameters = ConvertToDictionary(JObject.Parse(jsonPar).Descendants());
            }
            else
            {
                jsonPar = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings());
            }
            dynamic json = JsonConvert.DeserializeObject(jsonPar, new JsonSerializerSettings());

            string _fields = JsonConvert.SerializeObject(_parameters["fields"], new JsonApiSerializerSettings());

            return await CollectFields<T>(jsonPar, _fields, options);
        }


        public async Task<T> CollectFields<T>(string jsonPar = null, string fields = null, Options options = null)
        {
            string pars = string.Empty;
            if (!HasToken)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }
            dynamic json = JsonConvert.DeserializeObject(jsonPar, new JsonSerializerSettings());
            int? id = json?.id;
            if (id <= 0)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.FLOWIDREQUIRED);
            }
            string owner = json?.owner;
            if (string.IsNullOrWhiteSpace(owner))
            {
                owner = string.Empty;
            }
            var entry = json?.entry;
            entry = entry == null ? json?.data?.attributes?.entry : null;

            //dynamic _fieldsArray = JsonConvert.DeserializeObject(fields, new JsonSerializerSettings());
            //var _fields = _fieldsArray?.fields;

            if (entry != null) // v3 with entry
            {

                /*Dictionary<int, string> attributes = new Dictionary<int, string>();
                if (_fields != null && _fields.Count > 0)
                {
                    foreach (var f in fields)
                    {
                        // attributes.Add(f.InputId, f.Value);
                    }
                }*/

                pars = @"{
                    'data' : {
                            'type' : 'data_collection__flow_process_values'," +

                                           @"'relationships' : {
                                'flow_process' : {
                                    'data' : {
                                        'id'   : " + id.ToString() + @",
                                        'type' : 'data_collection__flow_processes'
                                    }
                                },
                                'owner' : {
                                    'data' : {
                                        'id'   : " + owner + @",
                                        'type' : 'mobile_users'
                                    }
                                }
                            }
                    }
                }";
                var path = string.Format(Constants.ApiRoutes.Flow.FLOW_PROCESS_VALUES, id);

                //return await Send<T>(path, httpMethod: HttpMethod.Post, requestData: pars);

                var response = await Send(
                    path,
                    (content) => JsonConvert.DeserializeObject<T>(content, new JsonApiSerializerSettings()),
                    (errMsg) => JsonConvert.DeserializeObject<ErrorResponse>(errMsg, new JsonApiSerializerSettings()),
                    HttpMethod.Post,
                    requestData: pars);

                return response.IsResponseSuccessful ? response.Data : JsonConvert.DeserializeObject<T>(string.Empty);
            }
            else
            {
                // v2
                throw new Exception("Flow must have entry for v3");
            }
        }

        public async Task<T> InsertKeyValue<T>(IParams parameters, string Json = null, Options options = null)
        {
            if (!HasToken)
            {
                throw new ArgumentException(Constants.ExceptionsMessages.TOKEN_REQUIRED);
            }
            string jsonPar = string.Empty;
            if (parameters != null)
            {
                jsonPar = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings());
            }
            else
            {
                jsonPar = Json;
            }
            dynamic json = JsonConvert.DeserializeObject(jsonPar, new JsonSerializerSettings());

            Flow flow = json?.flow;
            int? flowId = json?.Flow?.Id;
            string type = json?.type;
            if (flow == null)
            {
                throw new ArgumentException("Flow is required.");
            }
            if (flowId == null)
            {
                throw new ArgumentException("Flow Id is required.");
            }
            if (flow == null)
            {
                throw new ArgumentException("Flow Type is required.");
            }
            string owner = json?.owner;
            if (string.IsNullOrWhiteSpace(owner))
            {
                // owner = Client.Instance.User;
            }

            string url = Constants.ApiRoutes.Flow.FLOWS;

            var entry = json?.entry;
            if (entry != null)
            {
                int? entryId = entry?.entry?.id;
                if (entryId == null)
                {
                    throw new ArgumentException("Entry id is required.");
                }
                url = string.Format(Constants.ApiRoutes.Flow.FLOW_PROCESS_VALUES, entryId);
            }

            options.Url = url;

            var response = await new GenericJsonHttpService().Insert<T>(json, options);

            if (response.Success)
            {
                return response.Response;
            }
            else
                return default(T);
        }
    }
}
