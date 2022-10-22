using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum TravelModesEnum
    {
        [Alias(StoreApi, "drive")]
        drive,
        [Alias(StoreApi, "walk")]
        walk
    }
}