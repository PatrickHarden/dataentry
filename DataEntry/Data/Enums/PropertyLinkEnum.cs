using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum PropertyLink
    {
        [Alias(StoreApi, "specialty")]
        specialPurpose, 
        [Alias(StoreApi, "medical")]
        healthcare
    }
}
