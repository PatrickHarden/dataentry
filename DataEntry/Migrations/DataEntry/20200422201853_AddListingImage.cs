using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace dataentry.Migrations.DataEntry
{
    public partial class AddListingImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Url = table.Column<string>(nullable: true),
                    WatermarkProcessStatus = table.Column<int>(nullable: false),
                    UploadedAt = table.Column<DateTime>(nullable: false),
                    UploadedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ListingImages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ImageID = table.Column<int>(nullable: false),
                    ListingID = table.Column<int>(nullable: false),
                    IsPrimary = table.Column<bool>(nullable: true),
                    DisplayText = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    HasWatermark = table.Column<bool>(nullable: true),
                    ImageCategory = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingImages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ListingImages_Images_ImageID",
                        column: x => x.ImageID,
                        principalTable: "Images",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingImages_Listings_ListingID",
                        column: x => x.ListingID,
                        principalTable: "Listings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_ImageID",
                table: "ListingImages",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_ListingID",
                table: "ListingImages",
                column: "ListingID");
            
            //Move photos and floorplans from jsonb to images table 
            migrationBuilder.Sql(@"
            DO
            $BODY$
            DECLARE
                ""url"" text;
                ""active"" text;
                ""primary"" text;
                ""watermark"" text;
                ""displaytext"" text; 
                ""listingID"" text;
                ""last_id"" int;
				""dataType"" text;
            BEGIN
            FOR ""url"", ""active"", ""primary"", ""watermark"", ""displaytext"", ""listingID"", ""dataType""  IN
            SELECT  jsonb_array_elements(""Data"") ->> 'Url' AS ""URL"",
                        jsonb_array_elements(""Data"") ->> 'Active' AS ""Active"",
                        jsonb_array_elements(""Data"") ->> 'Primary' AS ""Primary"",
                        jsonb_array_elements(""Data"") ->> 'Watermark' AS ""Watermark"",
                        jsonb_array_elements(""Data"") ->> 'DisplayText' AS ""DisplayText"",
                        ""ListingID"", ""DataType"" FROM  ""ListingData"" Where ""DataType"" = 'Photo' or ""DataType"" = 'FloorPlan'
            LOOP
                INSERT INTO ""Images"" (""Url"", ""WatermarkProcessStatus"", ""UploadedAt"", ""IsDeleted"")
                    VALUES (""url"", 5, NOW(), false) RETURNING ""ID"" INTO ""last_id"";

                INSERT INTO ""ListingImages"" (""ImageID"", ""ListingID"", ""IsPrimary"", ""DisplayText"", ""IsActive"", ""HasWatermark"",
                                            ""ImageCategory"", ""Order"") 
                VALUES (""last_id"", Cast(""listingID"" AS INTEGER), Cast(""primary"" AS BOOLEAN), 
                        ""displaytext"", Cast(""active"" AS BOOLEAN),
                        Cast(""watermark"" AS BOOLEAN), ""dataType"", 0);
            END LOOP;
            END;
            $BODY$ language plpgsql;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingImages");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
