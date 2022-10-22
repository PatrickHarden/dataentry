using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class LocationUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{ref_location_status_desc,headquarter_location_f,agency_location_f,company_location_f,tenant_location_f,billing_location_f,shipping_location_f,expiry_date}";

        [JsonProperty(@"ref_location_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_location_status_desc { get; set; }

        [JsonProperty(@"headquarter_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? headquarter_location_f { get; set; }

        [JsonProperty(@"agency_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? agency_location_f { get; set; }

        [JsonProperty(@"company_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? company_location_f { get; set; }

        [JsonProperty(@"tenant_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? tenant_location_f { get; set; }

        [JsonProperty(@"billing_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? billing_location_f { get; set; }

        [JsonProperty(@"shipping_location_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? shipping_location_f { get; set; }

        [JsonProperty(@"expiry_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? expiry_date { get; set; }
    }
}
