using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addAvailabilityListingIDMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addAvailabilityListingID"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"availability_id", availability_id);
                yield return new KeyValuePair<string, object>(@"listing_ids", listing_ids);
            }
        }

        public RequestDetails request { get; set; }

        public int availability_id { get; set; }

        public List<ListingID> listing_ids { get; set; }
    }
}
