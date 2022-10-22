using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum ChargeKindEnum
    {
        [Alias(StoreApi, "SalePrice")]
        sale,
        [Alias(StoreApi, "Rent")]
        lease,
        [Alias(StoreApi, "Rent")]
        salelease,
        [Alias(StoreApi, "FlexRent")]
        FlexRent,
        [Alias(StoreApi, "Rent", Primary = true)]
        Rent,
        [Alias(StoreApi, "SalePrice", Primary = true)]
        SalePrice,
        [Alias(StoreApi, "AskingRentOffice")]
        AskingRentOffice,
        [Alias(StoreApi, "AskingRentMezzanine")]
        AskingRentMezzanine,
        [Alias(StoreApi, "AskingRentWarehouse")]
        AskingRentWarehouse,
        [Alias(StoreApi, "BusinessRates")]
        BusinessRates,
        [Alias(StoreApi, "ServiceCharge")]
        ServiceCharge,
        [Alias(StoreApi, "EstateCharge")]
        EstateCharge
    }
}