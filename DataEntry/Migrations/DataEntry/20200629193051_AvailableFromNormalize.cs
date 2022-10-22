using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class AvailableFromNormalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Listings""
	            SET ""AvailableFrom""=make_timestamp(
		            date_part('year', ""AvailableFrom"")::integer,
		            date_part('month', ""AvailableFrom"")::integer,
		            date_part('day', ""AvailableFrom"")::integer,
		            11, 
		            0, 
		            0::double precision)
	            WHERE ""AvailableFrom"" IS NOT NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
