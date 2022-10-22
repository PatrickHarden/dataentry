using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class NonRentableSpace : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{space_classification_type_desc,non_rentable_size,non_rentable_size_uom_desc,notes}";

        [JsonProperty(@"space_classification_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string space_classification_type_desc { get; set; }

        [JsonProperty(@"non_rentable_size", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? non_rentable_size { get; set; }

        [JsonProperty(@"non_rentable_size_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string non_rentable_size_uom_desc { get; set; }

        [JsonProperty(@"notes", NullValueHandling = NullValueHandling.Ignore)]
        public string notes { get; set; }
    }
}
