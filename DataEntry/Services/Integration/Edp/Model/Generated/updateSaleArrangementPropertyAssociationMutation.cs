using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updateSaleArrangementPropertyAssociationMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updateSaleArrangementPropertyAssociation"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"sale_id", sale_id);
                yield return new KeyValuePair<string, object>(@"propAssociation", propAssociation);
            }
        }

        public RequestDetails request { get; set; }

        public int sale_id { get; set; }

        public SalePropertyAssociation propAssociation { get; set; }
    }
}
