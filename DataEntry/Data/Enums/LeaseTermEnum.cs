using System.ComponentModel;
using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum LeaseTermEnum
    {
        [Alias(StoreApi, "Monthly")]
        monthly,
        [Alias(StoreApi, "Quarterly")]
        quarterly,
        [Alias(StoreApi, "Annually", Primary = true)]
        annually,
        [Alias(StoreApi, "Annually")]
        [Alias(ViewModel, "annually")]
        yearly,
        [Alias(StoreApi, "Once")]
        once,
    }
}