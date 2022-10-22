using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},company_name,organization_legal_name,organization_trade_name,ref_party_status_desc,status_change_reason_desc,bill_to_f,broker_f,federal_f,foreign_f,indirect_customer_f,primary_vendor_id_f,remit_to_vendor_f,sold_to_f,vat_f,withholding_f,stock_symbol,company_notes,ref_organisation_legal_structure_desc,company_registration_number,cbre_organization_f,total_number_of_employees,single_purpose_entity_indicator,annual_revenue_generated,change_status_reason,key_client_f,year_revenue_generated,single_point_of_contact_f,location{id,location_desc,location_long_desc,ref_location_status_code,ref_location_status_desc,headquarter_location_f,agency_location_f,company_location_f,tenant_location_f,billing_location_f,shipping_location_f,postal_address{action,postal_address_id,ref_address_usage_type_desc,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,district,state_province,postal_code,postal_code_extension,suburb_name,county,country,colloquial{id,street1,street2,address1,pre_street_direction_name,street_name,street_type,post_street_direction_name,city,state_province,district,suburb_name,county,country,language_desc,country_code_desc}},phone{local_area_code,phone_number,extension_number,phone_type_desc,usage_type_desc,location_id},lig_geography{id,global_region_id,global_region_desc,division_id,division_desc,market_area_id,market_area_desc,local_market_area_id,local_market_area_desc,market_id,market_desc,submarket_id,submarket_desc,district_id,district_desc,neighborhood_id,neighborhood_desc}},digital{digital_address,digital_type_desc,usage_type_desc,digital_address_display_text},phone{local_area_code,phone_number,extension_number,phone_type_desc,usage_type_desc,location_id},company_industry_classification{id,primary_naics_code_f,ref_naics_classification_desc,ref_cbre_industry_code_desc},national_f,publicly_traded_f,ref_organization_level_type_desc,investor_profile_type_desc,statistical_f,dig_notes,user_tag{name},colloquial{alternative_company_name,country_code_desc,language_desc}}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"company_name", Required = Required.Always)]
        public string company_name { get; set; }

        [JsonProperty(@"organization_legal_name", Required = Required.Always)]
        public string organization_legal_name { get; set; }

        [JsonProperty(@"organization_trade_name", Required = Required.Always)]
        public string organization_trade_name { get; set; }

        [JsonProperty(@"ref_party_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_party_status_desc { get; set; }

        [JsonProperty(@"status_change_reason_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string status_change_reason_desc { get; set; }

        [JsonProperty(@"bill_to_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? bill_to_f { get; set; }

        [JsonProperty(@"broker_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? broker_f { get; set; }

        [JsonProperty(@"federal_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? federal_f { get; set; }

        [JsonProperty(@"foreign_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? foreign_f { get; set; }

        [JsonProperty(@"indirect_customer_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? indirect_customer_f { get; set; }

        [JsonProperty(@"primary_vendor_id_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? primary_vendor_id_f { get; set; }

        [JsonProperty(@"remit_to_vendor_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? remit_to_vendor_f { get; set; }

        [JsonProperty(@"sold_to_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? sold_to_f { get; set; }

        [JsonProperty(@"vat_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? vat_f { get; set; }

        [JsonProperty(@"withholding_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? withholding_f { get; set; }

        [JsonProperty(@"stock_symbol", NullValueHandling = NullValueHandling.Ignore)]
        public string stock_symbol { get; set; }

        [JsonProperty(@"company_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string company_notes { get; set; }

        [JsonProperty(@"ref_organisation_legal_structure_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_organisation_legal_structure_desc { get; set; }

        [JsonProperty(@"company_registration_number", NullValueHandling = NullValueHandling.Ignore)]
        public string company_registration_number { get; set; }

        [JsonProperty(@"cbre_organization_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cbre_organization_f { get; set; }

        [JsonProperty(@"total_number_of_employees", NullValueHandling = NullValueHandling.Ignore)]
        public int? total_number_of_employees { get; set; }

        [JsonProperty(@"single_purpose_entity_indicator", NullValueHandling = NullValueHandling.Ignore)]
        public int? single_purpose_entity_indicator { get; set; }

        [JsonProperty(@"annual_revenue_generated", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? annual_revenue_generated { get; set; }

        [JsonProperty(@"change_status_reason", NullValueHandling = NullValueHandling.Ignore)]
        public string change_status_reason { get; set; }

        [JsonProperty(@"key_client_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? key_client_f { get; set; }

        [JsonProperty(@"year_revenue_generated", NullValueHandling = NullValueHandling.Ignore)]
        public int? year_revenue_generated { get; set; }

        [JsonProperty(@"single_point_of_contact_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? single_point_of_contact_f { get; set; }

        [JsonProperty(@"location", Required = Required.Always)]
        public List<Location> location { get; set; }

        [JsonProperty(@"digital", NullValueHandling = NullValueHandling.Ignore)]
        public List<Digital> digital { get; set; }

        ///<summary>
        /// deprecate phone instead use location.Phone
        ///</summary>
        [JsonProperty(@"phone", NullValueHandling = NullValueHandling.Ignore)]
        public List<Phone> phone { get; set; }

        [JsonProperty(@"company_industry_classification", NullValueHandling = NullValueHandling.Ignore)]
        public List<CompanyIndustryClassification> company_industry_classification { get; set; }

        [JsonProperty(@"national_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? national_f { get; set; }

        [JsonProperty(@"publicly_traded_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? publicly_traded_f { get; set; }

        [JsonProperty(@"ref_organization_level_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_organization_level_type_desc { get; set; }

        [JsonProperty(@"investor_profile_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string investor_profile_type_desc { get; set; }

        [JsonProperty(@"statistical_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? statistical_f { get; set; }

        [JsonProperty(@"dig_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string dig_notes { get; set; }

        [JsonProperty(@"user_tag", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserTag> user_tag { get; set; }

        [JsonProperty(@"colloquial", NullValueHandling = NullValueHandling.Ignore)]
        public List<CompanyColloquial> colloquial { get; set; }
    }
}
