using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Diagnostics.Contracts;
using Clinical6SDK.Services.Responses;
using System.Linq;

namespace Clinical6SDK.Models
{
    public class Flow : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "data_collection__flow_processes";

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("steps", NullValueHandling = NullValueHandling.Ignore)]
        public IList<FlowStep> Steps { get; set; }

        [JsonProperty("first_step", NullValueHandling = NullValueHandling.Ignore)]
        public int? FirstStep { get; set; }  // should this be a FlowStep??

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public int? Total { get; set; }

        // This is to help support v3, eventually we don't need this logic - From FlowModel


        private FlowSettings _Settings { get; set; } = new FlowSettings();
        public FlowSettings Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                _Settings = value;
            }
        }

        // from helpers properties

        /// <summary>
        /// The current value of fields in the flow for each flow step
        /// </summary>
        /// <value>The fields.</value>
        public IList<FlowField> Fields { get; set; } = new List<FlowField>();

        public IList<FlowField> ToSave { get; set; } = new List<FlowField>();

        private FlowOptions _Options;
        /// <summary>
        /// Used for API calls. This can be set using the options setter and getter.
        /// If the term `existingId` or `ownerType` is used, it convert it to use the
        ///  underscore `_` naming convention.
        /// </summary>
        /// <value>The options.</value>
        public FlowOptions Options
        {
            get
            {
                return _Options;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value.Owner))
                {
                    _Options.Owner = value.Owner;
                }
                if (!string.IsNullOrWhiteSpace(value.OwnerType))
                {
                    _Options.OwnerType = value.OwnerType;
                }
                if (!string.IsNullOrWhiteSpace(value.ExistingId))
                {
                    _Options.ExistingId = value.ExistingId;
                }
            }
        }

        /// <summary>
        /// The status of the flow.  Typically `initial`, `in_progress`, and `completed`.  
        /// This can be set using the `status` getter and setter.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FlowStep First { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Entry Entry
        {
            get
            {
                return _Options.Entry;
            }
            set
            {
                _Options.Entry = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Owner
        {
            get
            {
                return _Options.Owner;
            }
            set
            {
                _Options.Owner = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerType
        {
            get
            {
                return _Options.OwnerType;
            }
            set
            {
                _Options.OwnerType = value;
            }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExistingId
        {
            get
            {
                return _Options.ExistingId;
            }
            set
            {
                _Options.ExistingId = value;
            }
        }

        [JsonProperty("permanent_link", NullValueHandling = NullValueHandling.Ignore)]
        public string PermanentLink { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectType { get => "data_collection/flow_process"; }

        public Flow()
        {
            Fields = new List<FlowField>();
            // FieldsDict = new Dictionary<int, string>();
            _Options = new FlowOptions()
            {
                Owner = string.Empty,
                ExistingId = string.Empty,
                OwnerType = "mobile_user"
            };

            Settings = new FlowSettings
            {
                Transition = FlowSettings.TrasitionValues.AUTO
            };

            Status = FlowStatus.INITIAL;

            ToSave = new List<FlowField>();
            Steps = new List<FlowStep>();

            // Connect each step's next options to an actual FlowStep() object
            ConnectGraph();
        }

        public Flow(FlowSettings settings = null) : this()
        {
            if (settings != null && !string.IsNullOrWhiteSpace(settings.Transition))
            {
                Settings.Transition = settings.Transition;
            }
        }

        public void AddFieldsToSave(IList<string> fields)
        {
            int index = -1;
            if (fields != null && fields.Count > 0)
            {
                List<FlowField> tosave = ToSave as List<FlowField>;
                List<FlowField> _fields = Fields as List<FlowField>;



                foreach (var f in fields)
                {
                    if (!tosave.Exists((fi) => fi.InputId.ToString() == f))
                    {
                        index = _fields.FindIndex((fi) => fi.InputId.ToString() == f);
                        if (index >= 0)
                        {
                            tosave.Add(new FlowField
                            {
                                InputId = _fields[index].InputId,
                                Value = _fields[index].Value
                            });
                            ToSave.Add(new FlowField
                            {
                                InputId = _fields[index].InputId,
                                Value = _fields[index].Value
                            });
                        }
                    }
                }
            }
        }

        public Flow Clone()
        {
            Flow flow = new Flow
            {
                Id = Id,
                Name = Name,
                Title = Title,
                Total = Total,
                Description = Description,
                Position = Position,

                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,

                Status = Status,
                FirstStep = FirstStep,

                Owner = Owner,
                OwnerType = OwnerType,
                ExistingId = ExistingId,

                Options = new FlowOptions
                {
                    Owner = Options.Owner,
                    OwnerType = Options.OwnerType,
                    ExistingId = Options.ExistingId,
                    Entry = new Entry
                    {
                        Date = Options.Entry.Date,
                        Id = Options.Entry.Id,
                        Position = Options.Entry.Position,
                        Title = Options.Entry.Title,
                        Description = Options.Entry.Description,
                        CreatedAt = Options.Entry.CreatedAt,
                        UpdatedAt = Options.Entry.UpdatedAt
                    }
                },
                Settings = new FlowSettings
                {
                    Transition = Settings.Transition
                }
                // flow.Fields
                // flow.Steps
                // flow.ToSave
                // flow._toSave
            };

            return flow;

        }
        public void Complete()
        {
            // postInsights("complete");
            Transition("collect");
        }

        public void ConnectGraph()
        {
            if (Steps.Count > 0)
            {
                First = (Steps as List<FlowStep>).FindAll(s => s.Id == FirstStep)?[0];
            }

            List<FlowStep> _steps = Steps as List<FlowStep>;
            if (Steps != null)
            {
                foreach (var step in Steps)
                {
                    if (step.Paths != null)
                    {
                        foreach (var next in step.Paths)
                        {
                            if (next.Steps != null)
                            {
                                foreach (var _step in next.Steps)
                                {
                                    /* Link "step" directly with the object containing the id
                                    *  _step.step = self.steps.filter(obj => (obj.id === _step.step))[0];

                                    *  provide some way of getting the parent flow information
                                    *  if (_step.step && !_step.step.flow)
                                    *  {
                                    *     _step.step.flow = self;
                                    *  }
                                    */
                                }
                            }
                        }
                    }
                }
            }
        }

        // The final action after the last step.  This function is intended for the user to override.
        public FlowStep End(FlowStep step = null)
        {
            return step;
        }


        public List<FlowInput> FindInputs(string whereFn)
        {
            // los toma de steps
            //this.Steps

            //TODO Temrinar esto...
            var inputs = this.Steps.Distinct().Where(c => c.Inputs.Any(s => s.QuestionType == "single_choice" || s.QuestionType == "multiple_choice")).Distinct().SelectMany(item => item.Inputs).Distinct().ToList();

            return inputs.ToList();
        }

        /// <summary>
        /// Get value from key
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="key">Key.</param>
        public string Get(string key)
        {
            if (Fields != null)
            {
                List<FlowField> fields = Fields as List<FlowField>;
                int index = fields.FindIndex((f) => f.InputId.ToString() == key);
                if (index >= 0)
                {
                    return Fields[index].Value;
                }
            }
            return string.Empty;
        }

        public int GetBFS(FlowStep a = null)
        {
            int dist = 0;

            if (a == null && a.Id <= 0)
            {
                // return string.Empty;
            }


            FlowStep Q = a;
            // if(Q.)

            return dist;
        }

        public GetInputDataByFlowResponse GetData()
        {
            return new Clinical6SDK.Services.Clinical6FlowService().GetInputDataByFlowAsync(this).Result;
        }

        public int GetLongestDistance(FlowStep a)
        {
            var bfsArray = GetBFS(a);
            return bfsArray > 0 ? Math.Max(bfsArray, 0) : 0;
        }

        public Dictionary<int, string> GetJohnsonMatrix()
        {
            Dictionary<int, string> matrix = new Dictionary<int, string>();
            if (Steps != null)
            {
                foreach (var step in Steps)
                {
                    matrix.Add(step.Id ?? 0, GetBFS(step).ToString());
                }
            }
            return matrix;
        }

        public string GetStatus()
        {
            // validate
            // Get from StatusService

            // return FlowStatus.INPROGRESS;
            return string.Empty;
        }

        public IList<FlowStep> LoadSteps()
        {
            var flowService = new Clinical6SDK.Services.Clinical6FlowService();
            Flow flow = flowService.GetFlowAsync(PermanentLink).Result;
            Steps = flow.Steps;
            First = flow.First;
            Total = flow.Steps.Count;

            return Steps;
        }

        public void OnSave(object results)
        {

            return;
        }

        public string PostInsights(string action)
        {
            string r = string.Empty;

            // use of AnalyticsService...

            return r;
        }

        public void Reset()
        {
            this?.ToSave?.Clear();
            this?.Fields?.Clear();

            if (Steps != null)
            {
                foreach (var step in Steps)
                {
                    step.InitFields();
                }
            }
        }

        public async void SaveAsync()
        {
            List<FlowField> _fields = Fields as List<FlowField>;

            _fields = _fields.FindAll((f) => !string.IsNullOrEmpty(f.Value));

            var flowServie = new Clinical6SDK.Services.Clinical6FlowService();


            var response = await flowServie.CollectFieldsAsync(this, _fields);

            OnSave(response);
            // resolve(response);

        }

        public void Set(string key, string value, int existingId = -1)
        {
            string _value = string.Empty;

            List<FlowInput> inputs = FindInputs(""); // ???????
            inputs = inputs.FindAll((i) => (i.QuestionType == FlowInput.QuestionTypes.SINGLE_CHOICE || i.QuestionType == FlowInput.QuestionTypes.MULTIPLE_CHOICE) && i.Id.ToString() == key);

            if (inputs.Count > 0)
            {
                _value = inputs[0].FindChoiceByValue(_value)?.Id.ToString();
            }

            var _fields = Fields as List<FlowField>;
            int indexField = _fields.FindIndex(0, (f) => f.InputId.ToString() == key);
            //TODO: cambiar _fields a diccionario
            Dictionary<string, string> dictionary =
                        new Dictionary<string, string>();

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = _value;
            }
            else
            {
                dictionary.Add(key, _value);
            }

            dictionary.Add(key, _value);

            if (indexField >= 0)
            {
                Fields[indexField].Value = _value;
            }
            else
            {
                Fields.Add(new Models.FlowField() { InputId = int.Parse(key), Value = _value });
            }



            if (existingId > 0 && !string.IsNullOrWhiteSpace(ExistingId) && !string.IsNullOrWhiteSpace(ExistingId))
            {
                // ????????????????
                //Flow.Fields[0].ExistingId = existingId;
            }
            else
            {
                //Flow.Fields[0].ExistingId = string.Empty;
            }
        }

        public void Start()
        {
            if (Status == FlowStatus.INITIAL)
            {
                if (Transition("start"))
                {
                    // resolve("start");
                    PostInsights("Start");
                }
                else
                {
                    //                                                                                                                                                                                                                                                                                                                    resolve('Flow has already started
                }
            }
        }

        public bool Transition(string action)
        {
            bool r = false;

            // StatusService


            return r;
        }
    }


    internal static class FlowStatus
    {
        public const string INITIAL = "initial";
        public const string INPROGRESS = "in_progress";
        public const string COMPLETED = "completed";
    }
}
