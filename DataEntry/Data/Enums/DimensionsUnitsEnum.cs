using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum DimensionsUnitsEnum
    {
        [Alias(StoreApi, "sqft", Primary = true)]
        sqft,
        [Alias(StoreApi, "sqft")]
        sf,
        [Alias(StoreApi, "sqm")]
        sm,
        [Alias(StoreApi, "sqm", Primary = true)]
        sqm,
        [Alias(StoreApi, "hectare")]
        hectare,
        [Alias(StoreApi, "acre")]
        acre,
        [Alias(StoreApi, "ft")]
        ft,
        [Alias(StoreApi, "yd")]
        yd,
        [Alias(StoreApi, "m")]
        m,
        [Alias(StoreApi, "pp")]
        desk,
        [Alias(StoreApi, "pp", Primary = true)]
        person,
        [Alias(StoreApi, "pp")]
        room,
        [Alias(StoreApi, "Whole")]
        whole
    }
}