using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public class UserLookupOptions
    {
        public bool IncludeTeamMembers { get; set; }
        public bool IncludeFirstName { get; set; }
        public bool IncludeLastName { get; set; }
        public bool IncludeFullName { get; set; }
        public bool ShouldQueryUserClaims
        {
            get
            {
                return IncludeFirstName || IncludeLastName || IncludeFullName;
            }
        }

        public bool IncludeUsers { get;  set; }
        public bool IncludeTeams { get;  set; }
        public bool IncludeOwner { get; set; }
    }
}
