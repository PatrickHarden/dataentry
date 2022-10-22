using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class RequestDetails : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country}";

        [JsonProperty(@"request_id", Required = Required.Always)]
        public string request_id { get; set; }

        [JsonProperty(@"action", NullValueHandling = NullValueHandling.Ignore)]
        public string action { get; set; }

        [JsonProperty(@"source_system_name", Required = Required.Always)]
        public string source_system_name { get; set; }

        [JsonProperty(@"source_id", Required = Required.Always)]
        public int source_id { get; set; }

        [JsonProperty(@"source_submitter_name", Required = Required.Always)]
        public string source_submitter_name { get; set; }

        [JsonProperty(@"source_submitter_email", NullValueHandling = NullValueHandling.Ignore)]
        public string source_submitter_email { get; set; }

        [JsonProperty(@"user_role", NullValueHandling = NullValueHandling.Ignore)]
        public string user_role { get; set; }

        [JsonProperty(@"comment", NullValueHandling = NullValueHandling.Ignore)]
        public string comment { get; set; }

        [JsonProperty(@"global_region", NullValueHandling = NullValueHandling.Ignore)]
        public string global_region { get; set; }

        [JsonProperty(@"country", NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }
    }
}
