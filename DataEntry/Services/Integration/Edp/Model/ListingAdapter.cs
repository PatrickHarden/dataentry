using dataentry.Data.DBContext.Model;
using dataentry.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dataentry.Services.Integration.Edp.Model
{
    public class ListingAdapter : IListingAdapter
    {
        public PropertyUnified ConvertToPropertyUnified(Listing listing)
        {
            var specifications = listing.GetListingData<Specifications>();
            var sizes = listing.GetListingDataArray<PropertySize>();

            var property = new PropertyUnified();

            property.id = listing.ExternalID;
            property.property_name = listing.Name;

            property.property_usage =
                new List<PropertyUsageUnified> {
                    new PropertyUsageUnified {
                        ref_property_usage_type_desc = MapPropertyUsage(listing.GetListingData<PropertyType>().Value) ?? "Unknown"
                    }
                };

            if (listing.Address != null) {
                property.postal_address = new PostalAddressUnified
                {
                    street1 = listing.Address.Street1,
                    street2 = listing.Address.Street2,
                    city = listing.Address.City,
                    state_province = listing.Address.StateProvince,
                    postal_code = listing.Address.PostalCode,
                    country = listing.Region.CountryCode,
                    latitude = listing.Address.Latitude,
                    longitude = listing.Address.Longitude
                };
            }

            property.total_gross_area = specifications.TotalSpace;
            var landSize = sizes.FirstOrDefault(s => s.SizeKind == "LandSize");
            if (landSize != null)
            {
                property.total_land_area = Convert.ToDecimal(landSize.Amount);
                property.total_land_area_uom_desc = MapUnitOfMeasure(landSize.MeasureUnit);
            }
            property.property_amenity = listing.GetListingDataArray<Aspect>().Select(a => new PropertyAmenityUnified { amenity_desc = a.Value }).ToList();

            int order = 0;
            property.property_media_information = new List<PropertyMediaInformationUnified>();

            var media = listing.GetMedia();
            
            foreach (var m in media)
            {
                property.property_media_information.Add(new PropertyMediaInformationUnified
                {
                    media_name = m.DisplayText,
                    media_path = m.Url,
                    primary_image_f = m.Primary,
                    media_caption = m.DisplayText,
                    display_order = order++,

                    media_type_desc = m switch {
                        ListingImage i => i.ImageCategory.ToLower() switch {
                            "photo" => "Photo",
                            "floorplan" => "Floor Plan",
                            _ => null
                        },
                        Brochure _ => "Brochure",
                        EpcGraph _ => "EPC Graph",
                        _ => null
                    }
                });
            }
            
            return property;
        }

        public IEnumerable<AvailabilityUnified> CovertToAvailabilitiesUnified(Listing listing)
        {
            var listingSpecifications = listing.GetListingData<Specifications>();
            var spaces = listing.Spaces;
            if (spaces == null || spaces.Count == 0)
            {
                spaces = new List<Listing> { new Listing { ID = listing.ID } };
            }

            foreach (var space in spaces)
            {
                var availability = GetAvailabilityUnifiedBase(listing, space);

                var specifications = space.GetListingData<Specifications>();

                availability.listing_notes = space.GetListingDataAllLanguages<SpaceName>().FirstOrDefault()?.Text;
                availability.ref_property_usage_type_desc = MapPropertyUsage(space.UsageType) ?? MapPropertyUsage(listing.GetListingData<PropertyType>().Value) ?? "Unknown";
                if (availability.ref_availability_type_desc == "Lease" || availability.ref_availability_type_desc == "Sale or Lease")
                {
                    availability.ref_lease_type_desc = MapLeaseType(specifications.LeaseType) ?? MapLeaseType(listingSpecifications.LeaseType);
                    availability.minimum_lease_term = MapLeaseTerm(specifications.LeaseTerm) ?? MapLeaseTerm(listingSpecifications.LeaseTerm);

                    if (specifications.MaxPrice != null && specifications.MaxPrice != 0)
                    {
                        var normalizedLeaseTerm = NormalizeString(specifications.LeaseTerm);
                        if (normalizedLeaseTerm == "monthly")
                        {
                            availability.asking_rental_rate_monthly = specifications.MaxPrice;
                            availability.asking_rental_rate_monthly_uom_desc = specifications.CurrencyCode;
                        }
                        else if (normalizedLeaseTerm == "yearly" || normalizedLeaseTerm == "annually")
                        {
                            availability.asking_rental_rate_yearly = specifications.MaxPrice;
                            availability.asking_rental_rate_yearly_uom_desc = specifications.CurrencyCode;
                        }
                    }
                }
                if (availability.ref_availability_type_desc == "Sale" || availability.ref_availability_type_desc == "Sale or Lease")
                {
                    if (specifications.SalePrice != null && specifications.SalePrice != 0)
                    {
                        availability.asking_price_for_sale = specifications.SalePrice;
                        availability.asking_price_for_sale_uom_desc = specifications.CurrencyCode;
                    }
                }
                //TODO: Handle multi-language better (unnecessary for now since it's US only)
                availability.listing_descr = string.Join("\n\n", new string[] {
                    space.GetListingDataAllLanguages<BuildingDescription>().FirstOrDefault()?.Text,
                    listing.GetListingDataAllLanguages<BuildingDescription>().FirstOrDefault()?.Text,
                    listing.GetListingDataAllLanguages<LocationDescription>().FirstOrDefault()?.Text
                }.Where(v => !string.IsNullOrWhiteSpace(v))).Trim();

                var sizes = space.GetListingDataArray<PropertySize>().Concat(listing.GetListingDataArray<PropertySize>());
                availability.available_space = specifications.TotalSpace ?? 0;
                availability.available_space_uom_desc = MapUnitOfMeasure(specifications.Measure);
                var superArea = sizes.FirstOrDefault(s => s.SizeKind == "SuperArea");
                if (superArea != null)
                {
                    availability.total_area_of_space = Convert.ToDecimal(superArea.Amount);
                    availability.total_area_of_space_uom_desc = MapUnitOfMeasure(superArea.MeasureUnit);
                }
                var officeArea = sizes.FirstOrDefault(s => s.SizeKind == "OfficeArea");
                if (officeArea != null)
                {
                    availability.available_office_space = Convert.ToDecimal(officeArea.Amount);
                    availability.available_office_space_uom_desc = MapUnitOfMeasure(officeArea.MeasureUnit);
                }
                var contiguousSpace = sizes.FirstOrDefault(s => s.SizeKind == "TotalContiguousSpace");
                if (contiguousSpace != null)
                {
                    availability.total_contiguous_area_of_space = Convert.ToDecimal(officeArea.Amount);
                    availability.total_contiguous_area_of_space_uom_desc = MapUnitOfMeasure(officeArea.MeasureUnit);
                }
                var ceilingHeight = sizes.FirstOrDefault(s => s.SizeKind == "MinimumCeilingHeight");
                if (ceilingHeight != null)
                {
                    availability.ceiling_height = Convert.ToDecimal(ceilingHeight.Amount);
                }

                yield return availability;
            }
        }

        /// <summary>
        /// Instantiate AvailabilityUnified and map required data for creation
        /// </summary>
        public AvailabilityUnified GetAvailabilityUnifiedBase(Listing listing, Listing space)
        {
            var availability = new AvailabilityUnified();

            availability.id = space.ExternalID;
            // If MIQ property ID is unknown/does not yet exist, use listing's ExternalID
            availability.property_id = listing.ExternalID;
            availability.date_on_market = listing.CreatedAt;
            availability.date_available = space.AvailableFrom ?? listing.AvailableFrom;
            availability.ref_space_availability_status_desc = "Available";
            availability.ref_availability_type_desc = MapListingType(listing.GetListingData<ListingType>().Value);

            availability.listing_rep = new List<CompanyUnified>
                {
                    new CompanyUnified
                    {
                        name = "CBRE",
                        postal_address = new PostalAddressUnified
                        {
                            street1 = "400 S. Hope Street",
                            street2 = "25th Floor",
                            city = "Los Angeles",
                            state_province = "CA",
                            postal_code = "90071"
                        },
                        contact = listing.ListingBroker?
                            .OrderBy(x => x.Order)?
                            .Select(x => new ContactUnified
                            {
                                name = $"{x.Broker.FirstName} {x.Broker.LastName}",
                                email = new List<string>{ x.Broker.Email.Trim() },
                                phone = new List<string>{ x.Broker.Phone.Replace("_",string.Empty).Trim() }
                            })?.ToList() ?? new List<ContactUnified>()
                    }
                };
            
            return availability;
        }

        public string NormalizeString(string value)
        {
            return value?.Trim().ToLower();
        }

        public string MapPropertyUsage(string value)
        {
            switch (NormalizeString(value))
            {
                case "office":
                case "officecoworking":
                    return "Office";
                case "retail":
                    return "Retail";
                case "industrial":
                    return "Industrial";
                case "flex":
                case "flexindustrial":
                    return "Flex";
                case "residential":
                    return "Residential";
                case "land":
                    return "Land";
                case "healthcare":
                    return "Health Care";
                case "specialpurpose":
                    return "Special Purpose";
                case "multifamily":
                    return "Multi-Family Housing";
                case "shophouse":
                case "hospitality":
                default:
                    return null;
            }
        }

        public string MapUnitOfMeasure(string value)
        {
            switch (NormalizeString(value))
            {
                case "sqft":
                case "sf":
                    return "sqft";
                case "sm":
                case "sqm":
                    return "sqm";
                case "hectare":
                    return "hectare";
                case "acre":
                    return "acre";
                case "ft":
                    return "ft";
                case "yd":
                    return "yd";
                case "m":
                    return "m";
                case "desk":
                case "person":
                case "room":
                    return "pp";
                case "whole":
                    return "whole";
                default:
                    return "sqft";
            }
        }

        public string MapListingType(string value)
        {
            switch (NormalizeString(value))
            {
                case "sale":
                    return "Sale";
                case "lease":
                    return "Lease";
                case "salelease":
                    return "Sale or Lease";
                default:
                    return null;
            }
        }

        public string MapLeaseType(string value)
        {
            switch (NormalizeString(value))
            {
                case "sublease":
                    return "SubLease";
                case "assignment":
                    return "Assignment";
                case "freehold":
                case "groundlease":
                case "headlease":
                case "leasehold":
                case "licence":
                case "longleasehold":
                case "longlet":
                case "new":
                case "occupationallease":
                    return "Direct";
                case "flexlease":
                    return "FlexLease";
                case "unknown":
                default:
                    return "Unknown";
                    //return "Flex Lease";
            }
        }

        public string MapLeaseTerm(string value)
        {
            switch (NormalizeString(value))
            {
                case "monthly":
                    return "Monthly";
                case "quarterly":
                    return "Quarterly";
                case "annually":
                case "yearly":
                    return "Annually";
                case "once":
                    return "Once";
                default:
                    return null;
            }
        }
    }
}
