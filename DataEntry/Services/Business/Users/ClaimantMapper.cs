using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public class ClaimantMapper : IClaimantMapper
    {
        public ClaimantViewModel Map(Claimant claimant)
        {
            var result = new ClaimantViewModel();
            result.Name = claimant.Name;
            result.FirstName = claimant.FirstName;
            result.LastName = claimant.LastName;
            result.FullName = claimant.FullName;
            result.IsTeam = claimant.IsTeam;

            return result;
        }
    }
}
