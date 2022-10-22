using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class RemovePropertySize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""ListingData"" SET ""DataType"" = 'PropertySize_Bak' WHERE ""DataType"" = 'PropertySize' and ""Data"" != '[]';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""ListingData"" SET ""DataType"" = 'PropertySize' WHERE ""DataType"" = 'PropertySize_Bak';
            ");
        }
    }
}
