using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class FloorNSuite : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{floor_number,suite_number}";

        [JsonProperty(@"floor_number", Required = Required.Always)]
        public string floor_number { get; set; }

        [JsonProperty(@"suite_number", Required = Required.Always)]
        public string suite_number { get; set; }
    }
}
