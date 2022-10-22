using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PropertyMediaInformationUnified : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{id,media_name,media_path,primary_image_f,ref_property_image_type_desc,still_video_f,ref_image_size_type_desc,ref_media_source_system_desc,publish_image_f,image_cbre_owned_f,media_caption,image_width,image_height,ref_image_orientation_desc,thumbnail_f,loopnet_image_f,allow_web_view_f,source_notes,ref_media_quality_type_desc,ref_media_view_type_desc,ref_property_usage_type_desc,media_type_desc,media_content_type_desc,display_order}";

        [JsonProperty(@"id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(@"media_name", Required = Required.Always)]
        public string media_name { get; set; }

        [JsonProperty(@"media_path", Required = Required.Always)]
        public string media_path { get; set; }

        [JsonProperty(@"primary_image_f", Required = Required.Always)]
        public bool primary_image_f { get; set; }

        [JsonProperty(@"ref_property_image_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_image_type_desc { get; set; }

        [JsonProperty(@"still_video_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? still_video_f { get; set; }

        [JsonProperty(@"ref_image_size_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_image_size_type_desc { get; set; }

        [JsonProperty(@"ref_media_source_system_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_media_source_system_desc { get; set; }

        [JsonProperty(@"publish_image_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? publish_image_f { get; set; }

        [JsonProperty(@"image_cbre_owned_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? image_cbre_owned_f { get; set; }

        [JsonProperty(@"media_caption", NullValueHandling = NullValueHandling.Ignore)]
        public string media_caption { get; set; }

        [JsonProperty(@"image_width", NullValueHandling = NullValueHandling.Ignore)]
        public int? image_width { get; set; }

        [JsonProperty(@"image_height", NullValueHandling = NullValueHandling.Ignore)]
        public int? image_height { get; set; }

        [JsonProperty(@"ref_image_orientation_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_image_orientation_desc { get; set; }

        [JsonProperty(@"thumbnail_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? thumbnail_f { get; set; }

        [JsonProperty(@"loopnet_image_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? loopnet_image_f { get; set; }

        [JsonProperty(@"allow_web_view_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? allow_web_view_f { get; set; }

        [JsonProperty(@"source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string source_notes { get; set; }

        [JsonProperty(@"ref_media_quality_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_media_quality_type_desc { get; set; }

        [JsonProperty(@"ref_media_view_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_media_view_type_desc { get; set; }

        [JsonProperty(@"ref_property_usage_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_usage_type_desc { get; set; }

        [JsonProperty(@"media_type_desc", Required = Required.Always)]
        public string media_type_desc { get; set; }

        [JsonProperty(@"media_content_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string media_content_type_desc { get; set; }

        [JsonProperty(@"display_order", NullValueHandling = NullValueHandling.Ignore)]
        public int? display_order { get; set; }
    }
}
