using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class SetListingBrokerOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Default order to the listingbrokers ID
                migrationBuilder.Sql(
                    @"Update ""ListingBrokers"" 
                    Set ""Order"" = ""ID"" 
                    Where ""Order"" = 0;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Set order to 0 
                migrationBuilder.Sql(
                @"Update ""ListingBrokers"" 
                Set ""Order"" = 0
                Where ""Order"" = ""ID"";");
        }
    }
}
