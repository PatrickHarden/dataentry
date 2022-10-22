using System.Collections.Generic;
using dataentry.Data.DBContext.Model;
using dataentry.ViewModels.GraphQL;

namespace dataentry.Services.Integration.Edp.Model
{
    public interface IListingAdapter
    {
        PropertyUnified ConvertToPropertyUnified(Listing listing);
        IEnumerable<AvailabilityUnified> CovertToAvailabilitiesUnified(Listing listing);
        AvailabilityUnified GetAvailabilityUnifiedBase(Listing listing, Listing space);
        string MapLeaseTerm(string value);
        string MapLeaseType(string value);
        string MapListingType(string value);
        string MapPropertyUsage(string value);
        string MapUnitOfMeasure(string value);
        string NormalizeString(string value);
    }
}