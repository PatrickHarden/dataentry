using System;
using System.Collections.Generic;
using System.Linq;
using dataentry.Services.Integration.StoreApi.Model;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace dataentry.Test.StoreApi
{
    public class StoreApiListingMapperTest
    {
        private const string _resourcesBaseUrl = "https://www.cbre.com/";
        private Mock<IConfiguration> _configuration;
        private StoreApiListingMapper _mapper;
        private RegionViewModel _region;
        private PropertyListing _storeListing;

        public StoreApiListingMapperTest()
        {
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(c => c["StoreSettings:FlexRentList"]).Returns("FlexOffice,OfficeCoworking");
            _mapper = new StoreApiListingMapper(_configuration.Object);
            _region = new RegionViewModel { CultureCode = "en-US" };
            _storeListing = new PropertyListing();
        }

        [Fact]
        public void StoreApiListingMapper_NullCheck()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, null, null));
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null, _resourcesBaseUrl, _storeListing));
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(_region, null, _storeListing));
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(_region, _resourcesBaseUrl, null));

            _mapper.Map(_region, _resourcesBaseUrl, _storeListing);
        }

        [Fact]
        public void StoreApiListingMapper_Highlights()
        {
            var dataEntryListing = new ListingViewModel
            {
                Highlights = new[]
                {
                    MockHighlight("DE Highlight 1", 1, "en-US", "en-ES"),
                    MockHighlight("DE Highlight 2", 2, "en-US", "en-ES")
                }
                    .SelectMany(h => h)
                    .ToList()
            };
            _storeListing.CommonHighlights = new List<CommonHighlight>
            {
                MockCommonHighlight("Store Highlight 1", "en-US", "en-ES"),
                MockCommonHighlight("Store Highlight 2", "en-US", "en-ES"),
            };

            var result = _mapper.Map(_region, _resourcesBaseUrl, _storeListing, dataEntryListing);
            var resultHighlights = result.Highlights
                .OrderBy(highlight => highlight.Order)
                .ThenBy(highlight => highlight.CultureCode)
                .ToList();

            // Correct order
            Assert.Collection(
                resultHighlights,
                highlight =>
                {
                    Assert.Equal("(en-ES) DE Highlight 1", highlight.Text);
                    Assert.Equal("en-ES", highlight.CultureCode);
                },
                highlight =>
                {
                    Assert.Equal("(en-US) DE Highlight 1", highlight.Text);
                    Assert.Equal("en-US", highlight.CultureCode);
                    Assert.Equal(highlight.Order, resultHighlights[0].Order);
                },
                highlight =>
                {
                    Assert.Equal("(en-ES) DE Highlight 2", highlight.Text);
                    Assert.Equal("en-ES", highlight.CultureCode);
                    Assert.NotEqual(highlight.Order, resultHighlights[0].Order);
                },
                highlight =>
                {
                    Assert.Equal("(en-US) DE Highlight 2", highlight.Text);
                    Assert.Equal("en-US", highlight.CultureCode);
                    Assert.Equal(highlight.Order, resultHighlights[2].Order);
                },
                highlight =>
                {
                    Assert.Equal("(en-ES) Store Highlight 1", highlight.Text);
                    Assert.Equal("en-ES", highlight.CultureCode);
                    Assert.NotEqual(highlight.Order, resultHighlights[2].Order);
                },
                highlight =>
                {
                    Assert.Equal("(en-US) Store Highlight 1", highlight.Text);
                    Assert.Equal("en-US", highlight.CultureCode);
                    Assert.Equal(highlight.Order, resultHighlights[4].Order);
                },
                highlight =>
                {
                    Assert.Equal("(en-ES) Store Highlight 2", highlight.Text);
                    Assert.Equal("en-ES", highlight.CultureCode);
                    Assert.NotEqual(highlight.Order, resultHighlights[4].Order);
                },
                highlight =>
                {
                    Assert.Equal("(en-US) Store Highlight 2", highlight.Text);
                    Assert.Equal("en-US", highlight.CultureCode);
                    Assert.Equal(highlight.Order, resultHighlights[6].Order);
                }
            );
        }

        private static IEnumerable<HighlightViewModel> MockHighlight(
            string text,
            int order,
            params string[] cultureCodes
        )
        {
            return cultureCodes.Select(
                cultureCode =>
                    new HighlightViewModel
                    {
                        CultureCode = cultureCode,
                        Text = $"({cultureCode}) {text}",
                        Order = order
                    }
            );
        }

        private static CommonHighlight MockCommonHighlight(string text, params string[] cultureCodes)
        {
            return new CommonHighlight
            {
                CommonHighlights = cultureCodes
                    .Select(
                        cultureCode =>
                            new CommonCommentElement
                            {
                                CommonCultureCode = cultureCode,
                                CommonText = $"({cultureCode}) {text}"
                            }
                    )
                    .ToList()
            };
        }
    }
}
