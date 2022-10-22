using dataentry.Data.DBContext.Model;
using dataentry.Repository;
using dataentry.Services.Business.Users;
using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using dataentry.Services.Business.Publishing;
using dataentry.Extensions;
using dataentry.Data.Enums;
using dataentry.Services.Business.Regions;
using dataentry.Services.Integration.Edp.Consumption;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using dataentry.Utility;

namespace dataentry.Services.Business.Listings
{
    public class ListingService : IListingService
    {
        private readonly IDataEntryRepository _dataEntryRepository;
        private readonly ApplicationUserManager _userManager;
        private readonly IListingMapper _listingMapper;
        private readonly IUserMapper _userMapper;
        private readonly ITeamMapper _teamMapper;
        private readonly ILogger _logger;
        private readonly IPublishingService _publishingService;
        private readonly IListingSerializer _listingSerializer;
        private readonly IConsumptionService _edpGraphQLService;
        private readonly IJsonDeltaEvaluator _jsonDeltaEvaluator;

        public ListingService(
            IDataEntryRepository dataEntryRepository,
            ApplicationUserManager userManager,
            IListingMapper listingMapper,
            IUserMapper userMapper,
            ITeamMapper teamMapper,
            ILogger<ListingService> logger,
            IPublishingService publishingService,
            IListingSerializer listingSerializer,
            IConsumptionService edpGraphQLService,
            IJsonDeltaEvaluator jsonDeltaEvaluator
            )
        {
            _dataEntryRepository = dataEntryRepository ?? throw new ArgumentNullException(nameof(dataEntryRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _listingMapper = listingMapper ?? throw new ArgumentNullException(nameof(listingMapper));
            _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
            _teamMapper = teamMapper ?? throw new ArgumentNullException(nameof(teamMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _publishingService = publishingService ?? throw new ArgumentNullException(nameof(publishingService));
            _listingSerializer = listingSerializer ?? throw new ArgumentNullException(nameof(listingSerializer));
            _edpGraphQLService = edpGraphQLService ?? throw new ArgumentNullException(nameof(edpGraphQLService));
            _jsonDeltaEvaluator = jsonDeltaEvaluator ?? throw new ArgumentNullException(nameof(jsonDeltaEvaluator));
        }

        public async Task<ListingViewModel> CreateListingAsync(ClaimsPrincipal user, ListingViewModel vm, bool imported = false)
        {
            var listing = new Listing { IsParent = true };

            await PrepareListingForCreateOrUpdate(user, vm, listing);

            if (imported)
            {
                listing.CreatedAt = vm.DateCreated;
                listing.SetListingData(new PublishingState{
                    DateListed = vm.DateListed,
                    DateUpdated = listing.UpdatedAt,
                    Value = PublishingStateEnum.Published.ToAlias()
                });
            }

            await _dataEntryRepository.AddListing(listing, user, vm.UserNames, vm.TeamNames, ignoreDuplicateExternalId: !imported);
            SyncIDs(vm, listing);

            _logger.LogInformation($"User {user.Identity?.Name} has created listing ID: {listing.ID} on {DateTimeOffset.Now.ToString()}.");

            if (!imported)
            {
                vm = (await _publishingService.PreviewListingAsync(listing.ID, user)) ?? vm;

                if (_edpGraphQLService.Enabled 
                    && !string.IsNullOrEmpty(vm.MiqId) 
                    && int.TryParse(vm.MiqId, out var miqId))
                {
                    // Save original copy from MIQ to detect deltas later.
                    var edpVm = await _edpGraphQLService.GetListingByMiqId(miqId, listing.RegionID, false);
                    await _dataEntryRepository.UpdateListingSerialization(vm.Id, ListingSerializationType.LastImport, _listingSerializer.Serialize(edpVm));
                }
            }

            return vm;
        }

        public async Task<ListingViewModel> UpdateListingAsync(ClaimsPrincipal user, ListingViewModel vm)
        {
            Listing listing = await _dataEntryRepository.GetListingByID(vm.Id, user);

            await PrepareListingForCreateOrUpdate(user, vm, listing);

            var isSuccess = await _dataEntryRepository.UpdateListing(listing, user, vm.UserNames, vm.TeamNames);
            if (isSuccess == null)
            {
                throw new SecurityException("User does not have access to resource");
            }

            SyncIDs(vm, listing);

            _logger.LogInformation($"User {user.Identity?.Name} has updated listing ID: {listing.ID} on {DateTimeOffset.Now.ToString()}.");

            vm = (await _publishingService.PreviewListingAsync(listing.ID, user)) ?? vm;

            if (!string.IsNullOrEmpty(vm.MiqId) && vm.Deltas != null && vm.Deltas.Any())
            {
                // Update our version of the MIQ data with the deltas we detected on the get call.
                // Now our version will match MIQ so we don't present the deltas again.
                // This avoids a 2nd call to MIQ, and race conditions with MIQ updates between get and update calls will be avoided.
                var edpVm = await _dataEntryRepository.GetListingSerialization(vm.Id, ListingSerializationType.LastImport);
                edpVm = ApplyListingDeltas(vm, edpVm);
                await _dataEntryRepository.UpdateListingSerialization(vm.Id, ListingSerializationType.LastImport, edpVm);
            }

            return vm;
        }

        private static void SyncIDs(ListingViewModel vm, Listing listing)
        {
            vm.Id = listing.ID;
            vm.ExternalId = listing.ExternalID;
            vm.PreviewId = listing.PreviewID;

            if (listing.Spaces == null || vm.Spaces == null) return;
            var dbSpaces = listing.Spaces.OrderBy(s => s.GetListingData<SortOrder>().Value).ToList();
            var vmSpaces = vm.Spaces.ToList();

            for (int i = 0; i < dbSpaces.Count && i < vmSpaces.Count; i++)
            {
                var dbSpace = dbSpaces[i];
                var vmSpace = vmSpaces[i];

                vmSpace.Id = dbSpace.ID;
                vmSpace.ExternalId = dbSpace.ExternalID;
                vmSpace.PreviewId = dbSpace.PreviewID;
            }
        }

        private async Task PrepareListingForCreateOrUpdate(ClaimsPrincipal user, ListingViewModel vm, Listing listing)
        {
            _listingMapper.Map(listing, vm);
            listing.ListingBroker = (await AddOrUpdateBrokers(vm.Contacts, listing.ListingBroker)).ToList();
            listing.ListingImage
                .Where(x => x.IsUserOverride == true && string.IsNullOrWhiteSpace(x.OverridedBy)).ToList()
                .ForEach(c => c.OverridedBy = user.Identity?.Name);            
        }

        public async Task<bool> DeleteListingAsync(ClaimsPrincipal user, int id)
        {
            await _publishingService.UnPreviewListingAsync(id, user);

            var isSuccess = await _dataEntryRepository.DeleteListing(id, user);

            if (!isSuccess)
                throw new SecurityException("User does not have access to resource");
            _logger.LogInformation($"User {user.Identity?.Name} has deleted listing ID: {id} on {DateTimeOffset.Now.ToString()}.");

            return isSuccess;
        }

        public async Task<ListingViewModel> GetListingById(ClaimsPrincipal user, int id, UserLookupOptions options)
        {
            var listing = await _dataEntryRepository.GetListingByID(id, user);
            var result = _listingMapper.Map(listing);
            await AttachUsers(result, options);

            if (_edpGraphQLService.Enabled
                && !string.IsNullOrEmpty(result.MiqId) 
                && int.TryParse(result.MiqId, out var miqId))
            {
                // Find differences between EDP's current version and the version we saved during create/update
                var edpVm = await _edpGraphQLService.GetListingByMiqId(miqId, listing.RegionID, false);
                var original = await _dataEntryRepository.GetListingSerialization(result.Id, ListingSerializationType.LastImport);
                result.Deltas = FindListingDeltas(original, edpVm);
            }

            return result;
        }

        public async Task<IEnumerable<SpacesViewModel>> GetEdpSpacesById(ClaimsPrincipal user, int id, UserLookupOptions options)
        {
            var listing = await _dataEntryRepository.GetListingByID(id, user);
            var result = _listingMapper.Map(listing);
            var currentSpaces = result?.Spaces?.ToList() ?? new List<SpacesViewModel>();
            var currentSpacesIds = new HashSet<string>(currentSpaces?.Select(p => p.MiqId));

            if (_edpGraphQLService.Enabled
                && !string.IsNullOrEmpty(result.MiqId) 
                && int.TryParse(result.MiqId, out var miqId))
            {
                var edpVm = await _edpGraphQLService.GetListingByMiqId(miqId, listing.RegionID, false);
                var edpSpaces = edpVm?.Spaces;
                var newSpaces = edpSpaces?.Where(p => !currentSpacesIds.Contains(p.MiqId));
                if (newSpaces != null) currentSpaces.AddRange(newSpaces);
            }
            return currentSpaces;
        }

        public async Task<IEnumerable<ListingViewModel>> GetListings(ClaimsPrincipal user, int? skip, int? take, UserLookupOptions options, IEnumerable<FilterViewModel> filterOptionsViewModel, string regionID)
        {
            var userId = _userManager.GetUserId(user);
            List<FilterOption> filtersOptions = Map(filterOptionsViewModel);
            var isAdmin = await _userManager.IsAdminInRegionAsync(userId, regionID);
            
            var query = await _dataEntryRepository.GetAllListings(user, isAdmin, filtersOptions, regionID, skip, take );
            var result = query.Select(l => _listingMapper.Map(l)).ToList();
            await AttachUsers(result, options);

            return result;
        }

        public async Task<IEnumerable<ListingViewModel>> GetEdpImportListings(ClaimsPrincipal user, int id, UserLookupOptions options)
        {
            var listings = await _dataEntryRepository.GetEdpImportListings(user, id);
            var results = listings?.Select(l => _listingMapper.Map(l)).ToList();
            
            foreach (var listing in results)
            {
                listing.Spaces = await GetEdpSpacesById(user, listing.Id, null);
                await AttachUsers(listing, options);
            }
            return results;
        }

        public async Task<int> GetListingsCount(ClaimsPrincipal user, UserLookupOptions options, IEnumerable<FilterViewModel> filterOptionsViewModel, string regionID)
        {
            var userId = _userManager.GetUserId(user);
            List<FilterOption> filtersOptions = Map(filterOptionsViewModel);
            var isAdmin = await _userManager.IsAdminInRegionAsync(userId, regionID);
            var count = await _dataEntryRepository.GetListingsCount(user, isAdmin, filtersOptions, regionID);
            return count;
        }

        public async Task<ContactsViewModel> GetBrokerByEmail(string email)
        {
            var broker = await _dataEntryRepository.GetBrokerByEmail(email);
            if (broker == null) return null;
            return _listingMapper.Map(broker);
        }

        private async Task AttachUsers(IEnumerable<ListingViewModel> vms, UserLookupOptions options)
        {
            foreach (var vm in vms)
            {
                await AttachUsers(vm, options);
            }
        }

        private async Task AttachUsers(ListingViewModel vm, UserLookupOptions options)
        {
            if (options.IncludeUsers)
            {
                var users = await _userManager.GetUsersForListingAsync(vm.Id);
                var userVms = new List<UserViewModel>();
                foreach (var listingUser in users)
                {
                    var userVm = await _userMapper.Map(listingUser, options);
                    userVms.Add(userVm);
                }
                vm.Users = userVms;

                var teams = await _userManager.GetRolesForListingAsync(vm.Id);
                var teamVms = new List<TeamViewModel>();
                foreach (var team in teams)
                {
                    if (options.IncludeTeamMembers)
                    {
                        var teamMembers = await _userManager.GetUsersInRoleAsync(team.Name);
                        teamVms.Add(await _teamMapper.Map(team, teamMembers, options));
                    }
                    else
                    {
                        teamVms.Add(_teamMapper.Map(team, options));
                    }
                    var teamVm = _teamMapper.Map(team, options);
                }
                vm.Teams = teamVms;
            }

            if (options.IncludeOwner)
            {
                vm.Owner = (await _userManager.GetListingOwnerAsync(vm.Id))?.UserName;
            }
        }

        /// <summary>
        /// Used to add or update brokers
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="listingBrokers"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ListingBroker>> AddOrUpdateBrokers(IEnumerable<ContactsViewModel> contacts, IEnumerable<ListingBroker> listingBrokers = null)
        {
            if (listingBrokers == null)
                listingBrokers = new List<ListingBroker>();

            if (contacts == null)
            {
                contacts = new List<ContactsViewModel>();
            }

            // Update brokers
            var brokersWithoutIds = contacts.Where(x => x.ContactId == 0);
            var brokersByEmail = await _dataEntryRepository.GetBrokersByEmails(brokersWithoutIds.Select(x => x.Email));
            foreach (var broker in brokersWithoutIds)
            {
                broker.ContactId = brokersByEmail
                    .Where(b => !string.IsNullOrWhiteSpace(b.Email) && string.Equals(b.Email, broker.Email, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ID ?? 0;
            }

            var brokerIds = contacts.Where(x => x.ContactId != 0).Select(x => x.ContactId);
            var brokersNeedingUpdate = await _dataEntryRepository.GetBrokers(brokerIds);

            var brokers = _listingMapper.Map(brokersNeedingUpdate, contacts);

            // Update brokers on listing
            var result = _listingMapper.Map(listingBrokers, brokers);

            return result;
        }

        private List<FilterOption> Map(IEnumerable<FilterViewModel> filterOptions)
        {
            if (filterOptions == null || !filterOptions.Any()) return null;
            
            var filters = new List<FilterOption>();
            List<string> properyType = new List<string>();

            foreach (FilterViewModel f in filterOptions)
            {
                if (!Enum.TryParse<FilterTypeEnum>(f.Type, out var filterType)) continue;

                if (filterType == FilterTypeEnum.PropertyType)
                {
                    properyType.Add(f.Value);
                }
                else
                {
                    filters.Add(new FilterOption() { Type = filterType.ToString(), Value = f.Value });
                }
            };

            if (properyType.Any()) filters.Add(new FilterOption(){ Type = FilterTypeEnum.PropertyType.ToString(), Value = string.Join(",", properyType) });
            return filters;
        }

        public IEnumerable<ListingDeltaViewModel> FindListingDeltas(string originalDocument, ListingViewModel newDocument)
        {
            return FindListingDeltas(originalDocument, _listingSerializer.Serialize(newDocument));
        }
        public IEnumerable<ListingDeltaViewModel> FindListingDeltas(string originalDocument, string newDocument)
        {
            return _jsonDeltaEvaluator.Evaluate(originalDocument, newDocument);
        }

        private string ApplyListingDeltas(ListingViewModel vm, string targetDocument)
        {
            if (string.IsNullOrWhiteSpace(targetDocument)) 
            {
                targetDocument = _listingSerializer.Serialize(vm);
            }

            if (vm.Deltas == null || !vm.Deltas.Any())
            {
                return _listingSerializer.Serialize(vm);
            }

            return _jsonDeltaEvaluator.Apply(targetDocument, vm.Deltas);
        }
    }
}
