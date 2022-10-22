using Microsoft.EntityFrameworkCore.Migrations;

namespace dataentry.Migrations.DataEntry
{
    public partial class DeleteBrokersWithoutProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                Delete FROM ""Brokers"" 
                WHERE ""ID"" NOT IN (
                SELECT ""BrokerID"" FROM ""ListingBrokers"" Group by ""BrokerID""
                );
            ");

            migrationBuilder.Sql(@"   
                Update ""ListingData""
                Set ""Data"" = '{""Value"": ""healthcare""}'::jsonb
                where ""DataType"" = 'PropertyType' and ""Data""->>'Value' = 'Healthcare';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"   
                Update ""ListingData""
                Set ""Data"" = '{""Value"": ""Healthcare""}'::jsonb
                where ""DataType"" = 'PropertyType' and ""Data""->>'Value' = 'healthcare';
            ");
        }
    }
}
