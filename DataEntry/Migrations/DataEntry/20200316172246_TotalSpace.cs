using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class TotalSpace : Migration
    {
        /// <summary>
        /// For non-flex properties only, this query will migrate the MaxSpace value to TotalSpace, since we were using MaxSpace as TotalSpace in all cases except on flex properties.
        /// MaxSpace will be set to 0, unless MinSpace is null. This is because min and max will always appear as pairs now, but total may appear without a min/max.
        /// This is specific to the "flex" property type, not "flexindustrial"
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Move the values in MaxSpace to TotalSpace
            migrationBuilder.Sql(@"
                UPDATE ""ListingData"" d
                SET ""Data"" = 
	                d.""Data""
	                || (
		                '{' 
		                || CASE 
			                WHEN d.""Data""->>'TotalSpace' IS NULL
			                THEN CASE
				                WHEN d.""Data""->>'MaxSpace' IS NULL OR d.""Data""->>'MinSpace' IS NULL
				                THEN '""MaxSpace"":null,'
				                ELSE '""MaxSpace"":0,'
				                END
			                ELSE ''
			                END
		                || '""TotalSpace"":' 
		                || COALESCE(d.""Data""->>'TotalSpace', d.""Data""->>'MaxSpace', 'null') 
		                || '}'
	                )::jsonb
                FROM ""Listings"" l 
                LEFT JOIN ""ListingData"" t
	                ON t.""DataType"" = 'PropertyType'
	                AND t.""ListingID"" = COALESCE(l.""ParentListingID"", l.""ID"")
                WHERE d.""ListingID"" = l.""ID""
	                AND d.""DataType"" = 'Specifications'
	                AND d.""Data"" IS NOT NULL
	                AND (
		                t.""Data""->>'Value' IS NULL
		                OR NOT t.""Data""->>'Value' = 'flex'
	                );
            ");
        }

        /// <summary>
        /// This wil revert the change above, moving TotalSpace back to MaxSpace on non-flex properties. 
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""ListingData"" d
                SET ""Data"" = 
	                (
		                d.""Data""
		                || (
			                '{""MaxSpace"":' 
			                || COALESCE(d.""Data""->>'TotalSpace', d.""Data""->>'MaxSpace', 'null') 
			                || '}'
		                )::jsonb
	                )
	                - 'TotalSpace'
                FROM ""Listings"" l 
                LEFT JOIN ""ListingData"" t
	                ON t.""DataType"" = 'PropertyType'
	                AND t.""ListingID"" = COALESCE(l.""ParentListingID"", l.""ID"")
                WHERE d.""ListingID"" = l.""ID""
	                AND d.""DataType"" = 'Specifications'
	                AND d.""Data"" IS NOT NULL
	                AND (
		                t.""Data""->>'Value' IS NULL
		                OR NOT t.""Data""->>'Value' = 'flex'
	                );
            ");
        }
    }
}
