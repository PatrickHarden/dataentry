using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class BrandMediaInformation : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,media_name,media_path,media_caption,primary_image_f,media_type_desc}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public int? id { get; set; }

        [JsonProperty(@"media_name", Required = Required.Always)]
        public string media_name { get; set; }

        [JsonProperty(@"media_path", Required = Required.Always)]
        public string media_path { get; set; }

        [JsonProperty(@"media_caption", NullValueHandling = NullValueHandling.Ignore)]
        public string media_caption { get; set; }

        [JsonProperty(@"primary_image_f", Required = Required.Always)]
        public bool primary_image_f { get; set; }

        [JsonProperty(@"media_type_desc", Required = Required.Always)]
        public string media_type_desc { get; set; }
    }
}
