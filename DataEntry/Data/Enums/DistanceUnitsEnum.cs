using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum DistanceUnitsEnum
    {
        [Alias(StoreApi, "metre")]
        metre,
        [Alias(StoreApi, "yard")]
        yard,
        [Alias(StoreApi, "kilometre")]
        kilometre,
        [Alias(StoreApi, "mile")]
        mile,
        [Alias(StoreApi, "blocks")]
        blocks,
        [Alias(StoreApi, "onlocation")]
        onlocation,
    }
}