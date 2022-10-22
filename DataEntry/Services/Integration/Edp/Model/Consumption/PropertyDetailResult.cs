using dataentry.Services.Integration.Edp.Model.Consumption;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{   
    public class PropertyMediaInformation    {
        public int id { get; set; }
        public string media_name { get; set; } 
        public string media_path { get; set; } 
        public bool primary_image_f { get; set; } 
        public string media_caption { get; set; } 
        public string media_type_desc { get; set; } 
        public int? display_order { get; set; } 
        public string ref_property_image_type_desc { get; set; }
        public string watermark_label { get; set; }
        public bool? publish_image_f { get; set; }
        public string media_content_type_desc { get; set; }
    }

    public class PropertyDetail    {
        public string property_name { get; set; } 
        public string property_descr { get; set; }
        public string type_desc { get; set; } 
        public string ref_property_tenancy_type_desc { get; set; } 
        public string property_class_type_desc { get; set; } 
        public string ref_property_status_desc { get; set; }
        public int? year_property_built { get; set; }
        public Entity entity { get; set; } 
        public PostalAddress postal_address { get; set; } 
        public List<Usage> usages { get; set; }
        public List<PropertyNote> property_note { get; set; }
        public decimal? total_gross_area { get; set; } 
        public string total_gross_area_uom_desc { get; set; }
        public decimal? total_land_area { get; set; } 
        public string total_land_area_uom_desc { get; set; } 
        public List<PropertyAmenity> property_amenity { get; set; } 
        public List<PropertyMediaInformation> property_media_information { get; set; } 
        public List<PropertyMeasurement> property_measurement { get; set; }
        public string property_notes { get; set; } 
        public string record_source_desc { get; set; } 
        public string source_details { get; set; } 
        public string ref_data_acquired_from_desc { get; set; } 
        public string record_source_notes { get; set; } 
        public List<Digital> digital { get; set; }
        public List<PropertyEnergyRating> property_external_ratings { get; set; }
        public List<PostalAddress> alternate_postal_addresses { get; set; }
    }
    
    public class SearchPropertyResult    {
        public List<PropertyDetail> properties { get; set; } 
    }

    public class SearchPropertyData    {
        public SearchPropertyResult searchProperty { get; set; } 
    }

    public class PropertyDetailResult    {
        public SearchPropertyData data { get; set; } 
    }
} 
