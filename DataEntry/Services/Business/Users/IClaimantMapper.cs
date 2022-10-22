using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Users
{
    public interface IClaimantMapper
    {
        ClaimantViewModel Map(Claimant claimant);
    }
}