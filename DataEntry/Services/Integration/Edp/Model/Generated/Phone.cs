using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class Phone : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{local_area_code,phone_number,extension_number,phone_type_desc,usage_type_desc,location_id}";

        [JsonProperty(@"local_area_code", NullValueHandling = NullValueHandling.Ignore)]
        public string local_area_code { get; set; }

        [JsonProperty(@"phone_number", Required = Required.Always)]
        public string phone_number { get; set; }

        [JsonProperty(@"extension_number", NullValueHandling = NullValueHandling.Ignore)]
        public string extension_number { get; set; }

        ///<summary>
        /// possible values "Mobile Phone", "Business Phone", "Fax", "Main Phone", "Phone", "Fax"
        ///</summary>
        [JsonProperty(@"phone_type_desc", Required = Required.Always)]
        public string phone_type_desc { get; set; }

        ///<summary>
        /// possible values "Primary Address", "Work Address"
        ///</summary>
        [JsonProperty(@"usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string usage_type_desc { get; set; }

        ///<summary>
        /// Optional, can be used for Company with AddPhoneNumbers
        ///</summary>
        [JsonProperty(@"location_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? location_id { get; set; }
    }
}
