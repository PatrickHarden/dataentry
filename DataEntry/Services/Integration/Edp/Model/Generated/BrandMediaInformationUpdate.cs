using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class BrandMediaInformationUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{primary_image_f,media_name,media_caption}";

        [JsonProperty(@"primary_image_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? primary_image_f { get; set; }

        [JsonProperty(@"media_name", NullValueHandling = NullValueHandling.Ignore)]
        public string media_name { get; set; }

        [JsonProperty(@"media_caption", NullValueHandling = NullValueHandling.Ignore)]
        public string media_caption { get; set; }
    }
}
