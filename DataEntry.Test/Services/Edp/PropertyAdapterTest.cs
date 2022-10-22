using dataentry.Data.DBContext.Model;
using dataentry.Extensions;
using dataentry.Services.Business.Configs;
using dataentry.Services.Integration.Edp;
using dataentry.Services.Integration.Edp.Consumption;
using dataentry.Utility;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace dataentry.Test.Services.Integration.Edp
{
    public class PropertyAdapterTest
    {
        private Mock<IConfigService> _mockConfigService;
        private PropertyMapper _propertyMapper;
        private PropertyWithAvailability _property;
        private Entity _entity;
        private Region _region;

        public PropertyAdapterTest()
        {
            _mockConfigService = new Mock<IConfigService>();
            EnumAliasExtensions.Init(typeof(PropertyMapper).Assembly);
            var optionsMock = new Mock<IOptions<PropertyMapperOptions>>();
            var options = new PropertyMapperOptions{
                AmenitiesSource = new Dictionary<string, string> {
                    ["default"] = "FromAvailabilities"
                }
            };
            optionsMock.Setup(o => o.Value).Returns(options);
            _propertyMapper = new PropertyMapper(_mockConfigService.Object, optionsMock.Object);

            _property = new PropertyWithAvailability();
            _entity = new Entity { id = 1234 };
            _property.PropertyDetail = new PropertyDetail { entity = _entity };

            _region = new Region{
                ID = Region.DefaultID
            };
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_WithNoPropertyFound()
        {
            //Arrange
            _property.PropertyDetail = null;
            
            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_WithNoAvailbility()
        {
            //Arrange
            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Empty(result.Spaces);
            Assert.Null(result.PropertyType);
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_WithNoBroker()
        {
            //Arrange
            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Empty(result.Contacts);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("Office", "office")]
        [InlineData("Multi-Family Housing", "multifamily")]
        [InlineData("Unknown", null)]
        public void PropertyAdapter_ConvertToListing_MapPropertyUsage(string propertyType, string expected)
        {
            //Arrange
            List<Usage> usages = new List<Usage> { 
                new Usage { ref_property_usage_type_desc = propertyType },
                new Usage { ref_property_usage_type_desc = "second usage" }
             };
            _property.PropertyDetail.usages = usages;

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(expected, result.PropertyType);
        }

        [Theory]
        [InlineData(new string[] { null }, null)]
        [InlineData(new string[] { "Unknown" }, null)]
        [InlineData(new string[] { "sale" }, "sale")]        
        [InlineData(new string[] { "lease" }, "lease")]
        [InlineData(new string[] { "sale", "sale or lease" }, "salelease")]
        [InlineData(new string[] { "lease", "sale or lease" }, "salelease")]
        [InlineData(new string[] { "sale", "lease", "sale or lease" }, "salelease")]
        public void PropertyAdapter_ConvertToListing_MapListingType(string[] listingTypes, string expected)
        {
            //Arrange
            List<Availability> availabilities = new List<Availability>();
            foreach(var type in listingTypes)
            {
                availabilities.Add(new Availability { ref_availability_type_desc = type });
            }
            _property.Availability = availabilities;

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(expected, result.ListingType);
        }

        [Theory]
        [InlineData("en-US", "building description 1", "Property Notes", "building description 1", "en-US")]
        [InlineData(null, "building description 1", "Property Notes", "building description 1", "en-US")]
        [InlineData("", "building description 1", "Property Notes", "building description 1", "en-US")]
        [InlineData("en", "building description 1", "Property Notes", "building description 1", "en-US")]

        public void PropertyAdapter_ConvertToListing_MapCultureCode(string cultureCode, string desc, string noteType, string expectedText, string expectedCultureCode)
        {
            //Arrange
            _region.CultureCode = cultureCode;
            var c = new List<Colloquial>{new Colloquial{notes = desc}};
            _property.PropertyDetail.property_note = new List<PropertyNote>{new PropertyNote{ colloquial = c, property_notes_type_desc = noteType }}.ToList();

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(expectedText, result.BuildingDescription.FirstOrDefault().Text);
            Assert.Equal(expectedCultureCode, result.BuildingDescription.FirstOrDefault().CultureCode);
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_MapDescription()
        {
            //Arrange
            _region.CultureCode = "en-IT";
            var c1 = new List<Colloquial>{new Colloquial{notes = "property note in IT", language_desc="italian"}};
            var c2 = new List<Colloquial>{new Colloquial{notes = "location note in NO", language_desc="norwegian"}};
            var n1 = new PropertyNote{ colloquial = c1, property_notes_type_desc = "Property Notes", notes = "property note in EN" };
            var n2 = new PropertyNote{ colloquial = c2, property_notes_type_desc = "Location Description", notes = "location note in EN" };
            _property.PropertyDetail.property_note = new List<PropertyNote>{n1, n2};
            
            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal("property note in IT", result.BuildingDescription.FirstOrDefault().Text);
            Assert.Equal("it-IT", result.BuildingDescription.FirstOrDefault().CultureCode);
            Assert.Equal("property note in EN", result.BuildingDescription.LastOrDefault().Text);
            Assert.Equal("en-IT", result.BuildingDescription.LastOrDefault().CultureCode);
            Assert.Equal("location note in NO", result.LocationDescription.FirstOrDefault().Text);
            Assert.Equal("no-IT", result.LocationDescription.FirstOrDefault().CultureCode);
            Assert.Equal("location note in EN", result.LocationDescription.LastOrDefault().Text);
            Assert.Equal("en-IT", result.LocationDescription.LastOrDefault().CultureCode);
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_MapDescription_TestNull()
        {
            //Arrange
            _region.CultureCode = "";
            _property.PropertyDetail.property_notes = null;
            _property.PropertyDetail.property_note = null;
            
            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(null, result.BuildingDescription?.FirstOrDefault()?.Text);
            Assert.Equal(null, result.BuildingDescription?.FirstOrDefault()?.CultureCode);
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_PropertySize_LandSize()
        {
            //Arrange
            _property.PropertyDetail.total_land_area = 888;
            _property.PropertyDetail.total_land_area_uom_desc = "sqft";

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(888.0, result.PropertySizes.FirstOrDefault().Amount);
            Assert.Equal("sqft", result.PropertySizes.FirstOrDefault().MeasureUnit);
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_Property_PropertyMediaInformation()
        {
            //Arrange
            _property.PropertyDetail.property_media_information = new List<PropertyMediaInformation>
            {
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = null, watermark_label = null, media_path = "1.jpg", primary_image_f = true },
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = null, media_path = "2.jpg" },
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = "CoStarWaterMark", media_path = "3.jpg" },
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = "Floor Plan", watermark_label = null, media_path = "4.jpg" },
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Floor Plan", watermark_label = "CoStarWaterMark", media_path = "5.jpg" },
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = "Floor Plan", watermark_label = null, media_path = "6.jpg" },
            };

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(2, result.Photos?.Count());
            Assert.Equal(2, result.Floorplans?.Count());
            Assert.Equal("1.jpg", result.Photos?.Where(x => x.Primary == true).FirstOrDefault().Url);
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_Property_SpaceMediaInformation()
        {
            //Arrange
            _property.PropertyDetail.property_media_information = new List<PropertyMediaInformation>
            {
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = null, media_path = "1.jpg", primary_image_f = true },
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = null, media_path = "2.jpg" },
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = "CoStarWaterMark", media_path = "3.jpg" },
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = "Floor Plan", watermark_label = null, media_path = "4.jpg" },
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Floor Plan", watermark_label = "CoStarWaterMark", media_path = "5.jpg" },
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Floor Plan", watermark_label = null, media_path = "6.jpg" },
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Document", media_content_type_desc = "Brochures", media_path = "7.pdf" },
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Document", media_content_type_desc = "Brochures", media_path = "8.pdf" },
            };
            List<Availability> availabilities = new List<Availability>();
            Entity spaceEntity1 = new Entity {id = 1};
            Entity spaceEntity2 = new Entity {id = 2};
            Availability space1 = new Availability() { entity = spaceEntity1 };
            Availability space2 = new Availability() { entity = spaceEntity2 };
            space1.property = new Property();
            space2.property = new Property();
            space1.property.property_media_information = new  List<PropertyMediaInformation>
            {
                new PropertyMediaInformation { publish_image_f = true, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = null, media_path = "2.jpg", primary_image_f = true },
                new PropertyMediaInformation { media_type_desc = "Document", media_content_type_desc = "Brochures", media_path = "7.pdf" },
            };
            space2.property.property_media_information = new  List<PropertyMediaInformation>
            {
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = null, media_path = "12.jpg" },
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Loc-image", watermark_label = "CoStarWaterMark", media_path = "3.jpg" },
                new PropertyMediaInformation { publish_image_f = false, media_type_desc = "Image", media_content_type_desc = "Floor Plan", watermark_label = null, media_path = "4.jpg" },
            };

            availabilities.Add(space1);
            availabilities.Add(space2);
            
            _property.Availability = availabilities;
            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal("1.jpg", result.Photos?.FirstOrDefault().Url);
            Assert.Equal(1, result.Photos?.Count());
            Assert.Equal("8.pdf", result.Brochures?.FirstOrDefault().Url);
            Assert.Equal(1, result.Brochures?.Count());
            Assert.Equal(1, result.Spaces.Where(x => x.MiqId == "1").FirstOrDefault()?.Photos?.Count());
            Assert.Equal(0, result.Spaces.Where(x => x.MiqId == "2").FirstOrDefault()?.Photos?.Count());
            Assert.Equal(0, result.Spaces.Where(x => x.MiqId == "1").FirstOrDefault()?.Floorplans?.Count());
            Assert.Equal(1, result.Spaces.Where(x => x.MiqId == "2").FirstOrDefault()?.Floorplans?.Count());            
            Assert.Equal(1, result.Spaces.Where(x => x.MiqId == "1").FirstOrDefault()?.Brochures?.Count());
            Assert.Equal(0, result.Spaces.Where(x => x.MiqId == "2").FirstOrDefault()?.Brochures?.Count());
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("Zuid-Holland", "ZH")]
        [InlineData("", "")]
        public void propertyAdapter_ConvertToListing_MapProvinceState(string provinceState, string expected)
        {
            //Arrange
            PostalAddress address = new PostalAddress(){state_province = provinceState};
            _property.PropertyDetail.postal_address = address;

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(expected, result.StateOrProvince);
        }

        [Theory]
        [InlineData(2020, 2020)]
        [InlineData(null, null)]
        public void PropertyAdapter_ConvertToListing_YearBuilt_WithYear(int? Year, int? expected) 
        {
            //Arrange
            _property.PropertyDetail.year_property_built = Year;

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(expected, result.YearBuilt);
        }

        [Theory]
        [InlineData("TEST", "123", "TEST-123")]
        [InlineData("TEST", null, null)]
        [InlineData("NOT_TEST", "123", null)]
        [InlineData("", "", null)]
        [InlineData(null, null, null)]
        public void propertyAdapter_ConvertToListing_SourceSystemExternalID(string sourceSystem, string sourceID, string expected)
        {
            //Arrange
            _region.RegionalIDFormats = new List<RegionalIDFormat> {
                new RegionalIDFormat{
                    SourceSystemName = "TEST",
                    FormatString = "TEST-{0}"
                }
            };

            _property.Availability = new List<Availability>{
                new Availability{
                    listing_ids = new List<ListingIds>{
                        new ListingIds{
                            listing_id = sourceID,
                            listing_source = sourceSystem
                        }
                    }
                }
            };

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Equal(expected, result.ExternalId);
        }

        private void ArrangeUsageAmenities() {
            _property.Availability = new List<Availability>{
                new Availability{
                    property = new Property {
                        property_usage = new PropertyUsage {
                            usage_amenity = new List<PropertyAmenity> {
                                new PropertyAmenity {
                                    property_amenity_type_desc = "24/7 Access"
                                },
                                
                                new PropertyAmenity {
                                    property_amenity_type_desc = "Reception",
                                    amenity_subtype_desc = "Manned"
                                },
                                
                                new PropertyAmenity {
                                    property_amenity_type_desc = "Not Applicable",
                                    amenity_subtype_desc = "Parking Spaces",
                                    amenity_notes = "6"
                                },
                                
                                new PropertyAmenity {
                                    property_amenity_type_desc = "Other",
                                    amenity_subtype_desc = null,
                                    amenity_notes = "24/7 CCTV"
                                },

                                new PropertyAmenity {
                                    amenity_subtype_desc = "Break-Out Areas"
                                }
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public void PropertyAdapter_ConvertToListing_Highlights() 
        {
            //Arrange
            ArrangeUsageAmenities();
            
            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Collection(result.Highlights, 
                highlight => Assert.Equal("24/7 Access", highlight.Text),
                highlight => Assert.Equal("Manned Reception", highlight.Text),
                highlight => Assert.Equal("6 Parking Spaces", highlight.Text),
                highlight => Assert.Equal("24/7 CCTV", highlight.Text),
                highlight => Assert.Equal("Break-Out Areas", highlight.Text)
            );
        }

        
        [Fact]
        public void PropertyAdapter_ConvertToListing_Aspects() 
        {
            //Arrange
            ArrangeUsageAmenities();

            //Act
            var result = _propertyMapper.ConvertToListing(_property, _region);
            
            //Assert
            Assert.Collection(result.Aspects, 
                aspect => Assert.Equal("has24HourAccess", aspect),
                aspect => Assert.Equal("hasReception-Manned", aspect),
                aspect => Assert.Equal("hasBreakoutSpace", aspect)
            );
        }
    }
}