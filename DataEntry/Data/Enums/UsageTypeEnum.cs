using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum UsageTypeEnum
    {
        //     Commercial,
        //     Residential,
        [Alias(StoreApi, "Retail")]
        Retail,
        [Alias(StoreApi, "Office")]
        Office,
        [Alias(StoreApi, "Industrial")]
        Industrial,
        [Alias(StoreApi, "FlexOffice")]
        Flex,
        [Alias(StoreApi, "Logistics")]
        Logistics
        // DataCentre,
        // Hotels,
        // Agriculture,
        // PetroleumAutomotive,
        // Land,
        // Healthcare,
        // Investment,
        // LifeSciences,
        // SpecialPurpose,
        // AncillarySpace,
        // ColdChilledStores,
        // Logistics,
        // OpenStorage,
        // Site,
        // Religious,
        // IndoorRecreational,
        // OutdoorRecreational,
        // Pubs,
        // Restaurants,
        // Leisure,
        // DevelopmentLand,
        // HotelsAndLicensed,
    }
}