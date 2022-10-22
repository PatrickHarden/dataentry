using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyNote : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,property_notes_type_desc,notes,colloquial{id,notes,country_code_desc,language_desc}}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"property_notes_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_notes_type_desc { get; set; }

        [JsonProperty(@"notes", NullValueHandling = NullValueHandling.Ignore)]
        public string notes { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyNoteColloquial> colloquial { get; set; }
    }
}
