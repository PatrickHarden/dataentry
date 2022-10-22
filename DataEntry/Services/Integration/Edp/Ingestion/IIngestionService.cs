using System.Threading.Tasks;

namespace dataentry.Services.Integration.Edp.Ingestion
{
    public interface IIngestionService
    {
        bool Enabled { get; }
        
        bool EnabledInRegion(string siteId);

        Task SubmitListing(Data.DBContext.Model.Listing listing);
    }
}
