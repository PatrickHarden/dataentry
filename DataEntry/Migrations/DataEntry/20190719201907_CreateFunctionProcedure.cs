using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace dataentry.Migrations.DataEntry
{
    [ExcludeFromCodeCoverage]
    public partial class CreateFunctionProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
           CREATE FUNCTION GetListingByID(id int)
              RETURNS TABLE (
                ""ID"" integer,
                ""IsParent"" bool,
                ""ParentListingID"" integer,
                ""Name"" text,
                ""UsageType"" text,
                ""Status"" text,
                ""AddressID"" integer,
				""AvailableFrom"" text,
				""UpdatedAt"" text,
				""CreatedAt"" text
              ) AS
            $func$  
                SELECT*
                FROM public.""Listings""
                WHERE ""ID"" = id;
            $func$  
            LANGUAGE SQL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION GetListingByID;");
        }
    }
}
