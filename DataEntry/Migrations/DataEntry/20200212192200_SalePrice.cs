using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class SalePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                --Add the SalePrice field
                UPDATE ""ListingData"" s 
                    SET ""Data"" = s.""Data""::jsonb || 
						('{""SalePrice"":' || 
						 	COALESCE(NULLIF(s.""Data""->>'MinPrice', '0.0'), --Use MinPrice if not 0 or null
									 NULLIF(s.""Data""->>'MaxPrice', '0.0'), --Use MaxPrice if not 0 or null
									 COALESCE(s.""Data""->>'MinPrice',s.""Data""->>'MaxPrice')) || --Use 0 if either value is 0, otherwise null
						 '}')::jsonb
                FROM ""ListingData"" l
                WHERE s.""DataType"" = 'Specifications'
                    AND s.""Data""->> 'SalePrice' IS NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                --Remove the SalePrice field
			    UPDATE ""ListingData"" s 
                    SET ""Data"" = s.""Data""::jsonb - 'SalePrice'
                WHERE s.""DataType"" = 'Specifications';
            ");
        }
    }
}