using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum UseTypeEnum
    {
        [Alias(StoreApi, "Retail")]
        retail,
        [Alias(StoreApi, "Office")]
        office,
        [Alias(StoreApi, "Industrial")]
        industrial,
        [Alias(StoreApi, "HotDesk")]
        [Alias(StoreApiRelatedAspsect, "hasHotdesks")]
        [Alias(StoreApiDisplay, "Hot Desks")]
        hot,
        [Alias(StoreApi, "FixedDesk")]
        [Alias(StoreApiRelatedAspsect, "hasFixeddesks")]
        [Alias(StoreApiDisplay, "Fixed Desks")]
        @fixed,
        [Alias(StoreApi, "ServicedOffice")]
        [Alias(StoreApiRelatedAspsect, "hasServicedOffices")]
        [Alias(StoreApiDisplay, "Serviced Offices")]
        serviced
    }
}