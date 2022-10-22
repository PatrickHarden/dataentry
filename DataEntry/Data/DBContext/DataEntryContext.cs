using System;
using dataentry.Data.DBContext.Model;
using dataentry.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace dataentry.Data.DBContext
{
    public class DataEntryContext : DbContext
    {
        #region DbSet
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Listing> Listings { get; set; }
        public virtual DbSet<ListingData> ListingData { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<ListingBroker> ListingBrokers { get; set; }
        public virtual DbSet<Broker> Brokers { get; set; } 
        public virtual DbSet<ListingImage> ListingImages { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        #endregion

        public DbQuery<DEReport> ReportModel { get; set; }

        private readonly IConfiguration Configuration;

        public DataEntryContext(DbContextOptions<DataEntryContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Configuration.GetConnectionString("Postgres"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ForNpgsqlUseIdentityAlwaysColumns();

            #region Seed Data
            var defaultRegion = new Region()
            {
                ID = Region.DefaultID,
                Name = "Default",
                HomeSiteID = "us-comm",
                ListingPrefix = "US-SMPL",
                PreviewPrefix = "US-PREV",
                CountryCode = "US",
                CultureCode = "en-US",
                PreviewSiteID = "us-comm-prev"
            };
            modelBuilder.Entity<Region>().HasData(defaultRegion);

            modelBuilder.Entity<Listing>().HasData(
                new Listing()
                {
                    RegionID = defaultRegion.ID,
                    ID = -1,
                    IsParent = true,
                    Name = "Ross",
                    UsageType = "Office",
                    Status = "New",
                    AddressID = -1,
                    AvailableFrom = new System.DateTime(2019, 10, 10),
                    UpdatedAt = new System.DateTime(2019, 10, 09),
                    CreatedAt = new System.DateTime(2019, 10, 09),
                },
                new Listing()
                {
                    RegionID = defaultRegion.ID,
                    ID = -2,
                    IsParent = false,
                    ParentListingID = -1,
                    Name = "Space-1",
                    UsageType = "Office",
                    Status = "New",
                    AddressID = -1,
                    AvailableFrom = new System.DateTime(2019, 10, 11),
                    UpdatedAt = new System.DateTime(2019, 10, 09),
                    CreatedAt = new System.DateTime(2019, 10, 09),
                },
                new Listing()
                {
                    RegionID = defaultRegion.ID,
                    ID = -3,
                    IsParent = false,
                    ParentListingID = -1,
                    Name = "Space-2",
                    UsageType = "Office",
                    Status = "Closed",
                    AddressID = -1,
                    AvailableFrom = new System.DateTime(2019, 03, 11),
                    UpdatedAt = new System.DateTime(2019, 03, 09),
                    CreatedAt = new System.DateTime(2019, 03, 09),
                },
                new Listing()
                {
                    RegionID = defaultRegion.ID,
                    ID = -4,
                    IsParent = true,
                    Name = "WTC",
                    UsageType = "Office",
                    Status = "InProgress",
                    AddressID = -2,
                    AvailableFrom = new System.DateTime(2019, 07, 04),
                    UpdatedAt = new System.DateTime(2019, 07, 01),
                    CreatedAt = new System.DateTime(2019, 07, 01),
                },
                new Listing()
                {
                    RegionID = defaultRegion.ID,
                    ID = -5,
                    IsParent = false,
                    ParentListingID = -4,
                    Name = "Space-1",
                    UsageType = "Office",
                    Status = "InProgress",
                    AddressID = -2,
                    AvailableFrom = new System.DateTime(2019, 08, 04),
                    UpdatedAt = new System.DateTime(2019, 08, 01),
                    CreatedAt = new System.DateTime(2019, 08, 01),
                }
            );

            modelBuilder.Entity<ListingData>().HasData(
                new ListingData()
                {
                    ID = -1,
                    ListingID = -1,
                    DataType = "PrimaryKey",
                    Data = "{\"PrimaryKey\": \"CA-Plus-1111\"}",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -2,
                    ListingID = -1,
                    DataType = "Highlights",
                    Data = "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -3,
                    ListingID = -2,
                    DataType = "Photos",
                    Data = "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -4,
                    ListingID = -3,
                    DataType = "UnderOffer",
                    Data = "{\"UnderOffer\": \"True\"}",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -5,
                    ListingID = -4,
                    DataType = "PrimaryKey",
                    Data = "{\"PrimaryKey\": \"CA-Plus-1111\"}",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -6,
                    ListingID = -4,
                    DataType = "NewHome",
                    Data = "{\"NewHome\": \"False\"}",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -7,
                    ListingID = -4,
                    DataType = "Highlights",
                    Data = "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -8,
                    ListingID = -4,
                    DataType = "Photos",
                    Data = "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]",
                    Language = "en-US"
                },
                new ListingData()
                {
                    ID = -9,
                    ListingID = -5,
                    DataType = "Brochures",
                    Data = "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]",
                    Language = "en-US"
                }
                );

            modelBuilder.Entity<Address>().HasData(
                new Address()
                {
                    ID = -1,
                    PostalCode = "75202",
                    Street1 = "1st Street",
                    StreetName = "Ross Ave",
                    City = "Dallas",
                    StateProvince = "TX",
                    County = "Dallas",
                    Country = "USA",
                    Latitude = 1.23m,
                    Longitude = 2.32m
                },
                new Address()
                {
                    ID = -2,
                    PostalCode = "75207",
                    Street1 = "N Stemmons Fwy",
                    StreetName = "Stemmons Fwy",
                    City = "Dallas",
                    StateProvince = "TX",
                    County = "Dallas",
                    Country = "USA",
                    Latitude = 2.23m,
                    Longitude = 6.32m
                });

            modelBuilder.Entity<Broker>().HasData(
                new Broker()
                {
                    ID = -1,
                    FirstName = "Ben",
                    LastName = "Stoke",
                    Email = "ben.s@test.com",
                    Phone = "123456678",
                    Location = "ABQ, NM"
                },
                new Broker()
                {
                    ID = -2,
                    FirstName = "James",
                    LastName = "Anderson",
                    Email = "Jam@test.com",
                    Phone = "6785673445",
                    Location = "Irving, TX"
                },
                new Broker()
                {
                    ID = -3,
                    FirstName = "Tony",
                    LastName = "Ja",
                    Email = "tja@test.com",
                    Phone = "2223331111",
                    Location = "Dallas, TX"
                },
                new Broker()
                {
                    ID = -4,
                    FirstName = "Silva",
                    LastName = "T",
                    Email = "tsilv12@test.com",
                    Phone = "33322221111",
                    Location = "Fort Lee, NJ"
                });

            modelBuilder.Entity<ListingBroker>().HasData(
                new ListingBroker()
                {
                    ID = -1,
                    BrokerID = -1,
                    ListingID = -1
                },
                new ListingBroker()
                {
                    ID = -2,
                    BrokerID = -2,
                    ListingID = -1
                },
                new ListingBroker()
                {
                    ID = -3,
                    BrokerID = -3,
                    ListingID = -4
                },
                new ListingBroker()
                {
                    ID = -4,
                    BrokerID = -4,
                    ListingID = -4
                }
                );
            #endregion
        }
    }
}
