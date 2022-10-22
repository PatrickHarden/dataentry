using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum StatusEnum
    {
        [Alias(StoreApi, "Unavailable")]
        Unavailable,
        [Alias(StoreApi, "Available")]
        Available
    }
}