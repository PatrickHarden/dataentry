using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class AddIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Listings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Listings");
        }
    }
}
