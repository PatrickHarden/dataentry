using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class PortfolioCompInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},ref_portfolio_type_desc,portfolio_arrangement_name,arrangement_signed_date,portfolio_sale_close_date,portfolio_area,ref_portfolio_area_uom_desc,portfolio_remaining_area,ref_portfolio_remaining_area_uom_desc,total_number_of_property,portfolio_sale_price,ref_portfolio_sale_price_uom_desc,confidentiality_f,ref_record_source_desc,portfolio_price_per_area_value,ref_portfolio_price_per_area_value_area_uom_desc,ref_portfolio_price_per_area_value_price_uom_desc,portfolio_note,salecomps,company_contact_role_addresses{company_id,contact_id,role_desc,location_id}}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"ref_portfolio_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_portfolio_type_desc { get; set; }

        [JsonProperty(@"portfolio_arrangement_name", Required = Required.Always)]
        public string portfolio_arrangement_name { get; set; }

        [JsonProperty(@"arrangement_signed_date", Required = Required.Always)]
        public DateTime arrangement_signed_date { get; set; }

        [JsonProperty(@"portfolio_sale_close_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? portfolio_sale_close_date { get; set; }

        [JsonProperty(@"portfolio_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? portfolio_area { get; set; }

        [JsonProperty(@"ref_portfolio_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_portfolio_area_uom_desc { get; set; }

        [JsonProperty(@"portfolio_remaining_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? portfolio_remaining_area { get; set; }

        [JsonProperty(@"ref_portfolio_remaining_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_portfolio_remaining_area_uom_desc { get; set; }

        [JsonProperty(@"total_number_of_property", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_number_of_property { get; set; }

        [JsonProperty(@"portfolio_sale_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? portfolio_sale_price { get; set; }

        [JsonProperty(@"ref_portfolio_sale_price_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_portfolio_sale_price_uom_desc { get; set; }

        [JsonProperty(@"confidentiality_f", NullValueHandling = NullValueHandling.Ignore)]
        public string confidentiality_f { get; set; }

        [JsonProperty(@"ref_record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_record_source_desc { get; set; }

        [JsonProperty(@"portfolio_price_per_area_value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? portfolio_price_per_area_value { get; set; }

        [JsonProperty(@"ref_portfolio_price_per_area_value_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_portfolio_price_per_area_value_area_uom_desc { get; set; }

        [JsonProperty(@"ref_portfolio_price_per_area_value_price_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_portfolio_price_per_area_value_price_uom_desc { get; set; }

        [JsonProperty(@"portfolio_note", NullValueHandling = NullValueHandling.Ignore)]
        public string portfolio_note { get; set; }

        [JsonProperty(@"salecomps", Required = Required.Always)]
        public List<int> salecomps { get; set; }

        [JsonProperty(@"company_contact_role_addresses", NullValueHandling = NullValueHandling.Ignore)]
        public List<CompanyContactRoleAddress> company_contact_role_addresses { get; set; }
    }
}
