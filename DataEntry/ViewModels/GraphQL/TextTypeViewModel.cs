using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("TextType")]
    [AutoInputObjectGraphType("TextTypeInput")]
    [Description("A regional translation of a text value.")]
    public class TextTypeViewModel
    {
        [Description("The regional language code this text translation is for. A list of valid languages can be found here: https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c")]
        public string CultureCode { get; set; }
        public string Text { get; set; }
    }

    [AutoObjectGraphType("OrderedTextType")]
    [AutoInputObjectGraphType("OrderedTextTypeInput")]
    [Description("A regional translation of a text value in an ordered list.")]
    public class OrderedTextTypeViewModel : TextTypeViewModel
    {
        [Description("The sort order of this value. Values with smaller Orders appear first when returned.")]
        public int Order { get; set; }
    }
}
