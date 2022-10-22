using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class SaleCompUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{confidentiality_f,arrangement_signed_date,ref_arrangement_status_desc,ref_cancel_reason_type_desc,ref_commitment_agreementtype_desc,ref_arrangement_user_status_desc,ref_posting_status_desc,voucher,deal_iq_voucher_id,available_office_space_area,ref_available_office_space_area_uom_desc,reported_sale_price,ref_reported_sale_price_uom_desc,sales_price_per_square_foot,ref_sales_price_per_square_foot_uom_desc,net_rentable_area,ref_net_rentable_area_uom_desc,ref_sale_transaction_type_desc,ref_interest_transferred_type_desc,percentage_ownership_interest_purchased,recording_date,document_number,occupancy_at_sale_arrangement,net_operating_income,ref_net_operating_income_uom_desc,actual_caprate,proforma_caprate,scheduled_rent_amount,ref_scheduled_rent_amount_uom_desc,internal_rate_of_return,land_price_per_acre,ref_land_price_per_acre_uom_desc,actual_sales_price,ref_actual_sales_price_yearly_uom_desc,amperage_descr,minimum_clear_height,number_of_grade_level_doors,number_of_dock_doors,dock_doors_f,grade_level_door_f,available_office_area,ref_available_office_area_uom_desc,available_parking_ratio,available_retail_area,ref_available_retail_area_uom_desc,available_warehouse_area,ref_available_warehouse_area_uom_desc,building_gross_area,ref_building_gross_area_uom_desc,building_number,price_per_square_foot,ref_price_per_square_foot_uom_desc,price_per_acre,ref_price_per_acre_uom_desc,sale_arrangement_comments,sale_arrangement_long_note,sale_arrangement_notes,sale_arrangement_space_area,ref_sale_arrangement_space_area_uom_desc,electrical_phase_descr,number_of_days_in_market,occupancy_date,sprinkler_descr,vacant_f,voltage_descr,site_area,ref_site_area_uom_desc,campus_f,list_caprate,property_date_in_market,reference_retail_location_type_desc,number_of_property,ref_rail_service_type_desc,ref_development_type_desc,ref_space_type_desc,asking_sales_price,ref_asking_sales_price_uom_desc,sale_caprate,number_of_floors,hold_f,ref_hold_reason_desc,referral_f,cbre_client_representative_f,converted_arrangement_f,arrangement_live_f,arrangement_live_date,arrangement_complete_date,arrangement_in_process_f,total_budget,ref_total_budget_uom_desc,ref_comission_agreement_type_desc,cbre_dual_representation_f,cbre_affiliate_office_referral_f,ref_arrangement_phase_desc,ref_arrangement_stage_desc,arrangement_requestor,arrangement_owner_id,renewal_option_f,termination_option_f,other_option_f,arrangement_category,ref_arrangement_risk_status_desc,ref_arrangement_sub_category_desc,prime_property_f,property_zoning_code,sale_arrangement_total_consideration,ref_sale_arrangement_total_consideration_uom_desc,ref_price_type_desc,buyer_objective_type,leaseback_type,previous_sale_date,net_initial_yield_on_asking_sales_price_percent,wavg_unexpired_lease_term,ref_wavg_unexpired_lease_term_uom_desc,wavg_unexpired_term_certain,ref_wavg_unexpired_term_certain_uom_desc,ref_sale_condition_type_desc,verified_ts,verified_by,record_source_desc,source_details,data_aquired_from_desc,arrangement_source_notes,asking_rent_yearly_at_sale,ref_asking_rent_yearly_at_sale_uom_desc,asking_rent_yearly_per_area_at_sale,ref_asking_rent_yearly_per_area_at_sale_uom_desc,miq_record_hide_f,account_type_desc}";

        [JsonProperty(@"confidentiality_f", NullValueHandling = NullValueHandling.Ignore)]
        public string confidentiality_f { get; set; }

        [JsonProperty(@"arrangement_signed_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_signed_date { get; set; }

        [JsonProperty(@"ref_arrangement_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_status_desc { get; set; }

        [JsonProperty(@"ref_cancel_reason_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_cancel_reason_type_desc { get; set; }

        [JsonProperty(@"ref_commitment_agreementtype_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_commitment_agreementtype_desc { get; set; }

        [JsonProperty(@"ref_arrangement_user_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_user_status_desc { get; set; }

        [JsonProperty(@"ref_posting_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_posting_status_desc { get; set; }

        [JsonProperty(@"voucher", NullValueHandling = NullValueHandling.Ignore)]
        public string voucher { get; set; }

        [JsonProperty(@"deal_iq_voucher_id", NullValueHandling = NullValueHandling.Ignore)]
        public string deal_iq_voucher_id { get; set; }

        [JsonProperty(@"available_office_space_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_office_space_area { get; set; }

        [JsonProperty(@"ref_available_office_space_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_available_office_space_area_uom_desc { get; set; }

        [JsonProperty(@"reported_sale_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? reported_sale_price { get; set; }

        [JsonProperty(@"ref_reported_sale_price_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_reported_sale_price_uom_desc { get; set; }

        [JsonProperty(@"sales_price_per_square_foot", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sales_price_per_square_foot { get; set; }

        [JsonProperty(@"ref_sales_price_per_square_foot_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sales_price_per_square_foot_uom_desc { get; set; }

        [JsonProperty(@"net_rentable_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? net_rentable_area { get; set; }

        [JsonProperty(@"ref_net_rentable_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_net_rentable_area_uom_desc { get; set; }

        [JsonProperty(@"ref_sale_transaction_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sale_transaction_type_desc { get; set; }

        [JsonProperty(@"ref_interest_transferred_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_interest_transferred_type_desc { get; set; }

        [JsonProperty(@"percentage_ownership_interest_purchased", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? percentage_ownership_interest_purchased { get; set; }

        [JsonProperty(@"recording_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? recording_date { get; set; }

        [JsonProperty(@"document_number", NullValueHandling = NullValueHandling.Ignore)]
        public string document_number { get; set; }

        [JsonProperty(@"occupancy_at_sale_arrangement", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? occupancy_at_sale_arrangement { get; set; }

        [JsonProperty(@"net_operating_income", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? net_operating_income { get; set; }

        [JsonProperty(@"ref_net_operating_income_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_net_operating_income_uom_desc { get; set; }

        [JsonProperty(@"actual_caprate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? actual_caprate { get; set; }

        [JsonProperty(@"proforma_caprate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? proforma_caprate { get; set; }

        [JsonProperty(@"scheduled_rent_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? scheduled_rent_amount { get; set; }

        [JsonProperty(@"ref_scheduled_rent_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_scheduled_rent_amount_uom_desc { get; set; }

        [JsonProperty(@"internal_rate_of_return", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? internal_rate_of_return { get; set; }

        [JsonProperty(@"land_price_per_acre", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? land_price_per_acre { get; set; }

        [JsonProperty(@"ref_land_price_per_acre_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_land_price_per_acre_uom_desc { get; set; }

        [JsonProperty(@"actual_sales_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? actual_sales_price { get; set; }

        [JsonProperty(@"ref_actual_sales_price_yearly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_actual_sales_price_yearly_uom_desc { get; set; }

        [JsonProperty(@"amperage_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string amperage_descr { get; set; }

        [JsonProperty(@"minimum_clear_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_clear_height { get; set; }

        [JsonProperty(@"number_of_grade_level_doors", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_grade_level_doors { get; set; }

        [JsonProperty(@"number_of_dock_doors", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? number_of_dock_doors { get; set; }

        [JsonProperty(@"dock_doors_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? dock_doors_f { get; set; }

        [JsonProperty(@"grade_level_door_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? grade_level_door_f { get; set; }

        [JsonProperty(@"available_office_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_office_area { get; set; }

        [JsonProperty(@"ref_available_office_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_available_office_area_uom_desc { get; set; }

        [JsonProperty(@"available_parking_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_parking_ratio { get; set; }

        [JsonProperty(@"available_retail_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_retail_area { get; set; }

        [JsonProperty(@"ref_available_retail_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_available_retail_area_uom_desc { get; set; }

        [JsonProperty(@"available_warehouse_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? available_warehouse_area { get; set; }

        [JsonProperty(@"ref_available_warehouse_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_available_warehouse_area_uom_desc { get; set; }

        [JsonProperty(@"building_gross_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? building_gross_area { get; set; }

        [JsonProperty(@"ref_building_gross_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_building_gross_area_uom_desc { get; set; }

        [JsonProperty(@"building_number", NullValueHandling = NullValueHandling.Ignore)]
        public string building_number { get; set; }

        [JsonProperty(@"price_per_square_foot", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? price_per_square_foot { get; set; }

        [JsonProperty(@"ref_price_per_square_foot_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_price_per_square_foot_uom_desc { get; set; }

        [JsonProperty(@"price_per_acre", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? price_per_acre { get; set; }

        [JsonProperty(@"ref_price_per_acre_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_price_per_acre_uom_desc { get; set; }

        [JsonProperty(@"sale_arrangement_comments", NullValueHandling = NullValueHandling.Ignore)]
        public string sale_arrangement_comments { get; set; }

        [JsonProperty(@"sale_arrangement_long_note", NullValueHandling = NullValueHandling.Ignore)]
        public string sale_arrangement_long_note { get; set; }

        [JsonProperty(@"sale_arrangement_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string sale_arrangement_notes { get; set; }

        [JsonProperty(@"sale_arrangement_space_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sale_arrangement_space_area { get; set; }

        [JsonProperty(@"ref_sale_arrangement_space_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sale_arrangement_space_area_uom_desc { get; set; }

        [JsonProperty(@"electrical_phase_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string electrical_phase_descr { get; set; }

        [JsonProperty(@"number_of_days_in_market", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? number_of_days_in_market { get; set; }

        [JsonProperty(@"occupancy_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? occupancy_date { get; set; }

        [JsonProperty(@"sprinkler_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string sprinkler_descr { get; set; }

        [JsonProperty(@"vacant_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? vacant_f { get; set; }

        [JsonProperty(@"voltage_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string voltage_descr { get; set; }

        [JsonProperty(@"site_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? site_area { get; set; }

        [JsonProperty(@"ref_site_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_site_area_uom_desc { get; set; }

        [JsonProperty(@"campus_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? campus_f { get; set; }

        [JsonProperty(@"list_caprate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? list_caprate { get; set; }

        [JsonProperty(@"property_date_in_market", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? property_date_in_market { get; set; }

        [JsonProperty(@"reference_retail_location_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string reference_retail_location_type_desc { get; set; }

        [JsonProperty(@"number_of_property", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_property { get; set; }

        [JsonProperty(@"ref_rail_service_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_rail_service_type_desc { get; set; }

        [JsonProperty(@"ref_development_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_development_type_desc { get; set; }

        [JsonProperty(@"ref_space_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_space_type_desc { get; set; }

        [JsonProperty(@"asking_sales_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_sales_price { get; set; }

        [JsonProperty(@"ref_asking_sales_price_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_asking_sales_price_uom_desc { get; set; }

        [JsonProperty(@"sale_caprate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sale_caprate { get; set; }

        [JsonProperty(@"number_of_floors", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_floors { get; set; }

        [JsonProperty(@"hold_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? hold_f { get; set; }

        [JsonProperty(@"ref_hold_reason_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_hold_reason_desc { get; set; }

        [JsonProperty(@"referral_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? referral_f { get; set; }

        [JsonProperty(@"cbre_client_representative_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cbre_client_representative_f { get; set; }

        [JsonProperty(@"converted_arrangement_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? converted_arrangement_f { get; set; }

        [JsonProperty(@"arrangement_live_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? arrangement_live_f { get; set; }

        [JsonProperty(@"arrangement_live_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_live_date { get; set; }

        [JsonProperty(@"arrangement_complete_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_complete_date { get; set; }

        [JsonProperty(@"arrangement_in_process_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? arrangement_in_process_f { get; set; }

        [JsonProperty(@"total_budget", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_budget { get; set; }

        [JsonProperty(@"ref_total_budget_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_total_budget_uom_desc { get; set; }

        [JsonProperty(@"ref_comission_agreement_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_comission_agreement_type_desc { get; set; }

        [JsonProperty(@"cbre_dual_representation_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cbre_dual_representation_f { get; set; }

        [JsonProperty(@"cbre_affiliate_office_referral_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cbre_affiliate_office_referral_f { get; set; }

        [JsonProperty(@"ref_arrangement_phase_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_phase_desc { get; set; }

        [JsonProperty(@"ref_arrangement_stage_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_stage_desc { get; set; }

        [JsonProperty(@"arrangement_requestor", NullValueHandling = NullValueHandling.Ignore)]
        public string arrangement_requestor { get; set; }

        [JsonProperty(@"arrangement_owner_id", NullValueHandling = NullValueHandling.Ignore)]
        public string arrangement_owner_id { get; set; }

        [JsonProperty(@"renewal_option_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? renewal_option_f { get; set; }

        [JsonProperty(@"termination_option_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? termination_option_f { get; set; }

        [JsonProperty(@"other_option_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? other_option_f { get; set; }

        [JsonProperty(@"arrangement_category", NullValueHandling = NullValueHandling.Ignore)]
        public string arrangement_category { get; set; }

        [JsonProperty(@"ref_arrangement_risk_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_risk_status_desc { get; set; }

        [JsonProperty(@"ref_arrangement_sub_category_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_sub_category_desc { get; set; }

        [JsonProperty(@"prime_property_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? prime_property_f { get; set; }

        [JsonProperty(@"property_zoning_code", NullValueHandling = NullValueHandling.Ignore)]
        public string property_zoning_code { get; set; }

        [JsonProperty(@"sale_arrangement_total_consideration", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sale_arrangement_total_consideration { get; set; }

        [JsonProperty(@"ref_sale_arrangement_total_consideration_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sale_arrangement_total_consideration_uom_desc { get; set; }

        [JsonProperty(@"ref_price_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_price_type_desc { get; set; }

        [JsonProperty(@"buyer_objective_type", NullValueHandling = NullValueHandling.Ignore)]
        public string buyer_objective_type { get; set; }

        [JsonProperty(@"leaseback_type", NullValueHandling = NullValueHandling.Ignore)]
        public string leaseback_type { get; set; }

        [JsonProperty(@"previous_sale_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? previous_sale_date { get; set; }

        [JsonProperty(@"net_initial_yield_on_asking_sales_price_percent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? net_initial_yield_on_asking_sales_price_percent { get; set; }

        [JsonProperty(@"wavg_unexpired_lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? wavg_unexpired_lease_term { get; set; }

        [JsonProperty(@"ref_wavg_unexpired_lease_term_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_wavg_unexpired_lease_term_uom_desc { get; set; }

        [JsonProperty(@"wavg_unexpired_term_certain", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? wavg_unexpired_term_certain { get; set; }

        [JsonProperty(@"ref_wavg_unexpired_term_certain_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_wavg_unexpired_term_certain_uom_desc { get; set; }

        [JsonProperty(@"ref_sale_condition_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sale_condition_type_desc { get; set; }

        [JsonProperty(@"verified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? verified_ts { get; set; }

        [JsonProperty(@"verified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string verified_by { get; set; }

        [JsonProperty(@"record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_desc { get; set; }

        [JsonProperty(@"source_details", NullValueHandling = NullValueHandling.Ignore)]
        public string source_details { get; set; }

        [JsonProperty(@"data_aquired_from_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string data_aquired_from_desc { get; set; }

        [JsonProperty(@"arrangement_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string arrangement_source_notes { get; set; }

        [JsonProperty(@"asking_rent_yearly_at_sale", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rent_yearly_at_sale { get; set; }

        [JsonProperty(@"ref_asking_rent_yearly_at_sale_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_asking_rent_yearly_at_sale_uom_desc { get; set; }

        [JsonProperty(@"asking_rent_yearly_per_area_at_sale", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rent_yearly_per_area_at_sale { get; set; }

        [JsonProperty(@"ref_asking_rent_yearly_per_area_at_sale_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_asking_rent_yearly_per_area_at_sale_uom_desc { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }

        [JsonProperty(@"account_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string account_type_desc { get; set; }
    }
}
