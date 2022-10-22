using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Services.Business.BulkUpload;
using dataentry.Services.Business.Configs;
using dataentry.Services.Business.Contacts;
using dataentry.Services.Business.Listings;
using dataentry.ViewModels.GraphQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BulkUploadController : ControllerBase
    {
        public class BulkUploadException : Exception
        {
            public BulkUploadErrorLevel ErrorLevel { get; set; }

            public BulkUploadException(BulkUploadErrorLevel errorLevel, string message, Exception innerExcepion = null) : base(message, innerExcepion)
            {
                ErrorLevel = errorLevel;
            }
        }

        public enum BulkUploadErrorLevel
        {
            Warning,
            Error
        }

        private const int AGENTS_MAX = 4;
        private const int ASPECTS_MAX = 15;
        private const int CHARGEKINDS_MAX = 3;
        private const int HIGHLIGHTS_MAX = 5;

        private static readonly string[] MINPRICE_COLUMN_NAMES = { "MinimumPrice", "MinimumLeasePrice" };
        private static readonly string[] MAXPRICE_COLUMN_NAMES = { "MaximumPrice", "MaximumLeasePrice" };

        private readonly IBulkUploadService _bulkUploadProcessor;
        private readonly IListingService _listingService;
        private readonly IConfigService _configService;
        private readonly ILogger<BulkUploadController> _logger;

        public BulkUploadController(
            IBulkUploadService bulkUploadProcessor,
            IListingService listingService,
            IConfigService configService,
            ILogger<BulkUploadController> logger)
        {
            _bulkUploadProcessor = bulkUploadProcessor ?? throw new ArgumentNullException(nameof(bulkUploadProcessor));
            _listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
            _configService = configService ?? throw new ArgumentNullException(nameof(configService));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            var exceptions = new List<BulkUploadException>();
            bool flexMode = false;
            IEnumerable<DataRow> properties;
            DataTable excelProperties = null;

            // Only xlsx allowed
            if (Path.GetExtension(file.FileName) != ".xlsx")
            {
                exceptions.Add(new BulkUploadException(BulkUploadErrorLevel.Error, "File extension should be \".xlsx\""));
                properties = new List<DataRow>();
            }
            else
            {
                // Get the first sheet of the excel file
                excelProperties = _bulkUploadProcessor.GetDataSet(file.OpenReadStream()).Tables["Properties"];
                if (excelProperties == null)
                {
                    exceptions.Add(new BulkUploadException(BulkUploadErrorLevel.Error, "Missing \"Properties\" workbook."));
                    properties = new List<DataRow>();
                }
                else
                {
                    if (excelProperties.Columns.Contains("FlexSpaceName"))
                    {
                        flexMode = true;
                    }
                    else if (excelProperties.Columns.Contains("SpaceName"))
                    {
                        flexMode = false;
                    }

                    // Get all Buildings from the data set
                    properties = excelProperties.AsEnumerable()
                        .GroupBy(x => x.Field<string>("PropertyRecordName"))
                        .Select(group => group.FirstOrDefault());
                }
            }

            var vms = new List<ListingViewModel>();

            //TODO: bulk upload region support, currently using default region
            var cultureCode = await _configService.GetDefaultCultureCode();

            foreach (var property in properties)
            {
                // Map property to listing view model
                var vm = new ListingViewModel()
                {
                    BulkUploadFileName = file.FileName,
                    IsBulkUpload = true,
                    PropertyRecordName = ProcessField<string>(property, "PropertyRecordName", exceptions) ?? "",
                    PropertyType = ConvertPropertyType(ProcessField<string>(property, "PropertyType", exceptions)?.ToLower() ?? ""),
                    ListingType = ConvertListingType(ProcessField<string>(property, "ListingType", exceptions)?.ToLower() ?? "") ?? "", // Need Validation
                    PropertyName = ProcessField<string>(property, "PropertyBuildingName", exceptions),
                    Street = ProcessField<string>(property, "AddressLine1", exceptions),
                    Street2 = ProcessField<string>(property, "AddressLine2", exceptions),
                    City = ProcessField<string>(property, "City", exceptions),
                    StateOrProvince = ProcessField<string>(property, "StateOrProvince", exceptions), // Need Validation
                    PostalCode = ProcessField<double?>(property, "PostalCode", exceptions)?.ToString() ?? "",
                    Lat = Convert.ToDecimal(ProcessField<double?>(property, "Latitude", exceptions) ?? 0),
                    Lng = Convert.ToDecimal(ProcessField<double?>(property, "Longitude", exceptions) ?? 0),
                    Headline = ProcessTextType(property, cultureCode, "Headline", exceptions),
                    BuildingDescription = ProcessTextType(property, cultureCode, "PropertyDescription", exceptions),
                    LocationDescription = ProcessTextType(property, cultureCode, "LocationDescription", exceptions),
                    Website = ProcessField<string>(property, "Website", exceptions),
                    Specifications = new SpecificationsViewModel()
                    {
                        LeaseTerm = ConvertLeaseTerm(ProcessField<string>(property, "LeaseTerm", exceptions)),
                        LeaseType = ConvertLeaseType(ProcessField<string>(property, "LeaseType", exceptions)),
                        LeaseRateType = ConvertLeaseRateType(ProcessField<string>(property, "LeaseRateType", exceptions)),
                        Measure = ProcessField<string>(property, "AreaMeasure", exceptions)?.ToLower(),
                        MinSpace = Convert.ToInt32(ProcessField<double?>(property, "MinSpace", exceptions) ?? 0),
                        MaxSpace = Convert.ToInt32(ProcessField<double?>(property, "MaxSpace", exceptions) ?? 0),
                        TotalSpace = Convert.ToInt32(ProcessField<double?>(property, "TotalSpaceAvailable", exceptions) ?? 0),
                        MinPrice = Convert.ToDecimal(ProcessField<double?>(property, MINPRICE_COLUMN_NAMES, exceptions) ?? 0),
                        MaxPrice = Convert.ToDecimal(ProcessField<double?>(property, MAXPRICE_COLUMN_NAMES, exceptions) ?? 0),
                        SalePrice = Convert.ToDecimal(ProcessField<double?>(property, "SalePrice", exceptions) ?? 0),
                        ContactBrokerForPrice = ProcessField<bool>(property, "ContactBrokerForPrice", exceptions),
                        TaxModifer = ProcessField<string>(property, "TaxModifier", exceptions),
                    },
                };

                if (flexMode)
                {
                    vm.Operator = ProcessField<string>(property, "Operator", exceptions);
                }

                // Add MicroMarkets
                vm.MicroMarkets =
                    (ProcessField<string>(property, "MicroMarkets", exceptions) ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select((microMarket, index) => new MicroMarketsViewModel { Value = microMarket, Order = index })
                    .ToList();
                if (!vm.MicroMarkets.Any()) vm.MicroMarkets = null;

                // Get highlight values from excel
                var highlightValues = new List<OrderedTextTypeViewModel>();
                for (int i = 0; i < HIGHLIGHTS_MAX; i++)
                {
                    var highlight = ProcessOrderedTextType(property, cultureCode, $"Highlight{i}", exceptions);
                    highlight.Order = i;
                    if (!string.IsNullOrWhiteSpace(highlight.Text)) highlightValues.Add(highlight);
                }

                // Get aspects from excel
                var aspectValues = new List<string>();
                for (int i = 0; i < ASPECTS_MAX; i++)
                {
                    var aspect = ProcessField<string>(property, $"Spec{i}", exceptions);

                    if (!string.IsNullOrWhiteSpace(aspect))
                        aspectValues.Add(ConvertSpecs(aspect));
                }
                vm.Aspects = aspectValues;

                // Get chargekind from excel
                var chargeKindValues = new List<ChargesAndModifiersViewModel>();
                for (int i = 0; i < CHARGEKINDS_MAX; i++)
                {
                    var chargeKind = new ChargesAndModifiersViewModel();
                    chargeKind.ChargeType = ProcessField<string>(property, $"ChargeKind_{i}", exceptions);
                    if (!string.IsNullOrWhiteSpace(chargeKind.ChargeType))
                    {
                        chargeKind.ChargeModifier = ProcessField<string>(property, $"ChargeModifier", exceptions);
                        chargeKind.Term = ProcessField<string>(property, $"ChargeInterval_{i}", exceptions);
                        chargeKind.Amount = Convert.ToInt32(ProcessField<double?>(property, $"ChargeAmount_{i}", exceptions) ?? 0);
                        chargeKind.PerUnitType = ProcessField<string>(property, $"ChargePerUnitType_{i}", exceptions);
                        vm.ChargesAndModifiers = chargeKindValues;
                    }
                }

                // Get spaces from properties table
                var excelSpaces = excelProperties?.AsEnumerable()
                    .Where(x => x.Field<string>("PropertyRecordName") == property.Field<string>("PropertyRecordName"));

                // Create new listing for each space record
                var spaces = new List<SpacesViewModel>();
                if (excelSpaces != null)
                {
                    foreach (var excelSpace in excelSpaces)
                    {
                        var spaceVM = new SpacesViewModel();


                        if (flexMode)
                        {
                            spaceVM.Name = ProcessTextType(excelSpace, cultureCode, "FlexSpaceName", exceptions);
                            spaceVM.Name.Single().Text = ConvertFlexNames(spaceVM.Name.Single().Text);
                            spaceVM.Status = ProcessField<string>(excelSpace, "FlexSpaceStatus", exceptions)?.ToLower(); // Add Validation
                            spaceVM.AvailableFrom = ProcessField<DateTime?>(excelSpace, "FlexSpaceAvailableFrom", exceptions);
                            spaceVM.Specifications = new SpecificationsViewModel()
                            {
                                Measure = ConvertFlexMeasure(ProcessField<string>(excelSpace, "SpaceMeasure", exceptions)?.ToLower() ?? ""),
                                LeaseTerm = ConvertLeaseTerm(ProcessField<string>(excelSpace, "SpaceLeaseTerm", exceptions) ?? ""),
                                MinSpace = Convert.ToInt32(ProcessField<double?>(excelSpace, "SpaceDesksMinimum", exceptions) ?? 0),
                                MaxSpace = Convert.ToInt32(ProcessField<double?>(excelSpace, "SpaceDeskMaximum", exceptions) ?? 0),
                                TotalSpace = Convert.ToInt32(ProcessField<double?>(excelSpace, "SpaceDeskTotal", exceptions) ?? 0),
                                MinPrice = Convert.ToDecimal(ProcessField<double?>(excelSpace, "SpacePriceFrom", exceptions) ?? 0),
                                SalePrice = Convert.ToDecimal(ProcessField<double?>(property, "SpaceSalePrice", exceptions) ?? 0),
                                ContactBrokerForPrice = ProcessField<bool>(excelSpace, "SpaceContactBrokerForPrice", exceptions),
                            };
                        }
                        else
                        {
                            spaceVM.Name = ProcessTextType(excelSpace, cultureCode, "SpaceName", exceptions);
                            spaceVM.Status = ProcessField<string>(excelSpace, "SpaceStatus", exceptions)?.ToLower(); // Add Validation
                            spaceVM.SpaceType = ConvertPropertyType(ProcessField<string>(excelSpace, "SpaceType", exceptions)?.ToLower());
                            spaceVM.AvailableFrom = ProcessField<DateTime?>(excelSpace, "SpaceAvailableFrom", exceptions);
                            spaceVM.Specifications = new SpecificationsViewModel()
                            {
                                Measure = ProcessField<string>(excelSpace, "SpaceMeasure", exceptions)?.ToLower() ?? "",
                                TotalSpace = Convert.ToInt32(ProcessField<double?>(excelSpace, "SpaceSize", exceptions) ?? 0),
                                MaxPrice = Convert.ToInt32(ProcessField<double?>(excelSpace, "SpacePrice", exceptions) ?? 0),
                                SalePrice = Convert.ToDecimal(ProcessField<double?>(property, "SpaceSalePrice", exceptions) ?? 0),
                                ContactBrokerForPrice = ProcessField<bool>(excelSpace, "SpaceContactBrokerForPrice", exceptions),
                            };
                        }

                        if (!string.IsNullOrWhiteSpace(spaceVM.Name.SingleOrDefault()?.Text)
                            || !string.IsNullOrWhiteSpace(spaceVM.Status)
                            || !string.IsNullOrWhiteSpace(spaceVM.SpaceType))
                        {
                            spaces.Add(spaceVM);
                        }
                    }

                    if (spaces.Any())
                    {
                        vm.Spaces = spaces;
                    }
                }

                // Get highlight values from excel
                var contacts = new List<ContactsViewModel>();

                for (int i = 0; i < AGENTS_MAX; i++)
                {
                    var contact = new ContactsViewModel
                    {
                        FirstName = ProcessField<string>(property, $"AgentFirstName_{i}", exceptions),
                        LastName = ProcessField<string>(property, $"AgentLastName_{i}", exceptions),
                        Email = ProcessField<string>(property, $"AgentEmail_{i}", exceptions)?.Trim().ToLower(),
                        Phone = ProcessField<string>(property, $"AgentPhoneNumber_{i}", exceptions),
                        Location = ProcessField<string>(property, $"AgentBrokerageOffice_{i}", exceptions),
                        AdditionalFields = new AdditionalFieldsViewModel
                        {
                            License = ProcessField<string>(property, $"AgentLicense_{i}", exceptions),
                        }
                    };

                    if (!string.IsNullOrWhiteSpace(contact.Email))
                    {
                        var broker = await _listingService.GetBrokerByEmail(contact.Email);
                        contact.ContactId = broker?.ContactId ?? 0;
                    }

                    if (ContactHasValues(contact))
                        contacts.Add(contact);
                }

                if (contacts.Any())
                    vm.Contacts = contacts;

                vm.TeamNames = new List<string>();

                vms.Add(vm);
            }


            var contactIds = new Dictionary<string, int>();
            int documentCount = 0;
            if (!exceptions.Any(ex => ex.ErrorLevel == BulkUploadErrorLevel.Error))
            {
                foreach (var vm in vms)
                {
                    if (vm.Contacts != null)
                    {
                        foreach (var contact in vm.Contacts)
                        {
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(contact.Email))
                                {
                                    if (contactIds.TryGetValue(contact.Email, out int id))
                                    {
                                        contact.ContactId = id;
                                    }
                                    else
                                    {
                                        var existingContact = await _listingService.GetBrokerByEmail(contact.Email);
                                        if (existingContact != null)
                                        {
                                            contact.ContactId = existingContact.ContactId;
                                            contact.Avatar = existingContact.Avatar; // Don't change existing avatars during bulk upload.
                                            contactIds.Add(contact.Email, existingContact.ContactId);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(new BulkUploadException(BulkUploadErrorLevel.Error, $"Error while processing contact with email \"{contact.Email}\" for property \"{vm.PropertyRecordName}\"", ex));
                            }
                        }
                    }

                    try
                    {
                        await _listingService.CreateListingAsync(User, vm);
                        documentCount++;
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(new BulkUploadException(BulkUploadErrorLevel.Error, $"Error while saving property with name \"{vm.PropertyName}\"", ex));
                    }
                }
            }

            if (documentCount == 0)
            {
                exceptions.Add(new BulkUploadException(BulkUploadErrorLevel.Warning, "No properties were added"));
            }

            IEnumerable<string> errorMessages = null;
            if (exceptions.Any())
            {
                errorMessages = exceptions.Select(ex =>
                {
                    var result = ex.Message;
                    if (ex.InnerException != null)
                    {
                        result += $": {ex.InnerException?.GetType().Name} - {ex.InnerException?.Message}";
                    }
                    return result;
                });
                _logger.LogError($"User encountered errors during a bulk upload while processing file {file.FileName}\n{documentCount} property(ies) added\nError messages:\n\n{string.Join('\n', errorMessages)}");
            }

            return Ok(new { Count = documentCount, FileName = file.FileName, Errors = errorMessages });
        }

        private List<TextTypeViewModel> ProcessTextType(DataRow row, string cultureCode, string columnName, List<BulkUploadException> exceptions)
        {
            return new List<TextTypeViewModel>
                    {
                        new TextTypeViewModel
                        {
                            CultureCode = cultureCode,
                            Text = ProcessField<string>(row, columnName, exceptions)
                        }
                    };
        }
        private OrderedTextTypeViewModel ProcessOrderedTextType(DataRow row, string cultureCode, string columnName, List<BulkUploadException> exceptions)
        {   
            return new OrderedTextTypeViewModel
            {
                CultureCode = cultureCode,
                Text = ProcessField<string>(row, columnName, exceptions)
            };
        }

        private T ProcessField<T>(DataRow row, string columnName, List<BulkUploadException> exceptions)
        {
            try
            {
                return row.GetFieldIfExist<T>(columnName);
            }
            catch (Exception ex)
            {
                // row number: +1 for header row, +1 for 0-based index
                var rowNumber = row.Table.Rows.IndexOf(row) + 2;
                exceptions.Add(new BulkUploadException(BulkUploadErrorLevel.Error, $"Could not process column \"{columnName}\" on row {rowNumber}", ex));
                return default;
            }
        }

        private T ProcessField<T>(DataRow row, IEnumerable<string> columnNames, List<BulkUploadException> exceptions)
        {
            T result = default;
            foreach (var columnName in columnNames)
            {
                if (row.Table.Columns.Contains(columnName))
                {
                    result = ProcessField<T>(row, columnName, exceptions);
                    if (!result.Equals(default)) break;
                }
            }
            
            return result;
        }

        private bool ContactHasValues(ContactsViewModel contact)
        {
            if (contact.FirstName != null &&
                contact.LastName != null &&
                contact.Email != null)
                return true;

            return false;
        }

        private static string ConvertPropertyType(string value)
        {
            value = string.IsNullOrEmpty(value) ? default : value.ToLower().Trim();
            switch (value)
            {
                case "office - traditional": return "office";
                case "office - coworking": return "officecoworking";
                case "flex": return "flexindustrial";
                case "flexible space": return "flex";
                case "specialty": return "specialPurpose";
                case "life sciences": return "lifeSciences";
                case "data center": return "dataCentre";
                default: return value;
            }
        }

        private static string ConvertListingType(string value)
        {
            value = string.IsNullOrEmpty(value) ? "" : value.ToLower().Trim();
            switch (value)
            {
                case "for sale": return "sale";
                case "for lease": return "lease";
                case "for sale/lease": return "salelease";
                default: return value;
            }
        }

        private static string ConvertLeaseRateType(string value)
        {
            var normalizedValue = string.IsNullOrEmpty(value) ? default : value.ToLower().Trim();
            switch (normalizedValue)
            {
                case "full service gross": return "FullServiceGross";
                case "modified gross": return "ModifiedGross";
                case "triple net": return "TripleNet";
                case "net": return "Net";
                default: return value;
            }
        }

        private static string ConvertSpecs(string value)
        {
            value = string.IsNullOrEmpty(value) ? default : value.ToLower().Trim();
            switch (value)
            {
                case "lifts": return "hasElevators";
                case "car parking": return "hasParking";
                case "f&b facility": return "hasFoodService-Cafeteria";
                case "24*7 security": return "hasSecurity-24hours/7days";
                case "power backup": return "hasGenerator";
                case "gymnasium": return "hasGym";
                case "tea and coffee included": return "hasTeaCoffee";
                case "centrally air-conditioned": return "hasAir-Conditioning-Central";
                default: return value; 
            }
        }

        private static string ConvertFlexMeasure(string value)
        {
            value = string.IsNullOrEmpty(value) ? default : value.ToLower().Trim();
            switch (value)
            {
                case "per person": return "person";
                case "per desk": return "desk";
                case "per room": return "room";
                default: return value;
            }
        }

        private static string ConvertFlexNames(string value)
        {
            value = string.IsNullOrEmpty(value) ? default : value.ToLower().Trim();
            switch (value)
            {
                case "host desk": return "hot";
                case "serviced offices": return "serviced";
                case "fixed desk": return "fixed";
                default: return value;
            }
        }

        private static string ConvertLeaseType(string value)
        {
            var normalizedValue = string.IsNullOrEmpty(value) ? default : value.ToLower().Trim();
            switch (normalizedValue)
            {
                case "direct": return "LeaseHold";
                case "sublease": return "SubLease";
                default: return value;
            }
        }

        private static string ConvertLeaseTerm(string value)
        {
            value = value?.ToLower();
            if (value == LeaseTermEnum.annually.ToString()) 
                value = LeaseTermEnum.yearly.ToString();
            return value;
        }
    }
}