using Newtonsoft.Json;
using System.Collections.Generic;

namespace Clinical6SDK.Models
{
    public class FlowPath
    {
		/**
		 *           paths: [
		 *             {
		 *               capture: true,
		 *               button_name: 'Next',
		 *               steps: [
		 *                 {
		 *                   step: 13,
		 *                   conditions: [],
		 *                 },
		 *               ],
		 *             },
		 *           ],
		 * 
		 * paths: [
 *             {
 *               capture: true,
 *               button_name: 'Next',
 *               steps: [
 *                 {
 *                   step: 12,
 *                   conditions: [
 *                     {
 *                       criteria: '271',
 *                       operator: '=',
 *                       value: '371',
 *                     },
 *                   ],
 *                 },
 *               ],
 *             },
 *           ],
		 * 
		 * 
		 * 
		 */

        [JsonProperty("capture")]
		public bool Capture { get; set; }

		[JsonProperty("button_name")]
		public string ButtonName { get; set; }

        [JsonProperty("steps")]
        public List<FlowPathSteps> Steps { get; set; }
	}

    public class FlowPathSteps
    {
        [JsonProperty("step")]
        public int Step { get; set; }

        [JsonProperty("conditions")]
        public FlowPathsStepsConditions Conditions { get; set; }
    }

    public class FlowPathsStepsConditions
    {
        [JsonProperty("criteria")]
        public string criteria { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
