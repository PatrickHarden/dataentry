using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Configuration;
using static dataentry.Utility.AliasType;

namespace dataentry.Services.Integration.StoreApi.Model
{
    public interface IStoreApiListingMapper
    {
        ListingViewModel Map(RegionViewModel region, string resourcesBaseUrl, PropertyListing storeApiListing);
        ListingViewModel Map(
            RegionViewModel region,
            string resourcesBaseUrl,
            PropertyListing storeApiListing,
            ListingViewModel dataEntryListing
        );
        ListingViewModel Map(StoreApiListingMapperContext context);
    }

    public class StoreApiListingMapper : IStoreApiListingMapper
    {
        private const AliasType StoreApi = AliasType.StoreApi; // get around issue with compiler thinking it's a namespace
        private readonly List<PropertyTypeEnum?> _flexRentList;

        public StoreApiListingMapper(IConfiguration configuration)
        {
            var flexRentList =
                configuration["StoreSettings:FlexRentList"]
                ?? throw new ArgumentNullException("StoreSettings:FlexRentList");
            _flexRentList = flexRentList
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToEnum<PropertyTypeEnum>())
                .ToList();
        }

        public ListingViewModel Map(RegionViewModel region, string resourcesBaseUrl, PropertyListing storeApiListing) =>
            Map(region, resourcesBaseUrl, storeApiListing, new ListingViewModel());

        public ListingViewModel Map(
            RegionViewModel region,
            string resourcesBaseUrl,
            PropertyListing storeApiListing,
            ListingViewModel dataEntryListing
        ) => Map(new StoreApiListingMapperContext(region, resourcesBaseUrl, storeApiListing, dataEntryListing));

        public ListingViewModel Map(StoreApiListingMapperContext context)
        {
            if (context.Region == null)
                throw new ArgumentNullException("region");
            if (context.ResourcesBaseUrl == null)
                throw new ArgumentNullException("resourcesBaseUrl");
            if (context.StoreApiListing == null)
                throw new ArgumentNullException("storeApiListing");

            if (context.DataEntryListing.Specifications == null)
                context.DataEntryListing.Specifications = new SpecificationsViewModel();

            if (
                GlobalizationUtility.TryGetCurrencyDataFromCultureCode(context.Region.CultureCode, out var currencyData)
            )
            {
                context.CurrencyCode = currencyData.Code;
            }

            MapPropertyType(context);
            MapPropertySubType(context);
            MapExternalId(context);
            MapMiqId(context);
            MapRegion(context);
            MapDimensionsUnit(context);
            MapListingType(context);
            MapAspects(context);
            MapAddress(context);
            MapPropertyRecordName(context);
            MapEnergyRatings(context);
            MapAvailability(context);
            MapBrokers(context);
            MapWebsite(context);
            MapOperator(context);
            MapVideoLink(context);
            MapWalkthrough(context);
            MapBrochures(context);
            MapPhotos(context);
            MapFloorplans(context);
            MapHighlights(context);
            MapMicroMarkets(context);
            MapCharges(context);
            MapSpaces(context);
            MapLeaseType(context);
            MapLeaseRateType(context);
            MapDescription(context);
            MapBedrooms(context);
            MapArea(context);
            MapSizes(context);
            MapHeadline(context);
            MapParking(context);
            // Why are there two in the store schema???
            MapPointsOfInterest(context);
            MapPointsOfInterests(context);
            MapTransportationTypes(context);
            MapUseClass(context);
            MapFloors(context);
            MapYearBuilt(context);
            MapSyndicationFlag(context);

            return context.DataEntryListing;
        }

        private void MapPropertyType(StoreApiListingMapperContext context)
        {
            context.PropertyType =
                context.StoreApiListing.CommonUsageType?.ToEnum<PropertyTypeEnum>(StoreApi) ?? PropertyTypeEnum.office;
            context.IsFlexRent = _flexRentList.Contains(context.PropertyType);
            context.DataEntryListing.PropertyType = context.PropertyType.ToAlias(ViewModel);
        }

        private void MapPropertySubType(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.PropertySubType = context.StoreApiListing.CommonPropertySubType
                ?.ToEnum<PropertySubTypeEnum>(StoreApi)
                ?.ToAlias(ViewModel);
        }

        private void MapExternalId(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.ExternalId =
                context.StoreApiListing.CommonPrimaryKey ?? context.DataEntryListing.ExternalId;
        }

        private void MapMiqId(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.MiqId =
                context.StoreApiListing.CommonSourceSystem?.CommonName == "EDP ID"
                    ? context.StoreApiListing.CommonSourceSystem.CommonId
                    : null;
        }

        private void MapRegion(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.RegionID = context.Region?.ID ?? Region.DefaultID.ToString();
        }

        private void MapDimensionsUnit(StoreApiListingMapperContext context)
        {
            // Determine most common UoM for specifications fields
            var storeMeasureUnit = context.StoreApiListing.CommonSizes
                ?.GroupBy(s => (s.CommonSizeKind.ToEnum<SizeKindEnum>(StoreApi)))
                .Where(group => ManagedSizes.ContainsKey(group.Key))
                .SelectMany(group => group)
                .Where(s => s.CommonDimensions != null)
                .SelectMany(s => s.CommonDimensions)
                .Where(d => !(d.CommonDimensionsUnits == null))
                .Mode(d => d.CommonDimensionsUnits)
                .ToEnum<DimensionsUnitsEnum>(StoreApi);

            if (storeMeasureUnit == null)
            {
                // Try CommonTotalSize
                storeMeasureUnit = context.StoreApiListing.CommonTotalSize
                    ?.Where(storeSize => storeSize.CommonUnits != null)
                    .Mode(storeSize => storeSize.CommonUnits)
                    .ToEnum<DimensionsUnitsEnum>(StoreApi);
            }

            if (storeMeasureUnit == null)
            {
                // Determine most common UoM for all sizes
                storeMeasureUnit = context.StoreApiListing.CommonSizes
                    ?.Where(s => s.CommonDimensions != null)
                    .SelectMany(s => s.CommonDimensions)
                    .Where(d => !(d.CommonDimensionsUnits == null))
                    .Mode(d => d.CommonDimensionsUnits)
                    .ToEnum<DimensionsUnitsEnum>(StoreApi);
            }

            if (storeMeasureUnit != null)
            {
                context.DimensionsUnit = storeMeasureUnit;
            }
        }

        private void MapListingType(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonAspects == null || !context.StoreApiListing.CommonAspects.Any())
                return;
            var isSale = context.StoreApiListing.CommonAspects.Contains(AspectsEnum.sale.ToAlias(StoreApi));
            var isLetting = context.StoreApiListing.CommonAspects.Contains(AspectsEnum.lease.ToAlias(StoreApi));
            var isInvestment = context.StoreApiListing.CommonAspects.Contains(AspectsEnum.investment.ToAlias(StoreApi));
            if (isSale)
            {
                if (isLetting)
                    context.ListingType = AspectsEnum.salelease;
                else if (isInvestment)
                    context.ListingType = AspectsEnum.investment;
                else
                    context.ListingType = AspectsEnum.sale;
            }
            else
            {
                context.ListingType = AspectsEnum.lease;
            }
            context.DataEntryListing.ListingType = context.ListingType.ToAlias(ViewModel);
        }

        private static readonly ICollection<string> ManagedAspects = new string[]
        {
            AspectsEnum.sale.ToAlias(StoreApi),
            AspectsEnum.lease.ToAlias(StoreApi),
            AspectsEnum.investment.ToAlias(StoreApi)
        };

        private void MapAspects(StoreApiListingMapperContext context)
        {
            if (context.DataEntryListing.Aspects == null)
                context.DataEntryListing.Aspects = new List<string>();
            if (context.StoreApiListing.CommonAspects == null || !context.StoreApiListing.CommonAspects.Any())
                return;

            var aspects = context.StoreApiListing.CommonAspects.Where(aspect => !ManagedAspects.Contains(aspect));
            context.DataEntryListing.Aspects = context.DataEntryListing.Aspects
                .Concat(aspects.Select(a => a.ToEnum<AspectsEnum>(StoreApi).ToAlias(ViewModel) ?? a))
                .Distinct()
                .ToList();
        }

        private void MapAddress(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.Lat = context.StoreApiListing.CommonCoordinate?.Lat ?? default;
            context.DataEntryListing.Lng = context.StoreApiListing.CommonCoordinate?.Lon ?? default;

            if (context.StoreApiListing.CommonActualAddress == null)
                return;

            context.DataEntryListing.Country = context.StoreApiListing.CommonActualAddress.CommonCountry;
            context.DataEntryListing.StateOrProvince = context.StoreApiListing.CommonActualAddress.CommonRegion;
            context.DataEntryListing.City = context.StoreApiListing.CommonActualAddress.CommonLocallity;
            context.DataEntryListing.PostalCode = context.StoreApiListing.CommonActualAddress.CommonPostCode;
            context.DataEntryListing.PropertyName = context.StoreApiListing.CommonActualAddress.CommonLine1;
            context.DataEntryListing.Street = context.StoreApiListing.CommonActualAddress.CommonLine2;
            context.DataEntryListing.Street2 = Join(
                ' ',
                context.StoreApiListing.CommonActualAddress?.CommonLine3,
                context.StoreApiListing.CommonActualAddress?.CommonLine4
            );

            if (!(context.StoreApiListing.CommonActualAddress.CommonPostalAddresses?.Any() ?? false))
                return;

            Func<Func<CommonPostalAddress, string>, string> fallback = (selector) =>
                context.StoreApiListing.CommonActualAddress.CommonPostalAddresses
                    .Select(selector)
                    .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

            if (string.IsNullOrWhiteSpace(context.DataEntryListing.StateOrProvince))
                context.DataEntryListing.StateOrProvince = fallback(a => a.CommonRegion);
            if (string.IsNullOrWhiteSpace(context.DataEntryListing.City))
                context.DataEntryListing.City = fallback(a => a.CommonLocallity);
            if (string.IsNullOrWhiteSpace(context.DataEntryListing.PropertyName))
                context.DataEntryListing.PropertyName = fallback(a => a.CommonLine1);
            if (string.IsNullOrWhiteSpace(context.DataEntryListing.Street))
                context.DataEntryListing.Street = fallback(a => a.CommonLine2);
            if (string.IsNullOrWhiteSpace(context.DataEntryListing.Street2))
                context.DataEntryListing.Street2 = fallback(a => Join(' ', a.CommonLine3, a.CommonLine4));
        }

        private static string Join(char separator, params string[] values) =>
            string.Join(separator, values.Where(s => !string.IsNullOrWhiteSpace(s)));

        private void MapPropertyRecordName(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.PropertyRecordName = string.IsNullOrWhiteSpace(context.DataEntryListing.ExternalId)
                ? string.IsNullOrWhiteSpace(context.DataEntryListing.PropertyName)
                    ? "Imported Property"
                    : $"(Imported) {context.DataEntryListing.PropertyName}"
                : string.IsNullOrWhiteSpace(context.DataEntryListing.PropertyName)
                    ? context.DataEntryListing.ExternalId
                    : $"({context.DataEntryListing.ExternalId}) {context.DataEntryListing.PropertyName}";
        }

        private void MapEnergyRatings(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.EnergyRating = context.StoreApiListing.CommonEnergyPerformanceData
                ?.CommonCertificateType?.ToEnum<EnergyRatingEnum>(StoreApi)
                .ToAlias(ViewModel);

            if (!(context.StoreApiListing.CommonEnergyPerformanceInformation?.Any() ?? false))
                return;

            context.DataEntryListing.EpcGraphs = (context.DataEntryListing.EpcGraphs ?? new List<MediaViewModel>())
                .Concat(
                    context.StoreApiListing.CommonEnergyPerformanceInformation.Select(
                        i => new MediaViewModel { Url = MapUrl(context, i.CommonUri), Active = true }
                    )
                )
                .DistinctBy(e => e.Url)
                .ToList();
        }

        private string MapUrl(StoreApiListingMapperContext context, string relativePath)
        {
            if (!relativePath.StartsWith("/resources/", true, System.Globalization.CultureInfo.InvariantCulture))
                return relativePath;
            var uri = new UriBuilder(context.ResourcesBaseUrl);
            uri.Path = relativePath;
            return uri.Uri.AbsoluteUri;
        }

        private void MapAvailability(StoreApiListingMapperContext context)
        {
            var availableFrom =
                context.StoreApiListing.CommonAvailability?.CommonAvailabilityDate
                ?? context.StoreApiListing.CommonAvailableFrom;

            if (availableFrom == null)
                return;

            if (DateTime.TryParse(availableFrom, out var parsed))
            {
                context.DataEntryListing.AvailableFrom = parsed;
            }
        }

        private void MapBrokers(StoreApiListingMapperContext context)
        {
            var groupAgents =
                context.StoreApiListing.CommonContactGroup?.CommonContacts ?? new List<CommonAgentElement>();

            var agents = context.StoreApiListing.CommonAgents ?? new List<CommonAgentElement>();

            var mappedContacts = groupAgents
                .Concat(agents)
                .Select(a =>
                {
                    var nameSplit =
                        a.CommonAgentName?.Split(
                            ' ',
                            2,
                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                        ) ?? new string[0];

                    return new ContactsViewModel
                    {
                        FirstName = nameSplit.Length > 0 ? nameSplit[0] : null,
                        LastName = nameSplit.Length > 1 ? nameSplit[1] : null,
                        Email = a.CommonEmailAddress,
                        Phone = a.CommonTelephoneNumber,
                        Avatar = a.CommonAvatar,
                        AdditionalFields = string.IsNullOrWhiteSpace(a.CommonLicenseNumber)
                            ? null
                            : new AdditionalFieldsViewModel { License = a.CommonLicenseNumber },
                        PreventOverwrite = true
                    };
                })
                .ToList();

            if (mappedContacts.Count == 0)
                return;

            var originalContacts = context.DataEntryListing.Contacts ?? new List<ContactsViewModel>();

            context.DataEntryListing.Contacts = originalContacts
                .Concat(mappedContacts)
                .DistinctBy(c => c.Email)
                .ToList();
        }

        private void MapWebsite(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.Website = context.StoreApiListing.CommonWebsite;
        }

        private void MapOperator(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.Operator = context.StoreApiListing.CommonBuildingOperator;
        }

        private void MapVideoLink(StoreApiListingMapperContext context)
        {
            if (NullOrEmpty(context.StoreApiListing.CommonVideoLinks))
                return;

            context.DataEntryListing.Video = MapVideoLink(context, context.StoreApiListing.CommonVideoLinks);
        }

        private string MapVideoLink(StoreApiListingMapperContext context, IEnumerable<CommonVideoLink> storeVideoLinks)
        {
            return storeVideoLinks?.Prefer(v => v.CommonCultureCode == context.Region?.CountryCode)?.CommonLink;
        }

        private void MapWalkthrough(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.WalkThrough = context.StoreApiListing.CommonWalkthrough;
        }

        private void MapBrochures(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.Brochures = MapBrochures(context, context.StoreApiListing.CommonBrochures);
        }

        private IEnumerable<MediaViewModel> MapBrochures(
            StoreApiListingMapperContext context,
            IEnumerable<CommonBrochureElement> storeBrochures
        )
        {
            return storeBrochures?.Select(
                b =>
                    new MediaViewModel
                    {
                        Active = true,
                        Url = MapUrl(context, b.CommonUri),
                        DisplayText = b.CommonBrochureName
                    }
            );
        }

        private void MapPhotos(StoreApiListingMapperContext context)
        {
            if (NullOrEmpty(context.StoreApiListing.CommonPhotos))
                return;
            if (context.DataEntryListing.Photos == null)
                context.DataEntryListing.Photos = new List<ImagesViewModel>();

            var mappedPhotos = context.StoreApiListing.CommonPhotos.Select(p => MapImage(context, p));

            if (mappedPhotos == null)
                return;

            context.DataEntryListing.Photos = context.DataEntryListing.Photos
                .Concat(mappedPhotos)
                .DistinctBy(b => MapUrl(context, b.Url))
                .ToList();

            var i = 0;
            foreach (var photo in context.DataEntryListing.Photos)
            {
                photo.Order = i++;
            }
        }

        private void MapFloorplans(StoreApiListingMapperContext context)
        {
            if (NullOrEmpty(context.StoreApiListing.CommonFloorPlans))
                return;
            if (context.DataEntryListing.Floorplans == null)
                context.DataEntryListing.Floorplans = new List<ImagesViewModel>();

            var mappedFloorplans = context.StoreApiListing.CommonFloorPlans.Select(f => MapImage(context, f));

            context.DataEntryListing.Floorplans = context.DataEntryListing.Floorplans
                .Concat(mappedFloorplans)
                .DistinctBy(b => MapUrl(context, b.Url))
                .ToList();

            var i = 0;
            foreach (var photo in context.DataEntryListing.Floorplans)
            {
                photo.Order = i++;
            }
        }

        private ImagesViewModel MapImage(StoreApiListingMapperContext context, CommonImageType storeImage)
        {
            return new ImagesViewModel
            {
                Active = true,
                Url = MapUrl(
                    context,
                    storeImage.CommonImageResources
                        ?.OrderByDescending(p => p.CommonImageWidth)
                        .Prefer(p => p.CommonBreakpoint == "original")
                        ?.CommonResourceUri
                ),
                DisplayText = storeImage.CommonImageCaption
            };
        }

        private void MapHighlights(StoreApiListingMapperContext context)
        {
            if (NullOrEmpty(context.StoreApiListing.CommonHighlights))
                return;

            var existingHighlights = context.DataEntryListing.Highlights ?? new List<HighlightViewModel>();

            var existingHighlightGroups = existingHighlights.GroupBy(h => h.Order);

            var i = existingHighlightGroups.Any() ? existingHighlightGroups.Max(g => g.Key) : 0;

            var mappedHighlightGroups = context.StoreApiListing.CommonHighlights
                .SelectMany(g =>
                {
                    i++;
                    return g.CommonHighlights.Select(
                        h =>
                            new HighlightViewModel
                            {
                                Text = h.CommonText,
                                CultureCode = h.CommonCultureCode,
                                Order = i
                            }
                    );
                })
                .ToList()
                .GroupBy(h => h.Order);

            context.DataEntryListing.Highlights = existingHighlightGroups
                .Concat(mappedHighlightGroups)
                .DistinctBy(
                    g => string.Join("||", g.OrderBy(h => h.CultureCode).Select(h => $"{h.CultureCode}|{h.Text}"))
                )
                .SelectMany(g => g)
                .ToList();
        }

        private void MapMicroMarkets(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonMicromarket == null)
                return;

            var mappedMicromarket = new MicroMarketsViewModel
            {
                Value = string.Join(
                    ':',
                    new[]
                    {
                        context.StoreApiListing.CommonMicromarket.CommonMicromarketName,
                        context.StoreApiListing.CommonMicromarket.CommonSubMarketName
                    }.Where(m => !string.IsNullOrWhiteSpace(m))
                )
            };

            if (string.IsNullOrWhiteSpace(mappedMicromarket.Value))
                return;

            context.DataEntryListing.MicroMarkets = new List<MicroMarketsViewModel> { mappedMicromarket };
        }

        private void MapCharges(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonCharges == null)
                return;

            var dataEntryCharges =
                context.DataEntryListing.ChargesAndModifiers?.ToList() ?? new List<ChargesAndModifiersViewModel>();

            MapCharges(
                context.StoreApiListing.CommonCharges,
                dataEntryCharges,
                context.DataEntryListing.Specifications
            );

            context.DataEntryListing.ChargesAndModifiers = dataEntryCharges.DistinctBy(c => c.ChargeType);
        }

        private void MapCharges(
            IEnumerable<CommonCharge> storeCharges,
            List<ChargesAndModifiersViewModel> dataEntryCharges,
            SpecificationsViewModel specifications
        )
        {
            if (storeCharges == null)
                return;
            foreach (var storeCharge in storeCharges)
            {
                var amount = Convert.ToDecimal(storeCharge.CommonAmount);
                var measure = storeCharge.CommonPerUnit.ToEnum<PerUnitTypeEnum>(StoreApi);
                var chargeKind = storeCharge.CommonChargeKind.ToEnum<ChargeKindEnum>(StoreApi);
                if (
                    chargeKind == ChargeKindEnum.SalePrice
                    || chargeKind == ChargeKindEnum.Rent
                    || chargeKind == ChargeKindEnum.FlexRent
                )
                {
                    var chargeModifier = storeCharge.CommonChargeModifer.ToEnum<ChargeModifiersEnum>(StoreApi);
                    if (amount != default)
                    {
                        if (chargeKind == ChargeKindEnum.Rent || chargeKind == ChargeKindEnum.FlexRent)
                        {
                            if (chargeModifier == ChargeModifiersEnum.From)
                            {
                                specifications.MinPrice = Convert.ToDecimal(storeCharge.CommonAmount);
                            }
                            else
                            {
                                specifications.MaxPrice = Convert.ToDecimal(storeCharge.CommonAmount);
                            }
                        }
                        else
                        {
                            specifications.SalePrice = Convert.ToDecimal(storeCharge.CommonAmount);
                        }

                        if (measure != null)
                            specifications.Measure = measure.ToAlias(ViewModel);
                        var leaseTerm = storeCharge.CommonInterval.ToEnum<LeaseTermEnum>(StoreApi).ToAlias(ViewModel);
                        if (leaseTerm != null)
                            specifications.LeaseTerm = leaseTerm;
                    }

                    var showPriceWithUoM = storeCharge.CommonPerUnit != null;
                    var currencyCode = storeCharge.CommonCurrencyCode;
                    var taxModifier = storeCharge.CommonTaxModifer.ToEnum<TaxModifierEnum>(StoreApi).ToAlias(ViewModel);
                    var contactBroker = storeCharge.CommonOnApplication;
                    if (showPriceWithUoM)
                        specifications.ShowPriceWithUoM = true;
                    if (currencyCode != null)
                        specifications.CurrencyCode = currencyCode;
                    if (taxModifier != null)
                        specifications.TaxModifer = taxModifier;
                    if (contactBroker)
                        specifications.ContactBrokerForPrice = true;
                }
                else
                {
                    dataEntryCharges.Add(
                        new ChargesAndModifiersViewModel
                        {
                            ChargeType = chargeKind.ToAlias(ViewModel),
                            CurrencyCode = storeCharge.CommonCurrencyCode,
                            ChargeModifier = storeCharge.CommonChargeModifer,
                            Amount = amount,
                            Term = storeCharge.CommonInterval,
                            PerUnitType = measure.ToAlias(ViewModel),
                            Year = storeCharge.CommonYear
                        }
                    );
                }
            }
        }

        private void MapSpaces(StoreApiListingMapperContext context)
        {
            if (NullOrEmpty(context.StoreApiListing.CommonFloorsAndUnits))
                return;

            var existingSpaces = context.DataEntryListing.Spaces?.ToList() ?? new List<SpacesViewModel>();
            var existingCharges =
                context.DataEntryListing.ChargesAndModifiers?.ToList() ?? new List<ChargesAndModifiersViewModel>();

            var dataEntrySpaceCharges = new List<ChargesAndModifiersViewModel>();
            var storeSpaces = context.StoreApiListing.CommonFloorsAndUnits.Select(storeSpace =>
            {
                var dataEntrySpace = new SpacesViewModel { Specifications = new SpecificationsViewModel() };

                var area = storeSpace.CommonAreas?.FirstOrDefault();
                MapArea(dataEntrySpace.Specifications, area);

                dataEntrySpace.Specifications.LeaseType = storeSpace.CommonLeaseTypes
                    .ToEnum<LeaseTypesEnum>(StoreApi)
                    .ToAlias(ViewModel);
                MapCharges(storeSpace.CommonCharges, dataEntrySpaceCharges, dataEntrySpace.Specifications);
                dataEntrySpace.SpaceSizes = storeSpace.CommonSizes
                    ?.Select(storeSize =>
                    {
                        var dimension = storeSize.CommonDimensions.FirstOrDefault();
                        return new PropertySizesViewModel
                        {
                            Amount = dimension?.CommonAmount ?? 0,
                            MeasureUnit = dimension?.CommonDimensionsUnits
                                .ToEnum<DimensionsUnitsEnum>(StoreApi)
                                .ToAlias(ViewModel),
                            SizeKind = storeSize.CommonSizeKind.ToEnum<SizeKindEnum>(StoreApi).ToAlias(ViewModel)
                        };
                    })
                    .ToList();

                if (context.IsFlexRent)
                {
                    var usageType = storeSpace.CommonUnitUse.ToEnum<UseTypeEnum>(StoreApi);
                    dataEntrySpace.Name = new List<TextTypeViewModel>
                    {
                        new TextTypeViewModel
                        {
                            CultureCode = context.Region.CultureCode,
                            Text = usageType.ToAlias(ViewModel)
                        }
                    };
                }
                else
                {
                    dataEntrySpace.Name = storeSpace.CommonSubdivisionName
                        .Select(
                            storeName =>
                                new TextTypeViewModel
                                {
                                    CultureCode = storeName.CommonCultureCode,
                                    Text = storeName.CommonText
                                }
                        )
                        .ToList();
                }

                dataEntrySpace.Status = storeSpace.CommonUnitStatus.ToEnum<UnitStatus>(StoreApi).ToAlias(ViewModel);
                if (
                    DateTime.TryParse(storeSpace.CommonAvailableFrom, out var availableFrom)
                    || (
                        storeSpace.CommonAvailability?.CommonAvailabilityKind == "AvailableFromKnownDate"
                        && DateTime.TryParse(storeSpace.CommonAvailability.CommonAvailabilityDate, out availableFrom)
                    )
                )
                {
                    dataEntrySpace.AvailableFrom = availableFrom;
                }

                dataEntrySpace.SpaceDescription = storeSpace.CommonSpaceDescription?.Select(MapText).ToList();

                dataEntrySpace.Brochures = MapBrochures(context, storeSpace.CommonBrochures);
                dataEntrySpace.Photos = storeSpace.CommonPhotos?.Select(p => MapImage(context, p)).ToList();
                dataEntrySpace.Floorplans = storeSpace.CommonFloorPlans?.Select(f => MapImage(context, f)).ToList();
                dataEntrySpace.Video = MapVideoLink(context, storeSpace.CommonVideoLinks);
                dataEntrySpace.WalkThrough = storeSpace.CommonWalkthrough;
                return dataEntrySpace;
            });

            context.DataEntryListing.Spaces = existingSpaces.Concat(storeSpaces).ToList();
            context.DataEntryListing.ChargesAndModifiers = existingCharges
                .Concat(dataEntrySpaceCharges)
                .DistinctBy(c => c.ChargeType)
                .ToList();
        }

        private void MapArea(StoreApiListingMapperContext context) =>
            MapArea(
                context.DataEntryListing.Specifications,
                context.StoreApiListing.CommonTotalSize?.Prefer(
                    storeSize => storeSize.CommonUnits?.ToEnum<DimensionsUnitsEnum>(StoreApi) == context.DimensionsUnit
                )
            );

        private void MapArea(SpecificationsViewModel specifications, CommonLandSizeElement storeArea)
        {
            if (storeArea != null)
            {
                if (storeArea.CommonUnits != null)
                    specifications.Measure = storeArea.CommonUnits
                        .ToEnum<DimensionsUnitsEnum>(StoreApi)
                        .ToAlias(ViewModel);
                
                if (storeArea.CommonArea > 0)
                    specifications.TotalSpace = storeArea.CommonArea;
                if (storeArea.CommonMinArea != null && storeArea.CommonMinArea > 0)
                    specifications.MinSpace = storeArea.CommonMinArea;
                if (storeArea.CommonMaxArea != null && storeArea.CommonMaxArea > 0)
                    specifications.MaxSpace = storeArea.CommonMaxArea;
            }
        }

        private void MapLeaseType(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonLeaseTypes == null)
                return;

            context.DataEntryListing.Specifications.LeaseType = context.StoreApiListing.CommonLeaseTypes
                .Select(l => l.ToEnum<LeaseTypesEnum>(StoreApi))
                .FirstOrDefault(l => l != null)
                ?.ToAlias(ViewModel);
        }

        private void MapLeaseRateType(StoreApiListingMapperContext context)
        {
            var storeRateType = context.StoreApiListing.CommonLeaseRateType;
            if (storeRateType == null)
                return;
            var rateType = storeRateType.ToEnum<LeaseRateTypesEnum>(StoreApi);
            // TODO: I set up an enum based on our values in DataEntry, but it seems to overlap with LeaseTypeEnum.
            // Unsure about business logic so for now I'm just letting unknown values pass through
            context.DataEntryListing.Specifications.LeaseRateType = rateType?.ToAlias(ViewModel) ?? storeRateType;
        }

        private void MapDescription(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.BuildingDescription = context.StoreApiListing.CommonLongDescription
                ?.Select(MapText)
                .ToList();

            context.DataEntryListing.LocationDescription = context.StoreApiListing.CommonLocationDescription
                ?.Select(MapText)
                .ToList();
        }

        private void MapBedrooms(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.Specifications.Bedrooms = context.StoreApiListing.CommonNumberOfBedrooms;
        }

        private static Dictionary<SizeKindEnum?, Action<SpecificationsViewModel, decimal?>> ManagedSizes =
            new Dictionary<SizeKindEnum?, Action<SpecificationsViewModel, decimal?>>
            {
                { SizeKindEnum.MinimumSize, (s, v) => s.MinSpace = v },
                { SizeKindEnum.MaximumSize, (s, v) => s.MaxSpace = v },
                { SizeKindEnum.TotalSize, (s, v) => s.TotalSpace = v }
            };

        private void MapSizes(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonSizes == null)
                return;
            var dataEntrySizes = new List<PropertySizesViewModel>();
            // Group so we only have to parse once
            var storeSizesBySizeKind = context.StoreApiListing.CommonSizes
                .GroupBy(s => (s.CommonSizeKind.ToEnum<SizeKindEnum>(StoreApi)))
                .ToList();
            // Determine most common UoM for specifications fields
            var storeMeasureUnit = storeSizesBySizeKind
                .Where(group => ManagedSizes.ContainsKey(group.Key))
                .SelectMany(group => group)
                .Where(s => s.CommonDimensions != null)
                .SelectMany(s => s.CommonDimensions)
                .Where(d => !(d.CommonDimensionsUnits == null))
                .Mode(d => d.CommonDimensionsUnits);

            var measureUnit = storeMeasureUnit.ToEnum<DimensionsUnitsEnum>(StoreApi);
            if (measureUnit != null)
                context.DataEntryListing.Specifications.Measure = measureUnit.ToAlias(ViewModel);
            foreach (var group in storeSizesBySizeKind)
            {
                var sizeKind = group.Key;
                if (ManagedSizes.TryGetValue(sizeKind, out var action))
                {
                    var storeSize = group
                        .FirstOrDefault()
                        ?.CommonDimensions?.FirstOrDefault(d => d.CommonDimensionsUnits == storeMeasureUnit)
                        ?.CommonAmount;
                    if (storeSize != null && storeSize > 0)
                        action(context.DataEntryListing.Specifications, Convert.ToDecimal(storeSize));
                }
                else
                {
                    foreach (var storeSize in group)
                    {
                        var dimension = storeSize.CommonDimensions?.Prefer(
                            dimension => dimension.CommonDimensionsUnits == storeMeasureUnit
                        );
                        dataEntrySizes.Add(
                            new PropertySizesViewModel
                            {
                                SizeKind = storeSize.CommonSizeKind, // TODO: Investigate why alias isn't being used
                                MeasureUnit =
                                    dimension
                                        ?.CommonDimensionsUnits?.ToEnum<DimensionsUnitsEnum>(StoreApi)
                                        ?.ToAlias(ViewModel) ?? dimension?.CommonDimensionsUnits,
                                Amount = dimension?.CommonAmount ?? 0
                            }
                        );
                    }
                }
            }
        }

        private void MapHeadline(StoreApiListingMapperContext context)
        {
            context.DataEntryListing.Headline = context.StoreApiListing.CommonStrapline?.Select(MapText);
        }

        private TextTypeViewModel MapText(CommonCommentElement storeText) =>
            new TextTypeViewModel { CultureCode = storeText.CommonCultureCode, Text = storeText.CommonText };

        private void MapParking(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonParking == null)
                return;

            context.DataEntryListing.Parkings = new ParkingsViewModel
            {
                Ratio = context.StoreApiListing.CommonParking.CommonRatio,
                RatioPer = context.StoreApiListing.CommonParking.CommonRatioPer,
                RatioPerUnit = context.StoreApiListing.CommonParking.CommonRatioPerUnit,
                ParkingDetails = context.StoreApiListing.CommonParking.CommonParkingDetails?.Select(storeParking =>
                {
                    var storeCharge = storeParking.CommonParkingCharge?.Prefer(
                        storeCharge => storeCharge.CommonCurrencyCode == context.CurrencyCode
                    );
                    return new ParkingDetailViewModel
                    {
                        ParkingType = storeParking.CommonParkingType,
                        ParkingSpace = storeParking.CommonParkingSpace,
                        Interval = storeCharge?.CommonInterval,
                        Amount = storeCharge?.CommonAmount,
                        CurrencyCode = storeCharge?.CommonCurrencyCode
                    };
                })
            };
        }

        private void MapPointsOfInterest(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonPointsOfInterest == null)
                return;
            var existingPoIs =
                context.DataEntryListing.PointsOfInterests?.ToList() ?? new List<PointsOfInterestsViewModel>();
            var storePoIs = context.StoreApiListing.CommonPointsOfInterest.Select(storePoi =>
            {
                var storeDistance = storePoi.CommonDistances.FirstOrDefault();
                return new PointsOfInterestsViewModel
                {
                    InterestKind = storePoi.CommonInterestKind,
                    Places = new List<PlacesViewModel>
                    {
                        new PlacesViewModel
                        {
                            Name = storePoi.CommonNamesOfPlaces
                                ?.Prefer(n => n.CommonCultureCode == context.Region.CultureCode)
                                .CommonText,
                            Distances = Convert.ToDecimal(storeDistance?.CommonAmount),
                            DistanceUnits = storeDistance?.CommonDistanceUnits
                        }
                    }
                };
            }).Where(i => i.InterestKind != null);
            context.DataEntryListing.PointsOfInterests = MergePointsOfInterestsViewModels(existingPoIs, storePoIs)
                .ToList();
        }

        private static IEnumerable<PointsOfInterestsViewModel> MergePointsOfInterestsViewModels(
            IEnumerable<PointsOfInterestsViewModel> firstPoIs,
            IEnumerable<PointsOfInterestsViewModel> secondPoIs
        )
        {
            return firstPoIs
                .Concat(secondPoIs)
                .GroupBy(poi => poi.InterestKind)
                .Select(
                    poiGroup =>
                        new PointsOfInterestsViewModel
                        {
                            InterestKind = poiGroup.Key,
                            Places = poiGroup.SelectMany(poiGroup => poiGroup.Places).DistinctBy(place => place.Name)
                        }
                );
        }

        private void MapPointsOfInterests(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonPointsOfInterests == null)
                return;
            var existingPoIs =
                context.DataEntryListing.PointsOfInterests?.ToList() ?? new List<PointsOfInterestsViewModel>();
            var storePois = context.StoreApiListing.CommonPointsOfInterests.Select(
                storePoi =>
                    new PointsOfInterestsViewModel
                    {
                        InterestKind = storePoi.CommonInterestKind,
                        Places = storePoi.CommonPlaces.Select(storePlace =>
                        {
                            var storeDistance = storePlace.CommonDistances.Prefer(
                                storeDistance =>
                                    storeDistance.CommonAmount != 0
                                    && !string.IsNullOrWhiteSpace(storeDistance.CommonDistanceUnits)
                            );

                            var storeDuration = storePlace.CommonDurations?.Prefer(
                                storeDuration =>
                                    storeDuration.CommonAmount != 0
                                    && !string.IsNullOrWhiteSpace(storeDuration.CommonDistanceUnits)
                            );

                            return new PlacesViewModel
                            {
                                Name = storePlace.CommonName
                                    ?.Prefer(storeName => storeName.CommonCultureCode == context.Region.CultureCode)
                                    ?.CommonText,
                                Type = storePlace.CommonType
                                    ?.Prefer(storeType => storeType.CommonCultureCode == context.Region.CultureCode)
                                    ?.CommonText,
                                Distances = Convert.ToDecimal(storeDistance?.CommonAmount),
                                DistanceUnits = storeDistance?.CommonDistanceUnits,
                                Duration = Convert.ToDecimal(storeDuration?.CommonAmount),
                                TravelMode = storeDuration?.CommonTravelMode
                            };
                        })
                    }
            ).Where(i => i.InterestKind != null);
            context.DataEntryListing.PointsOfInterests = MergePointsOfInterestsViewModels(existingPoIs, storePois)
                .ToList();
        }

        private void MapTransportationTypes(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonTransportationTypes == null)
                return;
            context.DataEntryListing.TransportationTypes = context.StoreApiListing.CommonTransportationTypes.Select(
                storeTransportation =>
                    new TransportationTypesViewModel
                    {
                        Type = storeTransportation.CommonType.ToEnum<TransportationTypesEnum>(StoreApi).ToAlias(),
                        Places = storeTransportation.CommonPlaces?.Select(storePlace =>
                        {
                            var storeDistance = storePlace.CommonDistances.Prefer(
                                storeDistance =>
                                    storeDistance.CommonAmount > 0
                                    && !string.IsNullOrWhiteSpace(storeDistance.CommonDistanceUnits)
                            );

                            var storeDuration = storePlace.CommonDurations?.Prefer(
                                storeDuration =>
                                    storeDuration.CommonAmount != 0
                                    && !string.IsNullOrWhiteSpace(storeDuration.CommonDistanceUnits)
                            );

                            return new PlacesViewModel
                            {
                                Name = storePlace.CommonName
                                    ?.Prefer(storeName => storeName.CommonCultureCode == context.Region.CultureCode)
                                    ?.CommonText,
                                Distances = Convert.ToDecimal(storeDistance?.CommonAmount),
                                DistanceUnits = storeDistance?.CommonDistanceUnits,
                                Duration = Convert.ToDecimal(storeDuration?.CommonAmount),
                                TravelMode = storeDuration?.CommonTravelMode
                            };
                        })
                    }
            ).Where(t => t.Type != null).ToList();
        }

        private void MapUseClass(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.UnitedKingdomUseClass == null)
                return;

            context.DataEntryListing.PropertyUseClass = context.StoreApiListing.UnitedKingdomUseClass.FirstOrDefault();
        }

        private void MapFloors(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonNumberOfStoreys == null)
                return;

            context.DataEntryListing.Floors = context.StoreApiListing.CommonNumberOfStoreys;
        }

        private void MapYearBuilt(StoreApiListingMapperContext context)
        {
            if (context.StoreApiListing.CommonYearBuilt == null)
                return;

            context.DataEntryListing.YearBuilt = context.StoreApiListing.CommonYearBuilt;
        }

        private void MapSyndicationFlag(StoreApiListingMapperContext context)
        {
            if (!context.StoreApiListing.CommonPublishExternallyExternal)
                return;

            // Don't know how to map this
        }

        private static bool NullOrEmpty<T>(IEnumerable<T> value)
        {
            return value == null || !value.Any();
        }
    }
}
