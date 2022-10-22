using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addSaleCompsToPortfolioMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addSaleCompsToPortfolio"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"portfolio_id", portfolio_id);
                yield return new KeyValuePair<string, object>(@"salecomp_ids", salecomp_ids);
            }
        }

        public RequestDetails request { get; set; }

        public int portfolio_id { get; set; }

        public List<int> salecomp_ids { get; set; }
    }
}
