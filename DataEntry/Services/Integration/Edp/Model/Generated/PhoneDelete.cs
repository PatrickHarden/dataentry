using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PhoneDelete : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{phone_id,location_id}";

        [JsonProperty(@"phone_id", Required = Required.Always)]
        public int phone_id { get; set; }

        ///<summary>
        /// Optional, can be used for Company with DeletePhones
        ///</summary>
        [JsonProperty(@"location_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? location_id { get; set; }
    }
}
