using dataentry.AutoGraph.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Team")]
    [AutoInputObjectGraphType("TeamInput")]
    public class TeamViewModel
    {
        public string Name { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
