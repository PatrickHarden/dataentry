using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using ListingType = dataentry.Data.DBContext.Model.ListingType;
using Newtonsoft.Json.Linq;
using dataentry.Data;
using Microsoft.Extensions.Logging;
using static dataentry.Utility.AliasType;

namespace dataentry.Services.Business.Listings
{
    public class ListingMapper : IListingMapper
    {
        private static string _searchApiEndPoint;
        private static string _searchKey;
        private static string _previewFeatureFlag;
        private readonly IOptions<dataentry.Utility.Configs> _configs;
        private readonly ISiteMapsConfigDataProvider _siteMapConfigDataProvider;
        private readonly ILogger<ListingMapper> _logger;

        public ListingMapper(
            IConfiguration configuration, 
            IOptions<dataentry.Utility.Configs> configs, 
            ISiteMapsConfigDataProvider siteMapsConfigDataProvider, 
            ILogger<ListingMapper> logger)
        {
            _searchApiEndPoint = configuration["StoreSettings:SearchApiEndPoint"] ?? throw new ArgumentException("SearchApiEndPoint is null");
            _searchKey = configuration["StoreSettings:SearchKey"] ?? throw new ArgumentException("SearchKey is null");
            _previewFeatureFlag = configuration["FeatureFlags:PreviewFeatureFlag"] ?? throw new ArgumentException("PreviewFeatureFlag is null");
            _configs = configs ?? throw new ArgumentNullException(nameof(configs));
            _siteMapConfigDataProvider = siteMapsConfigDataProvider;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ListingViewModel Map(Listing listing)
        {
            if (listing == null) throw new ArgumentNullException(nameof(listing));

            var listingViewModel = new ListingViewModel();

            listingViewModel.Id = listing.ID;
            listingViewModel.ExternalId = listing.ExternalID;
            listingViewModel.ConfigId = listing.GetListingData<ConfigId>().Value;
            listingViewModel.PreviewId = listing.PreviewID;
            listingViewModel.MiqId = listing.MIQID;
            listingViewModel.RegionID = listing.RegionID.ToString();
            listingViewModel.PropertyRecordName = listing.GetListingData<PropertyRecordName>().Value;
            listingViewModel.PropertyName = listing.Name;
            listingViewModel.PropertyRecordName = listing.GetListingData<PropertyRecordName>()?.Value;
            listingViewModel.DateCreated = listing.CreatedAt.ToLocalTime();
            listingViewModel.DateUpdated = listing.UpdatedAt.ToLocalTime();
            listingViewModel.Status = listing.Status;
            listingViewModel.AvailableFrom = listing.AvailableFrom;
            listingViewModel.Street = listing.Address?.Street1;
            listingViewModel.Street2 = listing.Address?.Street2;
            listingViewModel.City = listing.Address?.City;
            listingViewModel.StateOrProvince = listing.Address?.StateProvince;
            listingViewModel.PostalCode = listing.Address?.PostalCode;
            listingViewModel.Country = listing.Address?.Country;
            listingViewModel.EnergyRating = listing.GetListingData<EnergyRating>().Value;
            listingViewModel.ExternalRatings = listing.GetListingDataArray<ExternalRatings>().Select(Map);
            listingViewModel.Lat = listing.Address?.Latitude ?? 0;
            listingViewModel.Lng = listing.Address?.Longitude ?? 0;
            listingViewModel.IsBulkUpload = listing.GetListingData<IsBulkUpload>()?.Value ?? false;
            listingViewModel.IsDeleted = listing.IsDeleted;
            listingViewModel.BulkUploadFileName = listing.GetListingData<IsBulkUpload>()?.FileName;
            listingViewModel.PropertyType = listing.GetListingData<PropertyType>().Value;
            listingViewModel.PropertySubType = listing.GetListingData<PropertySubType>().Value;
            listingViewModel.PropertyUseClass = listing.GetListingData<PropertyUseClass>().Value;
            listingViewModel.ListingType = listing.GetListingData<ListingType>().Value;
            listingViewModel.SyndicationFlag = listing.GetListingData<SyndicationFlag>()?.Value;
            listingViewModel.SyndicationMarket = listing.GetListingData<SyndicationFlag>()?.Market;
            listingViewModel.Website = listing.GetListingData<Website>().Value;
            listingViewModel.Video = listing.GetListingData<Video>().Value;
            listingViewModel.WalkThrough = listing.GetListingData<WalkThrough>().Value;
            listingViewModel.ImportedData = listing.GetListingData<ImportedData>().Value;
            listingViewModel.Operator = listing.GetListingData<Operator>().Value;
            listingViewModel.Headline = listing.GetListingDataAllLanguages<Headline>().Select(Map);
            listingViewModel.Floors = listing.GetListingData<Floor>().Value;
            listingViewModel.YearBuilt = listing.GetListingData<YearBuilt>().Value;
            listingViewModel.State = listing.GetListingData<PublishingState>().Value;
            listingViewModel.DatePublished = listing.GetListingData<PublishingState>().DateUpdated.ToLocalTime();
            listingViewModel.DateListed = listing.GetListingData<PublishingState>().DateListed.ToLocalTime();
            listingViewModel.PreviewState = listing.GetListingData<PreviewState>().Value;
            listingViewModel.BuildingDescription = listing.GetListingDataAllLanguages<BuildingDescription>().Select(Map);
            listingViewModel.LocationDescription = listing.GetListingDataAllLanguages<LocationDescription>().Select(Map);
            listingViewModel.Highlights = listing.GetListingDataAllLanguages<Highlight>().Select(Map).OrderBy(h => h.Order);
            listingViewModel.MicroMarkets = listing.GetListingDataArray<MicroMarket>().Select(Map);
            listingViewModel.ChargesAndModifiers = listing.GetListingDataArray<ChargesAndModifiers>().Select(Map);
            listingViewModel.Specifications = Map(listing.GetListingData<Specifications>());
            listingViewModel.PropertySizes = listing.GetListingDataArray<PropertySize>().Select(Map);
            listingViewModel.Aspects = listing.GetListingDataArray<Aspect>().Select(Map);
            listingViewModel.PointsOfInterests = listing.GetListingDataArray<PointsOfInterests>().Select(Map);
            listingViewModel.TransportationTypes = listing.GetListingDataArray<TransportationTypes>().Select(Map);
            listingViewModel.Parkings = Map(listing.GetListingData<Parkings>());
            listingViewModel.Photos = MapImages(listing.ListingImage, ImageCategory.Photo.ToString());
            listingViewModel.Floorplans = MapImages(listing.ListingImage, ImageCategory.FloorPlan.ToString());
            listingViewModel.Brochures = listing.GetListingDataArray<Brochure>().Select(Map);
            listingViewModel.EpcGraphs = listing.GetListingDataArray<EpcGraph>().Select(Map);
            listingViewModel.Spaces = listing.Spaces?
                .OrderBy(s => s.GetListingData<SortOrder>()?.Value ?? int.MaxValue)
                .ThenBy(s => s.ID)
                .Select(MapSpace);
            listingViewModel.SpacesCount = listing.Spaces?.Count() ?? 0;
            listingViewModel.Contacts = listing.ListingBroker?
                .Where(data => data.Broker != null)
                .OrderBy(x => x.Order)
                .Select(data => data.Broker)
                .Select(Map);

            listingViewModel.DataSource = Map(listing.GetListingData<DataSource>());
            listingViewModel.ListingAssignment = Map(listing.GetListingData<ListingAssignment>());

            if (listingViewModel.State == PublishingStateEnum.Published.ToString())
            {
                listingViewModel.ExternalPublishUrl = GenerateExternalUrl(listing, preview: false);
            }

            if (_previewFeatureFlag == "true")
            {
                listingViewModel.PreviewSearchApiEndPoint = GeneratePrevSearchApiEndpoint(listing);
                listingViewModel.ExternalPreviewUrl = GenerateExternalUrl(listing, preview: true);
            }
            return listingViewModel;
        }

        private string GeneratePreviewUrl(Listing listing)
        {
            var homeSiteId = listing.Region?.HomeSiteID;
            var propertyType = listing.GetListingData<PropertyType>()?.Value;
            var listingType = listing.GetListingData<ListingType>()?.Value;
            var externalPreviewUrl = listing.Region?.ExternalPreviewUrl;

            if (string.IsNullOrWhiteSpace(homeSiteId) || string.IsNullOrWhiteSpace(externalPreviewUrl) || string.IsNullOrWhiteSpace(listing.PreviewID)) return null;

            if (string.IsNullOrWhiteSpace(propertyType))
                propertyType = "default";

            // SearchAPI format for listingType: "aspects=isSale^isLease"
            // The "^" is a logical AND
            var aspect = string.Join('^', 
                (listingType.ToEnum<AspectsEnum>() ?? AspectsEnum.lease)
                .ExpandListingType()
                .Select(a => a.ToAlias(AliasType.StoreApi)));

            // Get country specific site
            var site = _configs.Value.PreviewSettings.Sites
                .Where(x => string.Equals(x.HomeSiteId, homeSiteId, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            var controllerPath = site?.ControllerPath;
            if (controllerPath == null) 
            {
                if (externalPreviewUrl.Contains("{1}")) return null;
                else controllerPath = string.Empty;
            }

            return string.Format(externalPreviewUrl, homeSiteId, controllerPath, propertyType, listing.PreviewID, aspect);
        }

        public Listing Map(Listing listing, ListingViewModel listingViewModel)
        {
            if (listingViewModel == null) throw new ArgumentNullException(nameof(listingViewModel));

            if (listingViewModel.ExternalId != null) listing.ExternalID = listingViewModel.ExternalId;
            if (listingViewModel.PreviewId != null) listing.PreviewID = listingViewModel.PreviewId;
            if (listingViewModel.MiqId != null) listing.MIQID = listingViewModel.MiqId;

            listing.Name = listingViewModel.PropertyName?.ToLower() == "null" ? null : listingViewModel.PropertyName;
            if (Guid.TryParse(listingViewModel.RegionID, out Guid regionId))
            {
                listing.RegionID = regionId;
            } else if (listing.RegionID == default)
            {
                listing.RegionID = Region.DefaultID;
            }
            listing.Status = listingViewModel.Status;
            listing.AvailableFrom = CleanDate(listingViewModel.AvailableFrom);
            //Not mapping DateCreated, controlled by repo
            if (listing.Address == null)
                listing.Address = new Address();

            listing.Address.Street1 = listingViewModel.Street;
            listing.Address.Street2 = listingViewModel.Street2;
            listing.Address.City = listingViewModel.City;
            listing.Address.StateProvince = listingViewModel.StateOrProvince;
            listing.Address.PostalCode = listingViewModel.PostalCode;
            listing.Address.Country = listingViewModel.Country;
            listing.Address.Latitude = listingViewModel.Lat;
            listing.Address.Longitude = listingViewModel.Lng;

            if (listing.ListingData == null) listing.ListingData = new List<ListingData>();
            listing.SetListingData(new EnergyRating() { Value = listingViewModel.EnergyRating });
            listing.SetListingDataArray(listingViewModel.ExternalRatings?.Select(Map));
            listing.SetListingData(new PropertyRecordName() { Value = listingViewModel.PropertyRecordName });
            listing.SetListingData(new ConfigId() { Value = listingViewModel.ConfigId });
            listing.SetListingData(new PropertyType() { Value = listingViewModel.PropertyType });
            listing.SetListingData(new Video() { Value = ConvertToUri(listingViewModel.Video) });
            listing.SetListingData(new WalkThrough() { Value = ConvertToUri(listingViewModel.WalkThrough) });
            listing.SetListingData(new ImportedData() { Value = listingViewModel.ImportedData });
            listing.SetListingData(new PropertySubType() { Value = listingViewModel.PropertySubType });
            listing.SetListingData(new PropertyUseClass() { Value = listingViewModel.PropertyUseClass });
            listing.SetListingData(new ListingType() { Value = listingViewModel.ListingType });
            listing.SetListingData(new SyndicationFlag() { Value = listingViewModel.SyndicationFlag ?? false, Market = listingViewModel.SyndicationMarket });
            listing.SetListingData(new Website() { Value = ConvertToUri(listingViewModel.Website) });
            listing.SetListingData(new Operator() { Value = listingViewModel.Operator });
            listing.SetListingData(new Floor() { Value = listingViewModel.Floors });
            listing.SetListingData(new YearBuilt() { Value = listingViewModel.YearBuilt });
            listing.SetListingDataAllLanguages(listingViewModel.Headline?.Select(Map<Headline>));
            listing.SetListingDataAllLanguages(listingViewModel.BuildingDescription?.Select(Map<BuildingDescription>));
            listing.SetListingDataAllLanguages(listingViewModel.LocationDescription?.Select(Map<LocationDescription>));
            listing.SetListingData(Map(listingViewModel.DataSource));
            listing.SetListingData(Map(listingViewModel.ListingAssignment));
            listing.SetListingDataArray(listingViewModel.Brochures?.Select(Map<Brochure>));
            listing.SetListingDataArray(listingViewModel.EpcGraphs?.Select(Map<EpcGraph>));
            listing.SetListingDataArrayAllLanguages(listingViewModel.Highlights?.Select(Map));
            listing.SetListingDataArray(listingViewModel.ChargesAndModifiers?.Select(Map));
            listing.SetListingData(Map(listingViewModel.Specifications));
            listing.SetListingDataArray(listingViewModel.PropertySizes?.Select(Map));
            listing.SetListingDataArray(listingViewModel.MicroMarkets?.Select(Map));
            listing.SetListingDataArray(listingViewModel.Aspects?.Select(MapAspect));
            listing.SetListingDataArray(listingViewModel.PointsOfInterests?.Select(Map));
            listing.SetListingDataArray(listingViewModel.TransportationTypes?.Select(Map));
            listing.SetListingData(Map(listingViewModel.Parkings));
            listing.SetListingData(new IsBulkUpload() { 
                Value = listingViewModel.IsBulkUpload ?? listing.GetListingData<IsBulkUpload>().Value, 
                FileName = listingViewModel.BulkUploadFileName ?? listing.GetListingData<IsBulkUpload>().FileName
            });

            // if this is new listing, default the publishingState 
            if (listing.ID == 0) listing.SetListingData(new PublishingState());

            // update images
            if (listing.ListingImage == null) listing.ListingImage = new List<ListingImage>();
            listing.ListingImage = Map(listing.ListingImage, listingViewModel.Photos, ImageCategory.Photo.ToString());
            listing.ListingImage = Map(listing.ListingImage, listingViewModel.Floorplans, ImageCategory.FloorPlan.ToString());

            // update spaces
            if (listing.Spaces == null) listing.Spaces = new List<Listing>();
            var spaceSortOrder = 0;
            var updatedSpaces = new HashSet<Listing>();
            foreach (var spaceViewModel in listingViewModel.Spaces ?? new List<SpacesViewModel>())
            {
                Listing space = null;
                if (spaceViewModel.Id > 0) space = listing.Spaces.FirstOrDefault(s => s.ID == spaceViewModel.Id);
                if (space == null)
                {
                    space = new Listing();
                    listing.Spaces.Add(space);
                }

                Map(space, spaceViewModel);
                space.RegionID = listing.RegionID;
                space.SetListingData(new SortOrder { Value = spaceSortOrder++ });
                updatedSpaces.Add(space);
            }

            foreach (var space in listing.Spaces.Where(s => !updatedSpaces.Contains(s)).ToList())
            {
                listing.Spaces.Remove(space);
            }

            return listing;
        }

        /// <summary>
        /// Sets DateTime to 11:00am UTC. Use on values where time of day is irrelevant and you don't want the date to change across timezones.
        /// </summary>
        /// <param name="value">The value to clean</param>
        /// <returns>A DateTime that will have the same date across all timezones</returns>
        private DateTime? CleanDate(DateTime? value)
        {
            if (value == null) return null;
            return new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 11, 0, 0, DateTimeKind.Utc);
        }

        private SpacesViewModel MapSpace(Listing listing)
        {
            return new SpacesViewModel()
            {
                Id = listing.ID,
                ExternalId = listing.ExternalID,
                PreviewId = listing.PreviewID,
                MiqId = listing.MIQID,
                Name = listing.GetListingDataAllLanguages<SpaceName>().Select(Map),
                Status = listing.Status,
                SpaceType = listing.UsageType,
                AvailableFrom = listing.AvailableFrom,
                SpaceDescription = listing.GetListingDataAllLanguages<BuildingDescription>().Select(Map),
                Brochures = listing.GetListingDataArray<Brochure>().Select(Map),
                Photos = MapImages(listing.ListingImage, ImageCategory.Photo.ToString()),
                Floorplans = MapImages(listing.ListingImage, ImageCategory.FloorPlan.ToString()),
                Specifications = Map(listing.GetListingData<Specifications>()),
                SpaceSizes = listing.GetListingDataArray<PropertySize>().Select(Map),
                Video = listing.GetListingData<Video>().Value,
                WalkThrough = listing.GetListingData<WalkThrough>().Value
            };
        }

        public Listing Map(Listing listing, SpacesViewModel spacesViewModel)
        {
            if (listing == null) throw new ArgumentNullException(nameof(listing));

            if (spacesViewModel.ExternalId != null) listing.ExternalID = spacesViewModel.ExternalId;
            if (spacesViewModel.PreviewId != null) listing.PreviewID = spacesViewModel.PreviewId;
            if (spacesViewModel.MiqId != null) listing.MIQID = spacesViewModel.MiqId;

            listing.SetListingDataAllLanguages(spacesViewModel.Name?.Select(Map<SpaceName>));
            listing.Status = spacesViewModel.Status;
            listing.UsageType = spacesViewModel.SpaceType;
            listing.AvailableFrom = CleanDate(spacesViewModel.AvailableFrom);
            if (listing.ListingData == null) listing.ListingData = new List<ListingData>();
            listing.SetListingDataAllLanguages(spacesViewModel.SpaceDescription?.Select(Map<BuildingDescription>));
            listing.SetListingData(Map(spacesViewModel.Specifications));
            listing.SetListingDataArray(spacesViewModel.Brochures?.Select(Map<Brochure>));
            listing.SetListingDataArray(spacesViewModel.SpaceSizes?.Select(Map));
            listing.SetListingData(new Video() { Value = ConvertToUri(spacesViewModel.Video) });
            listing.SetListingData(new WalkThrough() { Value = ConvertToUri(spacesViewModel.WalkThrough) });

            if (listing.ListingImage == null) listing.ListingImage = new List<ListingImage>();
            listing.ListingImage = Map(listing.ListingImage, spacesViewModel.Photos, ImageCategory.Photo.ToString());
            listing.ListingImage = Map(listing.ListingImage, spacesViewModel.Floorplans, ImageCategory.FloorPlan.ToString());

            return listing;
        }

        public IEnumerable<Tuple<Broker, int>> Map(IEnumerable<Broker> brokers, IEnumerable<ContactsViewModel> contacts)
        {
            //Tuple<Broker, int>: second value int is the broker order in the listing
            var brokersWithOrder = new List<Tuple<Broker, int>>();
            int i = 0;
            foreach (var contact in contacts)
            {
                var broker = brokers.FirstOrDefault(x => x.ID == contact.ContactId);
                int order = (contact.Order > 0) ? contact.Order : ++i;
                
                broker = Map(broker, contact);
                brokersWithOrder.Add(new Tuple<Broker, int>(broker, order));
            }       

            return brokersWithOrder;
        }

        public Broker Map(Broker broker, ContactsViewModel contact)
        {
            if (broker == null) broker = new Broker();

            Func<string, string, string> pick = contact.PreventOverwrite ?
                (brokerValue, contactValue) => brokerValue ?? contactValue :
                (brokerValue, contactValue) => contactValue ?? brokerValue;

            broker.FirstName = pick(broker.FirstName, contact.FirstName);
            broker.LastName = pick(broker.LastName, contact.LastName);
            broker.Phone = pick(broker.Phone, contact.Phone);
            broker.Email = pick(broker.Email, contact.Email);
            broker.Location = pick(broker.Location, contact.Location);
            broker.Avatar = pick(broker.Avatar, contact.Avatar);
            broker.License = pick(broker.License, contact.AdditionalFields?.License?.Trim());
            if (broker.License == "0" || string.IsNullOrWhiteSpace(broker.License)) broker.License = null;

            return broker;
        }

        public List<ListingBroker> Map(IEnumerable<ListingBroker> listingBrokers, IEnumerable<Tuple<Broker, int>> brokersWithOrder)
        {
            var result = new List<ListingBroker>();
            
            foreach (var brokerWithOrder in brokersWithOrder)
            {
                var broker = brokerWithOrder.Item1;
                var listingBroker = listingBrokers.FirstOrDefault(x => x.BrokerID == broker.ID);

                if (listingBroker == null)
                    listingBroker = new ListingBroker { Broker = broker };
                
                listingBroker.Order = brokerWithOrder.Item2;
                result.Add(listingBroker);
            }

            return result;
        }

        private List<ListingImage> Map(List<ListingImage> listingImages, IEnumerable<ImagesViewModel> imageViewModels, string imageCategory)
        {
            if (imageViewModels == null) return listingImages ?? new List<ListingImage>();
            var result = new HashSet<ListingImage>();
            var imageOrder = 1;
            foreach (var ivm in imageViewModels)
            {
                var listingImage = listingImages.FirstOrDefault(p => p.ImageID == ivm.Id && p.ImageCategory == imageCategory && p.ID != 0);
                if (listingImage == null)
                {
                    listingImage = new ListingImage();
                    listingImages.Add(listingImage);
                }
                Map(listingImage, ivm, imageCategory);
                listingImage.Order = listingImage.Order > 0 ? imageOrder = listingImage.Order : imageOrder++;
                result.Add(listingImage);
            }

            //Remove images no longer link to the listing
            foreach (var listingImage in listingImages.Where(s => (s.ImageCategory == imageCategory) && !result.Contains(s)).ToList())
            {
                listingImages.Remove(listingImage);
            }
            return listingImages;
        }

        private Specifications Map(SpecificationsViewModel data)
        {
            var result = new Specifications();
            if (data != null)
            {
                result.LeaseTerm = data.LeaseTerm;
                result.Measure = data.Measure;
                result.LeaseType = data.LeaseType;
                result.MinSpace = data.MinSpace;
                result.MaxSpace = data.MaxSpace;
                result.TotalSpace = data.TotalSpace;
                result.MaxPrice = data.MaxPrice;
                result.MinPrice = data.MinPrice;
                result.TaxModifer = data.TaxModifer;
                result.ContactBrokerForPrice = data.ContactBrokerForPrice;
                result.Bedrooms = data.Bedrooms;
                result.CurrencyCode = data.CurrencyCode;
                result.LeaseRateType = data.LeaseRateType.ToEnum<LeaseRateTypesEnum>(ViewModel).ToAlias() ?? data.LeaseRateType;
                result.SalePrice = data.SalePrice;
                result.ShowPriceWithUoM = data.ShowPriceWithUoM;
                result.AutoCalculateMinSpace = data.AutoCalculateMinSpace;
                result.AutoCalculateTotalSpace = data.AutoCalculateTotalSpace;
                result.AutoCalculateMinPrice = data.AutoCalculateMinPrice;
                result.AutoCalculateMaxPrice = data.AutoCalculateMaxPrice;
                result.AutoCalculateTotalPrice = data.AutoCalculateTotalPrice;
            }

            return result;
        }

        private ChargesAndModifiers Map(ChargesAndModifiersViewModel data)
        {
            return new ChargesAndModifiers()
            {
                ChargeType = data.ChargeType,
                ChargeModifier = data.ChargeModifier,
                Term = data.Term,
                Amount = data.Amount,
                PerUnitType = data.PerUnitType,
                CurrencyCode = data.CurrencyCode,
                Year = data.Year
            };
        }

        private PropertySize Map(PropertySizesViewModel data)
        {
            return new PropertySize()
            {
                SizeKind = data.SizeKind,
                MeasureUnit = data.MeasureUnit,
                Amount = data.Amount                
            };
        }

        private MicroMarket Map(MicroMarketsViewModel data)
        {
            return new MicroMarket()
            {
                Value = data.Value,
                Order = data.Order
            };
        }
        private PointsOfInterests Map(PointsOfInterestsViewModel data)
        {
            return new PointsOfInterests() 
            {
                InterestKind = data.InterestKind,
                Places = data.Places?.Where(p => !string.IsNullOrWhiteSpace(p.Name)).Select(Map).ToList()
            };
        
        }

        private Parkings Map(ParkingsViewModel data)
        {
            var result = new Parkings();
            if (data != null)
            {
                result.Ratio = data.Ratio;
                result.RatioPer = data.RatioPer;
                result.RatioPerUnit = data.RatioPerUnit;
                result.ParkingDetails = data.ParkingDetails?.Select(Map).ToList();
            }
            return result;
        }

        private ParkingDetail Map(ParkingDetailViewModel x)
        {
            return new ParkingDetail
            {
                ParkingType = x.ParkingType,
                ParkingSpace = x.ParkingSpace,
                Amount = x.Amount,
                Interval = x.Interval,
                CurrencyCode = x.CurrencyCode
            };
        }

        private TransportationTypes Map(TransportationTypesViewModel data)
        {
            return new TransportationTypes() 
            {
                Type = data.Type,
                Places = data.Places?.Where(p => !string.IsNullOrWhiteSpace(p.Name)).Select(Map).ToList()
            };
        }

        private Place Map(PlacesViewModel x)
        {
            return new Place() 
            {
                Name = x.Name,
                Type = x.Type,
                Distances = x.Distances,
                DistanceUnits = !string.IsNullOrWhiteSpace(x.DistanceUnits) ? 
                                x.DistanceUnits : "",
                Duration = x.Duration,
                TravelMode = !string.IsNullOrWhiteSpace(x.TravelMode) ? 
                                x.TravelMode : "",
                Order = x.Order
            };
        }

        private ListingImage Map(ListingImage listingImage, ImagesViewModel imagesViewModel, string imageCategory)
        {
            if (listingImage == null) throw new ArgumentNullException(nameof(listingImage));
            if (imagesViewModel.Id == 0) 
            {
                var image = new Image()
                {
                    Url = imagesViewModel.Url,
                    WatermarkProcessStatus = (int)WatermarkDetectResult.Not_Processed,
                    UploadedAt = DateTime.UtcNow,
                };
                listingImage.Image = image;
            }
            else
            {
                listingImage.ImageID = imagesViewModel.Id;
            }
            listingImage.IsPrimary = imagesViewModel.Primary;
            listingImage.IsActive = imagesViewModel.Active;
            listingImage.HasWatermark = imagesViewModel.Watermark;
            listingImage.IsUserOverride = imagesViewModel.UserOverride;
            listingImage.DisplayText = imagesViewModel.DisplayText;
            listingImage.Order = imagesViewModel.Order;
            listingImage.ImageCategory = imageCategory;
            
            return listingImage;
        }

        private static MediaViewModel Map(IMedia model) => Map(model, null);

        private static MediaViewModel Map(IMedia model, MediaViewModel viewModel) {
            if (model == null) return viewModel;
            if (viewModel == null) viewModel = new MediaViewModel();

            viewModel.Active = model.Active;
            viewModel.DisplayText = model.DisplayText;
            viewModel.Primary = model.Primary;
            viewModel.Url = model.Url;

            return viewModel;
        }

        private static T Map<T>(MediaViewModel viewModel)
            where T : Media, new()
            => Map(viewModel, new T());

        private static T Map<T>(MediaViewModel viewModel, T model = null)
            where T : Media, new()
        {
            if (viewModel == null) return model;
            if (model == null) model = new T();

            model.Active = viewModel.Active;
            model.DisplayText = viewModel.DisplayText;
            model.Primary = viewModel.Primary;
            model.Url = viewModel.Url;

            return model;
        }

        public ContactsViewModel Map(Broker broker)
        {
            return new ContactsViewModel()
            {
                ContactId = broker.ID,
                FirstName = broker.FirstName,
                LastName = broker.LastName,
                Location = broker.Location,
                Avatar = broker.Avatar,
                Phone = broker.Phone,
                Email = broker.Email,
                AdditionalFields = new AdditionalFieldsViewModel()
                {
                    License = broker.License
                }
            };
        }

        private IEnumerable<ImagesViewModel> MapImages(List<ListingImage> listingImages, string imageCategory)
        {
            return listingImages?
                .Where(data => data.Image != null && data.ImageCategory == imageCategory)
                .OrderBy(o => o.Order)
                .Select(Map);
        }

        private ImagesViewModel Map(ListingImage img)
        {
            return new ImagesViewModel()
            {
                Id = img.ImageID,
                Url = img.Image.Url,
                DisplayText = img.DisplayText,
                Primary = img.IsPrimary ?? false,
                Active = img.IsActive ?? false,
                Watermark = img.HasWatermark ?? false,
                UserOverride = img.IsUserOverride ?? false,
                WatermarkProcessStatus = img.Image.WatermarkProcessStatus,
                Order = img.Order
            };
        }

        private MicroMarketsViewModel Map(MicroMarket microMarket)
        {
            return new MicroMarketsViewModel()
            {
                Value = microMarket.Value,
                Order = microMarket.Order
            };
        }

        private PointsOfInterestsViewModel Map(PointsOfInterests data)
        {
            return new PointsOfInterestsViewModel()
            {
                InterestKind = data.InterestKind,
                Places = data.Places?.Select(Map).ToList()
            };
        }

        private TransportationTypesViewModel Map(TransportationTypes data)
        {
            return new TransportationTypesViewModel()
            {
                Type = data.Type, 
                Places = data.Places?.Select(Map).ToList()
            };
        }

        private PlacesViewModel Map(Place x)
        {
            return new PlacesViewModel()
            {
                Name = x.Name,
                Type = x.Type,
                Distances = x.Distances, 
                DistanceUnits = x.DistanceUnits, 
                Duration = x.Duration, 
                TravelMode = x.TravelMode, 
                Order = x.Order
            };
        }

        private ParkingsViewModel Map(Parkings data)
        {
            if (data is null) return new ParkingsViewModel();
            
            return new ParkingsViewModel()
            {
                Ratio = data.Ratio,
                RatioPer = data.RatioPer,
                RatioPerUnit = data.RatioPerUnit,
                ParkingDetails = data.ParkingDetails?.Select(Map).ToList()
            };
        }
        private ParkingDetailViewModel Map(ParkingDetail x)
        {
            return new ParkingDetailViewModel()
            {
                ParkingType = x.ParkingType,
                ParkingSpace = x.ParkingSpace,
                Amount = x.Amount,
                Interval = x.Interval,
                CurrencyCode = x.CurrencyCode
            };
        }

        private ChargesAndModifiersViewModel Map(ChargesAndModifiers chargesAndModifiers)
        {
            return new ChargesAndModifiersViewModel()
            {
                ChargeType = chargesAndModifiers.ChargeType,
                ChargeModifier = chargesAndModifiers.ChargeModifier,
                Term = chargesAndModifiers.Term,
                Amount = chargesAndModifiers.Amount,
                PerUnitType = chargesAndModifiers.PerUnitType,
                Year = chargesAndModifiers.Year,
                CurrencyCode = chargesAndModifiers.CurrencyCode
            };
        }

        private SpecificationsViewModel Map(Specifications specifications)
        {
            return new SpecificationsViewModel()
            {
                LeaseType = specifications.LeaseType,
                Measure = specifications.Measure,
                LeaseTerm = specifications.LeaseTerm,
                MinSpace = specifications.MinSpace,
                MaxSpace = specifications.MaxSpace,
                TotalSpace = specifications.TotalSpace,
                MaxPrice = specifications.MaxPrice,
                MinPrice = specifications.MinPrice,
                TaxModifer = specifications.TaxModifer,
                ContactBrokerForPrice = specifications.ContactBrokerForPrice,
                Bedrooms = specifications.Bedrooms,
                CurrencyCode = specifications.CurrencyCode,
                LeaseRateType = specifications.LeaseRateType.ToEnum<LeaseRateTypesEnum>().ToAlias(ViewModel) ?? specifications.LeaseRateType,
                SalePrice = specifications.SalePrice,
                ShowPriceWithUoM = specifications.ShowPriceWithUoM,
                AutoCalculateMinSpace = specifications.AutoCalculateMinSpace,
                AutoCalculateTotalSpace = specifications.AutoCalculateTotalSpace,
                AutoCalculateMinPrice = specifications.AutoCalculateMinPrice,
                AutoCalculateMaxPrice = specifications.AutoCalculateMaxPrice,
                AutoCalculateTotalPrice = specifications.AutoCalculateTotalPrice
            };
        }

        private PropertySizesViewModel Map(PropertySize propertySize)
        {
            return new PropertySizesViewModel()
            {
                SizeKind = propertySize.SizeKind,
                MeasureUnit = propertySize.MeasureUnit,
                Amount = propertySize.Amount
            };
        }

        private string Map(Aspect aspect)
        {
            return aspect.Value;
        }

        private Aspect MapAspect(string value)
        {
            return new Aspect
            {
                Value = value
            };
        }

        private DataSourceViewModel Map(DataSource dataSource)
        {
            return new DataSourceViewModel()
            {
                DataSources = dataSource.DataSources,
                Other = dataSource.Other
            };
        }

        private DataSource Map(DataSourceViewModel data)
        {
            var result = new DataSource();
            if (data != null)
            {
               result.DataSources = data.DataSources;
               result.Other = data.Other;
            }
            return result;
        }

        private ListingAssignmentViewModel Map(ListingAssignment data)
        {
            return new ListingAssignmentViewModel()
            {
                AssignedBy = data.AssignedBy, 
                AssignmentFlag = data.AssignmentFlag,
                AssignedDate = data.AssignedDate
            };
        }

        private ListingAssignment Map(ListingAssignmentViewModel data)
        {
            var result = new ListingAssignment();
            if (data != null)
            {
                result.AssignedBy = data.AssignedBy;
                result.AssignmentFlag = data.AssignmentFlag;
                result.AssignedDate = data.AssignedDate;
            }
            return result;
        }

        private TextTypeViewModel Map(TextType value) => Map(value, new TextTypeViewModel());
        private TextTypeViewModel Map(TextType value, TextTypeViewModel target)
        {
            if (target == null) return null;
            target.CultureCode = value.CultureCode;
            target.Text = value.Text;
            return target;
        }

        private OrderedTextTypeViewModel Map(OrderedTextType value) => Map(value, new OrderedTextTypeViewModel());
        private OrderedTextTypeViewModel Map(OrderedTextType value, OrderedTextTypeViewModel target)
        {
            if (target == null) return null;
            target = (OrderedTextTypeViewModel)Map((TextType)value, target);
            target.Order = value.Order;
            return target;
        }

        private T Map<T>(TextTypeViewModel value) where T : TextType, new() => Map(value, new T());
        private T Map<T>(TextTypeViewModel value, T target) where T : TextType
        {
            target.CultureCode = value.CultureCode;
            target.Text = value.Text;
            return target;
        }

        private T Map<T>(OrderedTextTypeViewModel value) where T : OrderedTextType, new() => Map(value, new T());
        private T Map<T>(OrderedTextTypeViewModel value, T target) where T : OrderedTextType
        {
            if (target == null) return null;
            target = Map((TextTypeViewModel)value, target);
            target.Order = value.Order;
            return target;
        }

        private HighlightViewModel Map(Highlight model)
        {
            var result = (HighlightViewModel)Map((OrderedTextType)model, new HighlightViewModel());
            result.MiqId = model.MIQID;
            return result;
        }
        
        
        private Highlight Map(HighlightViewModel value) => Map(value, new Highlight());
        private Highlight Map(HighlightViewModel value, Highlight target)
        {
            if (target == null) return null;
            target = Map((OrderedTextTypeViewModel)value, target);
            target.MIQID = value.MiqId;
            return target;
        }

        private ExternalRatings Map(ExternalRatingsViewModel data)
        {
            return new ExternalRatings()
            {
                RatingType = data.RatingType,
                RatingLevel = data.RatingLevel
            };
        }

        private string ConvertToUri(string url)
        {
            if (String.IsNullOrEmpty(url)) return null;

            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                try
                {
                    var uri = new UriBuilder(url).Uri;
                    return uri.AbsoluteUri;
                }
                catch(UriFormatException)
                {
                    return null;    // the problem with this approach is that user will not get back a value if it was invalid
                }
            }
            return null;
        }

        private string GenerateExternalUrl(Listing listing, bool preview)
        {
            if (listing.Region == null) return null;

            var propertyType = listing.GetListingData<PropertyType>().Value.ToEnum<PropertyTypeEnum>();
            var siteId = preview ? listing.Region.PreviewSiteID : listing.Region.HomeSiteID;
            var externalId = preview ? listing.PreviewID : listing.ExternalID;

            if (listing == null
                || propertyType == null
                || string.IsNullOrEmpty(externalId)
                || string.IsNullOrEmpty(siteId))
            {
                return null;
            }

            string canonicalExternalUrl = GetCanonicalUrlFromSiteMapConfig(siteId, propertyType.ToString());

            if (string.IsNullOrWhiteSpace(canonicalExternalUrl))
            {
                return null;
            }
            
            return string.Format(canonicalExternalUrl, externalId, "isLetting"); // The aspect type (isLetting) is corrected on the front end
        }

        private string GetCanonicalUrlFromSiteMapConfig(string siteId, string usageType)
        {
            try
            {
                var sitemapConfig = _siteMapConfigDataProvider.GetSitemapConfig();
                var usageDictionary = sitemapConfig?.GetValue(siteId.TrimEnd("-prev", StringComparison.OrdinalIgnoreCase), StringComparison.OrdinalIgnoreCase) as JObject;
                var languageDictionary = usageDictionary?.GetValue(usageType, StringComparison.OrdinalIgnoreCase);

                return (languageDictionary?.FirstOrDefault() as JProperty)?.Value?.ToString(); // DataEntry doesn't provide multiple links per language at the moment, so just pick the first one.
            } 
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Error generating external link format for site {siteId} and usage type {usageType}", siteId, usageType);
                return null;
            }
        }


        private string GeneratePrevSearchApiEndpoint(Listing listing)
        {
            if (listing == null 
                || string.IsNullOrEmpty(listing.Region?.PreviewSiteID) 
                || string.IsNullOrEmpty(listing.PreviewID))
            {
                return null;
            }
            return $"{_searchApiEndPoint}site={listing.Region.PreviewSiteID}&{_searchKey}={listing.PreviewID}"; 
        }

        private ExternalRatingsViewModel Map(ExternalRatings data)
        {
            if (data is null) return new ExternalRatingsViewModel();

            return new ExternalRatingsViewModel()
            {
                RatingType = data.RatingType,
                RatingLevel = data.RatingLevel
            };
        }
    }
}