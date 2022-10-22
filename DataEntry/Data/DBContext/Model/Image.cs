using System;

namespace dataentry.Data.DBContext.Model
{
    public class Image
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public int WatermarkProcessStatus { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
