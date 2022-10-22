using dataentry.Utility;
using static dataentry.Utility.AliasType;

namespace dataentry.Data.Enums
{
    public enum NoteTypesEnum
    {
        [Alias(MIQ, "Location Description")]
        LocationDescription,
        [Alias(MIQ, "Property Notes")]
        PropertyDescription
    }
}