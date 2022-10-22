using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Consumption
{   
    public class Usage    
    {
        public int id { get; set; }
        public DateTime? date_property_available { get; set; }
        public bool? primary_usage_f { get; set; }
        public string ref_property_usage_type_desc { get; set; } 
        public object available_land_area { get; set; } 
        public string ref_sublease_rate_type_desc { get; set; } 
        public string ref_direct_rate_type_desc { get; set; } 
        public decimal? minimum_yearly_direct_lease_rate_amount { get; set; }
        public string minimum_yearly_direct_lease_rate_amount_uom_desc { get; set; }
        public decimal? maximum_yearly_direct_lease_rate { get; set; }
        public string maximum_yearly_direct_lease_rate_uom_desc { get; set; }
        public decimal? minimum_yearly_sub_lease_rate { get; set; }
        public string minimum_yearly_sub_lease_rate_uom_desc { get; set; }
        public decimal? maximum_yearly_sub_lease_rate { get; set; }
        public string maximum_yearly_sub_lease_rate_uom_desc { get; set; }
        public decimal? minimum_monthly_direct_lease_rate { get; set; }
        public string minimum_monthly_direct_lease_rate_uom_desc { get; set; }
        public decimal? maximum_monthly_direct_lease_rate { get; set; }
        public string maximum_monthly_direct_lease_rate_uom_desc { get; set; }
        public decimal? minimum_monthly_sub_lease_rate { get; set; }
        public string minimum_monthly_sub_lease_rate_uom_desc { get; set; }
        public decimal? maximum_monthly_sub_lease_rate { get; set; }
        public string maximum_monthly_sub_lease_rate_uom_desc { get; set; }
        public decimal? minimum_available_area { get; set; }
        public decimal? minimum_divisible_surface { get; set; }
        public decimal? total_available_surface_area { get; set; }
        public float? maintenence_cost { get; set; }
        public string maintenence_cost_uom_desc { get; set; }
        public float? tax_expense_amount { get; set; }
        public string tax_expense_amount_uom_desc { get; set; }
        public float? other_expense_amount { get; set; }
        public string other_expense_amount_uom_desc { get; set; }
        public string total_available_surface_area_uom_desc { get; set; }
        public List<CompanyContactRoleAddress> company_contact_role_addresses { get; set; } 
        public List<PropertyAmenity> usage_amenity {get; set;}
    }
}