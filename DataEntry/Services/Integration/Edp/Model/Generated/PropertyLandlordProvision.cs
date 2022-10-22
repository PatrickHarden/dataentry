using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyLandlordProvision : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_landlord_provision_desc,landlord_provision_type_notes,landlord_provision_type_desc,colloquial{id,property_landlord_provision_note,country_code_desc,language_desc}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_landlord_provision_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_landlord_provision_desc { get; set; }

        [JsonProperty(@"landlord_provision_type_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string landlord_provision_type_notes { get; set; }

        [JsonProperty(@"landlord_provision_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string landlord_provision_type_desc { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyLandlordProvisionColloquial> colloquial { get; set; }
    }
}
