using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class HardfixData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Set duplicate 'PublishingState' datatype to 'Duplicate_PublishingState
            migrationBuilder.Sql(@"
            UPDATE ""ListingData"" SET ""DataType"" = 'Duplicate_PublishingState'
            WHERE ""ID"" NOT IN (SELECT MAX(""ID"") 
            FROM public.""ListingData"" 
            WHERE ""DataType"" = 'PublishingState'
            GROUP BY ""ListingID"") and ""DataType"" = 'PublishingState';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Revert duplicate 'Duplicate_PublishingState' datatype to 'PublishingState
            migrationBuilder.Sql(@"
            UPDATE ""ListingData"" SET ""DataType"" = 'PublishingState'
            WHERE ""DataType"" = 'Duplicate_PublishingState';
            ");
        }
    }
}
