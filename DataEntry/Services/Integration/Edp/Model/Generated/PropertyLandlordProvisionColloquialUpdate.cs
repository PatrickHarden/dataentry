using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyLandlordProvisionColloquialUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_landlord_provision_note}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_landlord_provision_note", NullValueHandling = NullValueHandling.Ignore)]
        public string property_landlord_provision_note { get; set; }
    }
}
