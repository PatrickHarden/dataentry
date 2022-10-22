using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum AspectsEnum
    {
        [Alias(StoreApi, "isSale")]
        sale,

        [Alias(StoreApi, "isLetting")]
        lease,

        salelease,

        [Alias(StoreApi, "isInvestment")]
        investment,

        hasWheelchairAccess,

        [Alias(MIQ, "Showers", "Shower(s)")]
        hasShowers,

        [Alias(MIQ, "24/7 Access", "24 Hour Access")]
        has24HourAccess,

        [Alias(MIQ, "Break-Out Areas", "Break Out Area", "Breakout Space / Business Lounge")]
        hasBreakoutSpace,

        [Alias(MIQ, "Rooftop Terrace", "Roof Terrace")]
        hasRoofTerrace,

        [Alias(MIQ, "Coffee / tea / beer", "Tea & Coffee Included")]
        hasTeaCoffee,

        [Alias(MIQ, "Meeting / Conference Facilities", "Meeting Rooms")]
        hasFreeMeetingRooms,

        [Alias(MIQ, "Cleaning Services", "Cleaning")]
        hasCleaning,

        [Alias(AliasType.StoreApi, "hasReception-Manned")]
        [Alias(MIQ, "Reception Services", "Reception|Manned")]
        hasReceptionManned,

        [Alias(MIQ, "Bike Racks", "Bicycle Racks")]
        hasBicycleRack
    }
}