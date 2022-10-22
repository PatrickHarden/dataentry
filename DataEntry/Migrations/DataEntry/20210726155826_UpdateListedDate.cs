using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class UpdateListedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE public.""ListingData"" SET ""Data"" = coalesce(""Data"", '{}') ||  jsonb_build_object('DateListed', to_json(""Data"" ->> 'DateUpdated')::jsonb)
                WHERE ""DataType"" = 'PublishingState' and ""Data"" ->> 'Value' = 'Published' 
                AND  (""Data""->>'DateListed' = '') IS NOT FALSE;
            ");
        }
    }
}
