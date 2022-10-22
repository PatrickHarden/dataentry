using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dataentry.Data.DBContext.Model
{
    public class ListingImage : IMedia
    {
        public int ID { get; set; }
        public int ImageID { get; set; }
        public int ListingID { get; set; }
        public bool? IsPrimary { get; set; }
        public string DisplayText { get; set; }
        public bool? IsActive { get; set; }
        public bool? HasWatermark { get; set; }
        public bool? IsUserOverride { get; set; } 
        public string OverridedBy { get; set; }
        public string ImageCategory { get; set; }
        public int Order { get; set; }

        [ForeignKey("ImageID")]
        public Image Image { get; set; }
        [ForeignKey("ListingID")]
        public Listing Listing { get; set; }

        string IMedia.Url => Image.Url;
        bool IMedia.Active => IsActive ?? false;
        bool IMedia.Primary => IsPrimary ?? false;
    }
}
