using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ContactUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,name,email,phone}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(@"name", Required = Required.Always)]
        public string name { get; set; }

        [JsonProperty(@"email", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> email { get; set; }

        [JsonProperty(@"phone", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> phone { get; set; }
    }
}
