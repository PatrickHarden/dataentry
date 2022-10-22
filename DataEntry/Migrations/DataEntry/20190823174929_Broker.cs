using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace dataentry.Migrations.DataEntry
{
    [ExcludeFromCodeCoverage]
    public partial class Broker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Brokers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Brokers",
                keyColumn: "ID",
                keyValue: -4,
                column: "Location",
                value: "Fort Lee, NJ");

            migrationBuilder.UpdateData(
                table: "Brokers",
                keyColumn: "ID",
                keyValue: -3,
                column: "Location",
                value: "Dallas, TX");

            migrationBuilder.UpdateData(
                table: "Brokers",
                keyColumn: "ID",
                keyValue: -2,
                column: "Location",
                value: "Irving, TX");

            migrationBuilder.UpdateData(
                table: "Brokers",
                keyColumn: "ID",
                keyValue: -1,
                column: "Location",
                value: "ABQ, NM");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -9,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -8,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -7,
                column: "Data",
                value: "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -3,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -2,
                column: "Data",
                value: "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Brokers");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -9,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\" }]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -8,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\" }]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -7,
                column: "Data",
                value: "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"} ]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -3,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\" }]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -2,
                column: "Data",
                value: "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"} ]");
        }
    }
}
