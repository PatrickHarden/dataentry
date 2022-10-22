using System;
using System.Globalization;
using System.Text.RegularExpressions;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Utility;

namespace dataentry.Services.Integration.Edp.Consumption
{
    public class ConsumptionQueries
    {
        private readonly ConsumptionOptions _consumptionOptions;
        private readonly string _appName;
        private readonly string _userId;
        private readonly string _userName;
        private readonly string _email;
        public ConsumptionQueries(ConsumptionOptions consumptionOptions)
        {
            _consumptionOptions = consumptionOptions;
            if (_consumptionOptions.Enabled)
            {
                _appName = _consumptionOptions.AppName ??
                    throw new ArgumentNullException(nameof(_consumptionOptions.AppName));
                _userId = _consumptionOptions.UserId ??
                    throw new ArgumentNullException(nameof(_consumptionOptions.UserId));
                _userName = _consumptionOptions.UserName ??
                    throw new ArgumentNullException(nameof(_consumptionOptions.UserName));
                _email = _consumptionOptions.Email ??
                    throw new ArgumentNullException(nameof(_consumptionOptions.Email));
            }
        }

        private static string RemoveSpecialCharacters(string str)
        {
            //To do: convert special characters to ASCII
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
        }

        private string SourceInfoFormatter(string requestId) => $"sourceInfo:   {{    app_name: \"{_appName}\",    user_id: \"{_userId}\",    username: \"{_userName}\",    email: \"{_email}\",    request_id: \"{requestId}\"   }}";

        public string GetSearchPropertyQuery(string keyword, string country, string requestId)
        {
            string filterByCountry = null;
            if (!string.IsNullOrWhiteSpace(country))
            {
                // Explicitly defined country names
                filterByCountry = country?.ToEnum<CountryEnum>(AliasType.MIQ)?.ToAlias(AliasType.MIQ);

                if (string.IsNullOrWhiteSpace(filterByCountry))
                {
                    // Fallback to .NET defined country names
                    filterByCountry = new RegionInfo(country)?.EnglishName;
                }
            }
            string filterByCountryCondition = !string.IsNullOrWhiteSpace(filterByCountry) ? $"AND postal_address.country = '{filterByCountry}'" : "";
            var isNumeric = int.TryParse(keyword, out _);
            string filterByMiqID = isNumeric ? $"entity.id = {keyword} OR" : "";
            return SearchPropertyQuery(requestId, filterByMiqID, filterByCountryCondition, keyword);
        }
        public string SearchPropertyQuery(string requestId, string filterByMiqID, string filterByCountryCondition, string keyword) =>
            $@"query{{  
                searchProperty
                    (  
                        {SourceInfoFormatter(requestId)},  
                        searchParams:
                        {{    
                            advanceSearch: 
                            {{ 
                                condition:""({filterByMiqID} property_name LIKE '%{RemoveSpecialCharacters(keyword)}%' OR postal_address.full_street_name LIKE '%{RemoveSpecialCharacters(keyword)}%') AND postal_address.postal_code IS NOT NULL {filterByCountryCondition}""
                            }}  
                        }}
                    )  
                    {{    
                        totalCount
                        properties
                        {{      
                            entity      
                            {{        
                                id
                                source_lineage {{
                                    source_system
                                    source_unique_id
                                }}
                            }}      
                        property_name      
                        postal_address      
                        {{        
                            full_street_name
                            street1
                            street2
                            city
                            state_province
                            postal_code
                            country      
                        }}      
                        usages      
                        {{        
                            ref_property_usage_type_desc      
                        }}    
                    }}  
                }}
            }}";
        public string GetPropertyQuery(int id, string requestId) =>
            $@"query {{
                searchProperty(
                    {SourceInfoFormatter(requestId)},
                    searchParams: {{
                        advanceSearch: {{
                            condition:""entity.id = {id}""
                        }}
                    }}
                )
                {{
                    properties {{
                        entity {{
                            id
                            source_lineage {{
                                source_system
                                source_unique_id
                            }}
                        }}
                        property_name
                        property_descr
                        type_desc
                        property_class_type_desc
                        ref_property_status_desc
                        year_property_built
                        postal_address {{
                            full_street_name
                            street1
                            street2
                            city
                            state_province
                            postal_code
                            country
                            latitude
                            longitude
                        }}
                        alternate_postal_addresses {{
                            full_street_name
                            street1
                            street2
                            city
                            state_province
                            postal_code
                            country
                            latitude
                            longitude
                        }}
                        usages {{
                            date_property_available
                            primary_usage_f
                            ref_property_usage_type_desc
                            available_land_area
                            ref_sublease_rate_type_desc
                            ref_direct_rate_type_desc
                            minimum_yearly_direct_lease_rate_amount
                            minimum_yearly_direct_lease_rate_amount_uom_desc
                            maximum_yearly_direct_lease_rate
                            maximum_yearly_direct_lease_rate_uom_desc
                            minimum_yearly_sub_lease_rate
                            minimum_yearly_sub_lease_rate_uom_desc
                            maximum_yearly_sub_lease_rate
                            maximum_yearly_sub_lease_rate_uom_desc
                            minimum_monthly_direct_lease_rate
                            minimum_monthly_direct_lease_rate_uom_desc
                            maximum_monthly_direct_lease_rate
                            maximum_monthly_direct_lease_rate_uom_desc
                            minimum_monthly_sub_lease_rate
                            minimum_monthly_sub_lease_rate_uom_desc
                            maximum_monthly_sub_lease_rate
                            maximum_monthly_sub_lease_rate_uom_desc
                            minimum_available_area
                            minimum_divisible_surface
                            total_available_surface_area
                            maintenence_cost
                            maintenence_cost_uom_desc
                            tax_expense_amount
                            tax_expense_amount_uom_desc
                            other_expense_amount
                            other_expense_amount_uom_desc
                            total_available_surface_area_uom_desc
                            company_contact_role_addresses {{
                                role_desc
                                first_name
                                last_name
                                email
                                full_phone_number
                            }}
                            usage_amenity {{
                                property_amenity_type_desc
                                amenity_subtype_desc
                                amenity_notes
                            }}
                        }}
                        property_note {{
                            property_notes_type_desc
                            notes
                            colloquial {{
                                language_desc
                                country_code_desc
                                notes
                            }}
                        }}
                        total_gross_area
                        total_gross_area_uom_desc
                        total_land_area
                        total_land_area_uom_desc
                        property_amenity {{
                            id
                            amenity_name
                            property_amenity_type_desc
                            amenity_type_desc
                        }}
                        property_media_information {{
                            id
                            media_name
                            media_path
                            primary_image_f
                            media_caption
                            media_type_desc
                            display_order
                            ref_property_image_type_desc
                            watermark_label
                            publish_image_f
                            media_content_type_desc
                        }}
                        property_measurement {{
                            id
                            property_measurement_size
                            property_element_type_desc
                            property_measurement_notes
                            property_measurement_size_uom
                        }}
                        property_external_ratings {{
                            green_building_cert_level_desc
                            green_building_rating_type_desc
                        }}
                        property_notes
                        record_source_desc
                        source_details
                        ref_data_acquired_from_desc
                        record_source_notes
                        digital {{
                            id
                            digital_type_desc
                            usage_type_desc
                            digital_address
                        }}
                    }}
                }}
            }}";

        public string GetAvailabilityQuery(int id, string requestId) =>
            $@"query Availability_AdvanceSearch {{ 
                searchAvailability (   
                    {SourceInfoFormatter(requestId)},   
                    searchParams: {{    
                        advanceSearch: {{ condition:""property.id = {id} and entity.transaction_flag != 'D' and (ref_space_availability_status_desc = 'Active' or ref_space_availability_status_desc = 'Current') and company_contact_role_addresses.company_name LIKE '%CBRE%' ""}}   
                    }}
                )
                {{   
                    totalCount
                    availability {{    
                        entity {{
                            id
                            source_lineage {{
                                source_system
                                source_unique_id 
                            }}       
                        }}
                        property {{      
                            property_name
                            property_usage {{  
                                id
                                ref_property_usage_type_desc   
                                usage_amenity {{
                                    property_amenity_type_desc
                                    amenity_subtype_desc
                                    amenity_notes
                                }} 
                            }}    
                            property_media_information {{
                                id
                                media_path  
                            }}
                        }}   
                        listing_ids {{
                            listing_id,
                            listing_source
                        }}
                        floor_suite {{      
                            floor_number      
                            suite_number    
                        }}
                        property_measurement_availability {{
                            id
                            property_measurement_size
                            property_element_type_desc
                            property_measurement_size_uom
                        }}
                        date_available    
                        listing_notes    
                        minumum_lease_term    
                        ref_availability_type_desc    
                        ref_lease_type_desc    
                        asking_rental_rate_monthly    
                        asking_rental_rate_monthly_uom_desc    
                        asking_rental_rate_yearly    
                        asking_rental_rate_yearly_uom_desc    
                        asking_price_for_sale    
                        asking_price_for_sale_uom_desc        
                        minimum_asking_rate_monthly    
                        minimum_asking_rate_monthly_uom    
                        maximum_asking_rate_monthly    
                        maximum_asking_rate_monthly_uom    
                        minimum_asking_rate_yearly    
                        minimum_asking_rate_yearly_uom    
                        maximum_asking_rate_yearly    
                        maximum_asking_rate_yearly_uom    
                        date_on_market    
                        date_available    
                        listing_descr    
                        available_space    
                        available_space_uom_desc    
                        total_area_of_space    
                        total_area_of_space_uom_desc    
                        available_office_space    
                        available_office_space_uom_desc    
                        total_contiguous_area_of_space    
                        total_contiguous_area_of_space_uom_desc    
                        ceiling_height  
                        ref_space_availability_status_desc  
                    }} 
                }}
            }}";
    }
}