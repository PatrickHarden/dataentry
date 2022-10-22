using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dataentry.Data.DBContext.Model
{
    public class ListingData
    {
        public int ID { get; set; }
        public int ListingID { get; set; }
        public string DataType { get; set; }
        [Column(TypeName = "jsonb")]        
        public string Data { get; set; }
        public string Language { get; set; }        

        [ForeignKey("ListingID")]
        public Listing Listing { get; set; }
    }
}
