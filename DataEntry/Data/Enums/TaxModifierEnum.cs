using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum TaxModifierEnum
    {
        [Alias(StoreApi, "None")]
        None,
        [Alias(StoreApi, "PlusVAT")]
        PlusVAT,
        [Alias(StoreApi, "PlusSalesTax")]
        PlusSalesTax,
        [Alias(StoreApi, "IncludingVAT")]
        IncludingVAT,
        [Alias(StoreApi, "IncludingSalesTax")]
        IncludingSalesTax
    }
}