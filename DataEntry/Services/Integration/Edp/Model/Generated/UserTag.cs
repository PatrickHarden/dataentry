using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class UserTag : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{name}";

        [JsonProperty(@"name", Required = Required.Always)]
        public string name { get; set; }
    }
}
