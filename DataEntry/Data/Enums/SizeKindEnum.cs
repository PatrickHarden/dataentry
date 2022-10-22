using dataentry.Utility;

namespace dataentry.Data.Enums
{
    public enum SizeKindEnum
    {
        [Alias(AliasType.StoreApi, "SuperArea")]
        SuperArea,
        [Alias(AliasType.StoreApi, "OfficeSize")]
        OfficeArea,
        [Alias(AliasType.StoreApi, "OfficeSize", Primary = true)]
        OfficeSize,
        [Alias(AliasType.StoreApi, "TotalContiguousSpace")]
        TotalContiguousSpace,
        [Alias(AliasType.StoreApi, "MinimumCeilingHeight")]
        MinimumCeilingHeight,
        [Alias(AliasType.StoreApi, "LandSize")]
        LandSize,
        [Alias(AliasType.StoreApi, "MezzanineSize")]
        MezzanineArea,
        [Alias(AliasType.StoreApi, "WarehouseSize")]
        WarehouseArea,
        [Alias(AliasType.StoreApi, "MezzanineSize", Primary = true)]
        MezzanineSize,
        [Alias(AliasType.StoreApi, "WarehouseSize", Primary = true)]
        WarehouseSize,
        [Alias(AliasType.StoreApi, "CanopyArea")]
        CanopyArea,
        [Alias(AliasType.StoreApi, "YardArea")]
        YardArea,
        [Alias(AliasType.StoreApi, "SiteArea")]
        SiteArea,
        [Alias(AliasType.StoreApi, "LoadingDockArea")]
        LoadingDockArea,
        [Alias(AliasType.StoreApi, "AverageFloorArea")]
        AverageFloorArea,
        [Alias(AliasType.StoreApi, "HotelArea")]
        HotelArea,
        [Alias(AliasType.StoreApi, "AverageRoomArea")]
        AverageRoomArea,
        [Alias(AliasType.StoreApi, "OtherArea")]
        OtherArea,
        [Alias(AliasType.StoreApi, "RetailArea")]
        RetailArea,
        [Alias(AliasType.StoreApi, "FactoryArea")]
        FactoryArea,
        [Alias(AliasType.StoreApi, "ClimateControlArea")]
        ClimateControlArea,
        [Alias(AliasType.StoreApi, "DangerousGoodsArea")]
        DangerousGoodsArea,
        [Alias(AliasType.StoreApi, "FloorVoidSize")]
        FloorVoidSize,
        [Alias(AliasType.StoreApi, "TotalArea")]
        TotalArea,
        [Alias(AliasType.StoreApi, "ShowroomSize")]
        ShowroomSize,
        [Alias(AliasType.StoreApi, "MaximumCeilingHeight")]
        MaximumCeilingHeight,
        [Alias(AliasType.StoreApi, "MinimumSize")]
        MinimumSize,
        [Alias(AliasType.StoreApi, "MaximumSize")]
        MaximumSize,
        [Alias(AliasType.StoreApi, "TotalSize")]
        TotalSize
    }
}