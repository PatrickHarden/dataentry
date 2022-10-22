using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class BrandUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{brand_desc,brand_code,origin_country,website,business_category_desc,ref_brand_status_desc}";

        [JsonProperty(@"brand_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string brand_desc { get; set; }

        [JsonProperty(@"brand_code", NullValueHandling = NullValueHandling.Ignore)]
        public string brand_code { get; set; }

        [JsonProperty(@"origin_country", NullValueHandling = NullValueHandling.Ignore)]
        public string origin_country { get; set; }

        [JsonProperty(@"website", NullValueHandling = NullValueHandling.Ignore)]
        public string website { get; set; }

        [JsonProperty(@"business_category_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string business_category_desc { get; set; }

        [JsonProperty(@"ref_brand_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_brand_status_desc { get; set; }
    }
}
