using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum UnitStatus
    {
        [Alias(StoreApi, "UnderOption")]
        unavailable,
        [Alias(StoreApi, "Available")]
        available
    }
}