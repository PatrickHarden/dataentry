using System.ComponentModel.DataAnnotations.Schema;

namespace dataentry.Data.DBContext.Model
{
    public class ListingBroker
    {
        public int ID { get; set; }
        public int BrokerID { get; set; }
        public int ListingID { get; set; }
        public int Order { get; set; }

        [ForeignKey("BrokerID")]
        public Broker Broker { get; set; }
        [ForeignKey("ListingID")]
        public Listing Listing { get; set; }
    }
}
