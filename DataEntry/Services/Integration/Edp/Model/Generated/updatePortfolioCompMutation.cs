using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class updatePortfolioCompMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"updatePortfolioComp"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"portfolio_id", portfolio_id);
                yield return new KeyValuePair<string, object>(@"portfolio", portfolio);
            }
        }

        public RequestDetails request { get; set; }

        public int portfolio_id { get; set; }

        public PortfolioCompUpdate portfolio { get; set; }
    }
}
