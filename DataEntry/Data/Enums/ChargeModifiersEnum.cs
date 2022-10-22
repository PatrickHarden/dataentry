using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum ChargeModifiersEnum
    {
        [Alias(StoreApi, "Statuatory")]
        Statuatory,
        [Alias(StoreApi, "Asking")]
        Asking,
        [Alias(StoreApi, "OnApplication")]
        OnApplication,
        [Alias(StoreApi, "Guide")]
        Guide,
        [Alias(StoreApi, "From")]
        From,
        [Alias(StoreApi, "To")]
        To,
        [Alias(StoreApi, "OffersInExcessOf")]
        OffersInExcessOf,
        [Alias(StoreApi, "Fixed")]
        Fixed,
        [Alias(StoreApi, "ReducedTo")]
        ReducedTo,
        [Alias(StoreApi, "CallForInfo")]
        CallForInfo,
        [Alias(StoreApi, "Estimated")]
        Estimated,
        [Alias(StoreApi, "PriceIsNegotiable")]
        PriceIsNegotiable,
        [Alias(StoreApi, "PriceOnListing")]
        PriceOnListing,
        [Alias(StoreApi, "Auction")]
        Auction,
        [Alias(StoreApi, "DeadlineSale")]
        DeadlineSale,
        [Alias(StoreApi, "ExpressionsOfInterest")]
        ExpressionsOfInterest,
        [Alias(StoreApi, "Outgoings")]
        Outgoings,
        [Alias(StoreApi, "PrivateTreaty")]
        PrivateTreaty,
        [Alias(StoreApi, "Tender")]
        Tender,
        [Alias(StoreApi, "ContactAgent")]
        ContactAgent
    }
}
