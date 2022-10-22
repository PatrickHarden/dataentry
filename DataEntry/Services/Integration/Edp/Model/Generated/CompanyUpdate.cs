using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class CompanyUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{ref_party_status_desc,status_change_reason_desc,bill_to_f,broker_f,federal_f,foreign_f,indirect_customer_f,primary_vendor_id_f,remit_to_vendor_f,sold_to_f,vat_f,withholding_f,stock_symbol,company_notes,ref_organisation_legal_structure_desc,company_registration_number,cbre_organization_f,total_number_of_employees,single_purpose_entity_indicator,annual_revenue_generated,change_status_reason,key_client_f,year_revenue_generated,single_point_of_contact_f,national_f,publicly_traded_f,dig_notes,ref_organization_level_type_desc,investor_profile_type_desc,statistical_f,verified_ts,verified_by}";

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

        [JsonProperty(@"national_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? national_f { get; set; }

        [JsonProperty(@"publicly_traded_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? publicly_traded_f { get; set; }

        [JsonProperty(@"dig_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string dig_notes { get; set; }

        [JsonProperty(@"ref_organization_level_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_organization_level_type_desc { get; set; }

        [JsonProperty(@"investor_profile_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string investor_profile_type_desc { get; set; }

        [JsonProperty(@"statistical_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? statistical_f { get; set; }

        [JsonProperty(@"verified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? verified_ts { get; set; }

        [JsonProperty(@"verified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string verified_by { get; set; }
    }
}
