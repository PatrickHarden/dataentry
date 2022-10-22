using System.Text;
using dataentry.Utility;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class NormalizeCurrencyCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update currency codes to match the region, many values were incorrectly USD or null
            var map = GlobalizationUtility.CurrencyByRegionCode;
            var query = new StringBuilder();
            query.Append("WITH lookup (k,v) AS (Values ");
            foreach (var kv in map) {
                query.Append("('");
                query.Append(Escape(kv.Key));
                query.Append("','");
                query.Append(Escape(kv.Value.Code));
                query.Append("')");
                query.Append(",");
            }
            query.Length = query.Length - 1;
            query.Append(@")
                UPDATE ""ListingData"" s
                SET ""Data"" = (
                    SELECT jsonb_set(s.""Data"", '{CurrencyCode}', to_jsonb(COALESCE(lookup.v, s.""Data""->>'CurrencyCode')))
                    FROM lookup
                    WHERE k = r.""CountryCode""
                    LIMIT 1
                )
                FROM ""Listings"" l 
                INNER JOIN ""Regions"" r ON r.""ID"" = l.""RegionID""
                WHERE l.""ID"" = s.""ListingID""
                AND s.""DataType"" = 'Specifications';");
            
            var result = migrationBuilder.Sql(query.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        private static string Escape(string value) => value.Replace("'","''");
    }
}
