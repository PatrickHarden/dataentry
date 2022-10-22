using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Diagnostics.CodeAnalysis;

namespace dataentry.Migrations.DataEntry
{
    [ExcludeFromCodeCoverage]
    public partial class ListingDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Listings");

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "Listings",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Listings",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Listings",
                nullable: false,
                defaultValueSql: "now()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Listings");

            migrationBuilder.AddColumn<string>(
                name: "AvailableFrom",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedAt",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedAt",
                table: "Listings",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ID",
                keyValue: -5,
                columns: new[] { "AvailableFrom", "CreatedAt", "UpdatedAt" },
                values: new object[] { "08/04/2019", "08/01/2019", "08/01/2019" });

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ID",
                keyValue: -4,
                columns: new[] { "AvailableFrom", "CreatedAt", "UpdatedAt" },
                values: new object[] { "07/04/2019", "07/01/2019", "07/01/2019" });

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ID",
                keyValue: -3,
                columns: new[] { "AvailableFrom", "CreatedAt", "UpdatedAt" },
                values: new object[] { "03/11/2019", "03/09/2019", "03/09/2019" });

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ID",
                keyValue: -2,
                columns: new[] { "AvailableFrom", "CreatedAt", "UpdatedAt" },
                values: new object[] { "10/11/2019", "10/09/2019", "10/09/2019" });

            migrationBuilder.UpdateData(
                table: "Listings",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "AvailableFrom", "CreatedAt", "UpdatedAt" },
                values: new object[] { "10/10/2019", "10/09/2019", "10/09/2019" });
        }
    }
}
