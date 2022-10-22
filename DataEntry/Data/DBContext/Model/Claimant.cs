using Microsoft.AspNetCore.Identity;
using System;

namespace dataentry.Data.DBContext.Model
{
    public class Claimant : IEquatable<Claimant>
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public bool IsTeam { get; set; }

        public bool Equals(Claimant other)
        {
            return NormalizedName == other.NormalizedName;
        }
    }
}
