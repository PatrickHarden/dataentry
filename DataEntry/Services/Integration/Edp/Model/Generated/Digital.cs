using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Digital : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{digital_address,digital_type_desc,usage_type_desc,digital_address_display_text}";

        [JsonProperty(@"digital_address", Required = Required.Always)]
        public string digital_address { get; set; }

        ///<summary>
        /// possible values "Email", "Web URL"
        ///</summary>
        [JsonProperty(@"digital_type_desc", Required = Required.Always)]
        public string digital_type_desc { get; set; }

        ///<summary>
        /// possible values "Primary Address", "Work Address"
        ///</summary>
        [JsonProperty(@"usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string usage_type_desc { get; set; }

        [JsonProperty(@"digital_address_display_text", NullValueHandling = NullValueHandling.Ignore)]
        public string digital_address_display_text { get; set; }
    }
}
