using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{   
    public class PropertyUsage    {
        public string ref_property_usage_type_desc { get; set; } 
        public List<PropertyAmenity> usage_amenity { get; set; }
    }

    public class Property    {
        public PropertyUsage property_usage { get; set; } 
        public string property_name { get; set; }
        public List<PropertyMediaInformation> property_media_information { get; set; } 

    }

    public class Availability    {
        public Entity entity { get; set; } 
        public Property property { get; set; } 
        public List<ListingIds> listing_ids { get; set; }
        public FloorSuite floor_suite { get; set; } 
        public DateTime? date_available { get; set; } 
        public string listing_notes { get; set; } 
        public List<PropertyMeasurement> property_measurement_availability { get; set; }
        public string ref_availability_type_desc { get; set; } 
        public string ref_lease_type_desc { get; set; } 
        public decimal? asking_rental_rate_monthly { get; set; } 
        public string asking_rental_rate_monthly_uom_desc { get; set; } 
        public decimal? asking_rental_rate_yearly { get; set; } 
        public string asking_rental_rate_yearly_uom_desc { get; set; } 
        public decimal? asking_price_for_sale { get; set; } 
        public string asking_price_for_sale_uom_desc { get; set; } 
        public decimal? minimum_asking_rate_monthly { get; set; } 
        public string minimum_asking_rate_monthly_uom { get; set; } 
        public decimal? maximum_asking_rate_monthly { get; set; } 
        public string maximum_asking_rate_monthly_uom { get; set; } 
        public decimal? minimum_asking_rate_yearly { get; set; } 
        public string minimum_asking_rate_yearly_uom { get; set; } 
        public decimal? maximum_asking_rate_yearly { get; set; } 
        public string maximum_asking_rate_yearly_uom { get; set; } 
        public DateTime? date_on_market { get; set; } 
        public string listing_descr { get; set; } 
        public decimal? available_space { get; set; } 
        public string available_space_uom_desc { get; set; } 
        public int? total_area_of_space { get; set; } 
        public string total_area_of_space_uom_desc { get; set; } 
        public int? available_office_space { get; set; } 
        public string available_office_space_uom_desc { get; set; } 
        public decimal? total_contiguous_area_of_space { get; set; } 
        public string total_contiguous_area_of_space_uom_desc { get; set; } 
        public decimal? ceiling_height { get; set; } 
        public string ref_space_availability_status_desc { get; set; } 
    }

    public class SearchAvailability    {
        public int totalCount { get; set; } 
        public List<Availability> availability { get; set; } 
    }

    public class AvailbityDetailResultData  {
        public SearchAvailability searchAvailability { get; set; } 
    }

    public class AvailbityDetailResult    {
        public AvailbityDetailResultData data { get; set; } 
    }
}