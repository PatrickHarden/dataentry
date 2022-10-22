using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class UpdateMaxAndTotalSpace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Set MaxSpace to TotalSpace, and update MaxSpace to Null
            migrationBuilder.Sql(@"
            UPDATE ""ListingData""
            SET ""Data"" = 
                ""Data""
                || (
                    '{' 
                    || '""MaxSpace"":null,'
                    || '""MaxSpace_bak"":' 
                    || COALESCE(""Data""->>'MaxSpace') 
                    || '}'
                )::jsonb
            WHERE ""ID"" in 
            (SELECT n.""ID""
            FROM (
            SELECT l.""ID"" FROM ""Listings"" l
            LEFT JOIN ""ListingData"" t
                ON t.""DataType"" = 'PropertyType'
                AND t.""ListingID"" = COALESCE(l.""ParentListingID"", l.""ID"")
            WHERE t.""Data""->>'Value' != '' AND t.""Data""->>'Value' != 'flex' AND t.""Data""->>'Value' != 'shophouse'
            ) m
            LEFT JOIN ""ListingData"" n ON n.""ListingID"" = m.""ID"" AND ""DataType"" = 'Specifications'
            WHERE n.""Data""->>'MaxSpace' IS NOT NULL);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Set MaxSpace to TotalSpace, and update MaxSpace to Null
            migrationBuilder.Sql(@"
                UPDATE ""ListingData""
                SET ""Data"" = 
                    ""Data""
                    || (
                        '{' 
                        ||'""MaxSpace"":' 
                        || COALESCE(""Data""->>'MaxSpace_bak') 
                        || '}'
                    )::jsonb
                Where ""DataType"" = 'Specifications' and ""Data""->>'MaxSpace_bak' is not null;

                UPDATE ""ListingData""
                SET ""Data"" = ""Data""::jsonb - 'MaxSpace_bak'
                WHERE ""DataType"" = 'Specifications' and ""Data""->>'MaxSpace_bak' is not null;
                ");
        }
    }
}
