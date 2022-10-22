using ClosedXML.Excel;
using dataentry.Data.DBContext;
using dataentry.Repository;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Regions;
using dataentry.Services.Business.Users;
using dataentry.ViewModels;
using dataentry.ViewModels.GraphQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Report
{
    public class ReportService : IReportService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly DataEntryContext _dataEntryContext;
        private readonly IOptions<ReportMappingOptions> _reportOptionsMapping;

        public ReportService(ApplicationUserManager userManager
                           , DataEntryContext dataEntryContext
                           , IOptions<ReportMappingOptions> reportOptionsMapping)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _dataEntryContext = dataEntryContext ?? throw new ArgumentNullException(nameof(dataEntryContext));
            _reportOptionsMapping = reportOptionsMapping;
        }

        private string Query(string regionId, bool isAdmin, string userId)
        {
            return $@"SELECT L.""ID"" AS ""Id"",
{BuildListingUrl()},
LO.""UserName"" ""ListingOwners"",
LD_PT.""Data""->>'Value' AS ""PropertyType"",
LD_PST.""Data""->>'Value' AS ""PropertySubType"",
LD_LT.""Data""->>'Value' AS ""ListingType"",
COALESCE(LD_PS.""Data""->>'Value','Draft') AS ""PublishingStatus"",
'' AS ""Market"",
LD_PRN.""Data""->>'Value' AS ""PropertyRecordName"",
L.""Name"" AS ""BuildingDisplayName"",
LD_A.""Street1"" AS ""Street1"",
LD_A.""Street2""  AS ""Street2"",
LD_A.""City"" AS ""City"",
LD_A.""StateProvince"" AS ""StateProvince"",
LD_A.""PostalCode""  AS ""PostalCode"",
LD_A.""Longitude"" AS ""Longitude"",
LD_A.""Latitude"" AS ""Latitude"",
CASE WHEN LI.""ID"" ISNULL THEN 'N' ELSE 'Y' END  AS ""PrimaryPhoto"",
CASE WHEN LI_F.""ListingID"" ISNULL THEN 'N' ELSE 'Y' END AS ""FloorPlan"",
CASE WHEN LD_B.""Data"" -> 0 ISNULL THEN 'N' ELSE 'Y' END AS ""Brochure"",
CASE WHEN LD_EPC.""Data"" -> 0 ISNULL THEN 'N' ELSE 'Y' END AS ""EPCPdf"",
(SELECT string_agg(CONCAT(t->>'RatingType',' ',t->>'RatingLevel'), '|')  FROM jsonb_array_elements(LD_ER.""Data""::jsonb) as x(t)) AS ""EnergyRating"",
LD_S.""Data"" ->> 'MinSpace'  AS ""MinimumSpace"",
LD_S.""Data"" ->> 'TotalSpace' AS ""TotalSpaceAvailable"",
LD_S.""Data"" ->> 'MinPrice' AS ""MinimumLeasePrice"",
LD_S.""Data"" ->> 'MaxPrice' AS ""MaximumLeasePrice"",
LD_S.""Data"" ->> 'SalePrice' AS ""SalePrice"",
(CASE WHEN (LD_S.""Data"" ->> 'ContactBrokerForPrice')::boolean=True THEN 'Y' ELSE 'N' END) AS ""ContactBrokerForPricing"",
AV_FN.""AvailabilityFloorNames"" AS ""AvailabilityFloorNames"",
AV_S.""AvailabilityFloorSize"" AS ""AvailabilityFloors"",
AV_S.""AvailabilityFloorPrice"" AS ""AvailabilityFloorPrice"",
(SELECT STRING_AGG(B.""FirstName"" || ' ' || B.""LastName"", '|')  FROM public.""ListingBrokers"" LB INNER JOIN public.""Brokers"" B ON LB.""BrokerID"" = B.""ID"" AND LB.""ListingID""=L.""ID"") AS ""BrokerName"",
(SELECT STRING_AGG(B.""Location"", '|')  FROM public.""ListingBrokers"" LB INNER JOIN public.""Brokers"" B ON LB.""BrokerID"" = B.""ID"" AND LB.""ListingID""=L.""ID"") AS ""BrokerOffice"",
(SELECT STRING_AGG(B.""License"", '|')  FROM public.""ListingBrokers"" LB INNER JOIN public.""Brokers"" B ON LB.""BrokerID"" = B.""ID"" AND LB.""ListingID""=L.""ID"") AS ""BrokerLicenseNumber"",
(SELECT STRING_AGG(B.""Email"", '|')  FROM public.""ListingBrokers"" LB INNER JOIN public.""Brokers"" B ON LB.""BrokerID"" = B.""ID"" AND LB.""ListingID""=L.""ID"") AS ""BrokerEmail"",
(SELECT STRING_AGG(B.""Phone"", '|')  FROM public.""ListingBrokers"" LB INNER JOIN public.""Brokers"" B ON LB.""BrokerID"" = B.""ID"" AND LB.""ListingID""=L.""ID"") AS ""BrokerPhone"",
L.""CreatedAt""::text AS ""CreatedDate"",
L.""UpdatedAt""::text AS ""LastUpdatedDate"",
CASE WHEN TO_DATE(LD_PS.""Data"" ->> 'DateListed', 'YYYY-MM-DD')::text ='0001-01-01' THEN '' WHEN TO_DATE(LD_PS.""Data"" ->> 'DateUpdated', 'YYYY-MM-DD')::text ='0001-01-01' THEN '' ELSE TO_TIMESTAMP(LD_PS.""Data"" ->> 'DateUpdated', 'YYYY-MM-DD HH24:MI:SS')::text END AS ""LastPublishedDate"",
CASE WHEN TO_DATE(LD_PS.""Data"" ->> 'DateListed', 'YYYY-MM-DD')::text ='0001-01-01' THEN '' WHEN TO_DATE(LD_PS.""Data"" ->> 'DateUpdated', 'YYYY-MM-DD')::text ='0001-01-01' THEN '' ELSE (NOW()::date - TO_TIMESTAMP(LD_PS.""Data"" ->> 'DateUpdated', 'YYYY-MM-DD')::date)::text END AS ""TimeSinceLastPublish""
FROM public.""Listings"" L
INNER JOIN public.""ListingData"" LD_PT ON L.""ID""=LD_PT.""ListingID"" AND LD_PT.""DataType""='PropertyType'
LEFT JOIN public.""ListingData"" LD_PST ON L.""ID""=LD_PST.""ListingID"" AND LD_PST.""DataType""='PropertySubType'
LEFT JOIN public.""ListingData"" LD_LT ON L.""ID""=LD_LT.""ListingID"" AND LD_LT.""DataType""='ListingType'
LEFT JOIN public.""ListingData"" LD_PS ON L.""ID"" = LD_PS.""ListingID"" AND LD_PS.""DataType"" = 'PublishingState'
LEFT JOIN public.""ListingData"" LD_PRN ON L.""ID"" = LD_PRN.""ListingID"" AND LD_PRN.""DataType"" = 'PropertyRecordName'
LEFT JOIN public.""ListingData"" LD_LS ON L.""ID""=LD_LS.""ListingID"" AND LD_LS.""DataType""='ListingSerialization'
LEFT JOIN public.""ListingData"" LD_F ON L.""ID""=LD_F.""ListingID"" AND LD_F.""DataType""='Floor'
LEFT JOIN public.""ListingData"" LD_B ON L.""ID""=LD_B.""ListingID"" AND LD_B.""DataType""='Brochure'
LEFT JOIN public.""ListingData"" LD_EPC ON L.""ID""=LD_EPC.""ListingID"" AND LD_EPC.""DataType""='EpcGraph'
LEFT JOIN public.""ListingData"" LD_S ON L.""ID""=LD_S.""ListingID"" AND LD_S.""DataType""='Specifications'
LEFT JOIN public.""ListingData"" LD_ER ON L.""ID""=LD_ER.""ListingID"" AND LD_ER.""DataType""='ExternalRatings'
LEFT JOIN(
 SELECT ML.""ID"",STRING_AGG(LD_AV_N.""Data"" ->> 'Text','|' ORDER BY LD_AV_N.""ListingID"") AS ""AvailabilityFloorNames"" FROM public.""Listings"" L_AV 
	INNER JOIN public.""ListingData"" LD_AV_N ON L_AV.""ID""=LD_AV_N.""ListingID"" 
		AND LD_AV_N.""DataType""='SpaceName' AND LD_AV_N.""Data"" ->> 'CultureCode' ILIKE '%en%'
	INNER JOIN public.""Listings"" ML ON ML.""ID""=L_AV.""ParentListingID""
	GROUP BY ML.""ID""
) AV_FN ON AV_FN.""ID""=L.""ID""
LEFT JOIN(
 SELECT ML.""ID"",STRING_AGG(CASE WHEN COALESCE(LD_AV_N.""Data"" ->> 'MaxPrice','')='' THEN LD_AV_N.""Data"" ->> 'SalePrice' ELSE LD_AV_N.""Data"" ->> 'MaxPrice' END,'|' ORDER BY LD_AV_N.""ListingID"") AS ""AvailabilityFloorPrice"" 
	,STRING_AGG(LD_AV_N.""Data"" ->> 'TotalSpace','|' ORDER BY LD_AV_N.""ListingID"") AS ""AvailabilityFloorSize"" 
	FROM public.""Listings"" L_AV 
	INNER JOIN public.""ListingData"" LD_AV_N ON L_AV.""ID""=LD_AV_N.""ListingID"" 
		AND LD_AV_N.""DataType""='Specifications' 
	INNER JOIN public.""Listings"" ML ON ML.""ID""=L_AV.""ParentListingID""
	GROUP BY ML.""ID""
) AV_S ON AV_S.""ID""=L.""ID""
LEFT JOIN public.""ListingImages"" LI ON L.""ID""=LI.""ListingID"" AND LI.""IsPrimary""=true AND LI.""ImageCategory""='Photo'
LEFT JOIN (SELECT DISTINCT ""ListingID"" FROM public.""ListingImages"" WHERE ""ImageCategory""='FloorPlan') LI_F ON L.""ID""=LI_F.""ListingID""
LEFT JOIN public.""Address"" LD_A ON L.""AddressID""=LD_A.""ID""
{UserCondition(isAdmin)} JOIN (
	SELECT UC.""ClaimValue"", STRING_AGG(U.""NormalizedUserName"",'|') AS ""UserName"" 
    FROM ""user"".""AspNetUserClaims"" UC 
    INNER JOIN ""user"".""AspNetUsers"" U ON UC.""UserId""=U.""Id"" 
    WHERE ""ClaimType""='ListingClaim' AND ({isAdmin} = true OR U.""Id""='{userId}')
	GROUP BY UC.""ClaimValue""
)LO ON L.""ID""::text=LO.""ClaimValue"" WHERE  L.""IsParent"" = true and L.""IsDeleted"" = false AND L.""RegionID""='{regionId}'";
        }

        private static string UserCondition(bool isAdmin)
        {
            return isAdmin ? "LEFT" : "INNER";
        }

        private string BuildListingUrl()
        {
            ReportMappingOptions reportOptions = _reportOptionsMapping.Value;
            StringBuilder sb = new();

            // has report mapping
            if (reportOptions.Mapping?.Count > 0)
            {
                sb.Append("CASE");
                foreach (var item in reportOptions.Mapping)
                {
                    sb.Append($@" WHEN R.""HomeSiteID""='{item.Key}' THEN CONCAT('{item.Value}',L.""ID"") ");
                }
                sb.Append($@"ELSE CONCAT('{reportOptions.DefaultUrl}',L.""ID"") END AS ""ListingURL""");
                return sb.ToString();
            }

            return $@"CONCAT('{reportOptions.DefaultUrl}',L.""ID"") AS ""ListingURL""";
        }

        public async Task<XLWorkbook> BuildExcelFile(ClaimsPrincipal user, string regionId)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = await _userManager.IsAdminInRegionAsync(userId, regionId);
            var reportResult = _dataEntryContext.ReportModel.FromSql(Query(regionId, isAdmin, userId)).ToList();

            var propertyTypes = reportResult.Where(p => p.PropertyType != "null" && !string.IsNullOrEmpty(p.PropertyType)).Select(r => r.PropertyType.ToLower().Trim()).Distinct().ToList();

            var wb = new XLWorkbook();

            //return empty excel file when no data
            if (propertyTypes?.Count == 0)
            {
                wb.AddWorksheet("No Data").FirstCell().SetValue("");
            }
            propertyTypes.ForEach(propertyType =>
            {
                var ws = wb.AddWorksheet(propertyType);
                var exportR = reportResult?.Where(r => r.PropertyType?.ToLower().Trim() == propertyType).ToList();
                ws.FirstCell().InsertTable(exportR, false);
            });
            return wb;
        }



    }
}
