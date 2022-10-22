using System.Collections.Generic;
using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("PropertySearchResult")]
    [AutoInputObjectGraphType("PropertySearchResultInput")]
    public class PropertySearchResultViewModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Street1 { get; set; } 
        public string Street2 { get; set; } 
        public string City { get; set; } 
        public string StateProvince { get; set; } 
        public string Country { get; set; }
        public string PostalCode { get; set; } 
        public string PropertyType { get; set; }
    }
}
