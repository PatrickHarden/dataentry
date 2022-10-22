using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class MultiLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Rename field 'Value' to 'Text'
                UPDATE ""ListingData""
                SET ""Data"" = json_build_object('Text', ""Data""->>'Value')
                WHERE (
	                ""DataType"" = 'BuildingDescription'
	                OR ""DataType"" = 'LocationDescription'
	                OR ""DataType"" = 'Headline'
                ) AND ""Data"" ? 'Value';

                -- Move space name to ListingData
                INSERT INTO ""ListingData"" (""ListingID"", ""DataType"", ""Data"")
                SELECT 
	                ""ID"",
	                'SpaceName',
	                json_build_object('Text', ""Name"") AS ""Data""
                FROM ""Listings"" 
                WHERE ""ParentListingID"" IS NOT NULL 
                AND ""Name"" IS NOT NULL
                AND ""Name"" != '';

                -- Clear out unused name column on Spaces
                UPDATE ""Listings""
                SET ""Name"" = NULL
                WHERE ""ParentListingID"" IS NOT NULL;

                -- Convert Highlights array into multiple rows
                INSERT INTO ""ListingData"" (""ListingID"", ""DataType"", ""Data"", ""Language"")
                SELECT 
	                ""ListingID"", 
	                'Highlight--NEW', 
	                json_build_object('Text', j.""Value"", 'Order', j.""Order""), 
	                ""Language""
                FROM ""ListingData""
                INNER JOIN jsonb_to_recordset(""Data"") AS j(""Value"" text, ""Order"" integer)
                ON ""DataType"" = 'Highlight';

                DELETE FROM ""ListingData""
                WHERE ""DataType"" = 'Highlight';

                UPDATE ""ListingData""
                SET ""DataType"" = 'Highlight'
                WHERE ""DataType"" = 'Highlight--NEW';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                -- Rename field 'Text' back to 'Value'
                UPDATE ""ListingData""
                SET ""Data"" = json_build_object('Value', ""Data""->>'Text')
                WHERE (
	                ""DataType"" = 'BuildingDescription'
	                OR ""DataType"" = 'LocationDescription'
	                OR ""DataType"" = 'Headline'
                ) AND ""Data"" ? 'Text';
                
                -- Move space name back to listing table
                UPDATE ""Listings""
                SET ""Name"" = ""ListingData"".""Data""->>'Text'
                FROM ""ListingData""
                WHERE ""Listings"".""ID"" = ""ListingData"".""ListingID"" 
                AND ""DataType"" = 'SpaceName'
                AND ""Listings"".""ParentListingID"" IS NOT NULL 
                AND ""ListingData"".""Data"" ? 'Text';

                -- Delete unused ListingData
                DELETE FROM ""ListingData""
                WHERE ""DataType"" = 'SpaceName';

                -- Revert Highlights back into single json array
                UPDATE ""ListingData"" 
                SET ""Data"" = json_build_object('Value', ""Data""->>'Text', 'Order', ""Data""->>'Order')
                WHERE ""DataType"" = 'Highlight';

                INSERT INTO ""ListingData"" (""ListingID"", ""DataType"", ""Data"", ""Language"")
                SELECT
	                ""ListingID"", 
	                'Highlight--OLD',
	                json_agg(""Data""),
	                ""Language""
                FROM ""ListingData"" l1
                WHERE ""DataType"" = 'Highlight'
                GROUP BY ""ListingID"", ""Language"";

                DELETE FROM ""ListingData""
                WHERE ""DataType"" = 'Highlight';

                UPDATE ""ListingData"" 
                SET ""DataType"" = 'Highlight'
                WHERE ""DataType"" = 'Highlight--OLD';
            ");
        }
    }
}
