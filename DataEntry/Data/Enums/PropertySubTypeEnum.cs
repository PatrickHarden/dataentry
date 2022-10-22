using dataentry.Utility;

namespace dataentry.Data.Enums
{
    public enum PropertySubTypeEnum
    {
        [Alias(AliasType.StoreApi, "OfficeLand")]
        office,
        [Alias(AliasType.StoreApi, "ResidentialLand")]
        residental,
        [Alias(AliasType.StoreApi, "RetailLand")]
        retail
    }
}
