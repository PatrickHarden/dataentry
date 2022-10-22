using dataentry.Utility;

namespace dataentry.Data.Enums
{
    public enum LeaseTypesEnum
    {
        [Alias(AliasType.StoreApi, "Assignment")]
        Assignment,
        [Alias(AliasType.StoreApi, "GroundLease")]
        GroundLease,
        [Alias(AliasType.StoreApi, "HeadLease")]
        HeadLease,
        [Alias(AliasType.StoreApi, "Licence")]
        Licence,
        [Alias(AliasType.StoreApi, "New")]
        New,
        [Alias(AliasType.StoreApi, "OccupationalLease")]
        OccupationalLease,
        [Alias(AliasType.StoreApi, "SubLease")]
        SubLease,
        [Alias(AliasType.StoreApi, "Unknown")]
        Unknown,
        [Alias(AliasType.StoreApi, "Freehold")]
        Freehold,
        [Alias(AliasType.StoreApi, "LeaseHold")]
        LeaseHold,
        [Alias(AliasType.StoreApi, "LongLeaseHold")]
        LongLeaseHold,
        [Alias(AliasType.StoreApi, "LongLet")]
        LongLet,
        [Alias(AliasType.StoreApi, "FlexLease")]
        FlexLease
    }
}
