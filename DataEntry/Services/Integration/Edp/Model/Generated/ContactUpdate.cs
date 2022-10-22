using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace dataentry.Services.Integration.Edp.Model
{
    public partial class ContactUpdate : EdpGraphQLObject
    {
        [JsonIgnore]
        public override string ResultFields => @"{first_name,last_name,familiar_name,legal_name,middle_name,maiden_name,salutation,suffix,title,ref_individual_status_desc,cbre_employee_f,do_not_call_f,email_bounced_f,created_by_user_country_code,ref_call_option_type_desc,ref_mail_option_type_desc,broker_f,gdpr_consent_date,ref_email_option_type_desc,ref_fax_option_type_desc,verified_ts,verified_by}";

        [JsonProperty(@"first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string first_name { get; set; }

        [JsonProperty(@"last_name", NullValueHandling = NullValueHandling.Ignore)]
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

        [JsonProperty(@"verified_ts", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? verified_ts { get; set; }

        [JsonProperty(@"verified_by", NullValueHandling = NullValueHandling.Ignore)]
        public string verified_by { get; set; }
    }
}
