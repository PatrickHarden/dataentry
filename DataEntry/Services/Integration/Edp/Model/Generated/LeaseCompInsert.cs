using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class LeaseCompInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},arrangement_start_date,arrangement_end_date,confidentiality_f,arrangement_signed_date,arrangement_desc,ref_arrangement_status_desc,ref_cancel_reason_type_desc,ref_commitment_agreementtype_desc,ref_arrangement_user_status_desc,ref_lease_arrangement_type_desc,ref_lease_income_type_desc,ref_posting_status_desc,ref_purchaser_type_desc,ref_transaction_type_desc,voucher,deal_iq_voucher_id,prior_location_descr,number_of_seats,effective_rent_yearly,ref_effective_rent_yearly_uom_desc,base_rent_yearly,ref_base_rent_yearly_uom_desc,ref_base_rent_yearly_area_uom_desc,base_rent_monthly,ref_base_rent_monthly_uom_desc,total_gross_payment,ref_total_gross_payment_uom_desc,operating_fixed_expenses,ref_operating_fixed_expenses_uom_desc,sales_volume,ref_sales_volume_uom_desc,calculated_percent_rent_clause,percent_rent_breakpoint,prior_location_area,annual_escalation_percent,option_notes,reserved_parking_spaces,operating_expenses,expense_stop,insurance_amount,ref_insurance_amount_uom_desc,utilities_expense,ref_utilities_expense_uom_desc,total_leased_space,ref_total_leased_space_uom_desc,tenant_improvement_allowance,ref_tenant_improvement_allowance_uom_desc,tenant_improvement_comments,tenant_improvement_as_is_f,number_of_month_free_rent,asking_rent_yearly,ref_asking_rent_yearly_uom_desc,asking_rent_monthly,ref_asking_rent_monthly_uom_desc,average_monthly_cost_per_space,ref_average_monthly_cost_per_space_uom_desc,other_concessions,grade_doors_dock_f,number_of_dock_doors,number_of_dock_grade_doors,minimum_clear_height,minimum_clear_height_uom_desc,number_of_grade_level_doors,lease_escalation_value,lease_escalation_created_by,lease_escalation_edp_create_ts,lease_escalation_modified_by,lease_escalation_modified_date,ref_lease_escalation_value_uom_desc,non_contiguous_floor_f,actual_lease_rate_yearly,ref_actual_lease_rate_yearly_uom_desc,actual_sales_price,ref_actual_sales_price_yearly_uom_desc,amperage_descr,area_not_leased,available_office_area,ref_available_office_area_uom_desc,available_parking_ratio,available_retail_area,ref_available_retail_area_uom_desc,available_warehouse_area,ref_available_warehouse_area_uom_desc,building_area,ref_building_area_uom_desc,price_per_acre,ref_price_per_acre_uom_desc,cam_charges,ref_cam_charges_uom_desc,electric_charges,ref_electric_charges_uom_desc,free_rent_months,ref_free_rent_months_uom_desc,effective_rent_monthly,ref_effective_rent_monthly_uom_desc,lease_arrangement_comments,lease_arrangement_long_note,lease_arrangement_notes,lease_arrangement_space_area,ref_lease_arrangement_space_area_uom_desc,electrical_phase_descr,number_of_days_in_market,occupancy_date,sprinkler_descr,arrangement_options{arrangement_option_type_desc,arrangement_option_start_date,arrangement_option_execution_date,arrangement_option_end_date,option_occurance,option_occurance_uom_desc},site_area,ref_site_area_uom_desc,triple_net_lease_charges,ref_triple_net_lease_charges_uom_desc,vacant_f,voltage_descr,ref_retail_location_type_desc,tenant_short_descr,ref_tenancy_type_desc,ref_lease_activity_type_desc,ref_lease_type_desc,sublease_f,rent_escalation_comments,lease_arrangement_close_date,number_of_property,ref_rail_service_type_desc,ref_development_type_desc,lease_term,ref_lease_term_uom_desc,arrangement_requestor,arrangement_owner_id,renewal_option_f,termination_option_f,other_option_f,arrangement_category,ref_arrangement_risk_status_desc,ref_arrangement_sub_category_desc,ref_party_category_type_desc,ref_cbre_industry_code_desc,ref_naics_classification_desc,prime_property_f,tax_amount,ref_tax_amount_uom_desc,tax_assessment_year,property_zoning_code,ref_property_class_type_desc,lease_arrangement_total_consideration,ref_lease_arrangement_total_consideration_uom_desc,lease_arrangement_savings_comments,lease_arrangement_savings_amount,ref_lease_arrangement_savings_amount_uom_desc,previous_years_rent,ref_previous_years_rent_uom_desc,ref_notice_type_desc,notice_date,lease_arrangement_owner_id,number_parking_space,reimbursement_notes,record_source_notes,operating_expense_amount,ref_operating_expense_amount_uom_desc,other_expense_amount,ref_other_expense_amount_uom_desc,dock_door_f,number_reserved_parking_spaces,full_service_gross_amount,ref_full_service_gross_amount_uom_desc,cost_per_reserved_parking_space,ref_cost_per_reserved_parking_space_uom_desc,cost_per_nonreserved_parking_space_monthly,cost_per_nonreserved_parking_space_monthly_uom,cost_per_nonreserved_parking_space_daily,cost_per_nonreserved_parking_space_daily_uom,maintenance_cost,ref_maintenance_cost_uom_desc,early_termination_date,ref_record_source_desc,record_source_desc,source_details,data_acquired_from_desc,arrangement_source_notes,voltage_desc,source_notes,account_type_desc,prior_location_status_type_desc,property_usage_sub_type_desc,property_sprinkler_type_desc,property_date_in_market,rent_type_desc,lock_in_duration,lock_in_duration_uom,base_rent_daily,ref_base_rent_daily_uom_desc,effective_rent_daily,ref_effective_rent_daily_uom_desc,rent_escalation_custom{escalation_value_amount,escalation_value_amount_uom,escalation_value_percent,escalation_order,escalation_period},rent_escalation_custom_f,agilecomp{agile_deal,account_driven,cost_per_seat,cost_per_seat_uom},property_id,property_usage_id,floor_suite{floor_number,suite_number,arrangement_space_area,ref_arrangement_space_area_uom_desc,full_floor_f,property_arrangement_status_type_desc},company_contact_role_addresses{company_id,contact_id,role_desc,location_id},user_tag{name},fit_out_period,fit_out_period_uom,brand_id,market_rent_trend_type_desc,base_rent_comment,effective_rent_comment,external_opportunity_number_source_desc,external_opportunity_number,property_measurement_arrangement{id,property_element_type_desc,property_measurement_notes,property_measurement_size,property_measurement_size_uom},miq_record_hide_f}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"arrangement_start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_start_date { get; set; }

        [JsonProperty(@"arrangement_end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? arrangement_end_date { get; set; }

        [JsonProperty(@"confidentiality_f", NullValueHandling = NullValueHandling.Ignore)]
        public string confidentiality_f { get; set; }

        [JsonProperty(@"arrangement_signed_date", Required = Required.Always)]
        public DateTime arrangement_signed_date { get; set; }

        [JsonProperty(@"arrangement_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string arrangement_desc { get; set; }

        [JsonProperty(@"ref_arrangement_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_status_desc { get; set; }

        [JsonProperty(@"ref_cancel_reason_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_cancel_reason_type_desc { get; set; }

        [JsonProperty(@"ref_commitment_agreementtype_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_commitment_agreementtype_desc { get; set; }

        [JsonProperty(@"ref_arrangement_user_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_arrangement_user_status_desc { get; set; }

        [JsonProperty(@"ref_lease_arrangement_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_arrangement_type_desc { get; set; }

        [JsonProperty(@"ref_lease_income_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_income_type_desc { get; set; }

        [JsonProperty(@"ref_posting_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_posting_status_desc { get; set; }

        [JsonProperty(@"ref_purchaser_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_purchaser_type_desc { get; set; }

        [JsonProperty(@"ref_transaction_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_transaction_type_desc { get; set; }

        [JsonProperty(@"voucher", NullValueHandling = NullValueHandling.Ignore)]
        public string voucher { get; set; }

        [JsonProperty(@"deal_iq_voucher_id", NullValueHandling = NullValueHandling.Ignore)]
        public string deal_iq_voucher_id { get; set; }

        [JsonProperty(@"prior_location_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string prior_location_descr { get; set; }

        [JsonProperty(@"number_of_seats", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_seats { get; set; }

        [JsonProperty(@"effective_rent_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? effective_rent_yearly { get; set; }

        [JsonProperty(@"ref_effective_rent_yearly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_effective_rent_yearly_uom_desc { get; set; }

        [JsonProperty(@"base_rent_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? base_rent_yearly { get; set; }

        [JsonProperty(@"ref_base_rent_yearly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_base_rent_yearly_uom_desc { get; set; }

        [JsonProperty(@"ref_base_rent_yearly_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_base_rent_yearly_area_uom_desc { get; set; }

        [JsonProperty(@"base_rent_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? base_rent_monthly { get; set; }

        [JsonProperty(@"ref_base_rent_monthly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_base_rent_monthly_uom_desc { get; set; }

        [JsonProperty(@"total_gross_payment", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? total_gross_payment { get; set; }

        [JsonProperty(@"ref_total_gross_payment_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_total_gross_payment_uom_desc { get; set; }

        [JsonProperty(@"operating_fixed_expenses", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? operating_fixed_expenses { get; set; }

        [JsonProperty(@"ref_operating_fixed_expenses_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_operating_fixed_expenses_uom_desc { get; set; }

        [JsonProperty(@"sales_volume", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? sales_volume { get; set; }

        [JsonProperty(@"ref_sales_volume_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_sales_volume_uom_desc { get; set; }

        [JsonProperty(@"calculated_percent_rent_clause", NullValueHandling = NullValueHandling.Ignore)]
        public string calculated_percent_rent_clause { get; set; }

        [JsonProperty(@"percent_rent_breakpoint", NullValueHandling = NullValueHandling.Ignore)]
        public string percent_rent_breakpoint { get; set; }

        [JsonProperty(@"prior_location_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? prior_location_area { get; set; }

        [JsonProperty(@"annual_escalation_percent", NullValueHandling = NullValueHandling.Ignore)]
        public string annual_escalation_percent { get; set; }

        [JsonProperty(@"option_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string option_notes { get; set; }

        [JsonProperty(@"reserved_parking_spaces", NullValueHandling = NullValueHandling.Ignore)]
        public int? reserved_parking_spaces { get; set; }

        [JsonProperty(@"operating_expenses", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? operating_expenses { get; set; }

        [JsonProperty(@"expense_stop", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? expense_stop { get; set; }

        [JsonProperty(@"insurance_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? insurance_amount { get; set; }

        [JsonProperty(@"ref_insurance_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_insurance_amount_uom_desc { get; set; }

        [JsonProperty(@"utilities_expense", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? utilities_expense { get; set; }

        [JsonProperty(@"ref_utilities_expense_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_utilities_expense_uom_desc { get; set; }

        [JsonProperty(@"total_leased_space", Required = Required.Always)]
        public decimal total_leased_space { get; set; }

        [JsonProperty(@"ref_total_leased_space_uom_desc", Required = Required.Always)]
        public string ref_total_leased_space_uom_desc { get; set; }

        [JsonProperty(@"tenant_improvement_allowance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? tenant_improvement_allowance { get; set; }

        [JsonProperty(@"ref_tenant_improvement_allowance_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_tenant_improvement_allowance_uom_desc { get; set; }

        [JsonProperty(@"tenant_improvement_comments", NullValueHandling = NullValueHandling.Ignore)]
        public string tenant_improvement_comments { get; set; }

        [JsonProperty(@"tenant_improvement_as_is_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? tenant_improvement_as_is_f { get; set; }

        [JsonProperty(@"number_of_month_free_rent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? number_of_month_free_rent { get; set; }

        [JsonProperty(@"asking_rent_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rent_yearly { get; set; }

        [JsonProperty(@"ref_asking_rent_yearly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_asking_rent_yearly_uom_desc { get; set; }

        [JsonProperty(@"asking_rent_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? asking_rent_monthly { get; set; }

        [JsonProperty(@"ref_asking_rent_monthly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_asking_rent_monthly_uom_desc { get; set; }

        [JsonProperty(@"average_monthly_cost_per_space", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? average_monthly_cost_per_space { get; set; }

        [JsonProperty(@"ref_average_monthly_cost_per_space_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_average_monthly_cost_per_space_uom_desc { get; set; }

        [JsonProperty(@"other_concessions", NullValueHandling = NullValueHandling.Ignore)]
        public string other_concessions { get; set; }

        [JsonProperty(@"grade_doors_dock_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? grade_doors_dock_f { get; set; }

        [JsonProperty(@"number_of_dock_doors", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_dock_doors { get; set; }

        [JsonProperty(@"number_of_dock_grade_doors", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_dock_grade_doors { get; set; }

        [JsonProperty(@"minimum_clear_height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? minimum_clear_height { get; set; }

        [JsonProperty(@"minimum_clear_height_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string minimum_clear_height_uom_desc { get; set; }

        [JsonProperty(@"number_of_grade_level_doors", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_grade_level_doors { get; set; }

        [JsonProperty(@"lease_escalation_value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? lease_escalation_value { get; set; }

        [JsonProperty(@"lease_escalation_created_by", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_escalation_created_by { get; set; }

        [JsonProperty(@"lease_escalation_edp_create_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? lease_escalation_edp_create_ts { get; set; }

        [JsonProperty(@"lease_escalation_modified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_escalation_modified_by { get; set; }

        [JsonProperty(@"lease_escalation_modified_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? lease_escalation_modified_date { get; set; }

        [JsonProperty(@"ref_lease_escalation_value_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_escalation_value_uom_desc { get; set; }

        [JsonProperty(@"non_contiguous_floor_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? non_contiguous_floor_f { get; set; }

        [JsonProperty(@"actual_lease_rate_yearly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? actual_lease_rate_yearly { get; set; }

        [JsonProperty(@"ref_actual_lease_rate_yearly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_actual_lease_rate_yearly_uom_desc { get; set; }

        [JsonProperty(@"actual_sales_price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? actual_sales_price { get; set; }

        [JsonProperty(@"ref_actual_sales_price_yearly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_actual_sales_price_yearly_uom_desc { get; set; }

        [JsonProperty(@"amperage_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string amperage_descr { get; set; }

        [JsonProperty(@"area_not_leased", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? area_not_leased { get; set; }

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

        [JsonProperty(@"building_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? building_area { get; set; }

        [JsonProperty(@"ref_building_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_building_area_uom_desc { get; set; }

        [JsonProperty(@"price_per_acre", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? price_per_acre { get; set; }

        [JsonProperty(@"ref_price_per_acre_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_price_per_acre_uom_desc { get; set; }

        [JsonProperty(@"cam_charges", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cam_charges { get; set; }

        [JsonProperty(@"ref_cam_charges_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_cam_charges_uom_desc { get; set; }

        [JsonProperty(@"electric_charges", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? electric_charges { get; set; }

        [JsonProperty(@"ref_electric_charges_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_electric_charges_uom_desc { get; set; }

        [JsonProperty(@"free_rent_months", NullValueHandling = NullValueHandling.Ignore)]
        public int? free_rent_months { get; set; }

        [JsonProperty(@"ref_free_rent_months_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_free_rent_months_uom_desc { get; set; }

        [JsonProperty(@"effective_rent_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? effective_rent_monthly { get; set; }

        [JsonProperty(@"ref_effective_rent_monthly_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_effective_rent_monthly_uom_desc { get; set; }

        [JsonProperty(@"lease_arrangement_comments", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_arrangement_comments { get; set; }

        [JsonProperty(@"lease_arrangement_long_note", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_arrangement_long_note { get; set; }

        [JsonProperty(@"lease_arrangement_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_arrangement_notes { get; set; }

        [JsonProperty(@"lease_arrangement_space_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? lease_arrangement_space_area { get; set; }

        [JsonProperty(@"ref_lease_arrangement_space_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_arrangement_space_area_uom_desc { get; set; }

        [JsonProperty(@"electrical_phase_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string electrical_phase_descr { get; set; }

        [JsonProperty(@"number_of_days_in_market", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_days_in_market { get; set; }

        [JsonProperty(@"occupancy_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? occupancy_date { get; set; }

        [JsonProperty(@"sprinkler_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string sprinkler_descr { get; set; }

        [JsonProperty(@"arrangement_options", NullValueHandling = NullValueHandling.Ignore)]
        public List<ArrangementOption> arrangement_options { get; set; }

        [JsonProperty(@"site_area", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? site_area { get; set; }

        [JsonProperty(@"ref_site_area_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_site_area_uom_desc { get; set; }

        [JsonProperty(@"triple_net_lease_charges", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? triple_net_lease_charges { get; set; }

        [JsonProperty(@"ref_triple_net_lease_charges_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_triple_net_lease_charges_uom_desc { get; set; }

        [JsonProperty(@"vacant_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? vacant_f { get; set; }

        [JsonProperty(@"voltage_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string voltage_descr { get; set; }

        [JsonProperty(@"ref_retail_location_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_retail_location_type_desc { get; set; }

        [JsonProperty(@"tenant_short_descr", NullValueHandling = NullValueHandling.Ignore)]
        public string tenant_short_descr { get; set; }

        [JsonProperty(@"ref_tenancy_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_tenancy_type_desc { get; set; }

        [JsonProperty(@"ref_lease_activity_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_activity_type_desc { get; set; }

        [JsonProperty(@"ref_lease_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_type_desc { get; set; }

        [JsonProperty(@"sublease_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? sublease_f { get; set; }

        [JsonProperty(@"rent_escalation_comments", NullValueHandling = NullValueHandling.Ignore)]
        public string rent_escalation_comments { get; set; }

        [JsonProperty(@"lease_arrangement_close_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? lease_arrangement_close_date { get; set; }

        [JsonProperty(@"number_of_property", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_of_property { get; set; }

        [JsonProperty(@"ref_rail_service_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_rail_service_type_desc { get; set; }

        [JsonProperty(@"ref_development_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_development_type_desc { get; set; }

        [JsonProperty(@"lease_term", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_term { get; set; }

        [JsonProperty(@"ref_lease_term_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_term_uom_desc { get; set; }

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

        [JsonProperty(@"ref_party_category_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_party_category_type_desc { get; set; }

        [JsonProperty(@"ref_cbre_industry_code_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_cbre_industry_code_desc { get; set; }

        [JsonProperty(@"ref_naics_classification_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_naics_classification_desc { get; set; }

        [JsonProperty(@"prime_property_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? prime_property_f { get; set; }

        [JsonProperty(@"tax_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? tax_amount { get; set; }

        [JsonProperty(@"ref_tax_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_tax_amount_uom_desc { get; set; }

        [JsonProperty(@"tax_assessment_year", NullValueHandling = NullValueHandling.Ignore)]
        public int? tax_assessment_year { get; set; }

        [JsonProperty(@"property_zoning_code", NullValueHandling = NullValueHandling.Ignore)]
        public string property_zoning_code { get; set; }

        [JsonProperty(@"ref_property_class_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_property_class_type_desc { get; set; }

        [JsonProperty(@"lease_arrangement_total_consideration", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? lease_arrangement_total_consideration { get; set; }

        [JsonProperty(@"ref_lease_arrangement_total_consideration_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_arrangement_total_consideration_uom_desc { get; set; }

        [JsonProperty(@"lease_arrangement_savings_comments", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_arrangement_savings_comments { get; set; }

        [JsonProperty(@"lease_arrangement_savings_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? lease_arrangement_savings_amount { get; set; }

        [JsonProperty(@"ref_lease_arrangement_savings_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_lease_arrangement_savings_amount_uom_desc { get; set; }

        [JsonProperty(@"previous_years_rent", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? previous_years_rent { get; set; }

        [JsonProperty(@"ref_previous_years_rent_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_previous_years_rent_uom_desc { get; set; }

        [JsonProperty(@"ref_notice_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_notice_type_desc { get; set; }

        [JsonProperty(@"notice_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? notice_date { get; set; }

        [JsonProperty(@"lease_arrangement_owner_id", NullValueHandling = NullValueHandling.Ignore)]
        public string lease_arrangement_owner_id { get; set; }

        [JsonProperty(@"number_parking_space", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_parking_space { get; set; }

        [JsonProperty(@"reimbursement_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string reimbursement_notes { get; set; }

        [JsonProperty(@"record_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_notes { get; set; }

        [JsonProperty(@"operating_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? operating_expense_amount { get; set; }

        [JsonProperty(@"ref_operating_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_operating_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"other_expense_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? other_expense_amount { get; set; }

        [JsonProperty(@"ref_other_expense_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_other_expense_amount_uom_desc { get; set; }

        [JsonProperty(@"dock_door_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? dock_door_f { get; set; }

        [JsonProperty(@"number_reserved_parking_spaces", NullValueHandling = NullValueHandling.Ignore)]
        public int? number_reserved_parking_spaces { get; set; }

        [JsonProperty(@"full_service_gross_amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? full_service_gross_amount { get; set; }

        [JsonProperty(@"ref_full_service_gross_amount_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_full_service_gross_amount_uom_desc { get; set; }

        [JsonProperty(@"cost_per_reserved_parking_space", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cost_per_reserved_parking_space { get; set; }

        [JsonProperty(@"ref_cost_per_reserved_parking_space_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_cost_per_reserved_parking_space_uom_desc { get; set; }

        [JsonProperty(@"cost_per_nonreserved_parking_space_monthly", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cost_per_nonreserved_parking_space_monthly { get; set; }

        [JsonProperty(@"cost_per_nonreserved_parking_space_monthly_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string cost_per_nonreserved_parking_space_monthly_uom { get; set; }

        [JsonProperty(@"cost_per_nonreserved_parking_space_daily", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? cost_per_nonreserved_parking_space_daily { get; set; }

        [JsonProperty(@"cost_per_nonreserved_parking_space_daily_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string cost_per_nonreserved_parking_space_daily_uom { get; set; }

        [JsonProperty(@"maintenance_cost", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? maintenance_cost { get; set; }

        [JsonProperty(@"ref_maintenance_cost_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_maintenance_cost_uom_desc { get; set; }

        [JsonProperty(@"early_termination_date", NullValueHandling = NullValueHandling.Ignore)]
        public string early_termination_date { get; set; }

        [JsonProperty(@"ref_record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_record_source_desc { get; set; }

        [JsonProperty(@"record_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string record_source_desc { get; set; }

        [JsonProperty(@"source_details", NullValueHandling = NullValueHandling.Ignore)]
        public string source_details { get; set; }

        [JsonProperty(@"data_acquired_from_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string data_acquired_from_desc { get; set; }

        [JsonProperty(@"arrangement_source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string arrangement_source_notes { get; set; }

        [JsonProperty(@"voltage_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string voltage_desc { get; set; }

        [JsonProperty(@"source_notes", NullValueHandling = NullValueHandling.Ignore)]
        public string source_notes { get; set; }

        [JsonProperty(@"account_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string account_type_desc { get; set; }

        [JsonProperty(@"prior_location_status_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string prior_location_status_type_desc { get; set; }

        [JsonProperty(@"property_usage_sub_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_usage_sub_type_desc { get; set; }

        [JsonProperty(@"property_sprinkler_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string property_sprinkler_type_desc { get; set; }

        [JsonProperty(@"property_date_in_market", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? property_date_in_market { get; set; }

        [JsonProperty(@"rent_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string rent_type_desc { get; set; }

        [JsonProperty(@"lock_in_duration", NullValueHandling = NullValueHandling.Ignore)]
        public string lock_in_duration { get; set; }

        [JsonProperty(@"lock_in_duration_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string lock_in_duration_uom { get; set; }

        [JsonProperty(@"base_rent_daily", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? base_rent_daily { get; set; }

        [JsonProperty(@"ref_base_rent_daily_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_base_rent_daily_uom_desc { get; set; }

        [JsonProperty(@"effective_rent_daily", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? effective_rent_daily { get; set; }

        [JsonProperty(@"ref_effective_rent_daily_uom_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_effective_rent_daily_uom_desc { get; set; }

        [JsonProperty(@"rent_escalation_custom", NullValueHandling = NullValueHandling.Ignore)]
        public List<LeaseEscalation> rent_escalation_custom { get; set; }

        [JsonProperty(@"rent_escalation_custom_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? rent_escalation_custom_f { get; set; }

        [JsonProperty(@"agilecomp", NullValueHandling = NullValueHandling.Ignore)]
        public AgileComp agilecomp { get; set; }

        [JsonProperty(@"property_id", Required = Required.Always)]
        public int property_id { get; set; }

        [JsonProperty(@"property_usage_id", Required = Required.Always)]
        public int property_usage_id { get; set; }

        [JsonProperty(@"floor_suite", NullValueHandling = NullValueHandling.Ignore)]
        public List<LeaseCompFloorNSuite> floor_suite { get; set; }

        [JsonProperty(@"company_contact_role_addresses", Required = Required.Always)]
        public List<CompanyContactRoleAddress> company_contact_role_addresses { get; set; }

        [JsonProperty(@"user_tag", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserTag> user_tag { get; set; }

        [JsonProperty(@"fit_out_period", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? fit_out_period { get; set; }

        [JsonProperty(@"fit_out_period_uom", NullValueHandling = NullValueHandling.Ignore)]
        public string fit_out_period_uom { get; set; }

        [JsonProperty(@"brand_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? brand_id { get; set; }

        [JsonProperty(@"market_rent_trend_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string market_rent_trend_type_desc { get; set; }

        [JsonProperty(@"base_rent_comment", NullValueHandling = NullValueHandling.Ignore)]
        public string base_rent_comment { get; set; }

        [JsonProperty(@"effective_rent_comment", NullValueHandling = NullValueHandling.Ignore)]
        public string effective_rent_comment { get; set; }

        [JsonProperty(@"external_opportunity_number_source_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string external_opportunity_number_source_desc { get; set; }

        [JsonProperty(@"external_opportunity_number", NullValueHandling = NullValueHandling.Ignore)]
        public string external_opportunity_number { get; set; }

        [JsonProperty(@"property_measurement_arrangement", NullValueHandling = NullValueHandling.Ignore)]
        public List<PropertyMeasurement> property_measurement_arrangement { get; set; }

        [JsonProperty(@"miq_record_hide_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? miq_record_hide_f { get; set; }
    }
}
