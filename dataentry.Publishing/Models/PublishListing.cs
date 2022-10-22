using System;
using System.Collections.Generic;
using System.Text;

namespace dataentry.Publishing.Models
{
    /// <summary>
    /// The model returned by the database function
    /// </summary>
    public class PublishListing
    {
        public int Id { get; set; }
        public string ExternalID { get; set; }
        public string State { get; set; }
        public DateTime DateUpdated { get; set; }
        public string HomeSiteID { get; set; }
    }
}
