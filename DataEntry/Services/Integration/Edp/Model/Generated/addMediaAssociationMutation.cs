using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addMediaAssociationMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addMediaAssociation"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"entity", entity);
                yield return new KeyValuePair<string, object>(@"entity_id", entity_id);
                yield return new KeyValuePair<string, object>(@"property_media_information_id", property_media_information_id);
                yield return new KeyValuePair<string, object>(@"primary_image_f", primary_image_f);
                yield return new KeyValuePair<string, object>(@"display_order", display_order);
            }
        }

        public RequestDetails request { get; set; }

        public string entity { get; set; }

        public int entity_id { get; set; }

        public int property_media_information_id { get; set; }

        public bool primary_image_f { get; set; }

        public int? display_order { get; set; }
    }
}
