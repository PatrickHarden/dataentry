using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyLandlordProvisionUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_landlord_provision_desc,landlord_provision_type_notes,colloquial{id,property_landlord_provision_note}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_landlord_provision_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_landlord_provision_desc { get; set; }

        [JsonProperty(@"landlord_provision_type_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string landlord_provision_type_notes { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyLandlordProvisionColloquialUpdate> colloquial { get; set; }
    }
}
