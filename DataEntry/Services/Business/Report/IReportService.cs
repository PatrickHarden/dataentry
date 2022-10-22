using ClosedXML.Excel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Report
{
    public interface IReportService
    {
        Task<XLWorkbook> BuildExcelFile(ClaimsPrincipal user, string regionId);
    }
}
