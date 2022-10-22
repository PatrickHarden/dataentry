using dataentry.AutoGraph.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Claimant")]
    public class ClaimantViewModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsTeam { get; set; }
    }
}
