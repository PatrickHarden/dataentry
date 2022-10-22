using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Repository;
using dataentry.Services.Business.Listings;
using dataentry.Services.Integration.Edp.Consumption;
using dataentry.Services.Integration.Edp.Model;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dataentry.Services.Integration.Edp.Ingestion
{
    public class IngestionService : IIngestionService
    {
        public bool Enabled => _edpOptions?.Enabled ?? false;
        private readonly EdpGraphQLService _edpGraphQLService;
        private readonly IngestionOptions _edpOptions;
        private readonly IListingAdapter _listingAdapter;
        private readonly ILogger<EdpGraphQLService> _log;
        private readonly IDataEntryRepository _dataEntryRepository;
        private readonly IListingMapper _listingMapper;
        private readonly IListingSerializer _listingSerializer;
        private readonly IJsonDeltaEvaluator _jsonDeltaEvaluator;
        private readonly IConsumptionService _consumptionService;

        public IngestionService(HttpClient httpClient, IOptions<IngestionOptions> edpOptions, IListingAdapter listingAdapter, IDataEntryRepository dataEntryRepository, IListingMapper listingMapper, IListingSerializer listingSerializer, IJsonDeltaEvaluator jsonDeltaEvaluator, IConsumptionService consumptionService, ILogger<EdpGraphQLService> log)
        {
            _edpOptions = edpOptions?.Value ?? 
                throw new ArgumentNullException(nameof(edpOptions));
            _listingAdapter = listingAdapter ?? 
                throw new ArgumentNullException(nameof(listingAdapter));
            _dataEntryRepository = dataEntryRepository ?? 
                throw new ArgumentNullException(nameof(dataEntryRepository));
            _listingMapper = listingMapper ?? 
                throw new ArgumentNullException(nameof(listingMapper));
            _listingSerializer = listingSerializer ?? 
                throw new ArgumentNullException(nameof(listingSerializer));
            _jsonDeltaEvaluator = jsonDeltaEvaluator ?? 
                throw new ArgumentNullException(nameof(jsonDeltaEvaluator));
            _consumptionService = consumptionService ?? 
                throw new ArgumentNullException(nameof(consumptionService));
            _log = log ?? 
                throw new ArgumentNullException(nameof(log));
            _edpGraphQLService = new EdpGraphQLService(httpClient, edpOptions, log);
        }

        public bool EnabledInRegion(string siteId) => _edpOptions?.EnabledInRegion(siteId) ?? false;

        public async Task SubmitListing(Listing listing)
        {
            using(_log.BeginScope(new Dictionary<string, object>
            {
                ["IngestionService_listingId"] = listing.ID,
                ["IngestionService_listingExternalId"] = listing.ExternalID
            }))
            {
                _log.LogDebug("IngestionService.SubmitListing({listingId})", listing.ID);
                
                if (listing.Region == null) throw new NullReferenceException("Listing region object is null");

                if (!EnabledInRegion(listing.Region.HomeSiteID)) ThrowDisabledException();
                
                if (string.IsNullOrWhiteSpace(listing.MIQID))
                {
                    await InsertListing(listing);
                } else
                {
                    if (!_consumptionService.Enabled)
                    {
                        throw new InvalidOperationException("Submitting a listing update requires the ConsumptionService to be enabled.");
                    }
                    var lastPublish = await _dataEntryRepository.GetListingSerialization(listing.ID, ListingSerializationType.LastPublish);
                    await UpdateListing(listing, lastPublish);
                }
            }
        }
        private async Task InsertListing(Listing listing) 
        { 
            var property = _listingAdapter.ConvertToPropertyUnified(listing);
            var availabilities = _listingAdapter.CovertToAvailabilitiesUnified(listing);
            bool sentProperty = false;
            foreach (var availability in availabilities) //TODO: use same code as UpdateListing for spaces?
            {
                using(_log.BeginScope(new Dictionary<string, object>
                {
                    ["IngestionService_availabilityId"] = availability.id
                }))
                {
                    if (!sentProperty)
                    {
                        _log.LogDebug("Attaching property information");
                        sentProperty = true;
                        availability.property = property;
                        availability.property_id = null;
                    }
                    else
                    {
                        _log.LogDebug("Skip attaching property information: already sent.");
                    }

                    var mutation = new createAvailabilityUnifiedMutation();
                    mutation.availability = availability;
                    mutation.request = BuildRequestDetailsUnified();
                    var result = await _edpGraphQLService.RunQueryRaw(mutation);

                    var vm = _listingMapper.Map(listing);
                    var serialization = _listingSerializer.Serialize(vm);
                    await _dataEntryRepository.UpdateListingSerialization(listing.ID, ListingSerializationType.LastPublish, serialization);
                }
            }
        }

        private async Task UpdateListing(Listing listing, string lastPublish)
        {
            var mutations = new List<EdpGraphQLMutation<ResultData>>();
            var requestIds = new Dictionary<EdpGraphQLMutation<ResultData>, string>();
            if (!int.TryParse(listing.MIQID, out var propertyId)) {
                _log.LogWarning("Ingestion Service failed to parse MIQ ID. MIQID: {1}", listing.MIQID ?? "[null]");
                return;
            }
            var regionId = listing.RegionID;

            var existing = await _consumptionService.GetPropertyWithAvailability(propertyId, regionId);

            var lazyPropertyUpdate = new Lazy<PropertyUpdate>(() =>
            {
                var propertyUpdate = new PropertyUpdate();
                var mutation = new updatePropertyMutation
                {
                    property_id = propertyId,
                    property = propertyUpdate,
                    request = BuildRequestDetails()
                };
                mutations.Add(mutation);
                requestIds[mutation] = mutation.request.request_id;
                return propertyUpdate;
            });

            //var photos = listing.GetListingDataArray<Photo>();
            //var photoAdds = new Dictionary<string, addPropertyMediaInformationMutation>();
            //var photoUpdates = new Dictionary<string, updatePropertyMediaMutation>();

            //var brochures = listing.GetListingDataArray<Brochure>();
            //var brochureAdds = new Dictionary<string, addPropertyMediaInformationMutation>();
            //var brochureUpdates = new Dictionary<string, updatePropertyMediaMutation>();

            //var floorplans = listing.GetListingDataArray<FloorPlan>();
            //var floorplanAdds = new Dictionary<string, addPropertyMediaInformationMutation>();
            //var floorplanUpdates = new Dictionary<string, updatePropertyMediaMutation>();

            var spaceAdds = new Dictionary<int, Lazy<createAvailabilityUnifiedMutation>>();
            var spaceUpdates = new Dictionary<int, Lazy<updateAvailabilityMutation>>();

            var current = _listingMapper.Map(listing);
            var currentSerialization = _listingSerializer.Serialize(current);

            if (string.IsNullOrEmpty(lastPublish))
            {
                var existingListing = await _consumptionService.ConvertPropertyWithAvailabilityToListingViewModel(existing, regionId);
                lastPublish = _listingSerializer.Serialize(existingListing);
            }

            var deltas = new Queue<ListingDeltaViewModel>(_jsonDeltaEvaluator.Evaluate(lastPublish, currentSerialization));
            
            while (deltas.Any())
            {
                var delta = deltas.Dequeue();
                if (delta.NewValue == "undefined")
                {
                    //whitelist for supported fields for delete
                    if (!delta.JsonPath.StartsWith("$.Aspects[")) continue;
                }
                if (delta.JsonPath == "$.Specifications.TotalSpace")
                {
                    lazyPropertyUpdate.Value.total_gross_area = Convert.ToDecimal(delta.NewValue);
                }
                else if (delta.JsonPath == "$.Sizes[?(@.SizeKind=='LandSize')].Amount")
                {
                    lazyPropertyUpdate.Value.total_land_area = Convert.ToDecimal(delta.NewValue);
                }
                else if (delta.JsonPath == "$.Sizes[?(@.SizeKind=='LandSize')].MeasureUnit")
                {
                    lazyPropertyUpdate.Value.total_land_area_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                }
                else if (delta.JsonPath == "$.PropertyType")
                {
                    var oldUsageType = _listingAdapter.MapPropertyUsage(delta.OriginalValue) ?? "Unknown";
                    var existingPropertyUsage = existing.PropertyDetail.usages.FirstOrDefault(u => u.ref_property_usage_type_desc == oldUsageType);

                    if (existingPropertyUsage != null)
                    {
                        var removeMutation = new deletePropertyUsageMutation
                        {
                            property_id = propertyId,
                            property_usage_id = existingPropertyUsage.id,
                            request = BuildRequestDetails()
                        };

                        mutations.Add(removeMutation);
                        requestIds[removeMutation] = removeMutation.request.request_id;
                    }

                    var addMutation = new addPropertyUsageMutation
                    {
                        property_id = propertyId,
                        property_usage = new Model.PropertyUsage
                        {
                            ref_property_usage_type_desc = _listingAdapter.MapPropertyUsage(delta.NewValue)
                        },
                        request = BuildRequestDetails()
                    };

                    mutations.Add(addMutation);
                    requestIds[addMutation] = addMutation.request.request_id;
                }
                else if (delta.JsonPath.StartsWith("$.Aspects["))
                {
                    if (delta.NewValue == "undefined")
                    {
                        var existingAmenity = existing.PropertyDetail.property_amenity.FirstOrDefault(a => a.property_amenity_type_desc == delta.OriginalValue);
                        if (existingAmenity != null)
                        {
                            var mutation = new deletePropertyAmenityMutation
                            {
                                property_id = propertyId,
                                amenity_id = existingAmenity.id,
                                request = BuildRequestDetails()
                            };

                            mutations.Add(mutation);
                            requestIds[mutation] = mutation.request.request_id;
                        }
                    }
                    else
                    {
                        var amenity = new Model.PropertyAmenity
                        {
                            property_amenity_type_desc = delta.NewValue
                        };

                        var mutation = new addPropertyAmenityMutation
                        {
                            property_id = propertyId,
                            amenity = amenity,
                            request = BuildRequestDetails()
                        };

                        mutations.Add(mutation);
                        requestIds[mutation] = mutation.request.request_id;
                    }
                }
                else if (delta.JsonPath == "ListingType") {
                    // This value is mapped in ListingAdapter.GetAvailabilityUnifiedBase, so it is already mapped for new availabilities.
                    // However, if it was changed, we need to update existing availabilities.
                    foreach (var space in listing.Spaces) {
                        deltas.Enqueue(new ListingDeltaViewModel{
                            JsonPath = $"$.Spaces[@.MiqId=='{space.MIQID}'].ListingType",
                            NewValue = delta.NewValue
                        });
                    }
                }
                //else if (delta.JsonPath.StartsWith("$.Photos"))
                //{
                //    var match = Regex.Match(delta.JsonPath, @"Photos\[\?\(@\.Url=='(.*?)'\)\]\.");
                //    if (match.Success)
                //    {
                //        var url = match.Groups[1].Value;

                //        addPropertyMediaInformationMutation photoAdd = null;
                //        updatePropertyMediaMutation photoUpdate = null;

                //        if (!(
                //            photoAdds.TryGetValue(url, out photoAdd)
                //            || photoUpdates.TryGetValue(url, out photoUpdate)
                //        ))
                //        {
                //            var existingMedia = existing.PropertyDetail.property_media_information.FirstOrDefault(m => m.media_path == url);
                //            if (existingMedia == null)
                //            {
                //                photoAdd = new addPropertyMediaInformationMutation
                //                {
                //                    property_id = propertyId,
                //                    property_media_information = new Model.PropertyMediaInformation
                //                    {
                //                        media_path = url,
                //                        media_type_desc = "Photo"
                //                    },
                //                    request = BuildRequestDetails()
                //                };

                //                photoAdds[url] = photoAdd;
                //                mutations.Add(photoAdd);
                //                requestIds[photoAdd] = photoAdd.request.request_id;
                //            }
                //            else
                //            {
                //                photoUpdate = new updatePropertyMediaMutation
                //                {
                //                    property_id = propertyId,
                //                    property_media_information_id = existingMedia.id,
                //                    property_media_information = new PropertyMediaInformationUpdate(),
                //                    request = BuildRequestDetails()
                //                };

                //                photoUpdates[url] = photoUpdate;
                //                mutations.Add(photoUpdate);
                //                requestIds[photoUpdate] = photoUpdate.request.request_id;
                //            }
                //        }

                //        var subPath = delta.JsonPath.Substring(match.Index + match.Length);
                //        if (subPath == "DisplayText")
                //        {
                //            if (photoAdd != null)
                //            {
                //                photoAdd.property_media_information.media_name = delta.NewValue;
                //                photoAdd.property_media_information.media_caption = delta.NewValue;
                //            }
                //            else
                //            {
                //                photoUpdate.property_media_information.media_name = delta.NewValue;
                //                photoUpdate.property_media_information.media_caption = delta.NewValue;
                //            }
                //        }
                //        else if (subPath == "Primary")
                //        {
                //            if (photoAdd != null)
                //            {
                //                photoAdd.property_media_information.primary_image_f = Convert.ToBoolean(delta.NewValue);
                //            }
                //            else
                //            {
                //                photoUpdate.property_media_information.primary_image_f = Convert.ToBoolean(delta.NewValue);
                //            }
                //        }
                //    }
                //}
                //else if (delta.JsonPath.StartsWith("$.Brochures"))
                //{
                //    var match = Regex.Match(delta.JsonPath, @"Brochures\[\?\(@\.Url=='(.*?)'\)\]\.");
                //    if (match.Success)
                //    {
                //        var url = match.Groups[1].Value; //TODO: JsonDecode

                //        addPropertyMediaInformationMutation brochureAdd = null;
                //        updatePropertyMediaMutation brochureUpdate = null;

                //        if (!(
                //            brochureAdds.TryGetValue(url, out brochureAdd)
                //            || brochureUpdates.TryGetValue(url, out brochureUpdate)
                //        ))
                //        {
                //            var existingMedia = existing.PropertyDetail.property_media_information.FirstOrDefault(m => m.media_path == url);
                //            if (existingMedia == null)
                //            {
                //                brochureAdd = new addPropertyMediaInformationMutation
                //                {
                //                    property_id = propertyId,
                //                    property_media_information = new Model.PropertyMediaInformation
                //                    {
                //                        media_path = url,
                //                        media_type_desc = "Image",
                //                        media_content_type_desc = "Brochure",
                //                        primary_image_f = false
                //                    },
                //                    request = BuildRequestDetails()
                //                };

                //                brochureAdds[url] = brochureAdd;
                //                mutations.Add(brochureAdd);
                //                requestIds[brochureAdd] = brochureAdd.request.request_id;
                //            }
                //            else
                //            {
                //                brochureUpdate = new updatePropertyMediaMutation
                //                {
                //                    property_id = propertyId,
                //                    property_media_information_id = existingMedia.id,
                //                    property_media_information = new PropertyMediaInformationUpdate(),
                //                    request = BuildRequestDetails()
                //                };

                //                brochureUpdates[url] = brochureUpdate;
                //                mutations.Add(brochureUpdate);
                //                requestIds[brochureUpdate] = brochureUpdate.request.request_id;
                //            }
                //        }

                //        var subPath = delta.JsonPath.Substring(match.Index + match.Length);
                //        if (subPath == "DisplayText")
                //        {
                //            if (brochureAdd != null)
                //            {
                //                brochureAdd.property_media_information.media_name = delta.NewValue;
                //                brochureAdd.property_media_information.media_caption = delta.NewValue;
                //            }
                //            else
                //            {
                //                brochureUpdate.property_media_information.media_name = delta.NewValue;
                //                brochureUpdate.property_media_information.media_caption = delta.NewValue;
                //            }
                //        }
                //    }
                //}
                //else if (delta.JsonPath.StartsWith("$.Floorplans"))
                //{
                //    var match = Regex.Match(delta.JsonPath, @"Floorplans\[\?\(@\.Url=='(.*?)'\)\]\.");
                //    if (match.Success)
                //    {
                //        var url = match.Groups[1].Value; //TODO: JsonDecode

                //        addPropertyMediaInformationMutation floorplanAdd = null;
                //        updatePropertyMediaMutation floorplanUpdate = null;

                //        if (!(
                //            floorplanAdds.TryGetValue(url, out floorplanAdd)
                //            || floorplanUpdates.TryGetValue(url, out floorplanUpdate)
                //        ))
                //        {
                //            var existingMedia = existing.PropertyDetail.property_media_information.FirstOrDefault(m => m.media_path == url);
                //            if (existingMedia == null)
                //            {
                //                floorplanAdd = new addPropertyMediaInformationMutation
                //                {
                //                    property_id = propertyId,
                //                    property_media_information = new Model.PropertyMediaInformation
                //                    {
                //                        media_path = url,
                //                        media_type_desc = "Image",
                //                        media_content_type_desc = "Floor Plan",
                //                        primary_image_f = false
                //                    },
                //                    request = BuildRequestDetails()
                //                };

                //                floorplanAdds[url] = floorplanAdd;
                //                mutations.Add(floorplanAdd);
                //                requestIds[floorplanAdd] = floorplanAdd.request.request_id;
                //            }
                //            else
                //            {
                //                floorplanUpdate = new updatePropertyMediaMutation
                //                {
                //                    property_id = propertyId,
                //                    property_media_information_id = existingMedia.id,
                //                    property_media_information = new PropertyMediaInformationUpdate(),
                //                    request = BuildRequestDetails()
                //                };

                //                floorplanUpdates[url] = floorplanUpdate;
                //                mutations.Add(floorplanUpdate);
                //                requestIds[floorplanUpdate] = floorplanUpdate.request.request_id;
                //            }
                //        }

                //        var subPath = delta.JsonPath.Substring(match.Index + match.Length);
                //        if (subPath == "DisplayText")
                //        {
                //            if (floorplanAdd != null)
                //            {
                //                floorplanAdd.property_media_information.media_name = delta.NewValue;
                //                floorplanAdd.property_media_information.media_caption = delta.NewValue;
                //            }
                //            else
                //            {
                //                floorplanUpdate.property_media_information.media_name = delta.NewValue;
                //                floorplanUpdate.property_media_information.media_caption = delta.NewValue;
                //            }
                //        }
                //    }
                //}
                else if (delta.JsonPath.StartsWith("$.Spaces"))
                {
                    var match = Regex.Match(delta.JsonPath, @"Spaces\[\?\(@\.MiqId=='?(\d+)'?\)\]\.");
                    if (match.Success)
                    {
                        var spaceMiqId = Convert.ToInt32(match.Groups[1].Value);
                        var space = listing.Spaces.FirstOrDefault(s => Convert.ToInt32(s.MIQID) == spaceMiqId);
                        if (space == null) continue;

                        Lazy<createAvailabilityUnifiedMutation> spaceAdd = null;
                        Lazy<updateAvailabilityMutation> spaceUpdate = null;

                        if (!(
                            spaceAdds.TryGetValue(spaceMiqId, out spaceAdd)
                            || spaceUpdates.TryGetValue(spaceMiqId, out spaceUpdate)
                        ))
                        {
                            var existingAvailability = 
                                existing.Availability.FirstOrDefault(a => 
                                    a.entity.id == spaceMiqId 
                                    || a.entity?.source_lineage?.FirstOrDefault(l => l?.source_system == "DataEntry")?.source_unique_id == space.ExternalID);

                            if (existingAvailability == null)
                            {
                                spaceAdd = new Lazy<createAvailabilityUnifiedMutation>(() =>
                                {
                                    var result = new createAvailabilityUnifiedMutation
                                    {
                                        availability = _listingAdapter.GetAvailabilityUnifiedBase(listing, space),
                                        request = BuildRequestDetailsUnified()
                                    };

                                    // Apply the known MIQ property id
                                    result.availability.property_id = propertyId.ToString();

                                    mutations.Add(result);
                                    requestIds[result] = result.request.request_id;
                                    return result;
                                });
                                spaceAdds[spaceMiqId] = spaceAdd;
                            }
                            else
                            {
                                spaceUpdate = new Lazy<updateAvailabilityMutation>(() =>
                                {
                                    var result = new updateAvailabilityMutation
                                    {
                                        availability_id = existingAvailability.entity.id,
                                        availability = new AvailabilityUpdate(),
                                        request = BuildRequestDetails()
                                    };
                                    mutations.Add(result);
                                    requestIds[result] = result.request.request_id;
                                    return result;
                                });

                                spaceUpdates[spaceMiqId] = spaceUpdate;
                            }
                        }

                        var subPath = delta.JsonPath.Substring(match.Index + match.Length);
                        if (subPath == "Name[0].Text")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.listing_notes = delta.NewValue;
                            else spaceUpdate.Value.availability.listing_notes = delta.NewValue;
                        }
                        else if (subPath == "SpaceType")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.ref_property_usage_type_desc = _listingAdapter.MapPropertyUsage(delta.NewValue) ?? "Unknown";
                            //else spaceUpdate.Value.availability.ref_property_usage_type_desc = _listingAdapter.MapPropertyUsage(delta.NewValue); //missing?
                        }
                        else if (subPath == "ListingType")
                        {
                            if (spaceAdd != null) {}
                            else spaceUpdate.Value.availability.ref_space_availability_status_desc = delta.NewValue;
                        }
                        else if (subPath == "Specifications.LeaseType")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.ref_lease_type_desc = _listingAdapter.MapLeaseType(delta.NewValue);
                            else spaceUpdate.Value.availability.ref_lease_type_desc = _listingAdapter.MapLeaseType(delta.NewValue);
                        }
                        else if (subPath == "Specifications.LeaseTerm")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.minimum_lease_term = _listingAdapter.MapLeaseTerm(delta.NewValue);
                            else spaceUpdate.Value.availability.minumum_lease_term = _listingAdapter.MapLeaseTerm(delta.NewValue);
                        }
                        else if (subPath == "Specifications.MaxPrice")
                        {
                            var specifications = space.GetListingData<Specifications>();
                            var normalizedLeaseTerm = _listingAdapter.NormalizeString(specifications.LeaseTerm);
                            if (normalizedLeaseTerm == "monthly")
                            {
                                if (spaceAdd != null) spaceAdd.Value.availability.asking_rental_rate_monthly = Convert.ToDecimal(delta.NewValue);
                                else spaceUpdate.Value.availability.asking_rental_rate_monthly = Convert.ToDecimal(delta.NewValue);
                            }
                            else if (normalizedLeaseTerm == "yearly" || normalizedLeaseTerm == "annually")
                            {
                                if (spaceAdd != null) spaceAdd.Value.availability.asking_rental_rate_yearly = Convert.ToDecimal(delta.NewValue);
                                else spaceUpdate.Value.availability.asking_rental_rate_yearly = Convert.ToDecimal(delta.NewValue);
                            }
                        }
                        else if (subPath == "Specifications.SalePrice")
                        {
                            var specifications = space.GetListingData<Specifications>();
                            if (spaceAdd != null) spaceAdd.Value.availability.asking_price_for_sale = Convert.ToDecimal(delta.NewValue);
                            else spaceUpdate.Value.availability.asking_price_for_sale = Convert.ToDecimal(delta.NewValue);
                        }
                        else if (subPath == "Specifications.CurrencyCode")
                        {
                            var listingType = listing.GetListingData<ListingType>()?.Value;

                            if (listingType == "sale" || listingType == "salelease")
                            {
                                if (spaceAdd != null) spaceAdd.Value.availability.asking_price_for_sale_uom_desc = delta.NewValue;
                                else spaceUpdate.Value.availability.asking_price_for_sale_uom_desc = delta.NewValue;
                            }

                            if (listingType == "lease" || listingType == "salelease")
                            {
                                var specifications = space.GetListingData<Specifications>();
                                var normalizedLeaseTerm = _listingAdapter.NormalizeString(specifications.LeaseTerm);
                                if (normalizedLeaseTerm == "monthly")
                                {
                                    if (spaceAdd != null) spaceAdd.Value.availability.asking_rental_rate_monthly_uom_desc = delta.NewValue;
                                    else spaceUpdate.Value.availability.asking_rental_rate_monthly_uom_desc = delta.NewValue;
                                }
                                else if (normalizedLeaseTerm == "yearly" || normalizedLeaseTerm == "annually")
                                {
                                    if (spaceAdd != null) spaceAdd.Value.availability.asking_rental_rate_yearly_uom_desc = delta.NewValue;
                                    else spaceUpdate.Value.availability.asking_rental_rate_yearly_uom_desc = delta.NewValue;
                                }
                            }
                        }
                        else if (subPath == "AvailableFrom")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.date_available = Convert.ToDateTime(delta.NewValue);
                            else spaceUpdate.Value.availability.date_available = Convert.ToDateTime(delta.NewValue);
                        }
                        else if (subPath == "SpaceDescription")
                        {
                            var desc = string.Join("\n\n", new string[] {
                                delta.NewValue,
                                listing.GetListingDataAllLanguages<BuildingDescription>().FirstOrDefault()?.Text,
                                listing.GetListingDataAllLanguages<LocationDescription>().FirstOrDefault()?.Text
                            }.Where(v => !string.IsNullOrWhiteSpace(v))).Trim();

                            if (spaceAdd != null) spaceAdd.Value.availability.listing_descr = desc;
                            else spaceUpdate.Value.availability.listing_descr = desc;
                        }
                        else if (subPath == "Specifications.TotalSpace")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.available_space = Convert.ToDecimal(delta.NewValue);
                            else spaceUpdate.Value.availability.available_space = Convert.ToDecimal(delta.NewValue);
                        }
                        else if (subPath == "Specifications.Measure")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.available_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                            else spaceUpdate.Value.availability.available_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='SuperArea'].Amount")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.total_area_of_space = Convert.ToDecimal(delta.NewValue);
                            else spaceUpdate.Value.availability.total_area_of_space = Convert.ToDecimal(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='SuperArea'].MeasureUnit")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.total_area_of_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                            else spaceUpdate.Value.availability.total_area_of_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='OfficeArea'].Amount")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.available_office_space = Convert.ToDecimal(delta.NewValue);
                            else spaceUpdate.Value.availability.available_office_space = Convert.ToDecimal(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='OfficeArea'].MeasureUnit")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.available_office_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                            else spaceUpdate.Value.availability.available_office_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='TotalContiguousSpace'].Amount")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.total_contiguous_area_of_space = Convert.ToDecimal(delta.NewValue);
                            else spaceUpdate.Value.availability.total_contiguous_area_of_space = Convert.ToDecimal(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='TotalContiguousSpace'].MeasureUnit")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.total_contiguous_area_of_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                            else spaceUpdate.Value.availability.total_contiguous_area_of_space_uom_desc = _listingAdapter.MapUnitOfMeasure(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='MinimumCeilingHeight'].Amount")
                        {
                            if (spaceAdd != null) spaceAdd.Value.availability.ceiling_height = Convert.ToDecimal(delta.NewValue);
                            else spaceUpdate.Value.availability.ceiling_height = Convert.ToDecimal(delta.NewValue);
                        }
                        else if (subPath == "PropertySizes[@.SizeKind=='MinimumCeilingHeight'].MeasureUnit")
                        {
                            // Missing?
                            //if (spaceAdd != null) spaceAdd.Value.availability.ceiling_height_uom_desc = MapUnitOfMeasure(delta.NewValue);
                            //else spaceUpdate.Value.availability.ceiling_height_uom_desc = MapUnitOfMeasure(delta.NewValue);
                        }
                    }
                }
            }

            foreach (var mutation in mutations)
            {
                var requestId = requestIds[mutation];
                _log.LogInformation("Generated request with id: {requestId}", requestId);
                var result = await _edpGraphQLService.RunQueryRaw(mutation);
                _log.LogInformation("Response for request with id: {requestId}, {response}", requestId, result);
            }

            var updatedSerialization = _jsonDeltaEvaluator.Apply(lastPublish, deltas);
            await _dataEntryRepository.UpdateListingSerialization(listing.ID, ListingSerializationType.LastPublish, updatedSerialization);
        }

        private RequestDetailsUnified BuildRequestDetailsUnified() => new RequestDetailsUnified
        {
            request_id = Guid.NewGuid().ToString(),
            source_system_name = _edpOptions.SourceSystemName,
            source_submitter_name = _edpOptions.SourceSubmitterName,
            user_role = _edpOptions.UserRole,
        };
        private RequestDetails BuildRequestDetails() => new RequestDetails
        {
            request_id = Guid.NewGuid().ToString(),
            source_system_name = _edpOptions.SourceSystemName,
            source_submitter_name = _edpOptions.SourceSubmitterName,
            user_role = _edpOptions.UserRole,
        };

        private void ThrowDisabledException()
        {
            throw new InvalidOperationException("IngestionService is not enabled.");
        }
    }
}
                
