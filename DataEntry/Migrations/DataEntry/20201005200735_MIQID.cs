using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class MIQID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MIQID",
                table: "Listings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_MIQID", 
                table: "Listings", 
                column: "MIQID", 
                filter: @"""MIQID"" IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Listings_MIQID");

            migrationBuilder.DropColumn(
                name: "MIQID",
                table: "Listings");
        }
    }
}
