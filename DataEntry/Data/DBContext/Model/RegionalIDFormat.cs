using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dataentry.Data.DBContext.Model
{
    public class RegionalIDFormat
    {
        public int ID { get; set; }
        public Guid RegionID { get; set; }
        public string SourceSystemName { get; set; }
        public string FormatString { get; set; }

        [ForeignKey("RegionID")]
        public Region Region { get; set; }
    }
}