using System.Data;
using System.IO;

namespace dataentry.Services.Business.BulkUpload
{
    public interface IBulkUploadService
    {
        DataSet GetDataSet(Stream fileStream);
    }
}
