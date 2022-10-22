using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum TransportationTypesEnum
    {
        [Alias(Default, "Transit/Subway")]
        [Alias(StoreApi, "Transit/Subway")]
        TransitSubway,

        [Alias(Default, "Commuter Rail")]
        [Alias(StoreApi, "CommuterRail")]
        CommuterRail,

        [Alias(StoreApi, "Airport")]
        Airport,

        [Alias(StoreApi, "Ferry")]
        Ferry,

        [Alias(StoreApi, "Shipyard")]
        Shipyard,

        [Alias(StoreApi, "Highway")]
        Highway,
    }
}