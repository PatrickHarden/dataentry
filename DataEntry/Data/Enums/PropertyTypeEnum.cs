using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum PropertyTypeEnum
    {
        [Alias(StoreApi, "Retail")]
        retail,
        [Alias(StoreApi, "Office")]
        office,
        [Alias(StoreApi, "Industrial")]
        industrial,
        [Alias(StoreApi, "FlexOffice", Primary = true)]
        flex,
        [Alias(StoreApi, "FlexOffice")]
        flexOffice,
        [Alias(StoreApi, "Residential")]
        residential,
        [Alias(StoreApi, "Shophouse")]
        shophouse,
        [Alias(StoreApi, "OfficeCoworking")]
        officecoworking,
        [Alias(StoreApi, "FlexIndustrial")]
        flexindustrial,
        [Alias(StoreApi, "Land")]
        land,
        [Alias(StoreApi, "Healthcare")]
        healthcare,
        [Alias(StoreApi, "SpecialPurpose")]
        specialPurpose,
        [Alias(StoreApi, "Hospitality")]
        hospitality,
        [Alias(StoreApi, "Multifamily")]
        multifamily,
        [Alias(StoreApi, "DataCentre")]
        dataCentre,
        [Alias(StoreApi, "LifeSciences")]
        lifeSciences,
        [Alias(StoreApi, "Logistics")]
        logistics,
        [Alias(StoreApi, "PetroleumAutomotive")]
        petroleumAutomotive,
        [Alias(StoreApi, "Other")]
        other,
        [Alias(StoreApi, "Alternatives")]
        alternatives,
        [Alias(StoreApi, "Investment")]
        investment,
    }
}