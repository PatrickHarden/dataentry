using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class UpdateListingImagesOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE public.""ListingImages"" SET ""Order"" = ""ID"";
                UPDATE public.""ListingImages"" SET ""Order"" = 1 where ""IsPrimary"" = TRUE;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE public.""ListingImages"" SET ""Order"" = 0;
            ");
        }
    }
}
