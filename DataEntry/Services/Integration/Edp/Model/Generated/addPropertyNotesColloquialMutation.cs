using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class addPropertyNotesColloquialMutation : EdpGraphQLMutation<ResultData>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @"addPropertyNotesColloquial"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
                yield return new KeyValuePair<string, object>(@"request", request);
                yield return new KeyValuePair<string, object>(@"property_id", property_id);
                yield return new KeyValuePair<string, object>(@"property_notes_id", property_notes_id);
                yield return new KeyValuePair<string, object>(@"colloquial", colloquial);
            }
        }

        public RequestDetails request { get; set; }

        public int property_id { get; set; }

        public int property_notes_id { get; set; }

        public PropertyNoteColloquial colloquial { get; set; }
    }
}
