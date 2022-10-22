using System;
using dataentry.Data.DBContext.Model;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class Regions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegionID",
                table: "Listings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    HomeSiteID = table.Column<string>(nullable: true),
                    ListingPrefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "ID", "HomeSiteID", "ListingPrefix", "Name" },
                values: new object[] { Region.DefaultID, "us-comm", "US-SMPL", "Default" });

            migrationBuilder.Sql($"UPDATE \"Listings\" SET \"RegionID\" = '{Region.DefaultID.ToString()}';");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_RegionID",
                table: "Listings",
                column: "RegionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Regions_RegionID",
                table: "Listings",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Regions_RegionID",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Listings_RegionID",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "RegionID",
                table: "Listings");
        }
    }
}
