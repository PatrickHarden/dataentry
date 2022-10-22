using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateSaleArrangementMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateSaleArrangement"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"sale_id", sale_id);
                yield return new KeyValuePair<string, object>(@"arrangement", arrangement);
            }
        }

        public RequestDetails request { get; set; }

        public int sale_id { get; set; }

        public SaleCompUpdate arrangement { get; set; }
    }
}
