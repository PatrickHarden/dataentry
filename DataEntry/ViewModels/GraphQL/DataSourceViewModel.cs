using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("DataSource")]
    [AutoInputObjectGraphType("DataSourceInput")]
    public class DataSourceViewModel
    {
        public IEnumerable<string> DataSources { get; set; }
        public string Other { get; set; }
    }
}