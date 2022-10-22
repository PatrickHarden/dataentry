using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Data.DBContext.Model
{
    public class Region
    {
        public static readonly Guid DefaultID = new Guid("00000000-0000-0000-0000-000000000001");

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string Name { get; set; }
        public string HomeSiteID { get; set; }
        public string ListingPrefix { get; set; }
        public string PreviewPrefix { get; set; }
        public string PreviewSiteID { get; set; }
        public string CultureCode { get; set; }
        public string CountryCode { get; set; }
        public string ExternalPublishUrl { get; set; }
        public string ExternalPreviewUrl { get; set; }
        
        public List<RegionalIDFormat> RegionalIDFormats { get; set; }
    }
}