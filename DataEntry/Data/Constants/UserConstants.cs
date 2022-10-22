using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Data.Constants
{
    public static class UserConstants
    {
        public const string UserClaimName = "dataentry:user";
        public const string TeamRoleNamePrefix = "dataentry:teams/";
        public const string TeamRoleNamePrefixNormalized = "DATAENTRY:TEAMS/";
        public const string TeamRoleClaimName = "dataentry:isTeam";
        public const string OwnerClaimName = "dataentry:listingOwner";
        public const string ListingClaimName = "ListingClaim";
        public const string AdminRoleName = "Admin";
        public const string AdminClaimType = "dataentry:admin";
        public const string RegionAdminRoleFormat = "dataentry:regionAdmin/{0}";
        public const string RegionAdminClaimType = "dataentry:regionAdmin";
    }
}
