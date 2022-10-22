using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ResultData : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{data,status,edp_request_id,edp_skey}";

        [JsonProperty(@"data", Required = Required.Always)]
        public string data { get; set; }

        [JsonProperty(@"status", Required = Required.Always)]
        public string status { get; set; }

        [JsonProperty(@"edp_request_id", Required = Required.Always)]
        public string edp_request_id { get; set; }

        [JsonProperty(@"edp_skey", Required = Required.Always)]
        public string edp_skey { get; set; }
    }
}
