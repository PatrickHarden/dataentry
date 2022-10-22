using dataentry.Repository;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace dataentry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
        }


        [HttpGet]
        [Route("download/{regionid}")]
        public async Task<IActionResult> DownloadFile(string regionId)
        {
            var wb = await _reportService.BuildExcelFile(User, regionId);
            var workbookBytes = new byte[0];
            using (var ms = new MemoryStream())
            {
                wb.SaveAs(ms);
                workbookBytes = ms.ToArray();
                return File(workbookBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DEReport.xlsx");
            }            
            
        }

    }
}
