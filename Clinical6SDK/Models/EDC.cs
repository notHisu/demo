using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clinical6SDK.Models
{

    public class EDCConnection :  JsonApiModel, ITaskable
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "edc__connections";

        [JsonProperty("form_oid", NullValueHandling = NullValueHandling.Ignore)]
        public string FormOId { get; set; }

        [JsonProperty("folder_oid", NullValueHandling = NullValueHandling.Ignore)]
        public string FolderOId { get; set; }

        [JsonProperty("provider", NullValueHandling = NullValueHandling.Ignore)]
        public string Provider { get; set; }



        /*
            "mappings": {
              "items": {
                "inc1": {
                  "output_field_name": "CEDAT",
                  "output_value_mapping": {
                    "1": "SUBJECT",
                    "2": "PARENT"
                  }
                },
                "inc2": {
                  "output_field_name": "ABPNDIST_CEOCCUR"
                }
              }
            }
          }
        */
        [JsonProperty("mappings", NullValueHandling = NullValueHandling.Ignore)]
        public JObject Mappings { get; set; }
    }
}
