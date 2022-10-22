using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{   
    public class Contact    {
        public string first_name { get; set; } 
        public string last_name { get; set; } 
        public string email { get; set; } 
        public string full_phone_number { get; set; } 
    }

    public class ListingRep    {
        public List<Contact> contacts { get; set; } 
    }

    public class GetStackingPlan    {
        public List<ListingRep> listing_reps { get; set; } 
    }

    public class BrokerResultsData    
    {
        public GetStackingPlan getStackingPlan { get; set; } 
    }

    public class BrokerResults    {
        public BrokerResultsData data { get; set; } 
    }
}