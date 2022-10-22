using System;

namespace dataentry.Data.DBContext.Model
{
    public class PublishingState
    {
        public string Value { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateListed { get; set; }
    }
}
