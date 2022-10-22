using System.Collections.Generic;

namespace dataentry.Data.DBContext.Model
{
    public class Broker
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Avatar { get; set; }
        public string Email{ get; set; }
        public string License { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public List<ListingBroker> ListingBroker { get; set; }
    }
}
