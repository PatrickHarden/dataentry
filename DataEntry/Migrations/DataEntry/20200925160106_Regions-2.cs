using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class Regions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CultureCode",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalPreviewUrl",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalPublishUrl",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewPrefix",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewSiteID",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalID",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviewID",
                table: "Listings",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Regions",
                keyColumn: "ID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CountryCode", "CultureCode", "PreviewPrefix", "PreviewSiteID" },
                values: new object[] { "US", "en-US", "US-PREV", "us-comm-prev" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "CultureCode",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ExternalPreviewUrl",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ExternalPublishUrl",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PreviewPrefix",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "PreviewSiteID",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ExternalID",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "PreviewID",
                table: "Listings");
        }
    }
}
