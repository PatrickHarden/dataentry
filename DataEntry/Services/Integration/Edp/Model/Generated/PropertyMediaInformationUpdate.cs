using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyMediaInformationUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{ref_property_image_type_desc,publish_image_f,image_cbre_owned_f,media_caption,ref_property_usage_type_desc,media_content_type_desc,ref_image_orientation_desc,primary_image_f,media_name,display_order}";

        [JsonProperty(@"ref_property_image_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_image_type_desc { get; set; }

        [JsonProperty(@"publish_image_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? publish_image_f { get; set; }

        [JsonProperty(@"image_cbre_owned_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? image_cbre_owned_f { get; set; }

        [JsonProperty(@"media_caption", NullValueHandling = NullValueHandling.Ignore)]
        public string media_caption { get; set; }

        [JsonProperty(@"ref_property_usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_type_desc { get; set; }

        [JsonProperty(@"media_content_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string media_content_type_desc { get; set; }

        [JsonProperty(@"ref_image_orientation_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_image_orientation_desc { get; set; }

        [JsonProperty(@"primary_image_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? primary_image_f { get; set; }

        [JsonProperty(@"media_name", NullValueHandling = NullValueHandling.Ignore)]
        public string media_name { get; set; }

        [JsonProperty(@"display_order", NullValueHandling = NullValueHandling.Ignore)]
        public int? display_order { get; set; }
    }
}
