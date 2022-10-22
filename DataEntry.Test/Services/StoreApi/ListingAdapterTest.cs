using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Services.Integration.StoreApi.Model;
using dataentry.Test.Utility;
using dataentry.Utility;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.Services.StoreApi
{
    public class ListingAdapterTest
    {
        private const string _listingOrigin = "TestListingOrigin";
        private const string _homeSiteId = "TestHomeSiteId";
        private const string _cultureCode = "latn";
        private const string _countryCode = "001";
        private const string _flexRentList = "FlexOffice";
        private const string _previewHomeSiteId = "PreviewHomeSiteId";
        private const string _previewOrigin = "PreviewOrigin";
        private Mock<IConfiguration> _mockConfiguration;
        private ListingAdapter _listingAdapter;
        private Listing _listing;

        public static IEnumerable<object[]> TestData_PropertyTypes => TestHelpers.AliasToTestData<PropertyTypeEnum>(AliasType.StoreApi);
        public static IEnumerable<object[]> TestData_UsageTypes => TestHelpers.AliasToTestData<UseTypeEnum>(AliasType.StoreApi);
        public static IEnumerable<object[]> TestData_Dimensions => TestHelpers.AliasToTestData<DimensionsUnitsEnum>(AliasType.StoreApi);
        public static IEnumerable<object[]> TestData_PerUnitTypes => TestHelpers.AliasToTestData<PerUnitTypeEnum>(AliasType.StoreApi);
        public static IEnumerable<object[]> TestData_ListingStatus => TestHelpers.AliasToTestData<UnitStatus>(AliasType.StoreApi);
        public static IEnumerable<object[]> TestData_LeaseTerm => TestHelpers.AliasToTestData<LeaseTermEnum>(AliasType.StoreApi);
        public static IEnumerable<object[]> TestData_EnergyRating => TestHelpers.AliasToTestData<EnergyRatingEnum>(AliasType.Default, AliasType.StoreApi);

        public ListingAdapterTest()
        {
            //Setup all required values in the setup method, without these an exception is thrown
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(m => m["StoreSettings:FlexRentList"]).Returns(_flexRentList);

            EnumAliasExtensions.Init(typeof(ListingAdapter).Assembly);

            _listingAdapter = new ListingAdapter(_mockConfiguration.Object);

            _listing = new Listing { ID = 123 };
            _listing.SetListingData(new PropertyType { Value = PropertyTypeEnum.office.ToString() });
            _listing.SetListingData(new ListingType { Value = "sale" });
            _listing.SetListingData(new Specifications { LeaseTerm = LeaseTermEnum.monthly.ToString(), CurrencyCode = "USD", TaxModifer = "IncludingSalesTax" });
            _listing.Region = new Region
            {
                Name = "Test Region",
                CountryCode = _countryCode,
                CultureCode = _cultureCode,
                HomeSiteID = _homeSiteId,
                ListingPrefix = _listingOrigin,
                PreviewSiteID = _previewHomeSiteId,
                PreviewPrefix = _previewOrigin,
                ExternalPublishUrl = "http://uat-www.cbre.us/properties/properties-for-lease/{1}/details/{0}?view=isLetting",
                ExternalPreviewUrl = "/{0}{1}{2}/details/{3}?view={4}"
            };
            _listing.VerifyExternalID().Wait();
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonPrimaryKey()
        {
            //Arrange

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal($"{_listingOrigin}-{_listing.ID}", result.CommonPrimaryKey);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonHomeSite()
        {
            //Arrange

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(_homeSiteId, result.CommonHomeSite);
        }

        [Fact]
        public void ListingAdapter_ConvertToPreviewPropertyListing_CommonPrimaryKey()
        {
            //Arrange
            _listing.ID = 123;

            //Act
            var result = _listingAdapter.ConvertToPreviewPropertyListing(_listing);

            //Assert
            Assert.Equal($"{_previewOrigin}-{_listing.ID}", result.CommonPrimaryKey);
        }

        [Fact]
        public void ListingAdapter_ConvertToPreviewPropertyListing_CommonSourceSystem()
        {
            //Arrange
            _listing.MIQID = "Test1234";

            //Act
            var result = _listingAdapter.ConvertToPreviewPropertyListing(_listing);

            //Assert
            Assert.Equal("Test1234", result.CommonSourceSystem?.CommonId);
            Assert.Equal("EDP ID", result.CommonSourceSystem?.CommonName);
        }

        [Fact]
        public void ListingAdapter_ConvertToPreviewPropertyListing_CommonHomeSite()
        {
            //Arrange

            //Act
            var result = _listingAdapter.ConvertToPreviewPropertyListing(_listing);

            //Assert
            Assert.Equal(_previewHomeSiteId, result.CommonHomeSite);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonAvailableFrom()
        {
            //Arrange
            _listing.AvailableFrom = new DateTime(2019, 10, 7);

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(_listing.AvailableFrom, DateTime.Parse(result.CommonAvailability.CommonAvailabilityDate));
            Assert.Equal("AvailableFromKnownDate", result.CommonAvailability.CommonAvailabilityKind);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonPropertySubType()
        {
            //Arrange
            _listing.SetListingData(new PropertySubType { Value = "retail" });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("RetailLand", result.CommonPropertySubType);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonCreated()
        {
            //Arrange
            _listing.CreatedAt = new DateTime(2019, 8, 1);

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("2019-08-01", result.CommonCreated);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonLastProcessed()
        {
            //Arrange
            _listing.SetListingData(new PublishingState { DateUpdated = new DateTime(2019, 10, 6) });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("2019-10-06", result.CommonLastProcessed);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonLastUpdated()
        {
            //Arrange
            _listing.UpdatedAt = new DateTime(2019, 10, 5);

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("2019-10-05", result.CommonLastUpdated);
        }

        [Theory]
        [MemberData(nameof(TestData_PropertyTypes))]
        public void ListingAdapter_ConvertToPropertyListing_CommonUsageType(string localValue, string storeValue)
        {
            //Arrange
            _listing.SetListingData(new PropertyType { Value = localValue });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(storeValue, result.CommonUsageType);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonIsParent()
        {
            //Arrange

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.True(result.CommonIsParent);
        }

        [Theory]
        [InlineData("sale", "isSale")]
        [InlineData("lease", "isLetting")]
        [InlineData("salelease", "isSale,isLetting")]
        public void ListingAdapter_ConvertToPropertyListing_CommonAspects(string localValue, string storeValue)
        {
            //Arrange
            _listing.SetListingData(new ListingType { Value = localValue });
            var additionalAspects = new[] { "aspect1", "aspect2" };
            _listing.SetListingDataArray(additionalAspects.Select(a => new Aspect { Value = a }));

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            var expectedListingTypes = storeValue.Split(',');
            var allExpectedAspects = expectedListingTypes.Concat(additionalAspects);
            Assert.Equal(allExpectedAspects, result.CommonAspects);
        }
        
        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonActualAddress()
        {
            //Arrange
            _listing.Name = "TestName";
            _listing.Address = new Address
            {
                City = "TestCity",
                StateProvince = "TestState",
                PostalCode = "TestPostalCode",
                Street1 = "TestStreet1",
                Street2 = "TestStreet2"
            };

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("TestName", result.CommonActualAddress.CommonLine1);
            Assert.Equal("TestStreet1", result.CommonActualAddress.CommonLine2);
            Assert.Equal("TestStreet2", result.CommonActualAddress.CommonLine3);
            Assert.Equal("TestCity", result.CommonActualAddress.CommonLocallity);
            Assert.Equal("TestState", result.CommonActualAddress.CommonRegion);
            Assert.Equal(_countryCode, result.CommonActualAddress.CommonCountry);
            Assert.Equal("TestPostalCode", result.CommonActualAddress.CommonPostCode);
            Assert.Equal(_cultureCode, result.CommonActualAddress.CommonPostalAddresses[0].CommonLanguage);
            Assert.Equal("TestName", result.CommonActualAddress.CommonPostalAddresses[0].CommonLine1);
            Assert.Equal("TestStreet1", result.CommonActualAddress.CommonPostalAddresses[0].CommonLine2);
            Assert.Equal("TestStreet2", result.CommonActualAddress.CommonPostalAddresses[0].CommonLine3);
            Assert.Equal("TestCity", result.CommonActualAddress.CommonPostalAddresses[0].CommonLocallity);
            Assert.Equal("TestState", result.CommonActualAddress.CommonPostalAddresses[0].CommonRegion);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonActualAvailability()
        {
            //Arrange
            _listing.AvailableFrom = new DateTime(2019, 10, 9);

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.NotNull(result.CommonAvailability);
            Assert.Equal("AvailableFromKnownDate", result.CommonAvailability.CommonAvailabilityKind);
            Assert.Equal("2019-10-09", result.CommonAvailability.CommonAvailabilityDate);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonSyndicationMarket()
        {
            //Arrange
            _listing.Name = "TestName";
            _listing.Address = new Address { Street1 = "Street test 1", Street2 = "Street test 2", Latitude = 50, Longitude = -50 };
            _listing.SetListingData(new SyndicationFlag { Value = true, Market = "Ruoholahti" });
            
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("Street test 1, Ruoholahti", result.CommonActualAddress?.CommonLine2);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonCoordinate()
        {
            //Arrange
            _listing.Address = new Address { Latitude = 50, Longitude = -50 };

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.NotNull(result.CommonCoordinate);
            Assert.Equal(50, result.CommonCoordinate.Lat);
            Assert.Equal(-50, result.CommonCoordinate.Lon);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonContactGroup()
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
                        License = "ABC222222"
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
                        License = "ABC11111"
                    }
                }
            };

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.NotNull(result.CommonContactGroup?.CommonContacts);
            Assert.Equal("TestFirstName1 TestLastName1", result.CommonContactGroup.CommonContacts[0].CommonAgentName);
            Assert.Equal("TestEmail1", result.CommonContactGroup.CommonContacts[0].CommonEmailAddress);
            Assert.Equal("TestPhone1", result.CommonContactGroup.CommonContacts[0].CommonTelephoneNumber);
            Assert.Equal("ABC11111", result.CommonContactGroup.CommonContacts[0].CommonLicenseNumber);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonAgents()
        {
        //Given
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
                        Location = "Dallas"
                    }
                },
                new ListingBroker
                {
                    Order = 1,
                    Broker = new Broker
                    {
                        FirstName = "TestFirstName",
                        LastName = "TestLastName",
                        Email = "TestEmail",
                        Phone = "TestPhone",
                        Location = "Dallas"
                    }
                }
            };

        //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
        
        //Assert
            Assert.NotNull(result?.CommonAgents.FirstOrDefault());
            Assert.Equal("TestFirstName TestLastName", result?.CommonAgents.FirstOrDefault().CommonAgentName);
            Assert.Equal("TestEmail", result?.CommonAgents.FirstOrDefault().CommonEmailAddress);
            Assert.Equal("TestPhone", result?.CommonAgents.FirstOrDefault().CommonTelephoneNumber);
            Assert.Equal("Dallas", result?.CommonAgents.FirstOrDefault().CommonAgentOffice);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonBrochures()
        {
            //Arrange
            _listing.SetListingDataArray(new List<Brochure> {
                new Brochure {
                    Url = "TestUrl1",
                    DisplayText = "TestText1",
                },
                new Brochure {
                    Url = "TestUrl2",
                    DisplayText = "TestText2",
                } });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonBrochures,
                brochureElement =>
                {
                    Assert.Equal("TestUrl1", brochureElement.CommonUri);
                    Assert.False(brochureElement.CommonUriExternal);
                    Assert.Equal("TestText1", brochureElement.CommonBrochureName);
                    Assert.Equal(_cultureCode, brochureElement.CommonCultureCode);
                },
                brochureElement =>
                {
                    Assert.Equal("TestUrl2", brochureElement.CommonUri);
                    Assert.False(brochureElement.CommonUriExternal);
                    Assert.Equal("TestText2", brochureElement.CommonBrochureName);
                    Assert.Equal(_cultureCode, brochureElement.CommonCultureCode);
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonPhotos()
        {
            //Arrange
            _listing.ListingImage = new List<ListingImage>
            {
                new ListingImage
                {
                    ImageCategory = "Photo",
                    DisplayText = "TestText1",
                    IsActive = true,
                    Order = 2,
                    IsPrimary = true,
                    HasWatermark = true,
                    Image = new Image
                    {
                        Url = "TestUrl1",
                        WatermarkProcessStatus = 4
                    }
                },
                //this will not get push to store since IsActive is false
                new ListingImage
                {
                    ImageCategory = "Photo",
                    Order = 1,
                    IsActive = false,
                    IsPrimary = false,
                    DisplayText = "TestPhoto2",
                    HasWatermark = false,
                    Image = new Image
                    {
                        Url = "TestUrl2",
                        WatermarkProcessStatus = 4
                    }
                }
            };
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonPhotos,
                photoElement =>
                {
                    Assert.Equal("TestText1", photoElement.CommonImageCaption);
                    Assert.True(photoElement.CommonAddWatermark);
                    Assert.Collection(photoElement.CommonImageResources, new Action<CommonImageResource>[]
                    {
                        imageResource => Assert.Equal("TestUrl1", imageResource.CommonResourceUri)
                    });
                });
        }

        [Fact]
        public void ListingMapperTest_MapListing_NumberOfStoreysDescription()
        {
            //Arrange
            _listing.SetListingData(new Floor { Value = 2 });
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
            //Assert
            Assert.Equal(2, result.CommonNumberOfStoreys);

            //Arrange
            _listing.SetListingData(new Floor { Value = null });
            //Act
            var result2 = _listingAdapter.ConvertToPropertyListing(_listing);
            //Assert
            Assert.Null(result2.CommonNumberOfStoreys);
        }

        [Fact]
        public void ListingMapperTest_MapListing_YearBuild()
        {
            //Arrange
            _listing.SetListingData(new YearBuilt { Value = 2020 });
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
            //Assert
            Assert.Equal(2020, result.CommonYearBuilt);

            //Arrange
            _listing.SetListingData(new YearBuilt { Value = null });
            //Act
            var result2 = _listingAdapter.ConvertToPropertyListing(_listing);
            //Assert
            Assert.Null(result2.CommonYearBuilt);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorPlans()
        {
            //Arrange
            _listing.ListingImage = new List<ListingImage>
            {
                new ListingImage
                {
                    ImageCategory = "FloorPlan",
                    DisplayText = "TestText1",
                    IsActive = true,
                    Order = 2,
                    IsPrimary = true,
                    HasWatermark = true,
                    Image = new Image
                    {
                        Url = "TestUrl1",
                        WatermarkProcessStatus = 4
                    }
                },
                new ListingImage
                {
                    ImageCategory = "FloorPlan",
                    Order = 1,
                    IsActive = false,
                    IsPrimary = false,
                    DisplayText = "TestFloorPlan2",
                    HasWatermark = false,
                    Image = new Image
                    {
                        Url = "TestUrl2",
                        WatermarkProcessStatus = 4
                    }
                }
            };

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonFloorPlans,
                floorPlanElement =>
                {
                    Assert.Equal("TestText1", floorPlanElement.CommonImageCaption);
                    Assert.True(floorPlanElement.CommonAddWatermark);
                    Assert.Collection(floorPlanElement.CommonImageResources, new Action<CommonImageResource>[]
                    {
                        imageResource => Assert.Equal("TestUrl1", imageResource.CommonResourceUri)
                    });
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonHighlights()
        {
            //Arrange
            _listing.SetListingDataArrayAllLanguages(new List<Highlight>
            {
                new Highlight
                {
                    Order = 2,
                    Text = "Highlight 2",
                    CultureCode = _cultureCode
                },
                new Highlight
                {
                    Order = 1,
                    Text = "Highlight 1",
                    CultureCode = _cultureCode
                }
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonHighlights,
                highlightElement =>
                {
                    Assert.Collection(highlightElement.CommonHighlights,
                        commentElement =>
                        {
                            Assert.Equal(_cultureCode, commentElement.CommonCultureCode);
                            Assert.Equal("Highlight 1", commentElement.CommonText);
                        });
                },
                highlightElement =>
                {
                    Assert.Collection(highlightElement.CommonHighlights,
                        commentElement =>
                        {
                            Assert.Equal(_cultureCode, commentElement.CommonCultureCode);
                            Assert.Equal("Highlight 2", commentElement.CommonText);
                        });
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonMicroMarket()
        {
            //Arrange
            _listing.SetListingDataArray(new List<MicroMarket>
                {
                    new MicroMarket
                    {
                        Order = 1, 
                        Value = "CBD: Tanjong Pagar/Shenton Way"
                    }
                }
            );

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("CBD", result.CommonMicromarket.CommonMicromarketName);
            Assert.Equal("Tanjong Pagar/Shenton Way", result.CommonMicromarket.CommonSubMarketName);
        }

        [Theory]
        [MemberData(nameof(TestData_PerUnitTypes))]
        public void ListingAdapter_ConvertToPropertyListing_CommonCharges_PerUnit(string localValue, string storeValue)
        {
            //Arrange
            _listing.SetListingData(new ListingType
            {
                Value = "lease"
            });
            _listing.SetListingData(new Specifications
            {
                Measure = localValue,
                MinPrice = 100,
                ShowPriceWithUoM = true
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonCharges,
                commonCharge =>
                {
                    Assert.Equal(storeValue, commonCharge.CommonPerUnit);
                    Assert.Equal(100, commonCharge.CommonAmount);
                });
        }

        [Theory]
        [MemberData(nameof(TestData_LeaseTerm))]
        public void ListingAdapter_ConvertToPropertyListing_CommonCharges_LeaseTerm(string localValue, string storeValue)
        {
            //Arrange
            _listing.SetListingData(new ListingType
            {
                Value = "lease"
            });
            _listing.SetListingData(new Specifications
            {
                LeaseTerm = localValue,
                MinPrice = 100, 
                ShowPriceWithUoM = true
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonCharges,
                commonCharge =>
                {
                    Assert.Equal(storeValue, commonCharge.CommonInterval);
                    Assert.Equal(100, commonCharge.CommonAmount);
                    Assert.Equal("Value", commonCharge.CommonAmountKind);
                    Assert.Equal("None", commonCharge.CommonTaxModifer);
                });
        }

        /// <summary>
        /// These values are required to render sale price correctly on the SPA
        /// </summary>
        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonCharges_SalePrice()
        {
            //Arrange
            _listing.SetListingData(new ListingType
            {
                Value = "sale",
            });
            _listing.SetListingData(new Specifications
            {
                SalePrice = 100
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonCharges,
                commonCharge =>
                {
                    Assert.Equal("Once", commonCharge.CommonInterval);
                    Assert.Equal("Whole", commonCharge.CommonPerUnit);
                    Assert.Equal("SalePrice", commonCharge.CommonChargeKind);
                    Assert.Equal(100, commonCharge.CommonAmount);
                });
        }

        /// <summary>
        /// These values are required to render sale price correctly on the SPA
        /// </summary>
        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonCharges_LeasePrice()
        {
            //Arrange
            _listing.SetListingData(new ListingType
            {
                Value = "lease",
            });
            _listing.SetListingData(new Specifications
            {
                MinPrice = 100,
                MaxPrice = 200,
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonCharges,
                commonCharge =>
                {
                    Assert.Equal(100, commonCharge.CommonAmount);
                    Assert.Equal("Rent", commonCharge.CommonChargeKind);
                    Assert.Equal("From", commonCharge.CommonChargeModifer);
                },
                commonCharge =>
                {
                    Assert.Equal(200, commonCharge.CommonAmount);
                    Assert.Equal("Rent", commonCharge.CommonChargeKind);
                    Assert.Equal("To", commonCharge.CommonChargeModifer);
                });
        }

        [Fact]
        public void ListingAdapter_ConverToProertyListing_CommonCharges_OnApplication()
        {
            //Arrange
            _listing.SetListingDataArray(new List<ChargesAndModifiers>
                {
                    new ChargesAndModifiers{
                        ChargeType = "OperatingCost",
                        ChargeModifier = "OnApplication",
                        PerUnitType = "sqm",
                        CurrencyCode = "INR",
                        Amount = 0,
                        Term = "Once",
                        Year = null}
                });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
            
            //Assert
            Assert.Collection(result.CommonCharges,
                commonCharge =>
                {
                    Assert.True(commonCharge.CommonOnApplication);
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorsAndUnit()
        {
            //Arrange
            var space = new Listing
            {
                AvailableFrom = new DateTime(2019, 10, 14)
            };
            space.SetListingData(new Specifications
            {
                Measure = "sqft",
                MinSpace = 10,
                MaxSpace = 20, 
                TotalSpace = 30,
                LeaseType = "New",
                SalePrice = 2115.20m,
                ContactBrokerForPrice = true
            });
            space.SetListingDataAllLanguages(new List<SpaceName>
            {
                new SpaceName
                {
                    CultureCode = _cultureCode,
                    Text = "TestSpace"
                }
            });

            var space2 = new Listing();
            space2.SetListingDataAllLanguages(new List<SpaceName>
            {
                new SpaceName
                {
                    CultureCode = _cultureCode,
                    Text = "TestSpace2"
                }
            });

            space.SetListingData(new SortOrder { Value = 1 }); 
            space2.SetListingData(new SortOrder { Value = 2 });

            _listing.Spaces = new List<Listing> { space2, space };

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonFloorsAndUnits, 
                commonFloorsAndUnit =>
                {
                    Assert.Collection(commonFloorsAndUnit.CommonSubdivisionName,
                        commonSubdivisionName =>
                        {
                            Assert.Equal(_cultureCode, commonSubdivisionName.CommonCultureCode);
                            Assert.Equal("TestSpace", commonSubdivisionName.CommonText);
                        });

                    Assert.Collection(commonFloorsAndUnit.CommonAreas,
                        commonLandSizeElement =>
                        {
                            Assert.Equal("sqft", commonLandSizeElement.CommonUnits);
                            Assert.Equal(30, commonLandSizeElement.CommonArea);
                            Assert.Equal(10, commonLandSizeElement.CommonMinArea);
                            Assert.Equal(20, commonLandSizeElement.CommonMaxArea);
                        });

                    Assert.Equal("2019-10-14", commonFloorsAndUnit.CommonAvailableFrom);
                    Assert.NotNull(commonFloorsAndUnit.CommonAvailability);
                    Assert.Equal("AvailableFromKnownDate", commonFloorsAndUnit.CommonAvailability.CommonAvailabilityKind);
                    Assert.Equal("2019-10-14", commonFloorsAndUnit.CommonAvailability.CommonAvailabilityDate);
                    Assert.Null(commonFloorsAndUnit.CommonLeaseTypes);
                    Assert.Equal(0.0d, commonFloorsAndUnit.CommonCharges.FirstOrDefault()?.CommonAmount);
                    Assert.True(commonFloorsAndUnit.CommonCharges.FirstOrDefault()?.CommonOnApplication);
                },
                commonFloorsAndUnit =>
                {
                    Assert.Collection(commonFloorsAndUnit.CommonSubdivisionName,
                        commonSubdivisionName =>
                        {
                            Assert.Equal(_cultureCode, commonSubdivisionName.CommonCultureCode);
                            Assert.Equal("TestSpace2", commonSubdivisionName.CommonText);
                        });
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorsAndUnit_LeasePrice()
        {
            //Arrange
            var space = new Listing();
            space.SetListingData(new Specifications
            {
                MaxPrice = 2115.20m
            });

            _listing.Spaces = new List<Listing> { space };
            _listing.SetListingData(new ListingType
            {
                Value = "lease",
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonFloorsAndUnits,
                commonFloorsAndUnit =>
                {
                    Assert.Collection(commonFloorsAndUnit.CommonCharges,
                        commonCharge =>
                        {
                            Assert.Equal(2115.20d, commonCharge.CommonAmount);
                            Assert.Equal("Rent", commonCharge.CommonChargeKind);
                            Assert.Null(commonCharge.CommonChargeModifer);
                        });
                });
        }

        [Theory]
        [InlineData("http://www.website.com", "http://www.website.com")]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorsAndUnit_ConvertToWebsite_ReturnWebsite(string website, string expected){
            //Arrange
            _listing.SetListingData(new Website() { Value = website });
        
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(expected, _listingAdapter.ConvertToWebsite());
            Assert.Equal(expected, result.CommonWebsite);
        }

        [Theory]
        [InlineData("http://www.youtube.com", "http://www.youtube.com")]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorsAndUnit_ConvertToVideoLinks_ReturnLink(string videoLink, string expected){
            //Arrange
            _listing.SetListingData(new Video() { Value = videoLink });
        
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
            
            //Assert
            Assert.Collection(result.CommonVideoLinks,
                video =>
                {
                    Assert.Equal(expected, video.CommonLink);
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorsAndUnit_ConvertToVideoLinks_ReturnNull(){
            //Arrange
        
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
            
            //Assert
            Assert.Null(result.CommonVideoLinks);
        }

        [Theory]
        [InlineData("http://www.walkthrough.com", "http://www.walkthrough.com")]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorsAndUnit_ConvertToWalkthrough_ReturnLink(string walkthrough, string expected){
            //Arrange
            _listing.SetListingData(new WalkThrough() { Value = walkthrough });
        
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
            //Assert
            Assert.Equal(expected, _listingAdapter.ConvertToWalkthrough(_listing));
            Assert.Equal(expected, result.CommonWalkthrough);
        }

        [Theory]
        [MemberData(nameof(TestData_ListingStatus))]
        public void ListingAdapterTest_ConvertToPropertyListing_CommonFloorsAndUnit_Status(string localValue, string storeValue)
        {
            //Arrange
            _listing.Spaces = new List<Listing> { new Listing {
                Status = localValue
            }};

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonFloorsAndUnits,
                commonFloorsAndUnit =>
                {
                    Assert.Equal(storeValue, commonFloorsAndUnit.CommonUnitStatus);
                });
        }

        [Theory]
        [MemberData(nameof(TestData_UsageTypes))]
        public void ListingAdapter_ConvertToPropertyListing_CommonFloorsAndUnit_UsageType(string localValue, string storeValue)
        {
            //Arrange
            var space = new Listing
            {
                UsageType = localValue
            };
            _listing.Spaces = new List<Listing> { space };

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonFloorsAndUnits,
                commonFloorsAndUnit =>
                {
                    Assert.Equal(storeValue, commonFloorsAndUnit.CommonUnitUse);
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonGeoLocation()
        {
            //Arrange
            _listing.Address = new Address { Latitude = 10m, Longitude = 20m };


            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.NotNull(result.CommonGeoLocation);
            Assert.NotNull(result.CommonGeoLocation.Geometry);
            Assert.Equal("Point", result.CommonGeoLocation.Geometry.Type);
            Assert.NotNull(result.CommonGeoLocation.Geometry.Coordinates);
            Assert.Collection(result.CommonGeoLocation.Geometry.Coordinates,
                lat => Assert.Equal(10m, lat),
                lon => Assert.Equal(20m, lon));
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonLeaseTypes()
        {
            //Arrange
            _listing.SetListingData(new ListingType{ Value = "saleLease"} );
            _listing.SetListingData(new Specifications() { LeaseType = "New" });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonLeaseTypes, leaseType => Assert.Equal("New", leaseType));
            Assert.Collection(result.CommonLeaseTypes, leaseType => Assert.Equal(_listingAdapter.ConvertToLeaseType("New").FirstOrDefault(), leaseType));
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonLeaseTypes_Null()
        {
            //Arrange
            _listing.SetListingData(new ListingType{ Value = null} );
            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(result.CommonLeaseTypes, null);
            }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonLeaseRateType()
        {
            //Arrange
            _listing.SetListingData(new ListingType
            {
                Value = "saleLease"
            });
            _listing.SetListingData(new Specifications
            {
                LeaseRateType = "TestLeaseRateType"
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("TestLeaseRateType", result.CommonLeaseRateType);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonLongDescription()
        {
            //Arrange
            _listing.SetListingDataAllLanguages(new List<BuildingDescription> { new BuildingDescription { CultureCode = _cultureCode, Text = "TestDescription" } });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonLongDescription, 
                comment =>
                {
                    Assert.Equal(_cultureCode, comment.CommonCultureCode);
                    Assert.Equal("TestDescription", comment.CommonText);
                });
        }

        [Fact]
        public void ListingAdapter_ConverToProertyListing_CommonLocationDescription()
        {
            //Arrange
            _listing.SetListingDataAllLanguages(new List<LocationDescription> { new LocationDescription { CultureCode = _cultureCode, Text = "Test Location Description" } });

            //Act 
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonLocationDescription, 
                comment =>
                {
                    Assert.Equal(_cultureCode, comment.CommonCultureCode);
                    Assert.Equal("Test Location Description", comment.CommonText);
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonNumberOfBedrooms()
        {
            //Arrange


            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(0, result.CommonNumberOfBedrooms);
        }

        [Theory]
        [MemberData(nameof(TestData_Dimensions))]
        public void ListingAdapter_ConvertToPropertyListing_CommonSizes(string localDimension, string storeDimension)
        {
            //Arrange
            _listing.SetListingData(new Specifications
            {
                Measure = localDimension,
                MinSpace = 1,
                MaxSpace = 2
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonSizes,
                size =>
                {
                    Assert.Equal("MinimumSize", size.CommonSizeKind);
                    Assert.Collection(size.CommonDimensions,
                        dimension =>
                        {
                            Assert.Equal(storeDimension, dimension.CommonDimensionsUnits);
                            Assert.Equal(1, dimension.CommonAmount);
                        });
                },
                size =>
                {
                    Assert.Equal("MaximumSize", size.CommonSizeKind);
                    Assert.Collection(size.CommonDimensions,
                        dimension =>
                        {
                            Assert.Equal(storeDimension, dimension.CommonDimensionsUnits);
                            Assert.Equal(2, dimension.CommonAmount);
                        });
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonSourceLastupdated()
        {
            //Arrange
            _listing.UpdatedAt = new DateTime(2019, 10, 25);

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal("2019-10-25", result.CommonSourceLastupdated);
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_CommonStrapline()
        {
            //Arrange
            _listing.SetListingDataAllLanguages(new List<Headline> { new Headline { CultureCode = _cultureCode, Text = "TestDescription" } });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonStrapline,
                comment =>
                {
                    Assert.Equal(_cultureCode, comment.CommonCultureCode);
                    Assert.Equal("TestDescription", comment.CommonText);
                });
        }

        [Theory]
        [MemberData(nameof(TestData_Dimensions))]
        public void ListingAdapter_ConvertToPropertyListing_CommonTotalSize(string localDimension, string storeDimension)
        {
            //Arrange
            _listing.SetListingData(new Specifications
            {
                Measure = localDimension,
                MinSpace = 20,
                MaxSpace = 40
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Collection(result.CommonTotalSize,
                size =>
                {
                    Assert.Equal(storeDimension, size.CommonUnits);
                    Assert.Equal(0, size.CommonArea);
                    Assert.Equal(20, size.CommonMinArea);
                    Assert.Equal(40, size.CommonMaxArea);
                });
        }

        [Fact]
        public void ListingAdapter_ConvertToPropertyListing_UnitedKingdom()
        {
            //Arrange


            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Null(result.UnitedKingdomUseClass);
            Assert.False(result.UnitedKingdomVatPayable);
        }

        [Fact]
        public void ListingAdapter_GetTaxModifier()
        {
            _listing.SetListingData(new ListingType
            {
                Value = "sale"
            });
            _listing.SetListingData(new Specifications
            {
                SalePrice = 100,
                TaxModifer = TaxModifierEnum.IncludingSalesTax.ToString()
            });
            var result = _listingAdapter.ConvertToPropertyListing(_listing);
            Assert.Equal("IncludingSalesTax", result.CommonCharges.FirstOrDefault().CommonTaxModifer);
        }

        [Theory]
        [InlineData("Transit/Subway", "Transit/Subway")]
        [InlineData("Commuter Rail", "CommuterRail")]
        [InlineData("Airport", "Airport")]
        [InlineData("BadData", null)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void ListingAdapter_ConvertToCommonTransportationTypes_CommonType(string type, string expected)
        {
             //Arrange
            List<Place> places = new List<Place>{ new Place { Name = "First Place"}};
            _listing.SetListingDataArray(new List<TransportationTypes>
            {
                new TransportationTypes{ Type = type, Places = places }
            });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(expected, result.CommonTransportationTypes?.FirstOrDefault()?.CommonType);
        }


        [Fact]
        public void ListingAdapter_ConvertToCommonHighlights_ShouldGroupByHighlight()
        {
            // The end result should be in this format:
            /*
            [
                {
                    "Common.Highlight": [
                    {
                        "Common.CultureCode": "en-NL",
                        "Common.Text": "English Highlight 1"
                    },
                    {
                        "Common.CultureCode": "nL-NL",
                        "Common.Text": "Dutch Highlight 1"
                    }
                    ]
                },
                {
                    "Common.Highlight": [
                    {
                        "Common.CultureCode": "en-NL",
                        "Common.Text": "English Highlight 2"
                    },
                    {
                        "Common.CultureCode": "nL-NL",
                        "Common.Text": "Dutch Highlight 2"
                    }
                    ]
                }
            ]
            */

            // Arrange
            _listing.SetListingDataArrayAllLanguages(new List<Highlight>
            {
                new Highlight
                {
                    Order = 1,
                    Text = "English Highlight 1",
                    CultureCode =  "en-NL"
                },
                new Highlight
                {
                    Order = 1,
                    Text = "Dutch Highlight 1",
                    CultureCode =  "nL-NL"
                },
                new Highlight
                {
                    Order = 2,
                    Text = "English Highlight 2",
                    CultureCode =  "en-NL"
                },
                new Highlight
                {
                    Order = 2,
                    Text = "Dutch Highlight 2",
                    CultureCode =  "nL-NL"
                }
            });

            // Act
            var result = _listingAdapter.ConvertToCommonHighlights(null, _listing);

            // Assert
            Assert.Equal(
                "[{\"Common.Highlight\":[{\"Common.CultureCode\":\"en-NL\",\"Common.Text\":\"English Highlight 1\"},{\"Common.CultureCode\":\"nL-NL\",\"Common.Text\":\"Dutch Highlight 1\"}]},{\"Common.Highlight\":[{\"Common.CultureCode\":\"en-NL\",\"Common.Text\":\"English Highlight 2\"},{\"Common.CultureCode\":\"nL-NL\",\"Common.Text\":\"Dutch Highlight 2\"}]}]", 
                Newtonsoft.Json.JsonConvert.SerializeObject(result)
            );
        }
    
        [Theory]
        [MemberData(nameof(TestData_EnergyRating))]
        public void ListingAdapter_ConvertToPropertyListing_CommonCertificateType(string localValue, string storeValue)
        {
            //Arrange
            _listing.SetListingData(new EnergyRating { Value = localValue });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(storeValue, result.CommonEnergyPerformanceData.CommonCertificateType);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("Bad Data", null)]
        public void ListingAdapter_ConvertToPropertyListing_CommonCertificateType_WithUndefinedEnum(string localValue, string storeValue)
        {
            //Arrange
            _listing.SetListingData(new EnergyRating { Value = localValue });

            //Act
            var result = _listingAdapter.ConvertToPropertyListing(_listing);

            //Assert
            Assert.Equal(storeValue, result.CommonEnergyPerformanceData?.CommonCertificateType);
        }
    }
}
