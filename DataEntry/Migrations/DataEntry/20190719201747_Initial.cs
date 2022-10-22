using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Diagnostics.CodeAnalysis;

namespace dataentry.Migrations.DataEntry
{
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PostalCode = table.Column<string>(nullable: true),
                    Street1 = table.Column<string>(nullable: true),
                    Street2 = table.Column<string>(nullable: true),
                    StreetName = table.Column<string>(nullable: true),
                    StreetType = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    StateProvince = table.Column<string>(nullable: true),
                    PreStreetDirectionName = table.Column<string>(nullable: true),
                    PostStreetDirectionName = table.Column<string>(nullable: true),
                    FullStreetName = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Latitude = table.Column<decimal>(nullable: false),
                    Longitude = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    License = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    IsParent = table.Column<bool>(nullable: false),
                    ParentListingID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UsageType = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AddressID = table.Column<int>(nullable: true),
                    AvailableFrom = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Listings_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ListingBrokers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    BrokerID = table.Column<int>(nullable: false),
                    ListingID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingBrokers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ListingBrokers_Brokers_BrokerID",
                        column: x => x.BrokerID,
                        principalTable: "Brokers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListingBrokers_Listings_ListingID",
                        column: x => x.ListingID,
                        principalTable: "Listings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ListingID = table.Column<int>(nullable: false),
                    DataType = table.Column<string>(nullable: true),
                    Data = table.Column<string>(type: "jsonb", nullable: true),
                    Language = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ListingData_Listings_ListingID",
                        column: x => x.ListingID,
                        principalTable: "Listings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "ID", "City", "Country", "County", "FullStreetName", "Latitude", "Longitude", "PostStreetDirectionName", "PostalCode", "PreStreetDirectionName", "StateProvince", "Street1", "Street2", "StreetName", "StreetType" },
                values: new object[,]
                {
                    { -1, "Dallas", "USA", "Dallas", null, 1.23m, 2.32m, null, "75202", null, "TX", "1st Street", null, "Ross Ave", null },
                    { -2, "Dallas", "USA", "Dallas", null, 2.23m, 6.32m, null, "75207", null, "TX", "N Stemmons Fwy", null, "Stemmons Fwy", null }
                });

            migrationBuilder.InsertData(
                table: "Brokers",
                columns: new[] { "ID", "Email", "Fax", "FirstName", "LastName", "License", "Phone" },
                values: new object[,]
                {
                    { -1, "ben.s@test.com", null, "Ben", "Stoke", null, "123456678" },
                    { -2, "Jam@test.com", null, "James", "Anderson", null, "6785673445" },
                    { -3, "tja@test.com", null, "Tony", "Ja", null, "2223331111" },
                    { -4, "tsilv12@test.com", null, "Silva", "T", null, "33322221111" }
                });

            migrationBuilder.InsertData(
                table: "Listings",
                columns: new[] { "ID", "AddressID", "AvailableFrom", "CreatedAt", "IsParent", "Name", "ParentListingID", "Status", "UpdatedAt", "UsageType" },
                values: new object[,]
                {
                    { -1, -1, "10/10/2019", "10/09/2019", true, "Ross", null, "New", "10/09/2019", "Office" },
                    { -2, -1, "10/11/2019", "10/09/2019", false, "Space-1", -1, "New", "10/09/2019", "Office" },
                    { -3, -1, "03/11/2019", "03/09/2019", false, "Space-2", -1, "Closed", "03/09/2019", "Office" },
                    { -4, -2, "07/04/2019", "07/01/2019", true, "WTC", null, "InProgress", "07/01/2019", "Office" },
                    { -5, -2, "08/04/2019", "08/01/2019", false, "Space-1", -4, "InProgress", "08/01/2019", "Office" }
                });

            migrationBuilder.InsertData(
                table: "ListingBrokers",
                columns: new[] { "ID", "BrokerID", "ListingID" },
                values: new object[,]
                {
                    { -1, -1, -1 },
                    { -2, -2, -1 },
                    { -3, -3, -4 },
                    { -4, -4, -4 }
                });

            migrationBuilder.InsertData(
                table: "ListingData",
                columns: new[] { "ID", "Data", "DataType", "Language", "ListingID" },
                values: new object[,]
                {
                    { -1, "{\"PrimaryKey\": \"CA-Plus-1111\"}", "PrimaryKey", "en-US", -1 },
                    { -2, "{\"Highlights\": [{\"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"Air Conditioning\" }, { \"CultureCode\": \"es-ES\", \"Text\": \"Aire Acondicionado\" } ] }, { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"Excellent BREEAM Rating\" } ] }, { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"1:797 sq ft Parking Ratio\" } ] } ]}", "Highlights", "en-US", -1 },
                    { -3, "{\"Photos\": [{\"ImageCaption\": \"Image.Photo-1\", \"ImageResources\": [{ \"Breakpoint\": \"Small\", \"Image.Height\": 480, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_1_640x480.jpg\", \"Image.Width\": 640 }, { \"Breakpoint\": \"Medium\", \"Image.Height\": 960, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_1_1280x960.jpg\", \"Image.Width\": 1280 }, { \"Breakpoint\": \"Large\", \"Image.Height\": 1920, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_1_2560x1920.jpg\", \"Image.Width\": 2560 } ] }, { \"ImageCaption\": \"Image.Photo-3\", \"ImageResources\": [{ \"Image.Height\": 480, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_3_640x480.jpg\", \"Image.Width\": 640 }, { \"Image.Height\": 960, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_3_1280x960.jpg\", \"Image.Width\": 1280 }, { \"Image.Height\": 1920, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_3_2560x1920.jpg\", \"Image.Width\": 2560 } ] } ]}", "Photos", "en-US", -2 },
                    { -4, "{\"UnderOffer\": \"True\"}", "UnderOffer", "en-US", -3 },
                    { -5, "{\"PrimaryKey\": \"CA-Plus-1111\"}", "PrimaryKey", "en-US", -4 },
                    { -6, "{\"NewHome\": \"False\"}", "NewHome", "en-US", -4 },
                    { -7, "{\"Highlights\": [ { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"Excellent BREEAM Rating\" } ] }, { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"B Rating EPC\" } ] } ]}", "Highlights", "en-US", -4 },
                    { -8, "{\"Photos\": [{ \"ImageCaption\": \"DSC06470\", \"ImageResources\": [{ \"Breakpoint\": \"original\", \"Image.Height\": 235, \"Image.Width\": 314, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" }, { \"Breakpoint\": \"small\", \"Image.Height\": 480, \"Image.Width\": 640, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470_Photo_1_small.jpg\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" }, { \"Breakpoint\": \"medium\", \"Image.Height\": 960, \"Image.Width\": 1280, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470_Photo_1_medium.jpg\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" }, { \"Breakpoint\": \"large\", \"Image.Height\": 1200, \"Image.Width\": 1600, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470_Photo_1_large.jpg\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" } ] } ]}", "Photos", "en-US", -4 },
                    { -9, "{\"Brochures\": [{ \"Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/Brochure/2_278296/Tempest%20eBrochure.pdf\", \"Culture\": \"en-GB\" }, { \"Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/Brochure/2_278296/Tempest%20eBrochure01.pdf\", \"Culture\": \"en-GB\" }, { \"Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/Brochure/2_278296/Tithebarn%20Project%20Leisure%20brochure%20Final.pdf\", \"Culture\": \"zh-CN\" } ]}", "Brochures", "en-US", -5 }
                });

            migrationBuilder.Sql("CREATE INDEX \"IX_ListingData_Data\" ON \"ListingData\" USING gin ((\"Data\" -> 'PrimaryKey'));");

            migrationBuilder.CreateIndex(
                name: "IX_ListingBrokers_BrokerID",
                table: "ListingBrokers",
                column: "BrokerID");

            migrationBuilder.CreateIndex(
                name: "IX_ListingBrokers_ListingID",
                table: "ListingBrokers",
                column: "ListingID");

            migrationBuilder.CreateIndex(
                name: "IX_ListingData_ListingID",
                table: "ListingData",
                column: "ListingID");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_AddressID",
                table: "Listings",
                column: "AddressID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingBrokers");

            migrationBuilder.DropTable(
                name: "ListingData");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
