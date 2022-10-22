using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace dataentry.Migrations.DataEntry
{
    [ExcludeFromCodeCoverage]
    public partial class AddSpacesFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Listings_ParentListingID",
                table: "Listings",
                column: "ParentListingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Listings_ParentListingID",
                table: "Listings",
                column: "ParentListingID",
                principalTable: "Listings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Listings_ParentListingID",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_ParentListingID",
                table: "Listings");
        }
    }
}
