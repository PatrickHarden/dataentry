using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace dataentry.Migrations.DataEntry
{
    [ExcludeFromCodeCoverage]
    public partial class UpdateSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -9,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -8,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -7,
                column: "Data",
                value: "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -3,
                column: "Data",
                value: "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -2,
                column: "Data",
                value: "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -9,
                column: "Data",
                value: "{\"Brochures\": [{ \"Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/Brochure/2_278296/Tempest%20eBrochure.pdf\", \"Culture\": \"en-GB\" }, { \"Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/Brochure/2_278296/Tempest%20eBrochure01.pdf\", \"Culture\": \"en-GB\" }, { \"Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/Brochure/2_278296/Tithebarn%20Project%20Leisure%20brochure%20Final.pdf\", \"Culture\": \"zh-CN\" } ]}");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -8,
                column: "Data",
                value: "{\"Photos\": [{ \"ImageCaption\": \"DSC06470\", \"ImageResources\": [{ \"Breakpoint\": \"original\", \"Image.Height\": 235, \"Image.Width\": 314, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" }, { \"Breakpoint\": \"small\", \"Image.Height\": 480, \"Image.Width\": 640, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470_Photo_1_small.jpg\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" }, { \"Breakpoint\": \"medium\", \"Image.Height\": 960, \"Image.Width\": 1280, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470_Photo_1_medium.jpg\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" }, { \"Breakpoint\": \"large\", \"Image.Height\": 1200, \"Image.Width\": 1600, \"Resource.Uri\": \"/resources/fileassets/CA-Plus-22169/b9f98dc7/DSC06470_Photo_1_large.jpg\", \"Source.Uri\": \"https://uatlistingssearchcbreeun.blob.core.windows.net/fileassets/CA-Plus-22169/b9f98dc7/DSC06470.JPG\" } ] } ]}");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -7,
                column: "Data",
                value: "{\"Highlights\": [ { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"Excellent BREEAM Rating\" } ] }, { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"B Rating EPC\" } ] } ]}");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -3,
                column: "Data",
                value: "{\"Photos\": [{\"ImageCaption\": \"Image.Photo-1\", \"ImageResources\": [{ \"Breakpoint\": \"Small\", \"Image.Height\": 480, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_1_640x480.jpg\", \"Image.Width\": 640 }, { \"Breakpoint\": \"Medium\", \"Image.Height\": 960, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_1_1280x960.jpg\", \"Image.Width\": 1280 }, { \"Breakpoint\": \"Large\", \"Image.Height\": 1920, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_1_2560x1920.jpg\", \"Image.Width\": 2560 } ] }, { \"ImageCaption\": \"Image.Photo-3\", \"ImageResources\": [{ \"Image.Height\": 480, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_3_640x480.jpg\", \"Image.Width\": 640 }, { \"Image.Height\": 960, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_3_1280x960.jpg\", \"Image.Width\": 1280 }, { \"Image.Height\": 1920, \"Resource.Uri\": \"file://emea.cbre.net/Data/WebPlatform/ListingService/PRD/Agency365Data/OfficePLUS/photos/cbrrps-BOW120225_Photo_3_2560x1920.jpg\", \"Image.Width\": 2560 } ] } ]}");

            migrationBuilder.UpdateData(
                table: "ListingData",
                keyColumn: "ID",
                keyValue: -2,
                column: "Data",
                value: "{\"Highlights\": [{\"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"Air Conditioning\" }, { \"CultureCode\": \"es-ES\", \"Text\": \"Aire Acondicionado\" } ] }, { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"Excellent BREEAM Rating\" } ] }, { \"Highlight\": [{ \"CultureCode\": \"en-GB\", \"Text\": \"1:797 sq ft Parking Ratio\" } ] } ]}");
        }
    }
}
