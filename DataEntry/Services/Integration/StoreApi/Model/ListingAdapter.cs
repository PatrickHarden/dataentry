using dataentry.Data.DBContext.Model;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ListingType = dataentry.Data.DBContext.Model.ListingType;

namespace dataentry.Services.Integration.StoreApi.Model
{
    public class ListingAdapter : IListingAdapter
    {

        private Listing _listing;
        private string _flexRentList;

        public ListingAdapter(IConfiguration configuration)
        {
            _flexRentList = configuration["StoreSettings:FlexRentList"] ?? throw new ArgumentNullException("StoreSettings:FlexRentList");
        }

        public PropertyListing ConvertToPropertyListing(Listing listing)
        {
            PropertyListing propertyListing = new PropertyListing();
            _listing = listing;
            var propertyType = ConvertToPropertyType();
            var specifications = ConvertSpecifications(_listing);

            propertyListing.CommonPrimaryKey = listing.ExternalID;

            propertyListing.CommonSourceSystem = ConvertToSourceSystem(listing.MIQID);
            
            propertyListing.CommonHomeSite = listing.Region.HomeSiteID;

            propertyListing.CommonPropertySubType = ConvertPropertySubType();

            propertyListing.CommonCreated = _listing.CreatedAt.ConvertToString();

            propertyListing.CommonLastProcessed = GetPublishingState()?.DateUpdated.ToString("yyyy-MM-dd");

            propertyListing.CommonLastUpdated = _listing.UpdatedAt.ConvertToString();

            propertyListing.CommonUsageType = propertyType;
            
            propertyListing.CommonAspects = ConvertToCommonAspects();

            propertyListing.CommonIsParent = true;

            propertyListing.CommonPropertyTypes = null;

            propertyListing.CommonActualAddress = ConvertToActualAddress();

            propertyListing.CommonEnergyPerformanceData = ConvertToCommonEnergyPerformanceData();

            propertyListing.CommonEnergyPerformanceInformation = ConvertToCommonEnergyPerformanceInformation();

            propertyListing.CommonAvailability = ConvertToCommonAvailability();

            propertyListing.CommonCoordinate = ConvertToCommonCoordinate();

            propertyListing.CommonContactGroup = ConvertToContactGroup();

            propertyListing.CommonAgents = ConvertToAgents();

            propertyListing.CommonWebsite = ConvertToWebsite();

            propertyListing.CommonBuildingOperator = ConvertToOperator();

            propertyListing.CommonVideoLinks = ConvertToVideoLinks(_listing);

            propertyListing.CommonWalkthrough = ConvertToWalkthrough(_listing);

            propertyListing.CommonBrochures = ConvertToCommonBrochures(propertyListing.CommonBrochures, _listing);

            propertyListing.CommonPhotos = ConvertToImages(_listing, ImageCategory.Photo.ToString());

            propertyListing.CommonFloorPlans = ConvertToImages(_listing, ImageCategory.FloorPlan.ToString());

            propertyListing.CommonHighlights = ConvertToCommonHighlights(propertyListing.CommonHighlights, _listing);
            
            propertyListing.CommonMicromarket = ConvertToCommonMicromarket();

            propertyListing.CommonCharges = ConvertToCommonCharges(specifications, true);

            propertyListing.CommonFloorsAndUnits = ConvertToFloorsAndUnits(ref propertyListing, _listing.Spaces);

            propertyListing.CommonGeoLocation = ConvertToCommonGeoLocation();

            propertyListing.CommonLeaseTypes = ConvertToLeaseType(specifications?.LeaseType);

            propertyListing.CommonLeaseRateType = ConvertToLeaseRateType(specifications?.LeaseRateType);

            propertyListing.CommonLongDescription = ConvertToCommonLongDescription();

            propertyListing.CommonLocationDescription = ConvertToCommonLocationDescription();

            propertyListing.CommonNumberOfBedrooms = specifications.Bedrooms ?? 0;

            propertyListing.CommonSizes = ConvertToCommonSize(specifications);

            propertyListing.CommonSourceLastupdated = _listing.UpdatedAt.ConvertToString();

            propertyListing.CommonStrapline = ConvertToCommonStrapline();

            propertyListing.CommonTotalSize = ConvertToCommonTotalSize(specifications);

            propertyListing.CommonParking = ConvertToCommonParking();

            propertyListing.CommonPointsOfInterests = ConvertToCommonPointsOfInterests();
            
            propertyListing.CommonTransportationTypes = ConvertToCommonTransportationTypes();

            propertyListing.UnitedKingdomUseClass = ConvertToUseClass();

            propertyListing.UnitedKingdomVatPayable = false;

            if (ConvertToFloors() != null){
                propertyListing.CommonNumberOfStoreys = ConvertToFloors();
            }

            if (ConvertToYearBuilt() != null){
                propertyListing.CommonYearBuilt = ConvertToYearBuilt();
            }

            if (GetSyndicationFlag() != null) {
                propertyListing.CommonPublishExternallyExternal = GetSyndicationFlag().Value;
                if (!string.IsNullOrWhiteSpace(GetSyndicationMarket))
                {
                    string updateLine2 = $"{propertyListing.CommonActualAddress.CommonLine2}, {GetSyndicationMarket}";
                    propertyListing.CommonActualAddress.CommonLine2 = updateLine2;
                    var actualAddress = propertyListing.CommonActualAddress.CommonPostalAddresses?.FirstOrDefault();
                    if (actualAddress != null) actualAddress.CommonLine2 = updateLine2;
                }
            }

            if (!string.IsNullOrWhiteSpace(_listing.Status)) propertyListing.CommonStatus = _listing.Status;

            return propertyListing;
        }

        public PropertyListing ConvertToPreviewPropertyListing(Listing listing)
        {
            var result = ConvertToPropertyListing(listing);
            result.CommonPrimaryKey = listing.PreviewID;
            result.CommonHomeSite = listing.Region.PreviewSiteID;
            return result;
        }

        public CommonActualAddress ConvertToActualAddress()
        {
            CommonActualAddress commonActualAddress = new CommonActualAddress()
            {
                CommonLine1 = _listing.Name.RefactorString(),
                CommonLine2 = _listing.Address?.Street1.RefactorString(),
                CommonLine3 = _listing.Address?.Street2.RefactorString(),
                CommonLocallity = _listing.Address?.City.ReturnValueOrNull(),
                CommonRegion = _listing.Address?.StateProvince.RefactorString(nullAllowed: true),
                CommonCountry = _listing.Region.CountryCode,
                CommonPostCode = _listing.Address?.PostalCode.RefactorString(),

                CommonPostalAddresses = new List<CommonPostalAddress>() 
                {
                    new CommonPostalAddress{
                        CommonLanguage = _listing.Region.CultureCode,
                        CommonLine1 =  _listing.Name.RefactorString(),
                        CommonLine2 = _listing.Address?.Street1.RefactorString(),
                        CommonLine3 = _listing.Address?.Street2.RefactorString(),
                        CommonLocallity = _listing.Address?.City.RefactorString(),
                        CommonRegion = _listing.Address?.StateProvince.RefactorString(nullAllowed: true)
                    }
                }
            };

            return commonActualAddress;
        }

        public CommonEnergyPerformanceData  ConvertToCommonEnergyPerformanceData()
        {
            string energyRating = ConvertToEnergyRating()?.ToEnum<EnergyRatingEnum>().ToAlias(AliasType.StoreApi);
            List<CommonExternalRatings> externalRatings = ConvertToCommonExternalRatings();
            
            if (String.IsNullOrWhiteSpace(energyRating) && externalRatings.Count == 0) 
                return null;
            
            return new CommonEnergyPerformanceData { CommonCertificateType = energyRating, CommonExternalRatings = externalRatings };
        }

        public List<CommonBrochureElement> ConvertToCommonEnergyPerformanceInformation() 
        {
            var result = new List<CommonBrochureElement>();

            var epcGraphs = _listing.GetListingDataArray<EpcGraph>();
            if (epcGraphs != null && epcGraphs.Any()) {
                foreach (var epcGraph in epcGraphs) {
                    if (epcGraph.Url != null) {
                        result.Add(new CommonBrochureElement {
                            CommonUri = epcGraph.Url,
                            CommonUriExternal = false,
                            CommonCultureCode = _listing.Region.CultureCode
                        });
                    }
                }
            }

            return result.Any() ? result : null;
        }

        public CommonAvailability ConvertToCommonAvailability()
        {
            return new CommonAvailability
            {
                CommonAvailabilityKind = "AvailableFromKnownDate",
                CommonAvailabilityDate = _listing.AvailableFrom?.ConvertToString()
            };
        }
        
        public CommonCoordinate ConvertToCommonCoordinate()
        {
            return new CommonCoordinate
            {
                Lat = _listing.Address?.Latitude ?? 0,
                Lon = _listing.Address?.Longitude ?? 0
            };
        }

        public CommonContactGroup ConvertToContactGroup()
        {
            CommonContactGroup commonContactGroup = new CommonContactGroup
            {
                CommonContacts = _listing.ListingBroker?
                    .OrderBy(x => x.Order)
                    .Select(x => new CommonAgentElement()
                    {
                        CommonAgentName = $"{x.Broker.FirstName} {x.Broker.LastName}",
                        CommonEmailAddress = x.Broker.Email.RefactorString(),
                        CommonTelephoneNumber = x.Broker.Phone.CleanPhoneNumber(),
                        CommonAvatar = x.Broker.Avatar.ReturnValueOrNull(),
                        CommonLicenseNumber = x.Broker.License.ReturnValueOrNull(),
                    }).ToList(),
            };

            return commonContactGroup;
        }

        public List<CommonAgentElement> ConvertToAgents()
        {
            return _listing.ListingBroker?
                    .OrderBy(x => x.Order)
                    .Select(x => new CommonAgentElement()
                    {
                        CommonAgentName = $"{x.Broker.FirstName} {x.Broker.LastName}",
                        CommonEmailAddress = x.Broker.Email.RefactorString(),
                        CommonTelephoneNumber = x.Broker.Phone.CleanPhoneNumber(),
                        CommonAgentOffice = x.Broker.Location.ReturnValueOrNull()
                    }).ToList();
        }

        public List<CommonVideoLink> ConvertToVideoLinks(Listing listing)
        {
            var link = ConvertVideoLink(listing);
            if (string.IsNullOrEmpty(link)) return null;

            return new List<CommonVideoLink>()
            {
                new CommonVideoLink()
                {
                    CommonLink = link,
                    CommonCultureCode = _listing.Region.CultureCode
                }
            };
        }

        public List<string> ConvertToLeaseType(string leaseType)
        {
            string listingType = ConvertListingType();
            if (string.IsNullOrWhiteSpace(listingType) || listingType.Equals("sale", StringComparison.OrdinalIgnoreCase)) return null;

            var leaseTypeEnum = leaseType.ToEnum<LeaseTypesEnum>().ToAlias(AliasType.StoreApi);
            if (string.IsNullOrEmpty(leaseTypeEnum)) return null;
            return new List<string> { leaseTypeEnum };
        }

        public string ConvertToLeaseRateType(string leaseRateType)
        {
            string listingType = ConvertListingType();
            if (string.IsNullOrWhiteSpace(listingType) || listingType.Equals("sale", StringComparison.OrdinalIgnoreCase)) return null;

            return leaseRateType;
        }

        public List<CommonCommentElement> ConvertToCommonLongDescription()
        {
            return ConvertToCommonCommentElement(_listing.GetListingDataAllLanguages<BuildingDescription>());
        }

        public List<CommonCommentElement> ConvertToCommonLocationDescription()
        {
            return ConvertToCommonCommentElement(_listing.GetListingDataAllLanguages<LocationDescription>());
        }

        public List<CommonCommentElement> ConvertToCommonCommentElement(string data)
        {
            TextType t = new TextType(){ Text = data };
            return ConvertToCommonCommentElement(new[] { t });
        }

        public List<CommonCommentElement> ConvertToCommonCommentElement(IEnumerable<TextType> textType)
        {
            var result = textType
                .Where(t => !string.IsNullOrWhiteSpace(t.Text))
                .Select(t => new CommonCommentElement
                {
                    CommonCultureCode = string.IsNullOrWhiteSpace(t.CultureCode) ? _listing.Region.CultureCode : t.CultureCode,
                    CommonText = t.Text
                })
                .GroupBy(t => t.CommonCultureCode)
                .Select(t => t.FirstOrDefault())
                .Where(t => t != null);

            return result.Any() ? result.ToList() : null;
        }

        public List<CommonFloorsAndUnit> ConvertToFloorsAndUnits(ref PropertyListing propertyListing, IEnumerable<Listing> spaces)
        {
            if (spaces == null) return null;
            string propertyType = ConvertToPropertyType();
            var commonFloorsAndUnits = new List<CommonFloorsAndUnit>();
            var newAspect = new List<string>();

            foreach (var space in spaces.OrderBy(s => s.GetListingData<SortOrder>()?.Value ?? int.MaxValue))
            {
                var commonFloorsAndUnit = new CommonFloorsAndUnit();
                var specifications = ConvertSpecifications(space);

                commonFloorsAndUnit.CommonAreas = new List<CommonLandSizeElement>()
                {
                    new CommonLandSizeElement 
                    {
                        CommonUnits = ConvertToDimensionsUnit(specifications?.Measure),
                        CommonArea= specifications?.TotalSpace ?? 0,
                        CommonMinArea= specifications?.MinSpace,
                        CommonMaxArea= specifications?.MaxSpace
                    }
                };

                commonFloorsAndUnit.CommonIdentifier = space.ID.ToString();
                commonFloorsAndUnit.CommonLeaseTypes = ConvertToLeaseType(specifications?.LeaseType)?.FirstOrDefault();
                commonFloorsAndUnit.CommonCharges = ConvertToCommonCharges(specifications, false);
                commonFloorsAndUnit.CommonSizes = ConvertToCommonSizes(space) ?? null;

                var tuple = SetCommonSubdivisionName(commonFloorsAndUnit, space, propertyType);
                commonFloorsAndUnit = tuple.CommonFloorsAndUnit;
                newAspect.AddRange(tuple.NewAspect);

                commonFloorsAndUnit.CommonUnitStatus = (space.Status?.ToEnum<UnitStatus>() ?? UnitStatus.available).ToAlias(AliasType.StoreApi);
                commonFloorsAndUnit.CommonAvailableFrom = space.AvailableFrom?.ConvertToString();
                commonFloorsAndUnit.CommonAvailability = new CommonAvailability
                {
                    CommonAvailabilityKind = "AvailableFromKnownDate",
                    CommonAvailabilityDate = space.AvailableFrom?.ConvertToString()
                };

                commonFloorsAndUnit.CommonSpaceDescription = ConvertToCommonCommentElement(space.GetListingDataAllLanguages<BuildingDescription>());

                commonFloorsAndUnit.CommonBrochures = ConvertToCommonBrochures(commonFloorsAndUnit.CommonBrochures, space);
                commonFloorsAndUnit.CommonPhotos = ConvertToImages(space, ImageCategory.Photo.ToString());
                commonFloorsAndUnit.CommonFloorPlans = ConvertToImages(space, ImageCategory.FloorPlan.ToString());
                commonFloorsAndUnit.CommonVideoLinks = ConvertToVideoLinks(space);
                commonFloorsAndUnit.CommonWalkthrough = ConvertToWalkthrough(space);
                commonFloorsAndUnits.Add(commonFloorsAndUnit);
            }

            if (newAspect.Count > 0) propertyListing.CommonAspects.AddRange(newAspect.Distinct());
            
            return commonFloorsAndUnits;
        }

        private (CommonFloorsAndUnit CommonFloorsAndUnit, IEnumerable<string> NewAspect) SetCommonSubdivisionName(CommonFloorsAndUnit commonFloorsAndUnit, Listing space, string propertyType)
        {
            var newAspect = new List<string>();

            if (_flexRentList.Split(",").Contains(propertyType))
            {
                var useTypeDesc = 
                    space.GetListingDataAllLanguages<SpaceName>()
                    .FirstOrDefault()?
                    .Text
                    .ToEnum<UseTypeEnum>();

                if (useTypeDesc != null)
                {
                    commonFloorsAndUnit.CommonUnitUse = useTypeDesc.ToAlias(AliasType.StoreApi);
                    newAspect.Add(useTypeDesc.ToAlias(AliasType.StoreApiRelatedAspsect));
                    var subdivisionName = useTypeDesc.ToAlias(AliasType.StoreApiDisplay);
                    if (!string.IsNullOrWhiteSpace(subdivisionName))
                    {
                        commonFloorsAndUnit.CommonSubdivisionName = new List<CommonSubdivisionName>()
                        {
                                new CommonSubdivisionName
                                {
                                    CommonCultureCode = _listing.Region.CultureCode,
                                    CommonText = subdivisionName
                                }
                        };
                    }
                }
            }
            else
            {
                commonFloorsAndUnit.CommonUnitUse = space.UsageType.ToEnum<UseTypeEnum>()?.ToAlias(AliasType.StoreApi);
                commonFloorsAndUnit.CommonSubdivisionName = space.GetListingDataAllLanguages<SpaceName>()
                .Where(x => !string.IsNullOrWhiteSpace(x.Text))
                .Select(n => new CommonSubdivisionName
                {
                    CommonCultureCode = n.CultureCode,
                    CommonText = n.Text
                }).ToList();
            }

            return (commonFloorsAndUnit, newAspect);
        }

        public CommonGeoLocation ConvertToCommonGeoLocation()
        {
            return new CommonGeoLocation()
            {
                Geometry = new Geometry
                {
                    Type = "Point",
                    Coordinates = new List<decimal>()
                    {
                        _listing.Address?.Latitude ?? 0, _listing.Address?.Longitude ?? 0
                    }
                },
                CommonExact = true
            };
        }

        public List<CommonSize> ConvertToCommonSize(Specifications specifications)
        {
            var measureUnit = specifications?.Measure;
            var result = new List<CommonSize>();

            if (specifications.MinSpace > 0)
            {
                result.Add(
                    new CommonSize
                    {
                        CommonSizeKind = "MinimumSize",
                        CommonDimensions = new List<CommonDimension>()
                        {
                            new CommonDimension
                            {
                                CommonDimensionsUnits = ConvertToDimensionsUnit(measureUnit),
                                CommonAmount= Convert.ToDouble(specifications.MinSpace)
                            }
                        }
                    }  
                );
            }

            if (specifications.MaxSpace > 0)
            {

                result.Add(
                    new CommonSize
                    {
                        CommonSizeKind = "MaximumSize",
                        CommonDimensions = new List<CommonDimension>()
                        {
                            new CommonDimension
                            {
                                CommonDimensionsUnits = ConvertToDimensionsUnit(measureUnit),
                                CommonAmount= Convert.ToDouble(specifications.MaxSpace)
                            }
                        }
                    }
                );
            }

            if (specifications.TotalSpace > 0)
            {
                result.Add(
                    new CommonSize
                    {
                        CommonSizeKind = "TotalSize",
                        CommonDimensions = new List<CommonDimension>()
                        {
                            new CommonDimension
                            {
                                CommonDimensionsUnits = ConvertToDimensionsUnit(measureUnit),
                                CommonAmount= Convert.ToDouble(specifications.TotalSpace)
                            }
                        }
                    }  
                );
            }

            //Property Level Sizes
            var propertySizes = ConvertToCommonSizes(_listing);
            if (propertySizes?.Count > 0) result.AddRange(propertySizes);

            return result;
        }

        public List<CommonSize> ConvertToCommonSizes(Listing listing)
        {
            var propertySizes = ConvertPropertySizes(listing);
            var Sizes = propertySizes.Where(s => s.SizeKind.ToEnum<SizeKindEnum>() != null).Select(x => new CommonSize()
                {
                    CommonSizeKind = x.SizeKind, // TODO: Investigate why alias isn't being used
                    CommonDimensions = new List<CommonDimension>()
                    {  
                        new CommonDimension() 
                        {
                            CommonAmount = x.Amount,
                            CommonDimensionsUnits = ConvertToDimensionsUnit(x.MeasureUnit)
                        }
                    }
                }
            );
            return Sizes.Any() ? Sizes.ToList() : null;
        }

        public List<CommonCharge> ConvertToCommonCharges(Specifications specifications, bool useLeaseModifiers)
        {
            string listingType = ConvertListingType();
            var chargesAndModifiers = ConvertToCommonCharges();
            if (string.IsNullOrWhiteSpace(listingType)) return chargesAndModifiers.ToList();
            
            var currencyCode = specifications.CurrencyCode?.ReturnValueOrNull();
            var taxModifer = specifications.TaxModifer.ToEnum<TaxModifierEnum>() ?? TaxModifierEnum.None;

            if (listingType.Equals("saleLease", StringComparison.OrdinalIgnoreCase) || listingType.Equals("lease", StringComparison.OrdinalIgnoreCase))
            {
                var perUnit = specifications.Measure.ToEnum<PerUnitTypeEnum>();

                //if measure unit equals to 'pp', store api only takes chargeKind as FlexRent
                var chargeKind = perUnit == PerUnitTypeEnum.person
                    ? ChargeKindEnum.FlexRent
                    : ChargeKindEnum.Rent;

                var leaseTerm = specifications.LeaseTerm.ToEnum<LeaseTermEnum>();

                if (specifications.MaxPrice != null && specifications.MaxPrice != 0)
                {
                    chargesAndModifiers = chargesAndModifiers.Prepend(new CommonCharge
                    {
                        CommonAmount = specifications.ContactBrokerForPrice ? 0 : Convert.ToDouble(specifications.MaxPrice),
                        CommonAmountKind = "Value",
                        CommonChargeModifer = useLeaseModifiers ? ChargeModifiersEnum.To.ToAlias(AliasType.StoreApi) : null,
                        CommonPerUnit = specifications.ShowPriceWithUoM == false ? null : perUnit.ToAlias(AliasType.StoreApi),
                        CommonChargeKind = chargeKind.ToAlias(AliasType.StoreApi),
                        CommonInterval = leaseTerm.ToAlias(AliasType.StoreApi),
                        CommonCurrencyCode = currencyCode,
                        CommonTaxModifer = taxModifer.ToAlias(AliasType.StoreApi),
                        CommonOnApplication = specifications.ContactBrokerForPrice ? true : false
                    });
                }

                if (specifications.MinPrice != null && specifications.MinPrice != 0)
                {
                    chargesAndModifiers = chargesAndModifiers.Prepend(new CommonCharge
                    {
                        CommonAmount = specifications.ContactBrokerForPrice ? 0 : Convert.ToDouble(specifications.MinPrice),
                        CommonAmountKind = "Value",
                        CommonChargeModifer = useLeaseModifiers ? ChargeModifiersEnum.From.ToAlias(AliasType.StoreApi) : null,
                        CommonPerUnit = specifications.ShowPriceWithUoM == false ? null : perUnit.ToAlias(AliasType.StoreApi),
                        CommonChargeKind = chargeKind.ToAlias(AliasType.StoreApi),
                        CommonInterval = leaseTerm.ToAlias(AliasType.StoreApi),
                        CommonCurrencyCode = currencyCode,
                        CommonTaxModifer = taxModifer.ToAlias(AliasType.StoreApi),
                        CommonOnApplication = specifications.ContactBrokerForPrice ? true : false
                    });
                }
            }

            if ((listingType.Equals("sale", StringComparison.OrdinalIgnoreCase) 
                || listingType.Equals("saleLease", StringComparison.OrdinalIgnoreCase)) 
                && specifications.SalePrice != null 
                && specifications.SalePrice != 0)
            {
                chargesAndModifiers = chargesAndModifiers.Prepend(new CommonCharge
                {
                    CommonAmount = specifications.ContactBrokerForPrice ? 0 : Convert.ToDouble(specifications.SalePrice ?? 0),
                    CommonAmountKind = "Value",
                    CommonPerUnit = DimensionsUnitsEnum.whole.ToAlias(AliasType.StoreApi),
                    CommonChargeKind = ChargeKindEnum.SalePrice.ToAlias(AliasType.StoreApi),
                    CommonInterval = LeaseTermEnum.once.ToAlias(AliasType.StoreApi),
                    CommonCurrencyCode = currencyCode,
                    CommonTaxModifer = taxModifer.ToAlias(AliasType.StoreApi),
                    CommonOnApplication = specifications.ContactBrokerForPrice ? true : false
                });
            }

            return chargesAndModifiers.ToList();
        }

        public IEnumerable<CommonCharge> ConvertToCommonCharges()
        {
            var chargesAndModifiers = ConvertChargesAndModifiers();
            if (chargesAndModifiers == null) return null;

            return chargesAndModifiers.Select(x => new CommonCharge()
            {
                CommonChargeKind = x.ChargeType,
                CommonCurrencyCode = x.CurrencyCode?.ReturnValueOrNull(),
                CommonChargeModifer = x.ChargeModifier.ReturnValueOrNull(),
                CommonAmount = Convert.ToDouble(x.Amount),
                CommonInterval = x.Term.ReturnValueOrNull(),
                CommonPerUnit = ConvertToPerUnitType(x.PerUnitType),
                CommonOnApplication = x.ChargeModifier == ChargeModifiersEnum.OnApplication.ToString() ? true : false,
                CommonYear = x.Year
            });
        }

        public IEnumerable<CommonCommentElement> ConvertToCommonStrapline() => ConvertToCommonCommentElement(_listing.GetListingDataAllLanguages<Headline>());

        public List<CommonLandSizeElement> ConvertToCommonTotalSize(Specifications specifications)
        {
            return new List<CommonLandSizeElement>()
            {
                new CommonLandSizeElement{
                    CommonUnits = ConvertToDimensionsUnit(specifications?.Measure),
                    CommonArea = 0,
                    CommonMinArea = specifications?.MinSpace,
                    CommonMaxArea = specifications?.MaxSpace,
                }
            };
        }
        
        public CommonParking ConvertToCommonParking()
        {
            var parkings = _listing.GetListingData<Parkings>();
            
            if (parkings != null) 
            {
                return new CommonParking() {
                    CommonRatio = parkings.Ratio ?? 0,
                    CommonRatioPer = parkings.RatioPer ?? 0,
                    CommonRatioPerUnit = string.IsNullOrWhiteSpace(parkings.RatioPerUnit) ? null : parkings.RatioPerUnit,
                    CommonParkingDetails = parkings.ParkingDetails?
                                        .Where(p => !string.IsNullOrWhiteSpace(p.ParkingType))?
                                        .Select(x => Map(x))
                                        .ToList()
                };
            }
            return null;
        }   

        private CommonParkingDetail Map(ParkingDetail p)
        {
            return new CommonParkingDetail()
            {
                CommonParkingType = p.ParkingType,
                CommonParkingSpace = p.ParkingSpace ?? 0,
                CommonParkingCharge = new List<CommonParkingCharge>()
                {
                    new CommonParkingCharge()
                    {
                        CommonInterval = string.IsNullOrWhiteSpace(p.Interval) ? null : p.Interval,
                        CommonAmount = p.Amount ?? 0,
                        CommonCurrencyCode = string.IsNullOrWhiteSpace(p.CurrencyCode) ? null : p.CurrencyCode
                    }
                }
            };
        }

        public List<CommonPointsOfInterests> ConvertToCommonPointsOfInterests()
        {
            var pointsOfInterests = _listing.GetListingDataArray<PointsOfInterests>();

            var list = pointsOfInterests?.Where(p => !string.IsNullOrWhiteSpace(p.InterestKind) && p.Places?.Count()>0).Select(x => new CommonPointsOfInterests()
            {
                CommonInterestKind = x.InterestKind,
                CommonPlaces = ConvertToCommonPlaces(x.Places)
            }).ToList();

            return list.Any() ? list : null;
        }

        public List<CommonPlace> ConvertToCommonPlaces(List<Place> places)
        {
            return places?.Select(p => new CommonPlace()
            {
                CommonName = ConvertToCommonCommentElement(p.Name),
                CommonType = ConvertToCommonCommentElement(p.Type),
                CommonDistances = new List<CommonDistance>()
                {
                    new CommonDistance() 
                    {
                        CommonDistanceUnits = !string.IsNullOrWhiteSpace(p.DistanceUnits) ? p.DistanceUnits : "mile",
                        CommonAmount = Convert.ToDouble(p.Distances)
                    }
                },
                CommonDurations = new List<CommonDuration>()
                {
                    new CommonDuration() {
                        CommonAmount = Convert.ToDouble(p.Duration),
                        CommonDistanceUnits = "minute",
                        CommonTravelMode = !string.IsNullOrWhiteSpace(p.TravelMode) ? p.TravelMode : null
                    }
                }
            }).ToList();
        }

        public List<CommonTransportationTypes> ConvertToCommonTransportationTypes()
        {
            var tansportationTypes = _listing.GetListingDataArray<TransportationTypes>();

            var list = tansportationTypes?
                .Where(p => !string.IsNullOrWhiteSpace(p.Type) && p.Places?.Count() > 0)
                .Select(x => new CommonTransportationTypes()
            {
                CommonType = x.Type.ToEnum<TransportationTypesEnum>(AliasType.Default).ToAlias(AliasType.StoreApi), 
                CommonPlaces = ConvertToCommonTransportationPlaces(x.Places)
            }).ToList();

            return list.Any() ? list : null;
        }

        public List<CommonTransportationPlace> ConvertToCommonTransportationPlaces(List<Place> places)
        {
            return places?.Select(p => new CommonTransportationPlace()
            {
                CommonName = ConvertToCommonCommentElement(p.Name),
                CommonDistances = new List<CommonDistance>()
                {
                    new CommonDistance() 
                    {
                        CommonDistanceUnits = !string.IsNullOrWhiteSpace(p.DistanceUnits) ? p.DistanceUnits : "mile",
                        CommonAmount = Convert.ToDouble(p.Distances)
                    }
                },
                CommonDurations = new List<CommonDuration>()
                {
                    new CommonDuration() {
                        CommonAmount = Convert.ToDouble(p.Duration),
                        CommonDistanceUnits = "minute",
                        CommonTravelMode = !string.IsNullOrWhiteSpace(p.TravelMode) ? p.TravelMode : null
                    }
                }
            }).ToList();
        }

        public List<CommonBrochureElement> ConvertToCommonBrochures(List<CommonBrochureElement> commonBrochures, Listing listing)
        {
            if (commonBrochures == null) commonBrochures = new List<CommonBrochureElement>();
            var brochures = ConvertBrochure(listing);
            var _brochures = brochures?
                            .Select(b => new CommonBrochureElement()
                            {
                                CommonUri = b.Url,
                                CommonUriExternal = false,
                                CommonBrochureName = b.DisplayText.RefactorString(),
                                CommonCultureCode = _listing.Region.CultureCode
                            }).ToList();
            if (_brochures != null) commonBrochures.AddRange(_brochures);
            return commonBrochures;
        }

        public List<CommonImageType> ConvertToImages(Listing listing, string imageCategory)
        {
            List<CommonImageType> commonImages = new List<CommonImageType>();
            var images = GetImages(listing, imageCategory);
            var address = $"{listing?.Name} {listing?.Address?.Street1}";

            commonImages = images?
                        .OrderBy(x => x.Order)
                        .Where(p => p.IsActive == true)
                        .Select(x => ConvertCommonImageType(x, address)).ToList();
            return commonImages;
        }

        private CommonImageType ConvertCommonImageType(ListingImage x, string address)
        {
            if (x == null) return new CommonImageType();
            string displayText = FormatImageCaption(x.DisplayText);
            return new CommonImageType(){
                CommonImageCaption = string.IsNullOrWhiteSpace(displayText) ? $"{address} - Image {x.Order}".Trim() : displayText,
                CommonAddWatermark = x.HasWatermark ?? false,
                CommonImageResources = new List<CommonImageResource>()
                {
                    new CommonImageResource
                    {
                        CommonResourceUri = x.Image.Url,
                    }   
                }
            };
        }

        private string FormatImageCaption(string displayText)
        {
            return Regex.Replace(displayText, @".(png|gif|jpg|jpeg)", "");
        }

        public CommonMicromarket ConvertToCommonMicromarket()
        {
            string[] markets = GetMicroMarket().FirstOrDefault()?.Value?.Split(":", StringSplitOptions.RemoveEmptyEntries);
            
            if (markets != null)
            {
                return new CommonMicromarket()
                {
                    CommonMicromarketName = markets[0].Trim(),
                    CommonSubMarketName = markets.Count() > 1 ? markets[1].Trim() : ""
                };
            }

            return null;
        }

        public List<CommonHighlight> ConvertToCommonHighlights(List<CommonHighlight> commonHighlights, Listing listing)
        {
            if (commonHighlights == null) commonHighlights = new List<CommonHighlight>();
            var highlights = ConvertHighlight(listing)?
                .OrderBy(x => x.Order)
                .Select(x => new CommonHighlight()
                {
                    CommonHighlights = new List<CommonCommentElement>
                    {
                        new CommonCommentElement
                        {
                            CommonCultureCode = x.CultureCode,
                            CommonText = x.Text
                        }
                    }
                }).ToList();

            // Group by translated highlight so the GL SPA will know what to do
            // See unit test `ListingAdapter_ConvertToCommonHighlights_ShouldGroupByHighlight()` for more details
            var languageCount = highlights.Select(h => h.CommonHighlights.First().CommonCultureCode).Distinct().Count();
            var groupedCommonHighlights = new List<CommonHighlight>();

            for (int i = 0; i < highlights.Count(); i=i+languageCount)
            {
                var groupedCommonCommentElements = new List<CommonCommentElement>();
                foreach(var item in highlights.Skip(i).Take(languageCount))
                {
                    var groupedCommonCommentElement = new CommonCommentElement();
                    groupedCommonCommentElement.CommonCultureCode = item.CommonHighlights.First().CommonCultureCode;
                    groupedCommonCommentElement.CommonText = item.CommonHighlights.First().CommonText;
                    groupedCommonCommentElements.Add(groupedCommonCommentElement);
                }
                var groupedCommonHighlight = new CommonHighlight();
                groupedCommonHighlight.CommonHighlights = groupedCommonCommentElements;
                groupedCommonHighlights.Add(groupedCommonHighlight);
            }

            return groupedCommonHighlights;
        }

        public List<string> ConvertToCommonAspects() {
            var result = new List<string>();
            
            var listingType = _listing.GetListingData<ListingType>();
            if(!string.IsNullOrWhiteSpace(listingType?.Value)) 
            {
                var listingTypeAliases = 
                    listingType.Value
                    .ToEnum<AspectsEnum>()?
                    .ExpandListingType()
                    .Select(a => a.ToAlias(AliasType.StoreApi));

                if (!listingTypeAliases?.Any() ?? true) 
                {
                    listingTypeAliases = new List<string>
                    {
                        AspectsEnum.lease.ToAlias(AliasType.StoreApi)
                    };
                };
                
                result.AddRange(listingTypeAliases);
            }

            var aspects = _listing.GetListingDataArray<Aspect>().Select(a => a.Value);
            result.AddRange(aspects);
            return result;
        }

        public string ConvertToDimensionsUnit(string unit)
        {
            return (
                unit.ToEnum<DimensionsUnitsEnum>() ?? 
                DimensionsUnitsEnum.sqft
            ).ToAlias(AliasType.StoreApi);
        }
        public string ConvertToPerUnitType(string unit)
        {
            return unit.ToEnum<PerUnitTypeEnum>()?.ToAlias(AliasType.StoreApi);
        }

        public string ConvertToPropertyType()
        {
            return (
                _listing
                    .GetListingData<PropertyType>()
                    .Value
                    .ToEnum<PropertyTypeEnum>() ?? 
                PropertyTypeEnum.office
            ).ToAlias(AliasType.StoreApi);
        }

        public List<string> ConvertToUseClass()
        {
            string useClass = ConvertPropertyUseClass();
            if (!string.IsNullOrWhiteSpace(useClass))
            {
                return new List<string> { useClass }; 
            } 
            return null;
        }

        public List<CommonExternalRatings> ConvertToCommonExternalRatings()
        {
            var externalRatings = _listing.GetListingDataArray<ExternalRatings>();
            List<CommonExternalRatings> commonExternalRatings = new List<CommonExternalRatings>();

            if (externalRatings != null)
            {
                foreach (var rating in externalRatings)
                {
                    string ratingLevel = rating.RatingLevel.ToEnum<EnergyRatingEnum>().ToAlias(AliasType.StoreApi);
                    if (!string.IsNullOrEmpty(rating.RatingLevel) && !string.IsNullOrEmpty(ratingLevel))
                    {
                        CommonExternalRatings commonExternalRating = new CommonExternalRatings();

                        commonExternalRating.CommonRatingType = rating.RatingType;
                        commonExternalRating.CommonRatingLevel = ratingLevel;
                        commonExternalRatings.Add(commonExternalRating);
                    }
                }
            }
            return commonExternalRatings;
        }

        public CommonSourceSystem ConvertToSourceSystem(string miqId) => !String.IsNullOrWhiteSpace(miqId) ? new CommonSourceSystem() { CommonName = "EDP ID", CommonId = miqId} : null;

        public string ConvertListingType() => _listing.GetListingData<ListingType>()?.Value;

        public string ConvertPropertySubType() => 
            _listing.GetListingData<PropertySubType>().Value
            .ToEnum<PropertySubTypeEnum>()
            .ToAlias(AliasType.StoreApi);

        public string ConvertPropertyUseClass() => 
            _listing.GetListingData<PropertyUseClass>().Value
            .ToEnum<UseClassEnum>()
            .ToAlias(AliasType.StoreApi);

        public PublishingState GetPublishingState() => _listing.GetListingData<PublishingState>();
        
        public bool? GetSyndicationFlag() => _listing.GetListingData<SyndicationFlag>()?.Value;

        public string GetSyndicationMarket => _listing.GetListingData<SyndicationFlag>()?.Market;

        public string ConvertToEnergyRating() => _listing.GetListingData<EnergyRating>().Value.ReturnValueOrNull();

        public string ConvertToWebsite() => _listing.GetListingData<Website>().Value.ReturnValueOrNull();

        public int? ConvertToFloors() => _listing.GetListingData<Floor>().Value;

        public int? ConvertToYearBuilt() => _listing.GetListingData<YearBuilt>()?.Value;

        public string ConvertToOperator() => _listing.GetListingData<Operator>().Value.ReturnValueOrNull();

        public string ConvertVideoLink(Listing listing) => listing.GetListingData<Video>().Value;

        public string ConvertToWalkthrough(Listing listing) => listing.GetListingData<WalkThrough>().Value.ReturnValueOrNull();

        public Specifications ConvertSpecifications(Listing listing) => listing.GetListingData<Specifications>();

        public IEnumerable<Brochure> ConvertBrochure(Listing listing) => listing.GetListingDataArray<Brochure>();

        public IEnumerable<ListingImage> GetImages(Listing listing, string imageCategory) => listing.ListingImage?.Where(l => l.ImageCategory == imageCategory).ToList();

        public IEnumerable<MicroMarket> GetMicroMarket() => _listing.GetListingDataArray<MicroMarket>();

        public IEnumerable<Highlight> ConvertHighlight(Listing listing) => listing.GetListingDataAllLanguages<Highlight>();
        
        public IEnumerable<ChargesAndModifiers> ConvertChargesAndModifiers() => _listing.GetListingDataArray<ChargesAndModifiers>().Where(x => !string.IsNullOrEmpty(x.ChargeType));

        public IEnumerable<PropertySize> ConvertPropertySizes(Listing listing) => listing.GetListingDataArray<PropertySize>();
    }
}
