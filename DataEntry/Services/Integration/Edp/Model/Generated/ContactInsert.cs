using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ContactInsert : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{request{request_id,action,source_system_name,source_id,source_submitter_name,source_submitter_email,user_role,comment,global_region,country},first_name,last_name,familiar_name,legal_name,middle_name,maiden_name,salutation,suffix,title,ref_individual_status_desc,digital{digital_address,digital_type_desc,usage_type_desc,digital_address_display_text},phone{local_area_code,phone_number,extension_number,phone_type_desc,usage_type_desc,location_id},company_role_addresses{company_id,role_desc,location_id,effective_date},licenses{license_number,license_issuing_entity,license_expiry_date},cbre_employee_f,do_not_call_f,email_bounced_f,created_by_user_country_code,ref_call_option_type_desc,ref_mail_option_type_desc,broker_f,gdpr_consent_date,ref_email_option_type_desc,ref_fax_option_type_desc,user_tag{name}}";

        [JsonProperty(@"request", Required = Required.Always)]
        public RequestDetails request { get; set; }

        [JsonProperty(@"first_name", Required = Required.Always)]
        public string first_name { get; set; }

        [JsonProperty(@"last_name", Required = Required.Always)]
        public string last_name { get; set; }

        [JsonProperty(@"familiar_name", NullValueHandling = NullValueHandling.Ignore)]
        public string familiar_name { get; set; }

        [JsonProperty(@"legal_name", NullValueHandling = NullValueHandling.Ignore)]
        public string legal_name { get; set; }

        [JsonProperty(@"middle_name", NullValueHandling = NullValueHandling.Ignore)]
        public string middle_name { get; set; }

        [JsonProperty(@"maiden_name", NullValueHandling = NullValueHandling.Ignore)]
        public string maiden_name { get; set; }

        [JsonProperty(@"salutation", NullValueHandling = NullValueHandling.Ignore)]
        public string salutation { get; set; }

        [JsonProperty(@"suffix", NullValueHandling = NullValueHandling.Ignore)]
        public string suffix { get; set; }

        [JsonProperty(@"title", NullValueHandling = NullValueHandling.Ignore)]
        public string title { get; set; }

        [JsonProperty(@"ref_individual_status_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_individual_status_desc { get; set; }

        [JsonProperty(@"digital", Required = Required.Always)]
        public List<Digital> digital { get; set; }

        [JsonProperty(@"phone", NullValueHandling = NullValueHandling.Ignore)]
        public List<Phone> phone { get; set; }

        [JsonProperty(@"company_role_addresses", Required = Required.Always)]
        public List<CompanyRoleAddress> company_role_addresses { get; set; }

        [JsonProperty(@"licenses", NullValueHandling = NullValueHandling.Ignore)]
        public List<Licenses> licenses { get; set; }

        [JsonProperty(@"cbre_employee_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? cbre_employee_f { get; set; }

        [JsonProperty(@"do_not_call_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? do_not_call_f { get; set; }

        [JsonProperty(@"email_bounced_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? email_bounced_f { get; set; }

        [JsonProperty(@"created_by_user_country_code", NullValueHandling = NullValueHandling.Ignore)]
        public string created_by_user_country_code { get; set; }

        [JsonProperty(@"ref_call_option_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_call_option_type_desc { get; set; }

        [JsonProperty(@"ref_mail_option_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_mail_option_type_desc { get; set; }

        [JsonProperty(@"broker_f", NullValueHandling = NullValueHandling.Ignore)]
        public bool? broker_f { get; set; }

        [JsonProperty(@"gdpr_consent_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? gdpr_consent_date { get; set; }

        [JsonProperty(@"ref_email_option_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_email_option_type_desc { get; set; }

        [JsonProperty(@"ref_fax_option_type_desc", NullValueHandling = NullValueHandling.Ignore)]
        public string ref_fax_option_type_desc { get; set; }

        [JsonProperty(@"user_tag", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserTag> user_tag { get; set; }
    }
}
