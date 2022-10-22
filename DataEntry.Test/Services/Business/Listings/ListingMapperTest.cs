﻿using dataentry.Data;
using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Services.Business.Listings;
using dataentry.Test.Utility;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace dataentry.Test.Services.Business.Listings
{
    public class ListingMapperTest
    {
        private Mock<IConfiguration> _mockConfiguration;
        private ListingMapper _listingMapper;
        private Listing _listing;
        private ListingViewModel _listingViewModel;
        private IOptions<dataentry.Utility.Configs> _config;
        private Mock<ILogger<ListingMapper>> _mockLogger;

        public static IEnumerable<object[]> TestData_Bool => new object[][]
        {
            new object[] {true},
            new object[] {false}
        };
        public static IEnumerable<object[]> TestData_UsageType => TestHelpers.EnumToTestData<UsageTypeEnum>();
        public static IEnumerable<object[]> TestData_ListingType => TestHelpers.EnumToTestData<ChargeKindEnum>();
        public static IEnumerable<object[]> TestData_PublishingState => TestHelpers.EnumToTestData<PublishingStateEnum>();

        public ListingMapperTest()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["StoreSettings:SearchApiEndPoint"]).Returns("TestSearchApiEndPoint");
            _mockConfiguration.Setup(c => c["StoreSettings:SearchKey"]).Returns("TestSearchKey");
            _mockConfiguration.Setup(c => c["FeatureFlags:PreviewFeatureFlag"]).Returns("true");
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .AddJsonFile("appsettings.Local.json", false)
               .Build();
            _config = Options.Create(configuration.Get<dataentry.Utility.Configs>());

            var fakeSiteMapsConfigDataProvider = new Mock<ISiteMapsConfigDataProvider>();
            fakeSiteMapsConfigDataProvider.Setup(f => f.GetSitemapConfig()).Returns(JsonConvert.DeserializeObject<JObject>("{\"us-comm\":{\"office\":{\"en-US\":\"https://www.cbre.us/properties/properties-for-lease/office/details/{0}?view={1}\"},\"Industrial\":{\"en-US\":\"https://www.cbre.us/properties/properties-for-lease/industrial/details/{0}?view={1}\"}}}"));

            _mockLogger = new Mock<ILogger<ListingMapper>>();

            _listingMapper = new ListingMapper(_mockConfiguration.Object, _config, fakeSiteMapsConfigDataProvider.Object, _mockLogger.Object);
            _listing = new Listing{ID = 1};
            _listing.ListingData = new List<ListingData>();
            _listingViewModel = new ListingViewModel();
            _listingViewModel.Specifications = new SpecificationsViewModel();

            _listing.Region = new Region
            {
                Name = "Test Region",
                CountryCode = "US",
                CultureCode = "en-US",
                HomeSiteID = "us-comm",
                ListingPrefix = "TestListingOrigin",
                PreviewSiteID = "us-comm-prev",
                PreviewPrefix = "TestListingPreviewOrigin",
                ExternalPublishUrl = "TestExternalPublishUrl",
                ExternalPreviewUrl = "TestExternalPreviewUrl"
            };

            _listing.VerifyExternalID().Wait();
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public void ListingMapperTest_MapListing()
        {
            //Arrange
            _listing.ID = 123;
            _listing.Name = "TestName";
            _listing.CreatedAt = new DateTime(2019, 10, 15);
            _listing.UpdatedAt = new DateTime(2019, 10, 16);
            _listing.AvailableFrom = new DateTime(2019, 10, 17);

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(123, result.Id);
            Assert.Equal("TestName", result.PropertyName);
            Assert.Equal(new DateTime(2019, 10, 15), result.DateCreated);
            Assert.Equal(new DateTime(2019, 10, 16), result.DateUpdated);
            Assert.Equal(new DateTime(2019, 10, 17), result.AvailableFrom);
        }

        [Fact]
        public void ListingMapperTest_MapListing_Address()
        {
            //Arrange
            _listing.Address = new Address
            {
                Street1 = "TestStreet1",
                Street2 = "TestStreet2",
                City = "TestCity",
                StateProvince = "TestState",
                PostalCode = "TestZip",
                Country = "US",
                Latitude = 123,
                Longitude = 456
            };

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestStreet1", result.Street);
            Assert.Equal("TestStreet2", result.Street2);
            Assert.Equal("TestCity", result.City);
            Assert.Equal("TestState", result.StateOrProvince);
            Assert.Equal("TestZip", result.PostalCode);
            Assert.Equal("US", result.Country);
            Assert.Equal(123, result.Lat);
            Assert.Equal(456, result.Lng);
        }

        [Theory]
        [MemberData(nameof(TestData_Bool))]
        public void ListingMapperTest_MapListing_IsBulkUpload(bool value)
        {
            //Arrange
            _listing.SetListingData(new IsBulkUpload { Value = value });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(value, result.IsBulkUpload);
        }

        [Theory]
        [MemberData(nameof(TestData_UsageType))]
        public void ListingMapperTest_MapListing_PropertyType(string value)
        {
            //Arrange
            _listing.SetListingData(new PropertyType { Value = value });
            _listing.SetListingData(new Data.DBContext.Model.ListingType { Value = "sale" });
            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(value, result.PropertyType);
        }

        [Theory]
        [MemberData(nameof(TestData_ListingType))]
        public void ListingMapperTest_MapListing_ListingType(string value)
        {
            //Arrange
            _listing.SetListingData(new Data.DBContext.Model.ListingType { Value = value });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(value, result.ListingType);
        }

        [Fact]
        public void ListingMapperTest_MapListing_Website()
        {
            //Arrange
            _listing.SetListingData(new Website { Value = "http://testwebsite/" });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("http://testwebsite/", result.Website);
        }

        [Fact]
        public void ListingMapperTest_MapListing_Video()
        {
            //Arrange
            _listing.SetListingData(new Video { Value = "http://testvideo/" });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("http://testvideo/", result.Video);
        }

        [Fact]
        public void ListingMapperTest_MapListing_WalkThrough()
        {
            //Arrange
            _listing.SetListingData(new WalkThrough { Value = "http://testwalkthrough/" });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("http://testwalkthrough/", result.WalkThrough);
        }

        [Fact]
        public void ListingMapperTest_MapListing_Operator()
        {
            //Arrange
            _listing.SetListingData(new Operator { Value = "TestOperator" });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestOperator", result.Operator);
        }

        [Fact]
        public void ListingMapperTest_MapListing_Headline()
        {
            //Arrange
            _listing.SetListingDataAllLanguages(new List<Headline> { new Headline { CultureCode = "en-US", Text = "TestHeadline" } });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("en-US", result.Headline.Single().CultureCode);
            Assert.Equal("TestHeadline", result.Headline.Single().Text);
        }

        [Theory]
        [MemberData(nameof(TestData_PublishingState))]
        public void ListingMapperTest_MapListing_PublishingState(string value)
        {
            //Arrange
            _listing.SetListingData(new PublishingState { Value = value });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(value, result.State);
        }

        [Theory]
        [MemberData(nameof(TestData_PublishingState))]
        public void ListingMapperTest_MapListing_PreviewState(string value)
        {
            //Arrange
            _listing.SetListingData(new PreviewState { Value = value });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(value, result.PreviewState);
        }

        [Fact]
        public void ListingMapperTest_MapListing_BuildingDescription()
        {
            //Arrange
            _listing.SetListingDataAllLanguages(
                new List<BuildingDescription>
                {
                    new BuildingDescription
                    {
                        CultureCode = "en-US",
                        Text = "TestBuildingDescription" 
                    } 
                });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("en-US", result.BuildingDescription.Single().CultureCode);
            Assert.Equal("TestBuildingDescription", result.BuildingDescription.Single().Text);
        }

        [Fact]
        public void ListingMapperTest_MapListing_LocationDescription()
        {
            //Arrange
            _listing.SetListingDataAllLanguages(
                new List<LocationDescription>
                {
                    new LocationDescription
                    {
                        CultureCode = "en-US",
                        Text = "TestLocationDescription"
                    }
                });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("en-US", result.LocationDescription.Single().CultureCode);
            Assert.Equal("TestLocationDescription", result.LocationDescription.Single().Text);
        }

        [Fact]
        public void ListingMapperTest_MapListing_ExternalPublishUrl()
        {
            //Arrange
            _listing.SetListingData(new PropertyType { Value = "office" });
            _listing.SetListingData(new PublishingState { Value = "Published" });
            _listing.SetListingData(new Data.DBContext.Model.ListingType { Value = "lease" });
            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("https://www.cbre.us/properties/properties-for-lease/office/details/TestListingOrigin-1?view=isLetting", result.ExternalPublishUrl);

        }

        [Fact]
        public void ListingMapperTest_MapListing_ExternalPreviewUrl()
        {
            // Arrange
            _listing.SetListingData(new PropertyType { Value = "office" });
            _listing.SetListingData(new PreviewState { Value = "Published" });
            _listing.SetListingData(new Data.DBContext.Model.ListingType { Value = "sale" });
        
            // Act
            var result = _listingMapper.Map(_listing);
        
            // Assert
            Assert.Equal("https://www.cbre.us/properties/properties-for-lease/office/details/TestListingPreviewOrigin-1?view=isLetting", result.ExternalPreviewUrl);
        }

        [Fact]
        public void ListingMapperTest_MapListing_PreviewSearchApiEndPoint()
        {
            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestSearchApiEndPointsite=us-comm-prev&TestSearchKey=TestListingPreviewOrigin-1", result.PreviewSearchApiEndPoint);
        }

        [Fact]
        public void ListingMapperTest_MapListing_PropertyRecordName()
        {
            //Arrange
            _listing.SetListingData(new PropertyRecordName { Value = "TestPropertyRecordName" });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestPropertyRecordName", result.PropertyRecordName);
        }

        [Fact]
        public void ListingMapperTest_MapListing_Brochures()
        {
            //Arrange
            _listing.SetListingDataArray(new List<Brochure> {
                new Brochure { Active = true, DisplayText = "TestBrochure1", Primary = true, Url = "TestUrl1" },
                new Brochure { Active = false, DisplayText = "TestBrochure2", Primary = false, Url = "TestUrl2" }
            });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Brochures,
                brochure =>
                {
                    Assert.True(brochure.Active);
                    Assert.Equal("TestBrochure1", brochure.DisplayText);
                    Assert.True(brochure.Primary);
                    Assert.Equal("TestUrl1", brochure.Url);
                },
                brochure =>
                {
                    Assert.False(brochure.Active);
                    Assert.Equal("TestBrochure2", brochure.DisplayText);
                    Assert.False(brochure.Primary);
                    Assert.Equal("TestUrl2", brochure.Url);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Photos()
        {
            //Arrange
            _listing.ListingImage = new List<ListingImage>
            {
                new ListingImage
                {
                    ImageCategory = "Photo",
                    DisplayText = "TestPhoto1",
                    IsActive = true,
                    Order = 2,
                    IsPrimary = true,
                    HasWatermark = true,
                    IsUserOverride = true,
                    Image = new Image
                    {
                        Url = "TestUrl1",
                        WatermarkProcessStatus = 4
                    }
                },
                new ListingImage
                {
                    ImageCategory = "Photo",
                    Order = 1,
                    IsActive = false,
                    IsPrimary = false,
                    DisplayText = "TestPhoto2",
                    HasWatermark = false,
                    IsUserOverride = false,
                    Image = new Image
                    {
                        Url = "TestUrl2",
                        WatermarkProcessStatus = 4
                    }
                }
            };

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Photos,
                photo =>
                {
                    Assert.False(photo.Active);
                    Assert.Equal("TestPhoto2", photo.DisplayText);
                    Assert.False(photo.Primary);
                    Assert.False(photo.UserOverride);
                    Assert.Equal("TestUrl2", photo.Url);
                    Assert.False(photo.Watermark);
                    Assert.Equal(1, photo.Order);
                },
                photo =>
                {
                    Assert.True(photo.Active);
                    Assert.Equal("TestPhoto1", photo.DisplayText);
                    Assert.True(photo.Primary);
                    Assert.True(photo.UserOverride);
                    Assert.Equal("TestUrl1", photo.Url);
                    Assert.True(photo.Watermark);
                    Assert.Equal(2, photo.Order);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_FloorPlans()
        {
            //Arrange
            _listing.ListingImage = new List<ListingImage>
            {
                new ListingImage
                {
                    ImageCategory = "FloorPlan",
                    Order = 2,
                    IsActive = false,
                    IsPrimary = false,
                    DisplayText = "TestFloorPlan2",
                    HasWatermark = false,
                    IsUserOverride = false,
                    Image = new Image
                    {
                        Url = "TestUrl2",
                        WatermarkProcessStatus = 4
                    }
                },
                new ListingImage
                {
                    ImageCategory = "FloorPlan",
                    DisplayText = "TestFloorPlan1",
                    IsActive = true,
                    Order = 1,
                    IsPrimary = true,
                    HasWatermark = true,
                    IsUserOverride = true,
                    Image = new Image
                    {
                        Url = "TestUrl1",
                        WatermarkProcessStatus = 4
                    }
                }
            };

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Floorplans,
                floorplan =>
                {
                    Assert.True(floorplan.Active);
                    Assert.Equal("TestFloorPlan1", floorplan.DisplayText);
                    Assert.True(floorplan.Primary);
                    Assert.Equal("TestUrl1", floorplan.Url);
                    Assert.True(floorplan.Watermark);
                    Assert.True(floorplan.UserOverride);
                    Assert.Equal(1, floorplan.Order);
                },
                floorplan =>
                {
                    Assert.False(floorplan.Active);
                    Assert.Equal("TestFloorPlan2", floorplan.DisplayText);
                    Assert.False(floorplan.Primary);
                    Assert.Equal("TestUrl2", floorplan.Url);
                    Assert.False(floorplan.Watermark);
                    Assert.False(floorplan.UserOverride);
                    Assert.Equal(2, floorplan.Order);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Highlights()
        {
            //Arrange
            _listing.SetListingDataArrayAllLanguages(new List<Highlight> {
                new Highlight { Order=1, Text="TestHighlight1" },
                new Highlight { Order=2, Text="TestHighlight2" }
            });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Highlights,
                highlight =>
                {
                    Assert.Equal(1, highlight.Order);
                    Assert.Equal("TestHighlight1", highlight.Text);
                },
                highlight =>
                {
                    Assert.Equal(2, highlight.Order);
                    Assert.Equal("TestHighlight2", highlight.Text);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_MicroMarkets()
        {
            //Arrange
            _listing.SetListingDataArray(new List<MicroMarket> {
                new MicroMarket { Order=1, Value="TestMicroMarket1" },
                new MicroMarket { Order=2, Value="TestMicroMarket2" }
            });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.MicroMarkets,
                microMarket =>
                {
                    Assert.Equal(1, microMarket.Order);
                    Assert.Equal("TestMicroMarket1", microMarket.Value);
                },
                microMarket =>
                {
                    Assert.Equal(2, microMarket.Order);
                    Assert.Equal("TestMicroMarket2", microMarket.Value);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Specifications()
        {
            //Arrange
            _listing.SetListingData(new Specifications
            {
                LeaseType = "TestLeaseType",
                Measure = "TestMeasure",
                LeaseTerm = "TestLeaseTerm",
                MinSpace = 10,
                MaxSpace = 20,
                MinPrice = 30,
                MaxPrice = 40,
                ContactBrokerForPrice = true,
                TaxModifer = "None"
            });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.NotNull(result.Specifications);
            Assert.Equal("TestLeaseType", result.Specifications.LeaseType);
            Assert.Equal("TestMeasure", result.Specifications.Measure);
            Assert.Equal("TestLeaseTerm", result.Specifications.LeaseTerm);
            Assert.Equal(10, result.Specifications.MinSpace);
            Assert.Equal(20, result.Specifications.MaxSpace);
            Assert.Equal(30, result.Specifications.MinPrice);
            Assert.Equal(40, result.Specifications.MaxPrice);
            Assert.True(result.Specifications.ContactBrokerForPrice);
            Assert.Equal("None", result.Specifications.TaxModifer);
        }

        [Fact]
        public void ListingMapperTest_MapListing_ListingBroker()
        {
            //Arrange
            _listing.ListingBroker = new List<ListingBroker>
            {
                new ListingBroker
                {
                    Order = 2,
                    Broker = new Broker
                    {
                        FirstName = "TestFirstName2",
                        LastName = "TestLastName2",
                        Email = "TestEmail2",
                        Phone = "TestPhone2",
                        License = "ABC22222",
                        Location = "Dallas"
                    }
                },
                new ListingBroker
                {
                    Order = 1,
                    Broker = new Broker
                    {
                        FirstName = "TestFirstName1",
                        LastName = "TestLastName1",
                        Email = "TestEmail1",
                        Phone = "TestPhone1",
                        License = "ABC11111",
                        Location = "Dallas"
                    }
                }
            };

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestFirstName1", result.Contacts.FirstOrDefault().FirstName);
            Assert.Equal("TestLastName1", result.Contacts.FirstOrDefault().LastName);
            Assert.Equal("TestEmail1", result.Contacts.FirstOrDefault().Email);
            Assert.Equal("TestPhone1", result.Contacts.FirstOrDefault().Phone);
            Assert.Equal("ABC11111", result.Contacts.FirstOrDefault().AdditionalFields.License);
            Assert.Equal("Dallas", result.Contacts.FirstOrDefault().Location);
        }

        [Fact]
        public void ListingMapperTest_MapListing_DataSource()
        {
            //Arrange
            _listing.SetListingData(new DataSource
                {
                    DataSources = new List<string>() {"Test1", "Test2", "Test3"},
                    Other = "Test4"
                }
            );

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.NotNull(result.DataSource);
            Assert.Equal("Test4", result.DataSource.Other);
            Assert.Equal(result.DataSource.DataSources, new[] {"Test1", "Test2", "Test3"});
        }

        [Fact]
        public void ListingMapperTest_MapListing_ListingAssignment()
        {
            //Arrange
            DateTime dt = DateTime.Parse("2010-11-23");

            _listing.SetListingData(new ListingAssignment
                {
                    AssignedBy = "Lingxi.Li@cbre.com",
                    AssignedDate = dt,
                    AssignmentFlag = true
                }
            );

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.NotNull(result.ListingAssignment);
            Assert.Equal("Lingxi.Li@cbre.com", result.ListingAssignment.AssignedBy);
            Assert.Equal(dt, result.ListingAssignment.AssignedDate);
        }

        [Fact]
        public void ListingMapperTest_MapListing_PropertySizes()
        {
            //Arrange
            _listing.SetListingDataArray(new List<PropertySize>()
                {
                    new PropertySize {
                        SizeKind = "MinimumSize",
                        MeasureUnit = "sf",
                        Amount = 200
                    },
                    new PropertySize {
                        SizeKind = "MaximumSize",
                        MeasureUnit = "sf",
                        Amount = 800
                    }
                }
            );

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.PropertySizes,
                x =>
                {
                    Assert.Equal("MinimumSize", x.SizeKind);
                    Assert.Equal("sf", x.MeasureUnit);
                    Assert.Equal(200, x.Amount);
                },
                x =>
                {
                    Assert.Equal("MaximumSize", x.SizeKind);
                    Assert.Equal("sf", x.MeasureUnit);
                    Assert.Equal(800, x.Amount);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Spaces()
        {
            //Arrange
            _listing.Spaces = new List<Listing>
            {
                new Listing
                {
                    ID = 123,
                    AvailableFrom = new DateTime(2019,10,15)
                },
                new Listing
                {
                    ID = 456,
                    AvailableFrom = new DateTime(2019,10,16)
                },
            };

            _listing.Spaces[0].SetListingDataAllLanguages(new List<SpaceName> { new SpaceName { CultureCode = "en-US", Text = "TestSpace1" } });
            _listing.Spaces[1].SetListingDataAllLanguages(new List<SpaceName> { new SpaceName { CultureCode = "en-US", Text = "TestSpace2" } });

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(2, result.SpacesCount);
            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.Equal(123, space.Id);
                    Assert.Equal("en-US", space.Name.Single().CultureCode);
                    Assert.Equal("TestSpace1", space.Name.Single().Text);
                    Assert.Equal(new DateTime(2019, 10, 15), space.AvailableFrom);
                },
                space =>
                {
                    Assert.Equal(456, space.Id);
                    Assert.Equal("en-US", space.Name.Single().CultureCode);
                    Assert.Equal("TestSpace2", space.Name.Single().Text);
                    Assert.Equal(new DateTime(2019, 10, 16), space.AvailableFrom);
                });
        }

        [Theory]
        [InlineData("Available")]
        [InlineData("Unavailable")]
        public void ListingMapperTest_MapListing_Spaces_Status(string value)
        {
            //Arrange
            _listing.Spaces = new List<Listing>
            {
                new Listing
                {
                    Status = value
                }
            };

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert

            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.Equal(value, space.Status);
                });
        }

        [Theory]
        [MemberData(nameof(TestData_UsageType))]
        public void ListingMapperTest_MapListing_Spaces_UsageType(string value)
        {
            //Arrange
            _listing.Spaces = new List<Listing>
            {
                new Listing
                {
                    UsageType = value
                }
            };

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.Equal(value, space.SpaceType);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Spaces_Description()
        {
            //Arrange
            {
                var space = new Listing();
                space.ListingData = new List<ListingData>();
                space.SetListingDataAllLanguages(new List<BuildingDescription> { new BuildingDescription { CultureCode = "en-US", Text = "TestDescription" } });
                _listing.Spaces = new List<Listing> { space };
            }

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.Equal("en-US", space.SpaceDescription.Single().CultureCode);
                    Assert.Equal("TestDescription", space.SpaceDescription.Single().Text);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Spaces_Brochures()
        {
            //Arrange
            {
                var space = new Listing();
                space.ListingData = new List<ListingData>();
                space.SetListingDataArray(new List<Brochure> {
                    new Brochure { Active = true, DisplayText = "TestBrochure1", Primary = true, Url = "TestUrl1" },
                    new Brochure { Active = false, DisplayText = "TestBrochure2", Primary = false, Url = "TestUrl2" }
                });
                _listing.Spaces = new List<Listing> { space };
            }

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.Collection(space.Brochures,
                        brochure =>
                        {
                            Assert.True(brochure.Active);
                            Assert.Equal("TestBrochure1", brochure.DisplayText);
                            Assert.True(brochure.Primary);
                            Assert.Equal("TestUrl1", brochure.Url);
                        },
                        brochure =>
                        {
                            Assert.False(brochure.Active);
                            Assert.Equal("TestBrochure2", brochure.DisplayText);
                            Assert.False(brochure.Primary);
                            Assert.Equal("TestUrl2", brochure.Url);
                        });
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Spaces_Photos()
        {
            //Arrange
            {
                var space = new Listing();
                
                //Arrange space photos
                space.ListingImage = new List<ListingImage>
                {
                    new ListingImage
                    {
                        ImageCategory = "Photo",
                        DisplayText = "TestPhoto1",
                        IsActive = true,
                        Order = 1,
                        IsPrimary = true,
                        HasWatermark = true,
                        IsUserOverride = true,
                        Image = new Image
                        {
                            Url = "TestUrl1",
                            WatermarkProcessStatus = 4
                        }
                    },
                    new ListingImage
                    {
                        ImageCategory = "Photo",
                        Order = 2,
                        IsActive = false,
                        IsPrimary = false,
                        DisplayText = "TestPhoto2",
                        HasWatermark = false,
                        IsUserOverride = false,
                        Image = new Image
                        {
                            Url = "TestUrl2",
                            WatermarkProcessStatus = 4
                        }
                    }
                };

                _listing.Spaces = new List<Listing> { space };
            }

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.Collection(space.Photos,
                        photo =>
                        {
                            Assert.True(photo.Active);
                            Assert.Equal("TestPhoto1", photo.DisplayText);
                            Assert.True(photo.Primary);
                            Assert.True(photo.UserOverride);
                            Assert.Equal("TestUrl1", photo.Url);
                        },
                        photo =>
                        {
                            Assert.False(photo.Active);
                            Assert.Equal("TestPhoto2", photo.DisplayText);
                            Assert.False(photo.Primary);
                            Assert.False(photo.UserOverride);
                            Assert.Equal("TestUrl2", photo.Url);
                        });
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Spaces_FloorPlans()
        {
            //Arrange
            {
                var space = new Listing();
                space.ListingImage = new List<ListingImage>
                {
                    new ListingImage
                    {
                        ImageCategory = "FloorPlan",
                        Order = 2,
                        IsActive = false,
                        IsPrimary = false,
                        DisplayText = "TestFloorPlan2",
                        HasWatermark = false,
                        IsUserOverride = false,
                        Image = new Image
                        {
                            Url = "TestUrl2",
                            WatermarkProcessStatus = 4
                        }
                    },
                    new ListingImage
                    {
                        ImageCategory = "FloorPlan",
                        DisplayText = "TestFloorPlan1",
                        IsActive = true,
                        Order = 1,
                        IsPrimary = true,
                        HasWatermark = true,
                        IsUserOverride = true,
                        Image = new Image
                        {
                            Url = "TestUrl1",
                            WatermarkProcessStatus = 4
                        }
                    }
                };
                _listing.Spaces = new List<Listing> { space };
            }

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.Collection(space.Floorplans,
                        floorPlan =>
                        {
                            Assert.True(floorPlan.Active);
                            Assert.Equal("TestFloorPlan1", floorPlan.DisplayText);
                            Assert.True(floorPlan.Primary);
                            Assert.Equal("TestUrl1", floorPlan.Url);
                            Assert.True(floorPlan.UserOverride);
                            Assert.Equal(1, floorPlan.Order);
                        },
                        floorPlan =>
                        {
                            Assert.False(floorPlan.Active);
                            Assert.Equal("TestFloorPlan2", floorPlan.DisplayText);
                            Assert.False(floorPlan.Primary);
                            Assert.Equal("TestUrl2", floorPlan.Url);
                            Assert.False(floorPlan.UserOverride);
                            Assert.Equal(2, floorPlan.Order);
                        });
                });
        }


        [Fact]
        public void ListingMapperTest_MapListing_Spaces_Specifications()
        {
            //Arrange
            {
                var space = new Listing();
                space.ListingData = new List<ListingData>();
                space.SetListingData(new Specifications
                {
                    LeaseType = "TestLeaseType",
                    Measure = "TestMeasure",
                    LeaseTerm = "TestLeaseTerm",
                    MinSpace = 10,
                    MaxSpace = 20,
                    MinPrice = 30,
                    MaxPrice = 40,
                    ContactBrokerForPrice = true,
                    TaxModifer = "None",
                });
                _listing.Spaces = new List<Listing> { space };
            }

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Collection(result.Spaces,
                space =>
                {
                    Assert.NotNull(space.Specifications);
                    Assert.Equal("TestLeaseType", space.Specifications.LeaseType);
                    Assert.Equal("TestMeasure", space.Specifications.Measure);
                    Assert.Equal("TestLeaseTerm", space.Specifications.LeaseTerm);
                    Assert.Equal(10, space.Specifications.MinSpace);
                    Assert.Equal(20, space.Specifications.MaxSpace);
                    Assert.Equal(30, space.Specifications.MinPrice);
                    Assert.Equal(40, space.Specifications.MaxPrice);
                    Assert.True(space.Specifications.ContactBrokerForPrice);
                    Assert.Equal("None", space.Specifications.TaxModifer);
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_Aspects()
        {
            //Arrange
            var aspects = new string[] { "aspect1", "aspect2" };
            _listing.SetListingDataArray(aspects.Select(a => new Aspect { Value = a }));
            
            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal(aspects, result.Aspects);
        }

        [Fact]
        public void ListingMapperTest_MapListing_ExternalID()
        {
            //Arrange
            _listing.ExternalID = "TestID";

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestID", result.ExternalId);
        }

        [Fact]
        public void ListingMapperTest_MapListing_PreviewID()
        {
            //Arrange
            _listing.PreviewID = "TestID";

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestID", result.PreviewId);
        }

        [Fact]
        public void ListingMapperTest_MapListing_MIQID()
        {
            //Arrange
            _listing.MIQID = "TestID";

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestID", result.MiqId);
        }

        [Fact]
        public void ListingMapperTest_MapListing_Spaces_MIQID()
        {
            //Arrange
            _listing.Spaces = new List<Listing>{
                new Listing {
                    MIQID = "TestID"
                }
            };

            //Act
            var result = _listingMapper.Map(_listing);

            //Assert
            Assert.Equal("TestID", result.Spaces.Single().MiqId);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel()
        {
            //Arrange
            _listingViewModel.PropertyName = "TestName";
            _listingViewModel.AvailableFrom = new DateTime(2019, 10, 16);

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("TestName", _listing.Name);
            Assert.NotNull(_listing.AvailableFrom);
            Assert.Equal(2019, _listing.AvailableFrom.Value.Year);
            Assert.Equal(10, _listing.AvailableFrom.Value.Month);
            Assert.Equal(16, _listing.AvailableFrom.Value.Day);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_Address()
        {
            //Arrange
            _listingViewModel.Street = "TestStreet1";
            _listingViewModel.Street2 = "TestStreet2";
            _listingViewModel.City = "TestCity";
            _listingViewModel.StateOrProvince = "TestState";
            _listingViewModel.PostalCode = "TestPostalCode";
            _listingViewModel.Country = "US";
            _listingViewModel.Lat = 123;
            _listingViewModel.Lng = 456;

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("TestStreet1", _listing.Address.Street1);
            Assert.Equal("TestStreet2", _listing.Address.Street2);
            Assert.Equal("TestCity", _listing.Address.City);
            Assert.Equal("TestState", _listing.Address.StateProvince);
            Assert.Equal("TestPostalCode", _listing.Address.PostalCode);
            Assert.Equal("US", _listing.Address.Country);
            Assert.Equal(123, _listing.Address.Latitude);
            Assert.Equal(456, _listing.Address.Longitude);
        }

        [Theory]
        [MemberData(nameof(TestData_Bool))]
        public void ListingMapperTest_MapListingViewModel_IsBulkUpload(bool value)
        {
            //Arrange
            _listingViewModel.IsBulkUpload = value;

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal(value, _listing.GetListingData<IsBulkUpload>().Value);
        }

        [Theory]
        [MemberData(nameof(TestData_UsageType))]
        public void ListingMapperTest_MapListingViewModel_PropertyType(string value)
        {
            //Arrange
            _listingViewModel.PropertyType = value;

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal(value, _listing.GetListingData<PropertyType>().Value);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_Aspects()
        {
            //Arrange
            var aspects = new string[] { "aspect1", "aspect2" };
            _listingViewModel.Aspects = aspects;

            //Act
            var result = _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal(aspects, result.GetListingDataArray<Aspect>().Select(a => a.Value));
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_Website()
        {
            //Arrange
            string website = "Website";
            _listingViewModel.Website = website;

            //Act
            var result = _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("http://website/", result.GetListingData<Website>().Value);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_Video()
        {
            //Arrange
            string video = "Video";
            _listingViewModel.Video = video;

            //Act
            var result = _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("http://video/", result.GetListingData<Video>().Value);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_WalkThrough()
        {
            //Arrange
            string walkthrough = "WalkThrough";
            _listingViewModel.WalkThrough = walkthrough;

            //Act
            var result = _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("http://walkthrough/", result.GetListingData<WalkThrough>().Value);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_DataSource()
        {
            //Arrange
            _listingViewModel.DataSource = new DataSourceViewModel
            {
                DataSources = new List<string>() {"Test1", "Test2", "Test3"},
                Other = "Test4"
            };

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("Test4", _listing.GetListingData<DataSource>().Other);
            Assert.Equal(new List<string>() {"Test1", "Test2", "Test3"}, _listing.GetListingData<DataSource>().DataSources);
        }
        
        [Fact]
        public void ListingMapperTest_MapListingViewModel_ListingAssignment()
        {
            //Arrange
            DateTime dt = DateTime.Parse("2010-11-23");

            _listingViewModel.ListingAssignment = new ListingAssignmentViewModel
            {
                AssignedBy = "Lingxi.Li@cbre.com",
                AssignedDate = dt,
                AssignmentFlag = true
            };

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("Lingxi.Li@cbre.com", _listing.GetListingData<ListingAssignment>().AssignedBy);
            Assert.Equal(dt, _listing.GetListingData<ListingAssignment>().AssignedDate);
        }


        [Fact]
        public void ListingMapperTest_MapListingViewModel_ExternalID() {
            //Arrange
            _listingViewModel.ExternalId = "TestID";

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("TestID", _listing.ExternalID);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_PreviewID()
        {
            //Arrange
            _listingViewModel.PreviewId = "TestID";

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("TestID", _listing.PreviewID);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_MIQID()
        {
            //Arrange
            _listingViewModel.MiqId = "TestID";

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("TestID", _listing.MIQID);
        }

        [Fact]
        public void ListingMapperTest_MapListingViewModel_Spaces_MIQID()
        {
            //Arrange
            _listingViewModel.Spaces = new List<SpacesViewModel>{
                new SpacesViewModel{
                    MiqId = "TestID"
                }
            };

            //Act
            _listingMapper.Map(_listing, _listingViewModel);

            //Assert
            Assert.Equal("TestID", _listing.Spaces.Single().MIQID);
        }
    }
}