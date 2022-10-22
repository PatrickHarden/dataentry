using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Services.Business.Configs;
using dataentry.Services.Integration.Edp.Consumption;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Options;

namespace dataentry.Services.Integration.Edp
{
    public class PropertyMapper : IPropertyMapper
    {
        private readonly IConfigService _configService;
        private readonly IOptions<PropertyMapperOptions> _options;
        private List<ImagesViewModel> _photos;
        private List<ImagesViewModel> _floorPlans;
        private List<MediaViewModel> _brochures;
        private IEnumerable<PropertyAmenity> _amenities;

        public PropertyMapper(IConfigService configService, IOptions<PropertyMapperOptions> options)
        {
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public PropertySearchResultViewModel Map(PropertyResult property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var propertyViewModel = new PropertySearchResultViewModel();
            propertyViewModel.Id = property.entity.id;
            propertyViewModel.Name = property.property_name;
            propertyViewModel.Street1 = property.postal_address.full_street_name;
            propertyViewModel.Street2 = property.postal_address.street2;
            propertyViewModel.StateProvince = property.postal_address.state_province;
            propertyViewModel.PostalCode = property.postal_address.postal_code;
            propertyViewModel.City = property.postal_address.city;
            propertyViewModel.Country = property.postal_address.country;
            propertyViewModel.PropertyType = GetPropertyType(GetPrimaryUsage(property.usages));
            return propertyViewModel;
        }

        public ListingViewModel ConvertToListing(PropertyWithAvailability property, Region region)
        {
            if (property.PropertyDetail == null) return null;
            var listingViewModel = new ListingViewModel();
            var primparyUsage = GetPrimaryUsage(property.PropertyDetail?.usages);
            listingViewModel.MiqId = property.PropertyDetail?.entity?.id.ToString();
            listingViewModel.PropertyName = property.PropertyDetail?.property_name;
            listingViewModel.PropertyRecordName = property.PropertyDetail?.property_name;
            listingViewModel.RegionID = region.ID.ToString();
            listingViewModel.PropertyType = GetPropertyType(primparyUsage);
            listingViewModel.BuildingDescription = MapNoteDescription(property.PropertyDetail.property_note, region, NoteTypesEnum.PropertyDescription.ToAlias(AliasType.MIQ));
            listingViewModel.LocationDescription = MapNoteDescription(property.PropertyDetail.property_note, region, NoteTypesEnum.LocationDescription.ToAlias(AliasType.MIQ));
            listingViewModel.Street = property.PropertyDetail.postal_address?.full_street_name;
            listingViewModel.Street2 = property.PropertyDetail.postal_address?.street2;
            listingViewModel.City = property.PropertyDetail.postal_address?.city;
            listingViewModel.PostalCode = property.PropertyDetail.postal_address?.postal_code;
            listingViewModel.StateOrProvince = MapProvinceState(property.PropertyDetail.postal_address?.state_province);
            listingViewModel.Country = MapCountry(property.PropertyDetail.postal_address?.country);
            listingViewModel.Lat = property.PropertyDetail.postal_address?.latitude ?? 0;
            listingViewModel.Lng = property.PropertyDetail.postal_address?.longitude ?? 0;
            listingViewModel.YearBuilt = property.PropertyDetail.year_property_built;
            listingViewModel.Website = property.PropertyDetail.digital?.Where(x => x.digital_type_desc == "Web URL").FirstOrDefault()?.digital_address;
            listingViewModel.Video = property.PropertyDetail.digital?.Where(x => x.digital_type_desc == "YouTube").FirstOrDefault()?.digital_address;
            listingViewModel.WalkThrough = property.PropertyDetail.digital?.Where(x => x.digital_type_desc == "CBRE Plans").FirstOrDefault()?.digital_address;
            listingViewModel.Specifications = MapSpecification(property.PropertyDetail);
            listingViewModel.Status = MapStatus(property.PropertyDetail.ref_property_status_desc);
            listingViewModel.AvailableFrom = primparyUsage?.date_property_available;
            listingViewModel.ExternalRatings = MapExternalRatings(property, region.CountryCode);
            listingViewModel.AlternatePostalAddresses = MapAlternatePostalAddresses(property);

            _amenities = GetUsageAmenities(property, region);
            listingViewModel.Highlights = MapHighlights(property, region);
            listingViewModel.Aspects = MapAspects(property);
            listingViewModel.ChargesAndModifiers = MapChargesAndModifiers(property);

            var propertySizes = ConvertToPropertySizes(property?.PropertyDetail.property_measurement)?.ToList() ?? new List<PropertySizesViewModel>();
            if (property.PropertyDetail?.total_land_area != null)
            {
                propertySizes.Add(new PropertySizesViewModel()
                {
                    SizeKind = SizeKindEnum.LandSize.ToString(),
                    Amount = Convert.ToDouble(property.PropertyDetail?.total_land_area),
                    MeasureUnit = MapSizesAndMeasurements(property.PropertyDetail?.total_land_area_uom_desc)
                });
            }
            listingViewModel.PropertySizes = propertySizes;

            var publishedMedia = property.PropertyDetail?.property_media_information?
                .Where(p => p.publish_image_f == true) ?? new List<PropertyMediaInformation>();

            _floorPlans = publishedMedia
                .Where(p => p.media_content_type_desc == "Floor Plan")
                .Select(MapImage)?.ToList() ?? new List<ImagesViewModel>();

            _photos = publishedMedia
                .Where(p => p.media_type_desc == "Image" && !_floorPlans.Any(i => i.Url == p.media_path))
                .Select(MapImage)?.ToList() ?? new List<ImagesViewModel>();

            _brochures = publishedMedia
                .Where(p => p.media_type_desc == "Document" && !_floorPlans.Any(i => i.Url == p.media_path))
                .Select(MapMedia)
                .ToList() ?? new List<MediaViewModel>();

            listingViewModel.Spaces = ConvertToSpaces(property.Availability, region);
            listingViewModel.Contacts = ConvertToBrokers(property.PropertyDetail?.usages);

            var spaceAttachments = property.Availability?.SelectMany(x => x.property?.property_media_information)?.Select(x => x.media_path) ?? new List<string>();
            listingViewModel.Photos = _photos?.Where(x => !spaceAttachments.Contains(x.Url));
            listingViewModel.Floorplans = _floorPlans?.Where(x => !spaceAttachments.Contains(x.Url));
            listingViewModel.Brochures = _brochures?.Where(x => !spaceAttachments.Contains(x.Url));

            listingViewModel.EpcGraphs = property.PropertyDetail.property_media_information?
                .Where(p => p.media_type_desc == "Epc Graph")
                .Select(MapMedia)
                .ToList();

            if (!(listingViewModel.EpcGraphs?.Any() ?? false))
            {
                listingViewModel.EpcGraphs = null;
            }

            // Get listing source and id from availability level
            ListingIds availabilityListingIds = property.Availability?.FirstOrDefault(a => a.listing_ids?.Count() > 0)?.listing_ids?.FirstOrDefault();
            string availabilityListingId = availabilityListingIds?.listing_id;
            string availabilityListingSource = availabilityListingIds?.listing_source;
            if (!string.IsNullOrWhiteSpace(availabilityListingId) && !string.IsNullOrWhiteSpace(availabilityListingSource))
            {
                var format = region.RegionalIDFormats?.FirstOrDefault(r => r.SourceSystemName?.Equals(availabilityListingSource, StringComparison.OrdinalIgnoreCase) ?? false);
                if (format != null)
                {
                    listingViewModel.ExternalId = string.Format(format.FormatString, availabilityListingId);
                }
            }

            var isSale = property.Availability?.Any(a => a.ref_availability_type_desc?.Contains("sale", StringComparison.OrdinalIgnoreCase) ?? false) ?? false;
            var isLease = (property.Availability?.Any(a => a.ref_availability_type_desc?.Contains("lease", StringComparison.OrdinalIgnoreCase) ?? false) ?? false) || listingViewModel.Specifications.CurrencyCode != null;
            listingViewModel.ListingType = isSale ?
                isLease ? "salelease" : "sale" :
                isLease ? "lease" : null;

            return listingViewModel;
        }

        private IList<ExternalRatingsViewModel> MapExternalRatings(PropertyWithAvailability property, string countryCode)
        {
            if (property?.PropertyDetail?.property_external_ratings == null) return null;
            Dictionary<string, string> epcRatings = EnergyRatingDictionary.GetEPCRatings(countryCode);

            List<ExternalRatingsViewModel> externalRatings = new List<ExternalRatingsViewModel>();
            foreach (var rating in property?.PropertyDetail?.property_external_ratings)
            {
                string energyRatingEnum = string.Empty;

                if (epcRatings != null && countryCode == "IT" && rating.green_building_rating_type_desc == "EPC")
                {
                    epcRatings.TryGetValue(rating.green_building_cert_level_desc, out var result);
                    energyRatingEnum = result;
                }
                else if (epcRatings != null && countryCode == "DK" && rating.green_building_rating_type_desc == "EPC")
                {
                    epcRatings.TryGetValue(rating.green_building_cert_level_desc, out var result);
                    energyRatingEnum = result;
                }
                else if (rating.green_building_rating_type_desc != "EPC")
                    energyRatingEnum = rating.green_building_cert_level_desc.ToEnum<EnergyRatingEnum>(AliasType.MIQ)?.ToString();

                var ratingsViewModel = new ExternalRatingsViewModel
                {
                    RatingType = rating.green_building_rating_type_desc == "Well Rating" ? "WELL" : rating.green_building_rating_type_desc,
                    RatingLevel = !string.IsNullOrEmpty(energyRatingEnum) ? energyRatingEnum.ToString() : rating.green_building_cert_level_desc
                };
                externalRatings.Add(ratingsViewModel);
            }
            return externalRatings?.Count() > 0 ? externalRatings : null;
        }

        private IList<AlternatePostalAddressViewModel> MapAlternatePostalAddresses(PropertyWithAvailability property)
        {
            if (property?.PropertyDetail?.alternate_postal_addresses == null) return null;

            IList<AlternatePostalAddressViewModel> alternateAddresses = new List<AlternatePostalAddressViewModel>();
            foreach (var address in property?.PropertyDetail?.alternate_postal_addresses)
            {
                AlternatePostalAddressViewModel addressViewModel = new AlternatePostalAddressViewModel
                {
                    Street = address.full_street_name,
                    Street2 = address.street2,
                    City = address.city,
                    PostalCode = address.postal_code,
                    StateOrProvince = MapProvinceState(address.state_province),
                    Country = MapCountry(address.country),
                    Lat = address.latitude ?? 0,
                    Lng = address.longitude ?? 0
                };
                alternateAddresses.Add(addressViewModel);
            }
            return alternateAddresses?.Count() > 0 ? alternateAddresses : null;
        }
        private IEnumerable<string> MapAspects(PropertyWithAvailability property)
        {
            if (_amenities == null) return new List<string>();

            return _amenities
                .Select(a => 
                    a.property_amenity_type_desc.ToEnum<AspectsEnum>(AliasType.MIQ) 
                    ?? a.amenity_subtype_desc.ToEnum<AspectsEnum>(AliasType.MIQ)
                    ?? $"{a.property_amenity_type_desc}|{a.amenity_subtype_desc}".ToEnum<AspectsEnum>(AliasType.MIQ))
                .Where(a => a != null)
                .Select(a => a.ToAlias(AliasType.StoreApi))
                .Distinct();
        }

        private IEnumerable<ChargesAndModifiersViewModel> MapChargesAndModifiers(PropertyWithAvailability property)
        {
            if (property?.PropertyDetail?.usages == null) return null;

            List<ChargesAndModifiersViewModel> charges = new List<ChargesAndModifiersViewModel>();
            foreach (var usage in property?.PropertyDetail?.usages)
            {
                if (usage.maintenence_cost != null)
                {
                    var charge = new ChargesAndModifiersViewModel
                    {
                        ChargeType = ChargeKindEnum.ServiceCharge.ToString(),
                        Amount = Convert.ToDecimal(usage.maintenence_cost),
                        CurrencyCode = MapCurrencyCode(usage.maintenence_cost_uom_desc)
                    };
                    charges.Add(charge);
                }
                if (usage.tax_expense_amount != null)
                {
                    var charge = new ChargesAndModifiersViewModel
                    {
                        ChargeType = ChargeKindEnum.BusinessRates.ToString(),
                        Amount = Convert.ToDecimal(usage.tax_expense_amount),
                        CurrencyCode = MapCurrencyCode(usage.tax_expense_amount_uom_desc)
                    };
                    charges.Add(charge);
                }
                if (usage.other_expense_amount != null)
                {
                    var charge = new ChargesAndModifiersViewModel
                    {
                        ChargeType = ChargeKindEnum.EstateCharge.ToString(),
                        Amount = Convert.ToDecimal(usage.other_expense_amount),
                        CurrencyCode = MapCurrencyCode(usage.other_expense_amount_uom_desc)
                    };
                    charges.Add(charge);
                }
            }
            return charges?.Count() > 0 ? charges : null;
        }

        private static readonly string[] Ignored_Amenity_Types = new[] { "Not Applicable", "Other" };
        private IEnumerable<HighlightViewModel> MapHighlights(PropertyWithAvailability property, Region region)
        {
            if (_amenities == null) return new List<HighlightViewModel>();

            var defaultAmenityCultureCode = region?.CountryCode != null ? $"en-{region.CountryCode}" : "en-US"; // Default amenity is always English

            var highlights = _amenities
                // Convert amenities to Highlight VM
                .Select(a => new HighlightViewModel
                {
                    Text = string.Join(
                        ' ',
                        new string[] {
                            a.amenity_notes,
                            a.amenity_subtype_desc,
                            a.property_amenity_type_desc
                        }
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Where(s => !Ignored_Amenity_Types.Contains(s, StringComparer.OrdinalIgnoreCase))
                    ),
                    CultureCode = defaultAmenityCultureCode,
                    MiqId = a.id.ToString()
                })

                // TODO: Uncomment this when MIQ provides test data for amenity colloquials
                // // Include amenity colloquials (aka translations)
                // .Concat(_amenities
                //     .SelectMany(a => a.colloquial ?? new List<PropertyAmenityColl>())
                //     .Select(c => {
                //         var culture = GlobalizationUtility.CultureInfoFromLanguageName(c.language_desc);
                //         var region = GlobalizationUtility.RegionInfoFromName(c.country_code_desc);
                //         var cultureCode = culture == null || region == null ? null : GlobalizationUtility.FormatCultureCode(culture, region);

                //         return new HighlightViewModel {
                //             Text = string.Join(
                //                 ' ', 
                //                 new string[] { 
                //                     c.amenity_notes,
                //                     c.amenity_subtype_desc, 
                //                     c.property_amenity_type_desc
                //                 }
                //                 .Where(s => !string.IsNullOrWhiteSpace(s))
                //                 .Where(s => !Ignored_Amenity_Types.Contains(s, StringComparer.OrdinalIgnoreCase))
                //             ),
                //             CultureCode = cultureCode, // Will be null if we failed to parse the _desc fields
                //             MiqId = c.id.ToString()
                //         };
                //     })
                // )

                .Where(h => !string.IsNullOrWhiteSpace(h.Text) && !string.IsNullOrWhiteSpace(h.CultureCode))

                // Remove duplicates
                .GroupBy(h => h.Text)
                .Select(h => h.First())
                .ToList();

            // Set order now that nulls are filtered out
            var order = 0;
            foreach (var highlight in highlights)
            {
                highlight.Order = order++;
            }

            // Map the English version amenities text to Dutch version for Netherland 
            if (region?.CountryCode?.ToLower() == "nl")
            {
                var siteId = region?.CultureCode?.ToString();
                var dutchHighlightsList = new List<HighlightViewModel>();
                foreach (var highlight in highlights.OrderBy(x => x.Order))
                {
                    if (!highlights.Any(x => x.Order == highlight.Order && x.CultureCode == siteId))
                    {
                        var dutchHighlight = new HighlightViewModel()
                        {
                            CultureCode = siteId,
                            MiqId = highlight.MiqId.ToString(),
                            Text = highlight.Text,
                            Order = highlight.Order
                        };
                        dutchHighlightsList.Add(dutchHighlight);
                    }
                }
                highlights.AddRange(dutchHighlightsList);
            }
            return highlights.OrderBy(x => x.Order).ToList();
        }

        private IEnumerable<PropertyAmenity> GetUsageAmenities(PropertyWithAvailability property, Region region)
        {
            if (_options.Value?.AmenitiesSourceForSite(region.HomeSiteID) == PropertyMapperOptions.AmenitiesSourceEnum.FromPropertyPropertyAmenity)
            {
                return property?.PropertyDetail?.property_amenity.Where(a => a != null && a.amenity_type_desc?.ToLower() == "property");
            }
            if (_options.Value?.AmenitiesSourceForSite(region.HomeSiteID) == PropertyMapperOptions.AmenitiesSourceEnum.FromProperty)
            {
                return property?
                    .PropertyDetail?
                    .usages?
                    .Where(u => u.usage_amenity != null)
                    .SelectMany(u => u.usage_amenity)
                    .Where(a => a != null);
            }
            else
            {
                return property?
                    .Availability?
                    .Select(a => a
                        .property?
                        .property_usage?
                        .usage_amenity)
                    .Where(a => a != null)
                    .SelectMany(a => a)
                    .Where(a => a != null);
            }
        }

        private IEnumerable<SpacesViewModel> ConvertToSpaces(List<Availability> availabilities, Region region)
        {
            var spaces = new List<SpacesViewModel>();
            if (availabilities == null) yield break;

            foreach (var availability in availabilities)
            {
                var space = new SpacesViewModel();
                space.MiqId = availability.entity.id.ToString();
                space.SpaceType = MapPropertyUsage(availability.property?.property_usage?.ref_property_usage_type_desc);
                space.Specifications = MapSpaceSpecification(availability);
                space.Status = availability.ref_space_availability_status_desc == "Active" || availability.ref_space_availability_status_desc == "Current" ? "available" : "unavailable";
                space.AvailableFrom = availability.date_available;
                space.Name = MapSpaceName(availability, region);
                space.SpaceDescription = MapDescription(availability?.listing_descr, region);

                var sizes = ConvertToPropertySizes(availability?.property_measurement_availability)?.ToList();
                space.SpaceSizes = sizes;
                var attachments = availability.property?.property_media_information?.Select(i => i.media_path) ?? new List<string>();
                space.Photos = _photos?.Where(x => attachments.Contains(x.Url));
                space.Floorplans = _floorPlans?.Where(x => attachments.Contains(x.Url));
                space.Brochures = _brochures?.Where(x => attachments.Contains(x.Url));
                yield return space;
            }
        }

        private IEnumerable<PropertySizesViewModel> ConvertToPropertySizes(List<PropertyMeasurement> measurements)
        {
            var propertySizes = new List<PropertySizesViewModel>();
            propertySizes = measurements?.Select(x => MapPropertySize(x)).Where(y => y != null && y.SizeKind != SizeKindEnum.TotalArea.ToString())?.ToList();
            return propertySizes;
        }

        private IEnumerable<ContactsViewModel> ConvertToBrokers(List<Usage> usages)
        {
            var contacts = new List<ContactsViewModel>();
            if (usages == null) return contacts;

            foreach (var usage in usages)
            {
                var b = usage.company_contact_role_addresses?.Where(y => y.role_desc == "Listing Representative")?.Select(x => MapBroker(x)).ToList();
                if (b != null) contacts.AddRange(b);
            }

            return contacts.GroupBy(c => c.Email, StringComparer.OrdinalIgnoreCase).Select(g => g.First());
        }

        private ContactsViewModel MapBroker(CompanyContactRoleAddress c)
        {
            return new ContactsViewModel()
            {
                FirstName = c.first_name,
                LastName = c.last_name,
                Email = c.email,
                Phone = c.full_phone_number
            };
        }

        private PropertySizesViewModel MapPropertySize(PropertyMeasurement measurement)
        {
            string sizeKind = MapSizeKind(measurement.property_element_type_desc);
            if (String.IsNullOrWhiteSpace(sizeKind)) return null;

            return new PropertySizesViewModel()
            {
                SizeKind = sizeKind,
                MeasureUnit = MapSizesAndMeasurements(measurement.property_measurement_size_uom),
                Amount = Convert.ToDouble(measurement.property_measurement_size)
            };
        }

        private string NormalizeString(string value)
        {
            return String.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLower();
        }

        private ImagesViewModel MapImage(PropertyMediaInformation x)
        {
            return new ImagesViewModel
            {
                Active = true,
                DisplayText = string.IsNullOrWhiteSpace(x.media_name) ? Guid.NewGuid().ToString() : x.media_name,
                Url = x.media_path,
                Primary = x.primary_image_f,
                Order = x.display_order ?? 0
            };
        }

        private static MediaViewModel MapMedia(PropertyMediaInformation media) => MapMedia(media, null);
        private static MediaViewModel MapMedia(PropertyMediaInformation media, MediaViewModel viewModel)
        {
            if (media == null) return viewModel;
            if (viewModel == null) viewModel = new MediaViewModel();
            viewModel.Active = true;
            viewModel.DisplayText = string.IsNullOrWhiteSpace(media.media_name) ? new Guid().ToString() : media.media_name;
            viewModel.Url = media.media_path;
            viewModel.Primary = media.primary_image_f;
            return viewModel;
        }

        private Usage GetPrimaryUsage(List<Usage> usages)
        {
            return usages?.OrderByDescending(y => y.primary_usage_f)?.FirstOrDefault() ?? new Usage();
        }

        private string GetPropertyType(Usage usage)
        {
            return MapPropertyUsage(usage?.ref_property_usage_type_desc);
        }

        private string MapPropertyUsage(string value)
        {
            switch (value)
            {
                case "Office":
                    return "office";
                case "Retail":
                    return "retail";
                case "Industrial":
                    return "industrial";
                case "Flex":
                    return "flex";
                case "Residential":
                    return "residential";
                case "Land":
                    return "land";
                case "Health Care":
                    return "healthcare";
                case "Special Purpose":
                    return "specialpurpose";
                case "Multi-Family Housing":
                    return "multifamily";
                default:
                    return null;
            }
        }

        private string MapStatus(string value)
        {
            switch (NormalizeString(value))
            {
                case "existing": return "available";
                case "under construction": return "underConstruction";
                case "planned": return "planned";
                case "out of use": return "outOfUse";
                case "planned for renovation": return "plannedForRenovation";
                case "proposed": return "poposed";
                case "project": return "project";
                case "unimproved": return "unimproved";
                case "unknown": return "unknown";
                default: return value;
            }
        }

        private SpecificationsViewModel MapSpecification(PropertyDetail propertyDetail)
        {
            var specification = new SpecificationsViewModel();
            if (propertyDetail != null)
            {
                var usage = GetPrimaryUsage(propertyDetail.usages);
                specification.TotalSpace = usage.total_available_surface_area ?? 0;
                specification.MinSpace = usage.minimum_divisible_surface ?? 0;
                specification.Measure = MapUnitOfMeasure(usage.total_available_surface_area_uom_desc);
                specification.LeaseRateType = MapLeaseRateType(usage.ref_direct_rate_type_desc ?? usage.ref_sublease_rate_type_desc);

                // sorry to whoever has to read this, this is basically an if/else chain to check if 
                // min or max for each lease term/type is specified and the currency code is legible
                if ((
                    usage.minimum_yearly_direct_lease_rate_amount > 0 ||
                    usage.maximum_yearly_direct_lease_rate > 0
                ) && (
                    specification.CurrencyCode =
                        MapCurrencyCode(usage.minimum_yearly_direct_lease_rate_amount_uom_desc) ??
                        MapCurrencyCode(usage.maximum_yearly_direct_lease_rate_uom_desc)
                ) != null)
                {
                    specification.LeaseTerm = "yearly";
                    specification.LeaseType = MapLeaseType("Direct");
                    specification.MinPrice = usage.minimum_yearly_direct_lease_rate_amount ?? 0;
                    specification.MaxPrice = usage.maximum_yearly_direct_lease_rate ?? 0;
                }
                else if ((
                    usage.minimum_yearly_sub_lease_rate > 0 ||
                    usage.maximum_yearly_sub_lease_rate > 0
                ) && (
                    specification.CurrencyCode =
                        MapCurrencyCode(usage.minimum_yearly_sub_lease_rate_uom_desc) ??
                        MapCurrencyCode(usage.maximum_yearly_sub_lease_rate_uom_desc)
                ) != null)
                {
                    specification.LeaseTerm = "yearly";
                    specification.LeaseType = MapLeaseType("SubLease");
                    specification.MinPrice = usage.minimum_yearly_sub_lease_rate ?? 0;
                    specification.MaxPrice = usage.maximum_yearly_sub_lease_rate ?? 0;
                }
                else if ((
                    usage.minimum_monthly_direct_lease_rate > 0 ||
                    usage.maximum_monthly_direct_lease_rate > 0
                ) && (
                    specification.CurrencyCode =
                        MapCurrencyCode(usage.minimum_monthly_direct_lease_rate_uom_desc) ??
                        MapCurrencyCode(usage.maximum_monthly_direct_lease_rate_uom_desc)
                ) != null)
                {
                    specification.LeaseTerm = "monthly";
                    specification.LeaseType = MapLeaseType("Direct");
                    specification.MinPrice = usage.minimum_monthly_direct_lease_rate ?? 0;
                    specification.MaxPrice = usage.maximum_monthly_direct_lease_rate ?? 0;
                }
                else if ((
                    usage.minimum_monthly_sub_lease_rate > 0 ||
                    usage.maximum_monthly_sub_lease_rate > 0
                ) && (
                    specification.CurrencyCode =
                        MapCurrencyCode(usage.minimum_monthly_sub_lease_rate_uom_desc) ??
                        MapCurrencyCode(usage.maximum_monthly_sub_lease_rate_uom_desc)
                ) != null)
                {
                    specification.LeaseTerm = "monthly";
                    specification.LeaseType = MapLeaseType("SubLease");
                    specification.MinPrice = usage.minimum_monthly_sub_lease_rate ?? 0;
                    specification.MaxPrice = usage.maximum_monthly_sub_lease_rate ?? 0;
                }
            }
            return specification;
        }

        private SpecificationsViewModel MapSpaceSpecification(Availability availability)
        {
            var specification = new SpecificationsViewModel();
            if (availability != null)
            {
                specification.LeaseType = MapLeaseType(availability.ref_lease_type_desc);
                if (availability.ref_availability_type_desc == "Lease" || availability.ref_availability_type_desc == "Sale or Lease")
                {
                    if (availability.minimum_asking_rate_yearly > 0 || availability.maximum_asking_rate_yearly > 0 || availability.asking_rental_rate_yearly > 0)
                    {
                        specification.LeaseTerm = "yearly";
                        specification.Measure = MapUnitOfMeasure(availability.available_space_uom_desc);
                        specification.CurrencyCode =
                            MapCurrencyCode(availability.minimum_asking_rate_yearly_uom) ??
                            MapCurrencyCode(availability.asking_rental_rate_yearly_uom_desc);
                        if (specification.CurrencyCode != null)
                        {
                            specification.MinPrice = availability.minimum_asking_rate_yearly ?? 0;
                            specification.MaxPrice = availability.asking_rental_rate_yearly ?? 0;
                        }
                    }
                    else
                    {
                        specification.LeaseTerm = "monthly";
                        specification.CurrencyCode =
                            MapCurrencyCode(availability.minimum_asking_rate_monthly_uom) ??
                            MapCurrencyCode(availability.maximum_asking_rate_monthly_uom);
                        if (specification.CurrencyCode != null)
                        {
                            specification.MinPrice = availability.minimum_asking_rate_monthly ?? 0;
                            specification.MaxPrice = availability.maximum_asking_rate_monthly ?? 0;
                        }
                    }

                    if (availability.ref_availability_type_desc == "Sale" || availability.ref_availability_type_desc == "Sale or Lease")
                    {
                        specification.SalePrice = availability.asking_price_for_sale ?? 0;
                        specification.CurrencyCode = availability.asking_price_for_sale_uom_desc;
                    }
                }
                specification.TotalSpace = availability.available_space ?? 0;
                specification.Measure = MapUnitOfMeasure(availability.available_space_uom_desc);
            }
            return specification;
        }

        private List<TextTypeViewModel> MapDescription(string desc, Region region)
        {
            if (string.IsNullOrWhiteSpace(desc)) return new List<TextTypeViewModel>();
            return new List<TextTypeViewModel> { new TextTypeViewModel { Text = desc, CultureCode = $"{MapCultureCode(region, !string.IsNullOrWhiteSpace(region.Name) ? region.Name : null)}" } };
        }

        private List<TextTypeViewModel> MapNoteDescription(IEnumerable<PropertyNote> propertyNote, Region region, string noteType)
        {
            var propertyDescription = propertyNote?.Where(x => x.property_notes_type_desc == noteType)?.FirstOrDefault();
            // We should not consider English note value in Colloquial as we are taking the English Note in the next step.
            // To Avoid this added condition to pick other than English language notes
            List<Tuple<string, string>> tuples = propertyDescription?.colloquial?.Where(c => c.language_desc?.ToLower() != "english").Select(x => Tuple.Create(x.notes, $"{MapCultureCode(region, x.language_desc)}")).ToList();
            return ConvertToTextTypeList(tuples).JoinTwoLists(MapDescription(propertyDescription?.notes, region));
        }

        private List<TextTypeViewModel> ConvertToTextTypeList(List<Tuple<string, string>> list)
        {
            var result = new List<TextTypeViewModel>();
            if (list is null) return result;
            foreach (var l in list)
            {
                var n = new TextTypeViewModel { Text = l.Item1, CultureCode = l.Item2 };
                result.Add(n);
            }
            return result;
        }

        private List<TextTypeViewModel> MapSpaceName(Availability availability, Region region)

        {
            if (availability == null) return new List<TextTypeViewModel>();

            string floorNumber = !string.IsNullOrWhiteSpace(availability.floor_suite?.floor_number) ? $"Floor {availability.floor_suite?.floor_number}" : "";
            string suiteNumber = !string.IsNullOrWhiteSpace(availability.floor_suite?.suite_number) ? $"Suite {availability.floor_suite?.suite_number}" : "";

            string spaceName = "";
            if (floorNumber == "" && suiteNumber == "")
            {
                var availableSpace = availability.available_space > 0 ? $"{availability.available_space} {availability.available_space_uom_desc}" : null;
                var array = new[] { availableSpace, $"{availability.property?.property_name}" };
                spaceName = string.Join(" on ", array.Where(x => !string.IsNullOrEmpty(x)));
            }
            else
            {
                spaceName = string.Join(", ", floorNumber, suiteNumber);
            }

            return MapDescription(spaceName, region);
        }

        private string MapCurrencyCode(string value)
        {
            if (GlobalizationUtility.TryGetCurrencyDataFromName(value, out var result))
            {
                return result.Code;
            }
            else
            {
                return null;
            }
        }

        private string MapLeaseRateType(string value)
        {
            switch (NormalizeString(value))
            {
                case "full service gross": return "FullServiceGross";
                case "modified gross": return "ModifiedGross";
                case "triple net": return "TripleNet";
                case "net": return "Net";
                default: return value;
            }
        }

        private string MapCountry(string value)
        {
            switch (NormalizeString(value))
            {
                case "singapore":
                    return "SG";
                case "india":
                    return "IN";
                case "netherlands":
                    return "NL";
                case "italy":
                    return "IT";
                case "poland":
                    return "PL";
                case "norway":
                    return "NO";
                case "united states of america":
                default:
                    return "US";
            }
        }

        private string MapCultureCode(Region region, string language = "english")
        {
            if (string.IsNullOrWhiteSpace(region.CultureCode) || region.CultureCode.Count() < 3) return "en-US";
            return MapLanguage(language) + region.CultureCode.AsSpan(2).ToString();
        }

        private string MapLanguage(string language)
        {
            switch (NormalizeString(language))
            {
                case "italian":
                    return "it";
                case "polish":
                    return "pl";
                case "finnish":
                    return "fi";
                case "norwegian":
                    return "no";
                case "dutch":
                    return "nl";
                case "danish":
                    return "da";
                case "english":
                default:
                    return "en";
            }
        }

        private string MapLeaseType(string value)
        {
            switch (NormalizeString(value))
            {
                case "subLease":
                    return "SubLease";
                case "assignment":
                    return "Assignment";
                case "unknown":
                    return "Unknown";
                case "license":
                    return "License";
                case "occupational":
                    return "Occupational";
                case "new":
                    return "New";
                case "flex lease":
                    return "FlexLease";
                case "direct":
                default:
                    return "LeaseHold";
            }
        }

        private string MapSizesAndMeasurements(string value)
        {
            switch (NormalizeString(value))
            {
                case "square foot":
                case "sqft":
                    return "sqft";
                case "square meter":
                    return "sqm";
                case "hectare":
                case "hectares":
                    return "hectare";
                case "acre":
                    return "acre";
                case "feet":
                    return "ft";
                case "yd":
                    return "yd";
                case "m":
                    return "m";
                case "person":
                    return "pp";
                case "whole":
                    return "whole";
                case "kilometre":
                case "kilometer":
                    return "km";
                default:
                    return "";
            }
        }

        private string MapUnitOfMeasure(string value)
        {
            switch (NormalizeString(value))
            {
                case "square foot":
                case "sqft":
                    return "sf";
                case "square meter":
                    return "sm";
                case "sqm":
                    return "sqm";
                case "hectare":
                case "hectares":
                    return "hectare";
                case "acre":
                    return "acre";
                case "ft":
                    return "ft";
                case "yd":
                    return "yd";
                case "m":
                    return "m";
                case "pp":
                    return "person";
                case "whole":
                    return "whole";
                default:
                    return "";
            }
        }

        private string MapSizeKind(string value)
        {
            string sizeKind = value.Replace(" ", String.Empty);
            return sizeKind.ToEnum<SizeKindEnum>(AliasType.StoreApi)?.ToString();
        }

        private string MapLeaseTerm(string value)
        {
            switch (NormalizeString(value))
            {
                case "monthly":
                    return "monthly";
                case "quarterly":
                    return "quarterly";
                case "annually":
                case "yearly":
                    return "annually";
                case "once":
                    return "once";
                default:
                    return null;
            }
        }

        private string MapProvinceState(string value)
        {
            switch (NormalizeString(value))
            {
                case "drenthe":
                    return "DR";
                case "flevoland":
                    return "FL";
                case "direct":
                    return "FL";
                case "fryslan":
                case "fryslân":
                    return "FR";
                case "gelderland":
                    return "GE";
                case "groningen":
                    return "GR";
                case "limburg":
                    return "LI";
                case "noord-brabant":
                    return "NB";
                case "noord-holland":
                    return "NH";
                case "overijssel":
                    return "OV";
                case "zuid-holland":
                    return "ZH";
                case "utrecht":
                    return "UT";
                case "zeeland":
                    return "ZE";
                default:
                    return "";
            }
        }
    }
}
