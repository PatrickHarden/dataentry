using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class UpdateNullListingName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    UPDATE ""Listings"" 
                    SET ""Name"" = '' WHERE LOWER(""Name"") = 'null' 
                        AND ""IsDeleted"" = false and ""IsParent"" = true;
                ");
        }

    }
}
