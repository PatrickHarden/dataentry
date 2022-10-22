using dataentry.Data.DBContext.Model;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class UniqueExternalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE public.""Listings"" l
                SET ""ExternalID"" = NULL
                WHERE l.""ID"" != (
                    SELECT l2.""ID"" 
                    FROM public.""Listings"" l2 
                    WHERE l2.""ExternalID"" = l.""ExternalID"" 
                    ORDER BY l2.""ID"" ASC 
                    LIMIT 1);
            ");
            
            migrationBuilder.CreateIndex(
                table: "Listings", 
                column: "ExternalID", 
                unique: true, // unique index
                name: "IX_Listings_ExternalID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                table: "Listings", 
                name: "IX_Listings_ExternalID");
        }
    }
}
