using System.ComponentModel;
using static dataentry.Utility.AliasType;
using dataentry.Utility;

namespace dataentry.Data.Enums
{
    public enum EnergyRatingEnum
    {
        [Alias(AliasType.Default, "Pass")]
        [Alias(AliasType.StoreApi, "Pass")]
        Pass,
        [Alias(AliasType.Default, "Good")]
        [Alias(AliasType.StoreApi, "Good")]
        Good,
        [Alias(AliasType.Default, "Very Good")]
        [Alias(AliasType.StoreApi, "Very Good")]
        VeryGood,
        [Alias(AliasType.Default, "Excellent")]
        [Alias(AliasType.StoreApi, "Excellent")]
        Excellent,
        [Alias(AliasType.Default, "Acceptable")]
        [Alias(AliasType.StoreApi, "Acceptable")]
        Acceptable,
        [Alias(AliasType.Default, "Outstanding")]
        [Alias(AliasType.StoreApi, "Outstanding")]
        Outstanding,
        [Alias(AliasType.Default, "Unclassified")]
        [Alias(AliasType.StoreApi, "Unclassified")]
        Unclassified,
        [Alias(AliasType.Default, "Bronze")]
        [Alias(AliasType.StoreApi, "Bronze")]
        [Alias(AliasType.MIQ, "Certified Bronze")]
        Bronze,
        [Alias(AliasType.Default, "Certified Compliant")]
        [Alias(AliasType.StoreApi, "Certified Compliant")]
        CertifiedCompliant,
        [Alias(AliasType.Default, "Certified Pilot")]
        [Alias(AliasType.StoreApi, "Certified Pilot")]
        CertifiedPilot,
        [Alias(AliasType.Default, "Gold")]
        [Alias(AliasType.StoreApi, "Gold")]
        [Alias(AliasType.MIQ, "Certified Gold")]
        Gold,
        [Alias(AliasType.Default, "Platinum")]
        [Alias(AliasType.StoreApi, "Platinum")]
        [Alias(AliasType.MIQ, "Certified Platinum")]
        Platinum,
        [Alias(AliasType.Default, "Pre-certified")]
        [Alias(AliasType.StoreApi, "Pre-certified")]
        Precertified,
        [Alias(AliasType.Default, "Registered")]
        [Alias(AliasType.StoreApi, "Registered")]
        Registered,
        [Alias(AliasType.Default, "Silver")]
        [Alias(AliasType.StoreApi, "Silver")]
        [Alias(AliasType.MIQ, "Certified Silver")]
        Silver,
        [Alias(AliasType.Default, "Certified")]
        [Alias(AliasType.StoreApi, "Certified")]
        Certified,
        [Alias(AliasType.Default, "Pre-certificate Silver")]
        [Alias(AliasType.StoreApi, "Pre-certificate Silver")]
        PreCertificateSilver,
        [Alias(AliasType.Default, "Pre-certificate Gold")]
        [Alias(AliasType.StoreApi, "Pre-certificate Gold")]
        PreCertificateGold,
        [Alias(AliasType.Default, "Pre-certificate Platinum")]
        [Alias(AliasType.StoreApi, "Pre-certificate Platinum")]
        PreCertificatePlatinum,
        [Alias(AliasType.Default, "Registered / In-progress")]
        [Alias(AliasType.StoreApi, "Registered / In-progress")]
        RegisteredInProgress,
        [Alias(AliasType.Default, "Zero Carbon")]
        [Alias(AliasType.StoreApi, "Zero Carbon")]
        ZeroCarbon,
        [Alias(AliasType.Default, "Zero Energy")]
        [Alias(AliasType.StoreApi, "Zero Energy")]
        ZeroEnergy,
        [Alias(AliasType.Default, "Zero Waste")]
        [Alias(AliasType.StoreApi, "Zero Waste")]
        ZeroWaste,
        [Alias(AliasType.Default, "Zero Water")]
        [Alias(AliasType.StoreApi, "Zero Water")]
        ZeroWater,
        [Alias(AliasType.Default, "A")]
        [Alias(AliasType.StoreApi, "A")]
        A,
        [Alias(AliasType.Default, "A+")]
        [Alias(AliasType.StoreApi, "A+")]
        APlus,
        [Alias(AliasType.Default, "A++")]
        [Alias(AliasType.StoreApi, "A++")]
        APlusPlus,
        [Alias(AliasType.Default, "A+++")]
        [Alias(AliasType.StoreApi, "A+++")]
        APlusPlusPlus,
        [Alias(AliasType.Default, "A1")]
        [Alias(AliasType.StoreApi, "A1")]
        A1,
        [Alias(AliasType.Default, "A2")]
        [Alias(AliasType.StoreApi, "A2")]
        A2,
        [Alias(AliasType.Default, "A3")]
        [Alias(AliasType.StoreApi, "A3")]
        A3,
        [Alias(AliasType.Default, "A4")]
        [Alias(AliasType.StoreApi, "A4")]
        A4,
        [Alias(AliasType.Default, "EU")]
        [Alias(AliasType.StoreApi, "EU")]
        EU,
        [Alias(AliasType.Default, "EK")]
        [Alias(AliasType.StoreApi, "EK")]
        EK,
        [Alias(AliasType.Default, "EP")]
        [Alias(AliasType.StoreApi, "EP")]
        EP,
        [Alias(AliasType.Default, "Eco2")]
        [Alias(AliasType.StoreApi, "Eco2")]
        Eco2,
        [Alias(AliasType.Default, "Uoze")]
        [Alias(AliasType.StoreApi, "Uoze")]
        Uoze,
        [Alias(AliasType.Default, "A2010")]
        [Alias(AliasType.StoreApi, "A2010")]
        A2010,
        [Alias(AliasType.Default, "A2015")]
        [Alias(AliasType.StoreApi, "A2015")]
        A2015,
        [Alias(AliasType.Default, "A2020")]
        [Alias(AliasType.StoreApi, "A2020")]
        A2020,
        [Alias(AliasType.Default, "B")]
        [Alias(AliasType.StoreApi, "B")]
        B,
        [Alias(AliasType.Default, "C")]
        [Alias(AliasType.StoreApi, "C")]
        C,
        [Alias(AliasType.Default, "D")]
        [Alias(AliasType.StoreApi, "D")]
        D,
        [Alias(AliasType.Default, "E")]
        [Alias(AliasType.StoreApi, "E")]
        E,
        [Alias(AliasType.Default, "F")]
        [Alias(AliasType.StoreApi, "F")]
        F,
        [Alias(AliasType.Default, "G")]
        [Alias(AliasType.StoreApi, "G")]
        G
    }
}
