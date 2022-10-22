using dataentry.Data.DBContext.Model;
using dataentry.Extensions;
using dataentry.Repository;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Publishing;
using dataentry.Services.Business.Users;
using dataentry.Services.Integration.Edp.Consumption;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.Services.Business.Listings
{
    public class ListingServiceTest
    {
        private Mock<IUserStore<IdentityUser>> _mockUserStore;
        private ClaimsPrincipal _user;
        private Mock<IDataEntryRepository> _mockDataEntryRepository;
        private Mock<ApplicationUserManager> _mockUserManager;
        private Mock<IListingMapper> _mockListingMapper;
        private Mock<IUserMapper> _mockUserMapper;
        private Mock<ITeamMapper> _mockTeamMapper;
        private Mock<ILogger<ListingService>> _mockLogger;
        private Mock<IPublishingService> _mockPublishingService;
        private Mock<IConsumptionService> _mockConsumptionService;
        private ListingSerializer _listingSerializer;
        private ListingService _listingService;

        public ListingServiceTest()
        {
            _mockUserStore = new Mock<IUserStore<IdentityUser>>();
            _user = new ClaimsPrincipal();

            _mockDataEntryRepository = new Mock<IDataEntryRepository>();
            _mockUserManager = new Mock<ApplicationUserManager>(_mockUserStore.Object, null, null, null, null, null, null, null, null, null, null);
            _mockListingMapper = new Mock<IListingMapper>();
            _mockUserMapper = new Mock<IUserMapper>();
            _mockTeamMapper = new Mock<ITeamMapper>();
            _mockLogger = new Mock<ILogger<ListingService>>();
            _mockPublishingService = new Mock<IPublishingService>();
            _mockConsumptionService = new Mock<IConsumptionService>();

            _listingSerializer = new ListingSerializer();

            _listingService = new ListingService(
                _mockDataEntryRepository.Object,
                _mockUserManager.Object,
                _mockListingMapper.Object,
                _mockUserMapper.Object,
                _mockTeamMapper.Object,
                _mockLogger.Object,
                _mockPublishingService.Object,
                _listingSerializer,
                _mockConsumptionService.Object,
                new JsonDeltaEvaluator()
                );

            _mockListingMapper
                .Setup(m => m.Map(It.IsAny<IEnumerable<ListingBroker>>(), It.IsAny<IEnumerable<Tuple<Broker, int>>>()))
                .Returns(new List<ListingBroker>());
            _mockListingMapper
                .Setup(m => m.Map(It.IsAny<Listing>(), It.IsAny<ListingViewModel>()))
                .Returns<Listing, ListingViewModel>((m, vm) =>
                {
                    m.ListingImage = m.ListingImage ?? new List<ListingImage>();
                    return m;
                });

            _mockConsumptionService.Setup(c => c.Enabled).Returns(true);
        }

        [Fact]
        public void ListingService_FindListingDeltas_Identical()
        {
            var testDocument =
            @"{
                ""Id"": 123,
                ""PropertyRecordName"": ""Test Property"",
	            ""Headline"": [{
			            ""Text"": ""Test Text 1""

                    }, {
			            ""Text"": ""Test Text 2""
		            }
	            ],
            }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument);

            Assert.Empty(result);
        }

        [Fact]
        public void ListingService_FindListingDeltas_DeltaFound()
        {
            var testDocument1 =
            @"{
                ""Id"": 123,
                ""PropertyRecordName"": ""Test Property"",
	            ""Headline"": [{
			            ""Text"": ""Test Text 1""

                    }, {
			            ""Text"": ""Test Text 2""
		            }
	            ],
                ""Street"": ""Identical Field"",
            }";

            var testDocument2 =
            @"{
                ""Id"": 456,
                ""PropertyRecordName"": ""Test Property 2"",
	            ""Headline"": [{
			            ""Text"": ""Test Text 1""

                    }, {
			            ""Text"": ""Test Text 3""
		            }
	            ],
                ""Street"": ""Identical Field""
            }";

            var result = _listingService.FindListingDeltas(testDocument1, testDocument2);

            Assert.Contains(result, delta => 
                delta.OriginalDocumentPath == "Id" 
                && delta.OriginalValue == "123" 
                && delta.NewDocumentPath == "Id"
                && delta.NewValue == "456");

            Assert.Contains(result, delta => 
                delta.OriginalDocumentPath == "PropertyRecordName" 
                && delta.OriginalValue == "Test Property"
                && delta.NewDocumentPath == "PropertyRecordName"
                && delta.NewValue == "Test Property 2");

            Assert.Contains(result, delta => 
                delta.OriginalDocumentPath == "Headline[1].Text" 
                && delta.OriginalValue == "Test Text 2"
                && delta.NewDocumentPath == "Headline[1].Text"
                && delta.NewValue == "Test Text 3");
        }

        [Fact]
        public void ListingService_FindListingDeltas_WithViewModel()
        {
            var testDocument =
            @"{
                ""Id"": 123,
                ""PropertyRecordName"": ""Test Property"",
	            ""Headline"": [{
			            ""Text"": ""Test Text 1""

                    }, {
			            ""Text"": ""Test Text 2""
		            }
	            ],
                ""Street"": ""Identical Field"",
            }";

            var vm = _listingSerializer.Deserialize(testDocument);

            var result = _listingService.FindListingDeltas(testDocument, vm);

            Assert.Empty(result);
        }

        [Fact]
        public void ListingService_FindListingDeltas_ArrayAdd()
        {
            var testDocument =
            @"{
                ""Id"": 123,
	            ""Headline"": [{
			            ""Text"": ""Test Text 1""

                    }
	            ],
            }";

            var testDocument2 =
            @"{
                ""Id"": 123,
	            ""Headline"": [
                    {
			            ""Text"": ""Test Text 1""
		            },
                    {
                        ""Text"": ""Test Text 2""
                    }
	            ],
            }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument2);

            Assert.Equal(2, result.Count());
            Assert.Contains(result, delta => 
                delta.JsonPath == "$.Headline[1]" // Note: Not using "new" expression on ordered lists, only on lists with matching fields (like Id)
                && delta.OriginalDocumentPath == null 
                && delta.OriginalValue == "undefined"
                && delta.NewDocumentPath == "Headline[1]"
                && delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Headline[1].Text"
                && delta.OriginalDocumentPath == null
                && delta.OriginalValue == "undefined"
                && delta.NewDocumentPath == "Headline[1].Text"
                && delta.NewValue == "Test Text 2");
        }

        [Fact]
        public void ListingService_FindListingDeltas_ArrayDelete()
        {
            var testDocument =
            @"{
                ""Id"": 123,
	            ""Headline"": [{
			            ""Text"": ""Test Text 1""

                    }, {
			            ""Text"": ""Test Text 2""
		            }
	            ],
            }";

            var testDocument2 =
            @"{
                ""Id"": 123,
	            ""Headline"": [
                    {
			            ""Text"": ""Test Text 2""
		            }
	            ],
            }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument2);

            Assert.Contains(result, delta => 
                delta.OriginalDocumentPath == "Headline[0].Text" 
                && delta.OriginalValue == "Test Text 1"
                && delta.NewDocumentPath == "Headline[0].Text"
                && delta.NewValue == "Test Text 2");

            Assert.Contains(result, delta => 
                delta.OriginalDocumentPath == "Headline[1]" 
                && delta.OriginalValue == "{object}"
                && delta.NewDocumentPath == null
                && delta.NewValue == "undefined");
        }

        [Fact]
        public void ListingService_FindListingDeltas_Spaces()
        {
            var testDocument =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Headline"": [{""Text"": ""Test Text""}],
                    ""Spaces"": [{
                        ""MiqId"": '432',
                        ""Name"": [{""Text"": ""Space 1""}],
                        ""Status"": ""Available""
                    },{
                        ""MiqId"": '765',
                        ""Name"": [{""Text"": ""Space 2""}],
                        ""Status"": ""Available""
                    },{
                        ""MiqId"": '98',
                        ""Name"": [{""Text"": ""Space 3""}],
                        ""Status"": ""Available""
                    }]
                }";

            var testDocument2 =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Headline"": [{""Text"": ""Test Text""}],
                    ""Spaces"": [{
                        ""MiqId"": '98',
                        ""Name"": [{""Text"": ""Space 3 New Name""}],
                        ""Status"": ""Unavailable""
                    },{
                        ""MiqId"": '432',
                        ""Name"": [{""Text"": ""Space 1""}],
                        ""Status"": ""Available""
                    },{
                        ""MiqId"": null,
                        ""Name"": [{""Text"": ""New Space 1""}],
                        ""Status"": ""Unavailable""
                    },{
                        ""MiqId"": null,
                        ""Name"": [{""Text"": ""New Space 2""}],
                        ""Status"": ""Unavailable""
                    }]  
                }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument2);

            Assert.Equal(15, result.Count());

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(@.MiqId=='98')].Name[0].Text"
                && delta.OriginalDocumentPath == "Spaces[2].Name[0].Text" 
				&& delta.NewDocumentPath == "Spaces[0].Name[0].Text" 
				&& delta.OriginalValue == "Space 3" 
				&& delta.NewValue == "Space 3 New Name");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(@.MiqId=='98')].Status"
                && delta.OriginalDocumentPath == "Spaces[2].Status" 
				&& delta.NewDocumentPath == "Spaces[0].Status" 
				&& delta.OriginalValue == "Available" 
				&& delta.NewValue == "Unavailable");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(2))]"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[2]" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(2))].MiqId"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[2].MiqId"
                && delta.OriginalValue == "undefined" 
				&& delta.NewValue == "");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(2))].Name"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[2].Name" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "{array}");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(2))].Name[0]"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[2].Name[0]" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(2))].Name[0].Text"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[2].Name[0].Text" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "New Space 1");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(2))].Status"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[2].Status" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "Unavailable");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(3))]"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[3]" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(3))].MiqId"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[3].MiqId"
                && delta.OriginalValue == "undefined" 
				&& delta.NewValue == "");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(3))].Name"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[3].Name" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "{array}");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(3))].Name[0]"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[3].Name[0]" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(3))].Name[0].Text"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[3].Name[0].Text" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "New Space 2");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(new(3))].Status"
                && delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Spaces[3].Status" 
				&& delta.OriginalValue == "undefined" 
				&& delta.NewValue == "Unavailable");

            Assert.Contains(result, delta =>
                delta.JsonPath == "$.Spaces[?(@.MiqId=='765')]"
                && delta.OriginalDocumentPath == "Spaces[1]" 
				&& delta.NewDocumentPath == null
                && delta.OriginalValue == "{object}" 
				&& delta.NewValue == "undefined");

        }

        [Fact]
        public void ListingService_FindListingDeltas_Photos()
        {
            var testDocument =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Photos"": [{
                        Url: ""https://www.google.com"",
                        MediaName: ""Google""

                    },{
                        Url: ""https://www.yahoo.com"",
                        MediaName: ""Yahoo""
                    }]
                }";

            var testDocument2 =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Photos"": [{
                        Url: ""https://www.yahoo.com"",
                        MediaName: ""Yahoooo""
                    },{
                        Url: ""https://www.askjeeves.com"",
                        MediaName: ""Jeeves""
                    }]
                }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument2);

            Assert.Equal(5, result.Count());

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == "Photos[0]"
                && delta.NewDocumentPath == null
                && delta.OriginalValue == "{object}"
                && delta.NewValue == "undefined");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == "Photos[1].MediaName"
                && delta.NewDocumentPath == "Photos[0].MediaName"
                && delta.OriginalValue == "Yahoo"
                && delta.NewValue == "Yahoooo");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Photos[1]"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Photos[1].Url"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "https://www.askjeeves.com");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Photos[1].MediaName"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "Jeeves");
        }

        [Fact]
        public void ListingService_FindListingDeltas_Brochures()
        {
            var testDocument =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Brochures"": [{
                        Url: ""https://www.google.com""
                    },{
                        Url: ""https://www.yahoo.com""
                    }]
                }";

            var testDocument2 =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Brochures"": [{
                        Url: ""https://www.bing.com""
                    },{
                        Url: ""https://www.google.com""
                    }]
                }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument2);

            Assert.Equal(3, result.Count());

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == "Brochures[1]"
                && delta.NewDocumentPath == null
                && delta.OriginalValue == "{object}"
                && delta.NewValue == "undefined");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Brochures[0]"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Brochures[0].Url"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "https://www.bing.com");
        }

        [Fact]
        public void ListingService_FindListingDeltas_Floorplans()
        {
            var testDocument =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Floorplans"": [{
                        Url: ""https://www.google.com"",
                        MediaName: ""Google""

                    },{
                        Url: ""https://www.yahoo.com"",
                        MediaName: ""Yahoo""
                    }]
                }";

            var testDocument2 =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Floorplans"": [{
                        Url: ""https://www.yahoo.com"",
                        MediaName: ""Yahoooo""
                    },{
                        Url: ""https://www.askjeeves.com"",
                        MediaName: ""Jeeves""
                    }]
                }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument2);

            Assert.Equal(5, result.Count());

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == "Floorplans[0]"
                && delta.NewDocumentPath == null
                && delta.OriginalValue == "{object}"
                && delta.NewValue == "undefined");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == "Floorplans[1].MediaName"
                && delta.NewDocumentPath == "Floorplans[0].MediaName"
                && delta.OriginalValue == "Yahoo"
                && delta.NewValue == "Yahoooo");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Floorplans[1]"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "{object}");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Floorplans[1].Url"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "https://www.askjeeves.com");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Floorplans[1].MediaName"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "Jeeves");
        }

        [Fact]
        public void ListingService_FindListingDeltas_Aspects()
        {
            var testDocument =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Aspects"": [
                        ""Aspect 1"",
                        ""Aspect 2""
                    ]
                }";

            var testDocument2 =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Aspects"": [
                        ""Aspect 2"",
                        ""Aspect 3""
                    ]
                }";

            var result = _listingService.FindListingDeltas(testDocument, testDocument2);

            Assert.Equal(2, result.Count());

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == "Aspects[0]"
                && delta.NewDocumentPath == null
                && delta.OriginalValue == "Aspect 1"
                && delta.NewValue == "undefined");

            Assert.Contains(result, delta =>
                delta.OriginalDocumentPath == null
                && delta.NewDocumentPath == "Aspects[1]"
                && delta.OriginalValue == "undefined"
                && delta.NewValue == "Aspect 3");
        }

        [Fact]
        public async void ListingService_UpdateListingAsync_ListingDeltas()
        {
            var oldListing = CreateListing();
            oldListing.Name = "DE Old Value";

            var newListingVm = CreateListingViewModel();
            newListingVm.Id = oldListing.ID;
            newListingVm.MiqId = "123";
            newListingVm.PropertyRecordName = "DE New Value";
            newListingVm.Deltas = new List<ListingDeltaViewModel>
                {
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.PropertyRecordName",
                        OriginalDocumentPath = "PropertyRecordName",
                        OriginalValue = "EDP Old Value",
                        NewDocumentPath = "PropertyRecordName",
                        NewValue = "EDP New Value"
                    }
                };

            var lastImport = @"{""PropertyRecordName"": ""EDP Old Value""}";

            var updatedSerializations = ListingService_UpdateListingAsync_ListingDeltas_Setup(oldListing, lastImport);

            var resultVm = await _listingService.UpdateListingAsync(_user, newListingVm);

            Assert.Equal("DE New Value", resultVm.PropertyRecordName);

            Assert.Single(updatedSerializations);
            var resultSerialization = _listingSerializer.Deserialize(updatedSerializations.Single());
            Assert.Equal("EDP New Value", resultSerialization.PropertyRecordName);
        }

        [Fact]
        public async void ListingService_UpdateListingAsync_ListingDeltas_ComplexObject()
        {

            var oldListing = CreateListing();

            var newListingVm = CreateListingViewModel();
            newListingVm.Id = oldListing.ID;
            newListingVm.MiqId = "123";
            newListingVm.Specifications = new SpecificationsViewModel
            {
                CurrencyCode = "USD"
            };
            newListingVm.Deltas = new List<ListingDeltaViewModel>
                {
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Specifications.CurrencyCode",
                        OriginalDocumentPath = "Specifications.CurrencyCode", 
                        OriginalValue = "null",
                        NewDocumentPath = "Specifications.CurrencyCode",
                        NewValue = "NZD"
                    },
                };

            string lastImport = null; //Just to test null values

            var updatedSerializations = ListingService_UpdateListingAsync_ListingDeltas_Setup(oldListing, lastImport);

            var resultVm = await _listingService.UpdateListingAsync(_user, newListingVm);

            Assert.Equal("USD", resultVm.Specifications.CurrencyCode);

            Assert.Single(updatedSerializations);
            var resultSerialization = _listingSerializer.Deserialize(updatedSerializations.Single());
            Assert.Equal("NZD", resultSerialization.Specifications.CurrencyCode);
        }

        [Fact]
        public async void ListingService_UpdateListingAsync_ListingDeltas_Array()
        {
            var oldListing = CreateListing();
            oldListing.MIQID = "123";
            oldListing.SetListingDataArrayAllLanguages(new List<Highlight>
            {
                new Highlight
                {
                    Text = "Hello"
                }
            });

            var newListingVm = CreateListingViewModel();
            newListingVm.Id = oldListing.ID;
            newListingVm.MiqId = oldListing.MIQID;
            newListingVm.Specifications = new SpecificationsViewModel
            {
                CurrencyCode = "USD"
            };
            newListingVm.Highlights = new List<HighlightViewModel>
            {
                new HighlightViewModel
                {
                    Text = "World!"
                }
            };

            newListingVm.Deltas = new List<ListingDeltaViewModel>
                {
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Highlights[0].Text",
                        OriginalDocumentPath = "Highlights[0].Text", 
                        OriginalValue = "Hello",
                        NewDocumentPath = "Highlights[0].Text",
                        NewValue = "World!"
                    }
                };

            var lastImport = @"{""Highlights"": [{""Text"": ""Hello""}]}";

            var updatedSerializations = ListingService_UpdateListingAsync_ListingDeltas_Setup(oldListing, lastImport);

            var resultVm = await _listingService.UpdateListingAsync(_user, newListingVm);

            Assert.Equal("World!", resultVm.Highlights.First().Text);

            Assert.Single(updatedSerializations);
            var resultSerialization = _listingSerializer.Deserialize(updatedSerializations.Single());
            Assert.Equal("World!", resultSerialization.Highlights.First().Text);
        }

        [Fact]
        public async void ListingService_UpdateListingAsync_ListingDeltas_ArrayDelete()
        {
            var oldListing = CreateListing();
            oldListing.SetListingDataArrayAllLanguages(
                new List<Highlight> {
                    new Highlight
                    {
                        Text = "DE Old Value 1"
                    },
                    new Highlight
                    {
                        Text = "DE Old Value 2"
                    },
                    new Highlight
                    {
                        Text = "DE Old Value 3"
                    }
                });

            var newListingVm = CreateListingViewModel();
            newListingVm.Id = oldListing.ID;
            newListingVm.MiqId = "123";
            newListingVm.Highlights = new List<HighlightViewModel>
            {
                new HighlightViewModel
                {
                    Text = "DE New Value"
                },
                new HighlightViewModel
                {
                    Text = "DE Old Value 3"
                }
            };
            newListingVm.Deltas = new List<ListingDeltaViewModel>
                {
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Highlights[0].Text",
                        OriginalDocumentPath = "Highlights[0].Text", 
                        OriginalValue = "EDP Old Value 1",
                        NewDocumentPath = "Highlights[0].Text",
                        NewValue = "EDP New Value 1"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Highlights[1].Text",
                        OriginalDocumentPath = "Highlights[1].Text", 
                        OriginalValue = "EDP Old Value 2",
                        NewDocumentPath = "Highlights[1].Text",
                        NewValue = "EDP New Value 3"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Highlights[2]",
                        OriginalDocumentPath = "Highlights[2]", 
                        OriginalValue = "{object}",
                        NewDocumentPath = "Highlights[2]",
                        NewValue = "undefined"
                    },
                };

            var lastImport = @"{""Highlights"": [{Text: ""EDP Old Value 1""}, {Text: ""EDP Old Value 2""}, {Text: ""EDP Old Value 3""}]}";

            var updatedSerializations = ListingService_UpdateListingAsync_ListingDeltas_Setup(oldListing, lastImport);

            var resultVm = await _listingService.UpdateListingAsync(_user, newListingVm);

            Assert.Collection(resultVm.Highlights,
                h =>
                {
                    Assert.Equal("DE New Value", h.Text);
                },
                h =>
                {
                    Assert.Equal("DE Old Value 3", h.Text);
                });

            Assert.Single(updatedSerializations);
            var resultSerialization = _listingSerializer.Deserialize(updatedSerializations.Single());
            Assert.Collection(resultSerialization.Highlights,
                h =>
                {
                    Assert.Equal("EDP New Value 1", h.Text);
                },
                h =>
                {
                    Assert.Equal("EDP New Value 3", h.Text);
                });
        }

        [Fact]
        public async void ListingService_UpdateListingAsync_ListingDeltas_Aspects()
        {
            var oldListing = CreateListing();
            oldListing.SetListingDataArray(
                new List<Aspect> {
                    new Aspect
                    {
                        Value = "DE Old Value 1"
                    },
                    new Aspect
                    {
                        Value = "DE Old Value 2"
                    }
                });

            var newListingVm = CreateListingViewModel();
            newListingVm.Id = oldListing.ID;
            newListingVm.MiqId = "123";
            newListingVm.Aspects = new List<string>
            {
                "DE Old Value 2",
                "DE New Value 1"
            };
            newListingVm.Deltas = new List<ListingDeltaViewModel>
                {
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Aspects[?(@=='EDP Old Value 1')]",
                        OriginalDocumentPath = "Aspects[0]",
                        OriginalValue = "EDP Old Value 1",
                        NewDocumentPath = null,
                        NewValue = "undefined"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Aspects[?(@=='EDP New Value 1')]",
                        OriginalDocumentPath = null,
                        OriginalValue = "undefined",
                        NewDocumentPath = "Aspects[1]",
                        NewValue = "EDP New Value 1"
                    }
                };

            var lastImport = @"{""Aspects"": [""EDP Old Value 1"", ""EDP Old Value 2""]}";

            var updatedSerializations = ListingService_UpdateListingAsync_ListingDeltas_Setup(oldListing, lastImport);

            var resultVm = await _listingService.UpdateListingAsync(_user, newListingVm);

            Assert.Collection(resultVm.Aspects,
                h =>
                {
                    Assert.Equal("DE Old Value 2", h);
                },
                h =>
                {
                    Assert.Equal("DE New Value 1", h);
                });

            Assert.Single(updatedSerializations);
            var resultSerialization = _listingSerializer.Deserialize(updatedSerializations.Single());
            Assert.Collection(resultSerialization.Aspects,
                h =>
                {
                    Assert.Equal("EDP Old Value 2", h);
                },
                h =>
                {
                    Assert.Equal("EDP New Value 1", h);
                });
        }

        [Fact]
        public async void ListingService_UpdateListingAsync_ListingDeltas_Spaces()
        {
            var oldListing = CreateListing();
            oldListing.ID = 123;
            oldListing.MIQID = "456";
            oldListing.Spaces = new List<Listing> { CreateSpace(), CreateSpace(), CreateSpace() };
            oldListing.Spaces[0].ID = 432;
            oldListing.Spaces[0].MIQID = "234";
            oldListing.Spaces[0].SetListingDataArrayAllLanguages(new List<TextType> { new TextType { Text = "Space 1" } });
            oldListing.Spaces[0].Status = "Available";
            oldListing.Spaces[1].ID = 765;
            oldListing.Spaces[1].MIQID = "567";
            oldListing.Spaces[1].SetListingDataArrayAllLanguages(new List<TextType> { new TextType { Text = "Space 2" } });
            oldListing.Spaces[1].Status = "Available";
            oldListing.Spaces[2].ID = 98;
            oldListing.Spaces[2].SetListingDataArrayAllLanguages(new List<TextType> { new TextType { Text = "Space 3" } });
            oldListing.Spaces[2].Status = "Available";

            var newListingVm = CreateListingViewModel();
            newListingVm.Id = 123;
            newListingVm.MiqId = "456";
            newListingVm.Spaces = new List<SpacesViewModel> {
                new SpacesViewModel {
                    Id = 98,
                    Name = new List<TextTypeViewModel>{new TextTypeViewModel { Text = "Space 3 New Name" } },
                    Status = "Unavailable"
                },
                new SpacesViewModel {
                    Id = 432,
                    MiqId = "234",
                    Name = new List<TextTypeViewModel>{new TextTypeViewModel { Text = "Space 1" } },
                    Status = "Available"
                },
                new SpacesViewModel
                {
                    Id = 0,
                    MiqId = null,
                    Name = new List<TextTypeViewModel>{new TextTypeViewModel { Text = "New Space 1"} },
                    Status = "Unavailable"
                },
                new SpacesViewModel
                {
                    Id = 0,
                    MiqId = null,
                    Name = new List<TextTypeViewModel>{new TextTypeViewModel { Text = "New Space 2"} },
                    Status = "Unavailable"
                }
            };
            newListingVm.Deltas = new List<ListingDeltaViewModel>
                {
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(@.Id==98)].Name[0].Text",
                        OriginalDocumentPath = "Spaces[2].Name[0].Text",
						OriginalValue = "Space 3",
						NewDocumentPath = "Spaces[0].Name[0].Text",
						NewValue = "Space 3 New Name"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(@.Id==98)].Status",
                        OriginalDocumentPath = "Spaces[2].Status",
						OriginalValue = "Available",
						NewDocumentPath = "Spaces[0].Status",
						NewValue = "Unavailable"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(2))]]",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[2]",
						NewValue = "{object}"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(2))].Id",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[2].Id",
						NewValue = "0"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(2))].Name",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[2].Name",
						NewValue = "{array}"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(2))].Name[0]",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[2].Name[0]",
						NewValue = "{object}"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(2))].Name[0].Text",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[2].Name[0].Text",
						NewValue = "New Space 1"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(2))].Status",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[2].Status",
						NewValue = "Unavailable"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(3))]",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[3]",
						NewValue = "{object}"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(3))].Id",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[3].Id",
						NewValue = "0"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(3))].Name",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[3].Name",
						NewValue = "{array}"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(3))].Name[0]",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[3].Name[0]",
						NewValue = "{object}"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(3))].Name[0].Text",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[3].Name[0].Text",
						NewValue = "New Space 2"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(new(3))].Status",
                        OriginalDocumentPath = null,
						OriginalValue = "undefined",
						NewDocumentPath = "Spaces[3].Status",
						NewValue = "Unavailable"
                    },
                    new ListingDeltaViewModel
                    {
                        JsonPath = "$.Spaces[?(@.Id==765)]",
                        OriginalDocumentPath = "Spaces[1]",
						OriginalValue = "{object}",
						NewDocumentPath = null,
						NewValue = "undefined"
                    },
                };

            var lastImport =
                @"{
                    ""Id"": 123,
                    ""MiqId"": 456,
                    ""Spaces"": [{
                        ""Id"": 432,
                        ""MiqId"": ""234"",
                        ""Name"": [{""Text"": ""Space 1""}],
                        ""Status"": ""Available""
                    },{
                        ""Id"": 765,
                        ""MiqId"": ""567"",
                        ""Name"": [{""Text"": ""Space 2""}],
                        ""Status"": ""Available""
                    },{
                        ""Id"": 98,
                        ""Name"": [{""Text"": ""Space 3""}],
                        ""Status"": ""Available""
                    }]
                }";

            var updatedSerializations = ListingService_UpdateListingAsync_ListingDeltas_Setup(oldListing, lastImport);

            _mockDataEntryRepository
                .Setup(r => r.UpdateListing(oldListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync((Listing l, ClaimsPrincipal c, IEnumerable<string> u, IEnumerable<string> t) =>
                {
                    // Simulate EF giving entities IDs
                    foreach (var space in l.Spaces)
                    {
                        if (space.ID == 0) space.ID = _id++;
                    }
                    return l;
                });

            var resultVm = await _listingService.UpdateListingAsync(_user, newListingVm);



            var resultSerialization = _listingSerializer.Deserialize(updatedSerializations.Single());
            Assert.Equal(123, resultSerialization.Id);
            Assert.Equal("456", resultSerialization.MiqId);
            Assert.Equal(4, resultSerialization.Spaces.Count());
            Assert.Contains(resultSerialization.Spaces, space =>
                space.Id == 98
                && space.Name.Single().Text == "Space 3 New Name"
                && space.Status == "Unavailable");
            Assert.Contains(resultSerialization.Spaces, space =>
                space.Id == 432
                && space.MiqId == "234"
                && space.Name.Single().Text == "Space 1"
                && space.Status == "Available");
            Assert.Contains(resultSerialization.Spaces, space =>
                space.Name.Single().Text == "New Space 1"
                && space.Status == "Unavailable");
            Assert.Contains(resultSerialization.Spaces, space =>
                space.Name.Single().Text == "New Space 2"
                && space.Status == "Unavailable");
            Assert.DoesNotContain(resultSerialization.Spaces, space => space.Id == 765);
        }

        [Fact]
        public async void ListingService_CreateListingAsync_ListingDeltas()
        {
            var vm = CreateListingViewModel();
            vm.MiqId = "123";

            var result = await _listingService.CreateListingAsync(_user, vm);

            _mockDataEntryRepository
                .Verify(r => r.UpdateListingSerialization(It.IsAny<int>(), Data.Enums.ListingSerializationType.LastImport, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void ListingService_CreateListingAsync_ListingDeltas_SkipWhenNoMiqId()
        {
            var vm = CreateListingViewModel();
            vm.MiqId = null;

            var result = await _listingService.CreateListingAsync(_user, vm);

            _mockDataEntryRepository
                .Verify(r => r.UpdateListingSerialization(It.IsAny<int>(), Data.Enums.ListingSerializationType.LastImport, It.IsAny<string>()), Times.Never);
        }

        private List<string> ListingService_UpdateListingAsync_ListingDeltas_Setup(Listing oldListing, string lastImport)
        {
            _mockDataEntryRepository
                            .Setup(r => r.GetListingByID(oldListing.ID, _user))
                            .ReturnsAsync(oldListing);

            _mockDataEntryRepository
                .Setup(r => r.GetListingSerialization(oldListing.ID, Data.Enums.ListingSerializationType.LastImport))
                .ReturnsAsync(lastImport);

            _mockDataEntryRepository
                .Setup(r => r.UpdateListing(oldListing, _user, It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(oldListing);

            var updatedSerializations = new List<string>();
            _mockDataEntryRepository
                .Setup(r => r.UpdateListingSerialization(oldListing.ID, Data.Enums.ListingSerializationType.LastImport, It.IsAny<string>()))
                .Returns<int, Data.Enums.ListingSerializationType, string>((id, type, value) =>
                {
                    updatedSerializations.Add(value);
                    return Task.CompletedTask;
                });

            return updatedSerializations;
        }

        int _id = 1;

        private Listing CreateListing() => new Listing
        {
            ID = _id++,
            ListingBroker = new List<ListingBroker>(),
            ListingImage = new List<ListingImage>(),
        };

        private ListingViewModel CreateListingViewModel() => new ListingViewModel
        {
            Contacts = new List<ContactsViewModel>()
        };

        private Listing CreateSpace() => new Listing
        {
            ID = _id++
        };
    }


    public class ListingSerializerTest {
        [Fact]
        public void ListingSerializer_Serialize()
        {
            var listingSerializer = new ListingSerializer();

            var vm = new ListingViewModel();
            vm.Id = 123;
            vm.PropertyRecordName = "Test Property";
            vm.DataSource = new DataSourceViewModel { Other = "Test Source" };
            vm.Headline = new List<TextTypeViewModel> { new TextTypeViewModel { Text = "Test Text 1" }, new TextTypeViewModel { Text = "Test Text 2" } };

            var result = listingSerializer.Serialize(vm);
        }
    }
}
