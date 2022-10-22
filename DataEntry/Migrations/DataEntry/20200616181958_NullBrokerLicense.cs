using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class NullBrokerLicense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Brokers""
	            SET ""License""=NULL
	            WHERE ""License"" = ''
	            OR ""License"" = '0'
	            OR ""License"" ~ '^\s*$';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
