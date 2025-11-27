using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Clinical6SDK.Models
{
    public class FlowStep : JsonApiModel
    {
        /**
		 * @param {Object}  response                  - JSON formatted response of a step
		 * @param {Number}  response.id               - The id of the step
		 * @param {Object}  response.title            - The title of the step
		 * @param {Object}  response.description      - Description, if any,  that explain how to answer
		 *                                              the step
		 * @param {String}  response.content_type     - The format of the step (e.g. single choice,
		 *                                              multiple choice)
		 * @param {String}  response.image            - An image
		 * @param {Array}   response.inputs           - Array of inputs
		 * @param {Array}   response.paths            - An array of next steps available to the user
		 * @param {Boolean} response.send_on_capture  - Save now or later
		 */


        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("image")]
        public ImageResource Image { get; set; }

        [JsonProperty("inputs")]
        public List<FlowInput> Inputs { get; set; }

        [JsonProperty("paths")]
        public IList<FlowPath> Paths { get; set; }

        [JsonProperty("send_on_capture")]
        public bool SendOnCapture { get; set; }

        [JsonProperty("choices")]
        public IList<FlowChoice> Choices { get; set; }

        public string PemanentLink
        {
            get
            {
                return Id.ToString();
            }
        }

        public int FlowProcessId { get; set; }
        /*{
            get
            {
                if(Flow != null)
                {
                    return this.Flow.Id.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }*/

        public string ExistingId
        {
            get
            {
                return this.Options.ExistingId;
            }
            set
            {
                this.Options.ExistingId = value;
            }
        }

        public Flow Flow { get; set; }

        internal FlowOptions Options { get; set; }

        public string Type { get => "data_collection__linked_steps"; }



        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="flow">Flow.</param>
        public FlowStep(Flow flow = null)
        {
            Options = new FlowOptions();

            this.Flow = flow;

            if (this.FlowProcessId > 0)
            {
                this.Flow.Id = this.FlowProcessId;
            }

            if (this.Flow != null)
            {
                InitFields();
            }

            if (!string.IsNullOrWhiteSpace(this.Flow.ExistingId))
            {
                this.ExistingId = this.Flow.ExistingId;
            }
        }

        public void InitFields()
        {
            if (this.Inputs != null && this.Inputs.Count > 0)
            {
                foreach (var input in this.Inputs)
                {
                    // this.Flow.Fields  ??????????????????????????????????????????????????
                }
            }
        }

        /// <summary>
        /// Get Fields used only for this step
        /// </summary>
        /// <returns>The fields Key/Value pair for quick retrival of values</returns>
        public Dictionary<int, string> Fields()
        {
            var ret = new Dictionary<int, string>();

            if (Flow != null && Flow.Fields != null && Flow.Fields.Count > 0)
            {
                foreach (var inp in this.Inputs)
                {
                    foreach (var f in this.Flow.Fields)
                    {
                        if (inp.Id == f.InputId)
                        {
                            ret.Add(inp.Id ?? 0, f.Value);
                        }
                    }

                }
            }

            return ret;
        }

        /// <summary>
        /// Save fields for later
        /// </summary>
        public void AddFieldsToSave()
        {
            var fields = new List<string>();

            if (this.Flow != null && this.Flow.Fields != null && this.Fields().Count > 0)
            {
                foreach (var f in this.Flow.Fields)
                {
                    if (!string.IsNullOrWhiteSpace(f.Value))
                    {
                        fields.Add(f.InputId.ToString());
                    }
                }
                this.Flow.AddFieldsToSave(fields);
            }
        }

        public string Get(int key)
        {
            if (this.Flow != null && this.Flow.Fields != null)
            {
                return this.Flow.Fields[0].InputId.ToString();
            }
            return null;
        }

        public int GetNextStep(List<FlowPathSteps> steps)
        {
            foreach (var s in steps)
            {
                if (s.Conditions != null)
                {
                    if (FlowStep.Evaluate(this.Get(Convert.ToInt32(s.Conditions.criteria)), s.Conditions.Operator, s.Conditions.Value))
                    {
                        return s.Step;  // must return  FlowStep or StepId ????????????
                    }
                }
            }
            return 0; // ???????????????????
        }

        public FlowPath GetPath(string name)
        {
            var filter = new List<FlowPath>();
            if (this.Paths != null && this.Paths.Count > 0)
            {
                foreach (var p in this.Paths)
                {
                    if (p.ButtonName == name)
                    {
                        filter.Add(p);
                    }
                }
                if (filter.Count > 0)
                {
                    return filter[0];
                }
            }
            return null;
        }

        public bool HasSteps(FlowPath path)
        {
            return path != null && (path.Steps != null ? path.Steps.Count > 0 : false);
        }

        public bool IsAutoSave(FlowPath path)
        {
            return path != null ? path.Capture : false;
        }

        public bool IsFieldRequired(int id)
        {
            List<FlowInput> filter = new List<FlowInput>();

            if (this.Inputs != null && this.Inputs.Count > 0)
            {
                foreach (var input in this.Inputs)
                {
                    if (input.Id == id && input.Required)
                    {
                        filter.Add(input);
                    }
                }
            }

            return filter.Count > 0;
        }

        public object OnSave(object results) // ???????????????
        {
            return results;
        }

        public void PostInsights(string action)
        {
            ////  Falta definir Client......
        }

        public async void SaveAsync()
        {
            this.AddFieldsToSave();
            var flowService = new Clinical6SDK.Services.Clinical6FlowService();
            var r = await flowService.CollectFieldsAsync(this.Flow, this.Flow.ToSave as List<FlowField>);

            if (r != null && r.IsResponseSuccessful)
            {
                this.Flow.ToSave.Clear();
                this.Flow.Status = r.ProcessStatus;
                this.OnSave(null);
                this.PostInsights("Collect"); // ??????????
                // resolve();
            }
        }

        public void Set(string key, string value, int existingId = -1)
        {
            var _inputs = this.Inputs as List<FlowInput>;
            int indexInput = _inputs.FindIndex(0, (i) => i.Id.ToString() == key);

            var _fields = this.Flow.Fields as List<FlowField>;
            int indexField = _fields.FindIndex(0, (f) => f.InputId.ToString() == key);


            string _value = value;

            if (_inputs != null && _inputs.Count > 0)
            {
                foreach (var input in _inputs)
                {
                    if ((input.QuestionType == FlowInput.QuestionTypes.SINGLE_CHOICE ||
                        input.QuestionType == FlowInput.QuestionTypes.MULTIPLE_CHOICE) && input.Id.ToString() == key)
                    {
                        _inputs.Add(input);
                    }
                }
            }
            if (_inputs.Count > 0)
            {
                _value = _inputs[0].FindChoiceByValue(value).Id.ToString();
            }


            this.Flow.Fields[indexField].Value = _value;

            if (existingId > 0 && !string.IsNullOrWhiteSpace(this.ExistingId) && !string.IsNullOrWhiteSpace(this.Flow.ExistingId))
            {
                // ????????????????
                //this.Flow.Fields[0].ExistingId = existingId;
            }
            else
            {
                //this.Flow.Fields[0].ExistingId = string.Empty;
            }
        }

        public void ValidateRequiredFields(FlowPath path)
        {
            List<FlowField> fields = new List<FlowField>();
            List<FlowField> _fields = this.Flow.Fields as List<FlowField>;
            if (_fields != null && _fields.Count > 0)
            {
                List<int> fieldIdsList = new List<int>();

                foreach (var f in _fields)
                {
                    if (string.IsNullOrEmpty(f.Value) && this.IsFieldRequired(f.InputId))
                    {
                        fields.Add(f);
                    }

                    fieldIdsList.Add(f.InputId);
                }
            }

            List<FlowInput> inputs = new List<FlowInput>();
            //this.Inputs
            // convert to readable text `${input.title} (id: ${input.id})`
            string fieldString = string.Empty;

            if (this.Inputs != null && this.Inputs.Count > 0)
            {

                foreach (var i in this.Inputs)
                {
                    if (fields.Exists((f) => f.InputId == i.Id))
                    {
                        inputs.Add(i);
                    }
                }
            }
            fieldString = JsonConvert.SerializeObject(inputs);

            if (fields.Count == 0 || !((this.IsAutoSave(path) || this.SendOnCapture) && fields.Count >= 0))
            {
                throw new Exception("cannot proceed to path '" + path.ButtonName +
                                        "' with empty required field(s) " +
                                        fieldString + ")");
            }
        }


        public static bool Evaluate(string a, string Operator, string b)
        {
            double _a = 0;
            double _b = 0;

            try
            {
                _a = Convert.ToDouble(a);
            }
            catch
            {

            }
            try
            {
                _b = Convert.ToDouble(b);
            }
            catch
            {

            }
            return Oper(_a, _b, Operator);
        }

        public static bool Oper(double a, double b, string oper)
        {
            switch (oper)
            {
                case Operators._M:
                    return a > b;
                case Operators._ME:
                    return a >= b;
                case Operators._L:
                    return a < b;
                case Operators._LE:
                    return a <= b;
                case Operators._E:
                    return a == b;
                case Operators._D:
                    return a != b;
                default:
                    return false;
            }
        }

        public static class Operators
        {
            public const string _M = ">";
            public const string _ME = ">=";
            public const string _L = "<";
            public const string _LE = "<=";
            public const string _E = "=";
            public const string _D = "<>";
        }
    }
}
