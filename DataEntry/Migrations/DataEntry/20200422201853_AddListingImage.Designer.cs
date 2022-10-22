﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using dataentry.Data.DBContext;

namespace dataentry.Migrations.DataEntry
{
    [DbContext(typeof(DataEntryContext))]
    [Migration("20200422201853_AddListingImage")]
    partial class AddListingImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("dataentry.Data.DBContext.Model.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("County");

                    b.Property<string>("FullStreetName");

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.Property<string>("PostStreetDirectionName");

                    b.Property<string>("PostalCode");

                    b.Property<string>("PreStreetDirectionName");

                    b.Property<string>("StateProvince");

                    b.Property<string>("Street1");

                    b.Property<string>("Street2");

                    b.Property<string>("StreetName");

                    b.Property<string>("StreetType");

                    b.HasKey("ID");

                    b.ToTable("Address");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            City = "Dallas",
                            Country = "USA",
                            County = "Dallas",
                            Latitude = 1.23m,
                            Longitude = 2.32m,
                            PostalCode = "75202",
                            StateProvince = "TX",
                            Street1 = "1st Street",
                            StreetName = "Ross Ave"
                        },
                        new
                        {
                            ID = -2,
                            City = "Dallas",
                            Country = "USA",
                            County = "Dallas",
                            Latitude = 2.23m,
                            Longitude = 6.32m,
                            PostalCode = "75207",
                            StateProvince = "TX",
                            Street1 = "N Stemmons Fwy",
                            StreetName = "Stemmons Fwy"
                        });
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.Broker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<string>("Email");

                    b.Property<string>("Fax");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("License");

                    b.Property<string>("Location");

                    b.Property<string>("Phone");

                    b.HasKey("ID");

                    b.ToTable("Brokers");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            Email = "ben.s@test.com",
                            FirstName = "Ben",
                            LastName = "Stoke",
                            Location = "ABQ, NM",
                            Phone = "123456678"
                        },
                        new
                        {
                            ID = -2,
                            Email = "Jam@test.com",
                            FirstName = "James",
                            LastName = "Anderson",
                            Location = "Irving, TX",
                            Phone = "6785673445"
                        },
                        new
                        {
                            ID = -3,
                            Email = "tja@test.com",
                            FirstName = "Tony",
                            LastName = "Ja",
                            Location = "Dallas, TX",
                            Phone = "2223331111"
                        },
                        new
                        {
                            ID = -4,
                            Email = "tsilv12@test.com",
                            FirstName = "Silva",
                            LastName = "T",
                            Location = "Fort Lee, NJ",
                            Phone = "33322221111"
                        });
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.Image", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("UploadedAt");

                    b.Property<string>("UploadedBy");

                    b.Property<string>("Url");

                    b.Property<int>("WatermarkProcessStatus");

                    b.HasKey("ID");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.Listing", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressID");

                    b.Property<DateTime?>("AvailableFrom");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsParent");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentListingID");

                    b.Property<string>("Status");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UsageType");

                    b.HasKey("ID");

                    b.HasIndex("AddressID");

                    b.HasIndex("ParentListingID");

                    b.ToTable("Listings");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            AddressID = -1,
                            AvailableFrom = new DateTime(2019, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedAt = new DateTime(2019, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            IsParent = true,
                            Name = "Ross",
                            Status = "New",
                            UpdatedAt = new DateTime(2019, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UsageType = "Office"
                        },
                        new
                        {
                            ID = -2,
                            AddressID = -1,
                            AvailableFrom = new DateTime(2019, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedAt = new DateTime(2019, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            IsParent = false,
                            Name = "Space-1",
                            ParentListingID = -1,
                            Status = "New",
                            UpdatedAt = new DateTime(2019, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UsageType = "Office"
                        },
                        new
                        {
                            ID = -3,
                            AddressID = -1,
                            AvailableFrom = new DateTime(2019, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedAt = new DateTime(2019, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            IsParent = false,
                            Name = "Space-2",
                            ParentListingID = -1,
                            Status = "Closed",
                            UpdatedAt = new DateTime(2019, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UsageType = "Office"
                        },
                        new
                        {
                            ID = -4,
                            AddressID = -2,
                            AvailableFrom = new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedAt = new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            IsParent = true,
                            Name = "WTC",
                            Status = "InProgress",
                            UpdatedAt = new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UsageType = "Office"
                        },
                        new
                        {
                            ID = -5,
                            AddressID = -2,
                            AvailableFrom = new DateTime(2019, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CreatedAt = new DateTime(2019, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            IsParent = false,
                            Name = "Space-1",
                            ParentListingID = -4,
                            Status = "InProgress",
                            UpdatedAt = new DateTime(2019, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UsageType = "Office"
                        });
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.ListingBroker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrokerID");

                    b.Property<int>("ListingID");

                    b.Property<int>("Order");

                    b.HasKey("ID");

                    b.HasIndex("BrokerID");

                    b.HasIndex("ListingID");

                    b.ToTable("ListingBrokers");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            BrokerID = -1,
                            ListingID = -1,
                            Order = 0
                        },
                        new
                        {
                            ID = -2,
                            BrokerID = -2,
                            ListingID = -1,
                            Order = 0
                        },
                        new
                        {
                            ID = -3,
                            BrokerID = -3,
                            ListingID = -4,
                            Order = 0
                        },
                        new
                        {
                            ID = -4,
                            BrokerID = -4,
                            ListingID = -4,
                            Order = 0
                        });
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.ListingData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data")
                        .HasColumnType("jsonb");

                    b.Property<string>("DataType");

                    b.Property<string>("Language");

                    b.Property<int>("ListingID");

                    b.HasKey("ID");

                    b.HasIndex("ListingID");

                    b.ToTable("ListingData");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            Data = "{\"PrimaryKey\": \"CA-Plus-1111\"}",
                            DataType = "PrimaryKey",
                            Language = "en-US",
                            ListingID = -1
                        },
                        new
                        {
                            ID = -2,
                            Data = "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]",
                            DataType = "Highlights",
                            Language = "en-US",
                            ListingID = -1
                        },
                        new
                        {
                            ID = -3,
                            Data = "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]",
                            DataType = "Photos",
                            Language = "en-US",
                            ListingID = -2
                        },
                        new
                        {
                            ID = -4,
                            Data = "{\"UnderOffer\": \"True\"}",
                            DataType = "UnderOffer",
                            Language = "en-US",
                            ListingID = -3
                        },
                        new
                        {
                            ID = -5,
                            Data = "{\"PrimaryKey\": \"CA-Plus-1111\"}",
                            DataType = "PrimaryKey",
                            Language = "en-US",
                            ListingID = -4
                        },
                        new
                        {
                            ID = -6,
                            Data = "{\"NewHome\": \"False\"}",
                            DataType = "NewHome",
                            Language = "en-US",
                            ListingID = -4
                        },
                        new
                        {
                            ID = -7,
                            Data = "[{\"value\": \"Is Amazing\"}, {\"value\": \"Is Awesome\"}]",
                            DataType = "Highlights",
                            Language = "en-US",
                            ListingID = -4
                        },
                        new
                        {
                            ID = -8,
                            Data = "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]",
                            DataType = "Photos",
                            Language = "en-US",
                            ListingID = -4
                        },
                        new
                        {
                            ID = -9,
                            Data = "[{\"Url\": \"http://placekitten.com/200/300\", \"Active\": true, \"Primary\": true, \"DisplayText\": \"placekitten.jpg\"}]",
                            DataType = "Brochures",
                            Language = "en-US",
                            ListingID = -5
                        });
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.ListingImage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayText");

                    b.Property<bool?>("HasWatermark");

                    b.Property<string>("ImageCategory");

                    b.Property<int>("ImageID");

                    b.Property<bool?>("IsActive");

                    b.Property<bool?>("IsPrimary");

                    b.Property<int>("ListingID");

                    b.Property<int>("Order");

                    b.HasKey("ID");

                    b.HasIndex("ImageID");

                    b.HasIndex("ListingID");

                    b.ToTable("ListingImages");
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.Listing", b =>
                {
                    b.HasOne("dataentry.Data.DBContext.Model.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressID");

                    b.HasOne("dataentry.Data.DBContext.Model.Listing")
                        .WithMany("Spaces")
                        .HasForeignKey("ParentListingID");
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.ListingBroker", b =>
                {
                    b.HasOne("dataentry.Data.DBContext.Model.Broker", "Broker")
                        .WithMany("ListingBroker")
                        .HasForeignKey("BrokerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("dataentry.Data.DBContext.Model.Listing", "Listing")
                        .WithMany("ListingBroker")
                        .HasForeignKey("ListingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.ListingData", b =>
                {
                    b.HasOne("dataentry.Data.DBContext.Model.Listing", "Listing")
                        .WithMany("ListingData")
                        .HasForeignKey("ListingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("dataentry.Data.DBContext.Model.ListingImage", b =>
                {
                    b.HasOne("dataentry.Data.DBContext.Model.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("dataentry.Data.DBContext.Model.Listing", "Listing")
                        .WithMany("ListingImage")
                        .HasForeignKey("ListingID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
