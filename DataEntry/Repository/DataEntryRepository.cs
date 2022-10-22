using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dataentry.Data.Constants;
using dataentry.Data.DBContext;
using dataentry.Data.DBContext.Model;
using dataentry.Data.DBContext.SQL;
using dataentry.Data.Enums;
using dataentry.Extensions;
using dataentry.Services.Integration.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace dataentry.Repository
{
    public class DataEntryRepository : IDataEntryRepository
    {
        private readonly DataEntryContext _dataEntryContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationUserManager _userManager;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IRawSqlProvider _rawSqlProvider;
        private readonly ILogger<DataEntryRepository> _logger;

        public DataEntryRepository(DataEntryContext DataEntry,
            IAuthorizationService authorizationService,
            ApplicationUserManager userManager,
            ILookupNormalizer lookupNormalizer,
            IRawSqlProvider sqlProvider,
            ILogger<DataEntryRepository> logger)
        {
            _dataEntryContext = DataEntry;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _lookupNormalizer = lookupNormalizer;
            _rawSqlProvider = sqlProvider;
            _logger = logger;
        }

        /// <summary>
        /// Add new building to data entry context
        /// </summary>
        /// <param name="listing"></param>
        /// <returns></returns>
        public async Task<Listing> AddListing(Listing listing, ClaimsPrincipal claimsPrincipal, IEnumerable<string> userNames, IEnumerable<string> teamNames, bool ignoreDuplicateExternalId = true)
        {
            if (userNames == null) userNames = new List<string>();
            if (teamNames == null) teamNames = new List<string>();

            await _dataEntryContext.Listings.AddAsync(listing);
            listing.CreatedAt = listing.UpdatedAt = DateTime.UtcNow;

            try 
            {
                await _dataEntryContext.SaveChangesAsync();
            } 
            catch (DbUpdateException ex) when (ignoreDuplicateExternalId)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_Listings_ExternalID", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(ex, "Creating listing with duplicate external ID: \"{externalID}\".", listing.ExternalID);
                    listing.ExternalID = null;
                    foreach (var space in listing.Spaces) 
                    {
                        space.ExternalID = null;
                    }
                    await _dataEntryContext.SaveChangesAsync();
                }
            }
            catch
            {
                _dataEntryContext.Reset();
                throw;
            }

            if (listing.ParentListingID == null)
            {
                await AddUserOwnershipClaimToListing(listing.ID, claimsPrincipal);

                if (userNames.Any() || teamNames.Any())
                {
                    var users = await _userManager.FindByNamesAsync(userNames);
                    foreach (var user in users)
                    {
                        await AddUserClaimToListing(listing.ID, user);
                    }

                    var roles = await _userManager.FindRolesByTeamNamesAsync(teamNames);
                    foreach (var role in roles)
                    {
                        await AddRoleClaimToListing(listing.ID, role);
                    }
                }
                else
                {
                    var currentUser = await _userManager.GetUserAsync(claimsPrincipal);
                    await AddUserClaimToListing(listing.ID, currentUser);
                }
            }

            // ID is no longer 0 so we can generate external IDs
            await listing.VerifyExternalID(_dataEntryContext);
            await _dataEntryContext.SaveChangesAsync();

            return listing;
        }

        /// <summary>
        /// Update a listing in the data entry context
        /// </summary>
        /// <param name="listing"></param>
        /// <returns></returns>
        public async Task<Listing> UpdateListing(Listing listing, ClaimsPrincipal user, IEnumerable<string> userNames = null, IEnumerable<string> teamNames = null)
        {
            if (userNames == null) userNames = new List<string>();
            if (teamNames == null) teamNames = new List<string>();

            var currentUser = _userManager.GetUserAsync(user);
            var listingOwner = await _userManager.GetListingOwnerAsync(listing.ID);

            // Check is user has access to listing
            var authResult = await _authorizationService.AuthorizeAsync(user, listing, Operations.Update);
            if (!authResult.Succeeded)
            {
                return null;
            }

            if (userNames.Count() > 0 || teamNames.Count() > 0) listing.IsDeleted = false;

            listing.UpdatedAt = DateTime.UtcNow;
            await listing.VerifyExternalID(_dataEntryContext);

            try
            {
                await _dataEntryContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                await _dataEntryContext.SaveChangesAsync();
            }

            //Update user claims
            if (userNames.Any() || teamNames.Any())
            {
                var users = await _userManager.GetUsersForListingAsync(listing.ID);
                var normalizedUserNames = userNames.Select(userName => _lookupNormalizer.NormalizeName(userName)).ToList();

                var userNamesToAdd = normalizedUserNames.Where(normalizedUserName => !users.Any(u => u.NormalizedUserName == normalizedUserName));
                foreach (var userNameToAdd in userNamesToAdd)
                {
                    var userToAdd = await _userManager.FindByNameAsync(userNameToAdd);
                    if (userToAdd != null)
                    {
                        await AddUserClaimToListing(listing.ID, userToAdd);
                    }
                }

                var userNamesToRemove = users.Where(u => !normalizedUserNames.Any(normalizedUserName => u.NormalizedUserName == normalizedUserName)).Select(u => u.NormalizedUserName);
                foreach (var userNameToRemove in userNamesToRemove)
                {
                    var userToRemove = await _userManager.FindByNameAsync(userNameToRemove);
                    if (userToRemove != null)
                    {
                        // if one of the users removed is an owner
                        if (userToRemove.NormalizedUserName == listingOwner.NormalizedUserName)
                        {
                            // remove the current user's claim before adding them as an owner
                            await RemoveUserClaimToListing(listing.ID, currentUser.Result);

                            // and assign ownership claim to the current user
                            await AddUserOwnershipClaimToListing(listing.ID, user);
                            await AddUserClaimToListing(listing.ID, currentUser.Result);

                            // remove the old user
                            await RemoveUserClaimToListing(listing.ID, userToRemove);
                            await RemoveUserOwnershipClaimToListing(listing.ID, userToRemove);
                        }
                        else
                        {
                            await RemoveUserClaimToListing(listing.ID, userToRemove);
                        }

                    }
                }

                //Update team claims
                var teams = await _userManager.GetRolesForListingAsync(listing.ID);
                var normalizedTeamNames = teamNames.Select(teamName => _lookupNormalizer.NormalizeName(_userManager.ConvertTeamNameToRoleName(teamName)));

                var teamNamesToAdd = normalizedTeamNames.Where(normalizedTeamName => !teams.Any(t => t.NormalizedName == normalizedTeamName));
                var teamsToAdd = await _userManager.FindRolesByTeamNamesAsync(teamNamesToAdd);
                foreach (var teamToAdd in teamsToAdd)
                {
                    await AddRoleClaimToListing(listing.ID, teamToAdd);
                }

                var teamNamesToRemove = teams.Where(t => !normalizedTeamNames.Any(normalizedTeamName => t.NormalizedName == normalizedTeamName)).Select(t => t.NormalizedName);
                var teamsToRemove = await _userManager.FindRolesByTeamNamesAsync(teamNamesToRemove);
                foreach (var teamToRemove in teamsToRemove)
                {
                    await RemoveRoleClaimToListing(listing.ID, teamToRemove);
                }
            }

            return listing;
        }

        /// <summary>
        /// Get total listig counts for pagination
        /// </summary>
        /// <returns>listing counts</returns>
        public async Task<int> GetListingsCount(ClaimsPrincipal user, bool isAdmin, IEnumerable<FilterOption> filterOptions, string regionID)
        {
            IQueryable<Listing> query = GenerateRawSql(user, isAdmin, filterOptions, regionID);
            return await query.CountAsync();
        }

        /// <summary>
        /// Below method is an example
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Listing>> GetAllListings(ClaimsPrincipal user, bool isAdmin, IEnumerable<FilterOption> filterOptions, string regionID, int? skip = null, int? take = null)
        {
            IQueryable<Listing> query = GenerateRawSql(user, isAdmin, filterOptions, regionID);

            query = GetListingsQueryable(query, includeSpaces: false)
                .OrderBy(x => x.AssignmentFlag == true ? 0 : 1)
                .ThenByDescending(x => x.UpdatedAt)
                .AsNoTracking();

            if (skip != null) query = query.Skip(skip.Value);
            if (take != null) query = query.Take(take.Value);

            var result = await query.ToListAsync();
            foreach (var listing in result)
            {
                await listing.VerifyExternalID();
            }

            return result;
        }

        /// <summary>
        /// Retrieve all brokers
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Broker>> GetAllBrokers()
        {
            return await _dataEntryContext.Brokers.ToListAsync();
        }

        /// <summary>
        /// Retrieve a listing by ID
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when a listing with the given ID does not exist.</exception>
        /// <exception cref="SecurityException">Thrown when the user does not have access to the requested listing.</exception>
        public async Task<Listing> GetListingByID(int ID, ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var listing = GetListingsQueryable()
                .Where(l => l.ID == ID && l.IsParent)
                .FirstOrDefault();

            if (listing == null)
            {
                throw new ArgumentException($"Listing with ID {ID} does not exist.", nameof(ID));
            }

            var authResult = await _authorizationService.AuthorizeAsync(user, listing, Operations.Read);
            if (!authResult.Succeeded)
            {
                throw new SecurityException($"User {user.Identity?.Name} does not have access to listing with ID {ID}");
            }

            // Reload Publishing State
            await ReloadPublishingStates(new List<Listing>() { listing });
            await listing.VerifyExternalID();

            return listing;
        }

        /// <summary>
        /// Retrieve listings that imported from EDP by Edp ID
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when a listing with the given ID does not exist.</exception>
        /// <exception cref="SecurityException">Thrown when the user does not have access to the requested listing.</exception>
        public async Task<IEnumerable<Listing>> GetEdpImportListings(ClaimsPrincipal user, int ID)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var query = GetListingsQueryable()
                .Where(l => l.MIQID == ID.ToString() && l.IsParent && l.IsDeleted == false);
            var results = await query.ToListAsync();

            foreach (var listing in results.ToList())
            {
                var authResult = await _authorizationService.AuthorizeAsync(user, listing, Operations.Read);
                if (!authResult.Succeeded)
                {
                    results.Remove(listing);
                }
            }

            return results;
        }

        /// <summary>
        /// Used to delete a listing from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteListing(int id, ClaimsPrincipal user)
        {
            var listing = _dataEntryContext.Listings.FirstOrDefault(x => x.ID == id);

            if (listing == null) return false;

            // Check is user has access to parent listing
            var authResult = await _authorizationService.AuthorizeAsync(user, listing, Operations.Delete);
            if (!authResult.Succeeded)
            {
                return false;
            }

            listing.IsDeleted = true;
            listing.DeletedAt = DateTime.UtcNow;

            await _dataEntryContext.SaveChangesAsync();

            // Remove claims for listing
            var listingUsers = await _userManager.GetUsersForListingAsync(id);
            foreach (var listingUser in listingUsers)
            {
                await RemoveUserClaimToListing(id, listingUser);
            }

            var listingRoles = await _userManager.GetRolesForListingAsync(id);
            foreach (var listingRole in listingRoles)
            {
                await RemoveRoleClaimToListing(id, listingRole);
            }

            var listingOwner = await _userManager.GetListingOwnerAsync(id);
            await RemoveUserOwnershipClaimToListing(id, listingOwner);

            return true;
        }

        /// <summary>
        /// Used to include the tables necessary for listing query
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<Listing> GetListingsQueryable(
            IQueryable<Listing> query = null,
            bool includeAddress = true,
            bool includeListingData = true,
            bool includeBroker = true,
            bool includeSpaces = true,
            bool includeSpacesListingData = true,
            bool includeImage = true,
            bool includeSpaceImage = true,
            bool includeRegion = true
            )
        {
            if (query == null) query = _dataEntryContext.Listings;
            if (includeAddress) query = query.Include(l => l.Address);
            if (includeListingData) query = query.Include(l => l.ListingData);
            if (includeBroker) query = query.Include(l => l.ListingBroker).ThenInclude(ListingBroker => ListingBroker.Broker);
            if (includeSpaces)
            {
                if (includeSpacesListingData) query = query.Include(l => l.Spaces).ThenInclude(space => space.ListingData);
                else query = query.Include(l => l.Spaces);
                if (includeSpaceImage) query = query.Include(l => l.Spaces).ThenInclude(l => l.ListingImage).ThenInclude(ListingImage => ListingImage.Image);
            }
            if (includeImage)
                query = query.Include(l => l.ListingImage).ThenInclude(ListingImage => ListingImage.Image);
            if (includeRegion)
                query = query.Include(l => l.Region);
            
            return query;
        }

        /// <summary>
        /// Used to Reload all the publishing states from the database
        /// 
        /// The publishing states are changed by the dataentry.Publishing project
        /// </summary>
        /// <param name="listings"></param>
        /// <returns></returns>
        private async Task ReloadPublishingStates(IEnumerable<Listing> listings)
        {
            var publishingStates = listings.Select(x => x.ListingData.FirstOrDefault(y => y.DataType == "PublishingState"))
                                    .Where(x => x != null);

            foreach (var publishingState in publishingStates)
            {
                await _dataEntryContext.Entry(publishingState).ReloadAsync();
            }
        }

        /// <summary>
        /// Used to find existing broker
        /// </summary>
        /// <param name="brokerId"></param>
        /// <returns>Get broker from the db</returns>
        public async Task<Broker> GetBrokerById(int brokerId)
        {
            var brokers = await _dataEntryContext.Brokers.Where(x => x.ID == brokerId).FirstOrDefaultAsync();
            return brokers;
        }

        /// <summary>
        /// Used to find existing brokers
        /// </summary>
        /// <param name="brokerIds"></param>
        /// <returns>List of brokers from the db</returns>
        public async Task<IEnumerable<Broker>> GetBrokers(IEnumerable<int> brokerIds)
        {
            var brokers = await _dataEntryContext.Brokers.Where(x => brokerIds.Contains(x.ID)).ToListAsync();
            return brokers;
        }

        public async Task<Broker> AddOrUpdateBroker(Broker broker)
        {
            if (broker.ID == 0) await _dataEntryContext.Brokers.AddAsync(broker);
            await _dataEntryContext.SaveChangesAsync();
            return broker;
        }

        private async Task<IdentityResult> AddUserClaimToListing(int listingId, IdentityUser user)
        {
            var claim = new Claim(UserConstants.ListingClaimName, listingId.ToString());
            return await _userManager.AddClaimAsync(user, claim);
        }

        private async Task<IdentityResult> RemoveUserClaimToListing(int listingId, IdentityUser user)
        {
            var claim = new Claim(UserConstants.ListingClaimName, listingId.ToString());
            return await _userManager.RemoveClaimAsync(user, claim);
        }

        private async Task<IdentityResult> AddUserOwnershipClaimToListing(int listingId, ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            return await AddUserOwnershipClaimToListing(listingId, user);
        }
        private async Task<IdentityResult> AddUserOwnershipClaimToListing(int listingId, IdentityUser user)
        {
            var claim = new Claim(UserConstants.OwnerClaimName, listingId.ToString());
            return await _userManager.AddClaimAsync(user, claim);
        }
        private async Task<IdentityResult> RemoveUserOwnershipClaimToListing(int listingId, IdentityUser user)
        {
            var claim = new Claim(UserConstants.OwnerClaimName, listingId.ToString());
            return await _userManager.RemoveClaimAsync(user, claim);
        }
        private async Task<IdentityResult> AddRoleClaimToListing(int listingId, IdentityRole role)
        {
            var claim = new Claim(UserConstants.ListingClaimName, listingId.ToString());
            return await _userManager.AddClaimAsync(role, claim);
        }
        private async Task<IdentityResult> RemoveRoleClaimToListing(int listingId, IdentityRole role)
        {
            var claim = new Claim(UserConstants.ListingClaimName, listingId.ToString());
            return await _userManager.RemoveClaimAsync(role, claim);
        }

        public async Task<Image> AddImage(Image image)
        {
            await _dataEntryContext.Images.AddAsync(image);
            await _dataEntryContext.SaveChangesAsync();
            return image;
        }

        public async Task<Image> UpdateImage(int id, int processStatus)
        {
            var image = _dataEntryContext.Images.FirstOrDefault(x => x.ID == id);
            image.WatermarkProcessStatus = processStatus;
            image.UpdatedAt = DateTime.UtcNow;
            await _dataEntryContext.SaveChangesAsync();
            return image;
        }

        public async Task<IEnumerable<Image>> GetImagesByWatermarkProcessState(int state, bool includeDeleted = true, int? imageType = null)
        {
            // if (includeDeleted) return await _dataEntryContext.Images.Where(x => x.WatermarkProcessStatus == state).ToListAsync();

            var query = _dataEntryContext.ListingImages
                    .Where(x => x.Image.WatermarkProcessStatus == state);

            if (!includeDeleted)
            {
                query = query.Where(i => (i.Listing.IsDeleted == false && i.Listing.IsParent == true
                          || i.Listing.IsParent == false && i.Listing.ParentListingID != null)
                          && _dataEntryContext.Listings.Where(l => l.ID == i.Listing.ParentListingID).FirstOrDefault().IsDeleted == false);
            }
                    
            if (imageType != null)
            {
                String ic = ((ImageCategory)imageType).ToString();
                query = query.Where(i => i.ImageCategory == ic);
            }

            var result = await query.Select(i => i.Image)
                    .Distinct()
                    .OrderBy(o => o.ID)
                    .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Image>> ResetImagesWatermarkProcessState(int targetState, int finalState)
        {
            var images = await _dataEntryContext.Images.Where(x => x.WatermarkProcessStatus == targetState).ToListAsync();
            DateTime updateTime = DateTime.UtcNow;

            foreach (var image in images)
            {
                image.WatermarkProcessStatus = finalState;
                image.UpdatedAt = updateTime;
            }

            await _dataEntryContext.SaveChangesAsync();
            return images;
        }

        public async Task<IEnumerable<Image>> GetImages(int? listingId, IEnumerable<int?> imageIds, ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var result = new List<Image>();
            var imagesIdsBylistingId = new List<int>();
            if (listingId > 0)
            {
                var listing = await _dataEntryContext.Listings.Where(x => x.ID == listingId).FirstOrDefaultAsync();
                if (listing == null)
                {
                    throw new ArgumentException($"Listing with ID {listingId} does not exist.", nameof(listingId));
                }

                var authResult = await _authorizationService.AuthorizeAsync(user, listing, Operations.Read);
                if (!authResult.Succeeded)
                {
                    throw new SecurityException($"User {user.Identity?.Name} does not have access to listing with ID {listingId}");
                }

                List<int> listingIds = await _dataEntryContext.Listings.Where(l => l.ParentListingID == listingId).Select(x => x.ID).ToListAsync();
                listingIds.Add((int)listingId);
                imagesIdsBylistingId = await _dataEntryContext.ListingImages
                    .Where(l => listingIds.Contains(l.ListingID)).Select(s => s.Image.ID)
                    .ToListAsync();
            }
            var query = _dataEntryContext.Images.Where(
                x => imageIds != null && x.UploadedBy == user.Identity.Name && imageIds.Contains(x.ID)
                || imagesIdsBylistingId.Count > 0 && imagesIdsBylistingId.Contains(x.ID));
            return await query.ToListAsync();
        }

        public async Task<Broker> GetBrokerByEmail(string email)
        {
            email = CleanEmail(email);
            return await _dataEntryContext.Brokers.FirstOrDefaultAsync(b => EF.Functions.ILike(b.Email, email));
        }

        public async Task<IEnumerable<Broker>> GetBrokersByEmails(IEnumerable<string> emails)
        {
            if (emails == null || !emails.Any()) return new List<Broker>();
            var cleanEmails = emails.Select(CleanEmail).ToList();
            return await _dataEntryContext.Brokers.Where(b => cleanEmails.Any(c => EF.Functions.ILike(b.Email, c))).ToListAsync();
        }

        private string CleanEmail(string email)
        {
            return string.IsNullOrWhiteSpace(email) ? "" : email.Trim().Replace("%", "[%]");
        }

        private IQueryable<Listing> GenerateRawSql(ClaimsPrincipal user, bool IsAdmin, IEnumerable<FilterOption> filterOptions, string regionID)
        {
            IQueryable<Listing> query = null;

            List<DbParameter> parameters = new List<DbParameter>();
            var rawQuery = new StringBuilder(@"SELECT l.""ID"", 
                                                    l.""IsParent"", 
                                                    l.""ParentListingID"", 
                                                    l.""Name"", 
                                                    l.""UsageType"", 
                                                    l.""Status"", 
                                                    l.""AddressID"", 
                                                    l.""AvailableFrom"", 
                                                    l.""CreatedAt"", 
                                                    l.""UpdatedAt"", 
                                                    l.""DeletedAt"", 
                                                    l.""IsDeleted"", 
                                                    l.""RegionID"", 
                                                    l.""ExternalID"", 
                                                    l.""PreviewID"", 
                                                    l.""MIQID"", 
                                                    CASE WHEN clm.""ClaimValue"" IS NOT NULL AND UPPER(la.""Data"" ->> 'AssignedBy') != UPPER(@userName) THEN (la.""Data"" ->> 'AssignmentFlag')::boolean ELSE FALSE END AS ""AssignmentFlag"" FROM ""Listings"" l ");
            var rawQueryCondition = new StringBuilder(@"WHERE l.""IsParent"" = TRUE ");

            if (!string.IsNullOrWhiteSpace(regionID))
            {
                Append(rawQueryCondition, $@"AND l.""RegionID"" = '{regionID}' ");
            }
            
            //For admin user, uses Left join to return all the listings, also we need to check if the admin is member of that listing  
            string op = "INNER";
            if (IsAdmin) op = "LEFT"; 
            var getUserListing = op + @" JOIN (
                        SELECT ""ClaimValue""
                        FROM ""user"".""AspNetUserClaims"" uc
                        WHERE uc.""UserId"" = @userId
                            AND uc.""ClaimType"" = 'ListingClaim'
                        UNION
                        SELECT ""ClaimValue""
                        FROM ""user"".""AspNetUserRoles"" ur
                        INNER JOIN ""user"".""AspNetRoleClaims"" rc
                            ON ur.""RoleId"" = rc.""RoleId""
                        WHERE ur.""UserId"" = @userId
                            AND rc.""ClaimType"" = 'ListingClaim'
                    ) AS clm on clm.""ClaimValue"" = l.""ID""::text ";

            Append(rawQuery, getUserListing);
            parameters.Add(_rawSqlProvider.GetDbParameter("@userId", user.FindFirstValue(ClaimTypes.NameIdentifier)));
            parameters.Add(_rawSqlProvider.GetDbParameter("@userName", user.FindFirstValue(ClaimTypes.Name)));
            Append(rawQuery, @"LEFT JOIN ""ListingData"" la on la.""ListingID"" = l.""ID"" AND la.""DataType"" = 'ListingAssignment'");

            int i = 0;
            bool isDeletedFilterOn = false;
            foreach (var f in filterOptions ?? Enumerable.Empty<FilterOption>()) 
            {
                var parameterName = $"@p{i++}";
                
                if (f.Type == FilterTypeEnum.Keyword.ToString())
                {   
                    string conditon = "";
                    Append(rawQuery, @"LEFT JOIN ""Address"" a on l.""AddressID"" = a.""ID""");
                    Append(rawQuery, @"LEFT JOIN ""ListingData"" r on r.""ListingID"" = l.""ID"" AND r.""DataType"" = 'PropertyRecordName'");
                    
                    AppendOr(ref conditon, $@"l.""ExternalID"" ILIKE {parameterName}");
                    AppendOr(ref conditon, $@"LOWER(l.""Name"") ILIKE '%' || {parameterName} || '%'");
                    AppendOr(ref conditon, (int.TryParse(f.Value, out _)) ? $@"l.""ID"" = CAST ({parameterName} AS INTEGER)" : null);
                    AppendOr(ref conditon, $@"LOWER(a.""Street1"") ILIKE '%' || {parameterName} || '%'");
                    AppendOr(ref conditon, $@"LOWER(a.""City"") ILIKE '%' || {parameterName} || '%'");
                    AppendOr(ref conditon, $@"LOWER(a.""StateProvince"") ILIKE '%' || {parameterName} || '%'");
                    AppendOr(ref conditon, $@"LOWER(a.""PostalCode"") ILIKE '%' || {parameterName} || '%'");
                    AppendOr(ref conditon, $@"LOWER(r.""Data"" ->> 'Value') ILIKE '%' || {parameterName} || '%'");

                    Append(rawQueryCondition, $@"AND ({conditon}) ");
                    parameters.Add(_rawSqlProvider.GetDbParameter(parameterName, f.Value.ToLower()));
                }
                else if (f.Type == FilterTypeEnum.PropertyType.ToString())
                {
                    Append(rawQuery, @"LEFT JOIN ""ListingData"" p on p.""ListingID"" = l.""ID"" AND p.""DataType"" = 'PropertyType'");
                    var propertyType = from val in f.Value.Split(',') select val;
                    List<string> propertyParams = new List<string>();

                    foreach (string p in propertyType)
                    {
                        parameterName = $"@p{i++}";
                        propertyParams.Add(parameterName);
                        parameters.Add(_rawSqlProvider.GetDbParameter(parameterName, p));
                    }

                    Append(rawQueryCondition, $@"AND p.""Data"" ->> 'Value' in ({string.Join(",", propertyParams)})");
                }
                else if (f.Type == FilterTypeEnum.PublishingStatus.ToString())
                {
                    Append(rawQuery, @"LEFT JOIN ""ListingData"" s on s.""ListingID"" = l.""ID"" AND s.""DataType"" = 'PublishingState'");
                    switch (f.Value)
                    {
                        case "Pending":
                            Append(rawQueryCondition, $@"AND s.""Data"" ->> 'Value' in ('{PublishingStateEnum.Publishing.ToString()}', '{PublishingStateEnum.Unpublishing.ToString()}')");
                        break;

                        case "Draft":
                                Append(rawQueryCondition, $@"AND (s.""Data"" ->> 'Value' IS NULL OR
                                s.""Data"" ->> 'Value' IN
                                (
                                '{PublishingStateEnum.PublishFailed.ToString()}', 
                                '{PublishingStateEnum.UnpublishFailed.ToString()}',
                                '{PublishingStateEnum.Unpublished.ToString()}'
                                ))");
                        break;

                        case "Published":
                            Append(rawQueryCondition, $@"AND s.""Data"" ->> 'Value' = '{PublishingStateEnum.Published.ToString()}'");
                        break;
                        
                        default: 
                        break;
                    }     
                }
                else if (f.Type == FilterTypeEnum.Deleted.ToString() && f.Value == "true" && IsAdmin)
                {
                    isDeletedFilterOn = true;
                    Append(rawQueryCondition, @"AND l.""IsDeleted"" = TRUE");
                }
                else if (f.Type == FilterTypeEnum.MiqOnly.ToString() && f.Value == "true")
                {
                    Append(rawQueryCondition, @"AND l.""MIQID"" IS NOT NULL");
                }
                else if (f.Type == FilterTypeEnum.BulkUploadOnly.ToString() && f.Value == "true")
                {
                    Append(rawQuery, @"LEFT JOIN ""ListingData"" b on b.""ListingID"" = l.""ID"" AND b.""DataType"" = 'IsBulkUpload'");
                    Append(rawQueryCondition, @"AND b.""Data"" ->> 'Value' = 'true'");
                }
                else if (f.Type == FilterTypeEnum.AssignmentStatus.ToString() && f.Value == "true")
                {
                    Append(rawQueryCondition, @"AND clm.""ClaimValue"" IS NOT NULL AND la.""Data"" ->> 'AssignmentFlag' = 'true' AND UPPER(la.""Data"" ->> 'AssignedBy') != UPPER(@userName)");
                }
            }
            if (!isDeletedFilterOn) Append(rawQueryCondition, @"AND l.""IsDeleted"" = FALSE");
            rawQuery = rawQuery.Append(rawQueryCondition);
            query = _dataEntryContext.Listings
                .FromSql(rawQuery.ToString(), parameters.ToArray<object>());

            return query;
        }

        private StringBuilder Append(StringBuilder currentQuery, string newQuery){
            if (string.IsNullOrWhiteSpace(newQuery)) return currentQuery;
            return currentQuery.Append(newQuery + " ");
        }

        private void AppendOr(ref string currentQuery, string newQuery)
        {
            currentQuery = string.IsNullOrWhiteSpace(newQuery) || string.IsNullOrWhiteSpace(currentQuery) ? 
                            currentQuery + newQuery + " " : currentQuery + "OR " + newQuery + " ";
        }

        public async Task<IEnumerable<Region>> GetAllRegions()
        {
            return await _dataEntryContext.Regions.Include(r => r.RegionalIDFormats).ToListAsync();
        }
        
        public async Task<Region> GetRegionByHomeSiteId(string homeSiteId)
        {
            return await _dataEntryContext.Regions.Include(r => r.RegionalIDFormats).FirstOrDefaultAsync(r => r.HomeSiteID == homeSiteId);
        }

        public Task<Region> GetDefaultRegion() => GetRegionById(Region.DefaultID);

        public async Task AddRegion(ClaimsPrincipal user, Region region)
        {
            var authResult = await _authorizationService.AuthorizeAsync(user, region, Operations.Create);
            if (!authResult.Succeeded)
            {
                throw new SecurityException($"User {user.Identity.Name} does not have permission to create regions");
            }
            await _dataEntryContext.Regions.AddAsync(region);
            await _dataEntryContext.SaveChangesAsync();
        }

        public async Task<Region> GetRegionById(Guid id)
        {
            return await _dataEntryContext.Regions.Include(r => r.RegionalIDFormats).FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task UpdateRegion(ClaimsPrincipal user, Region region)
        {
            var authResult = await _authorizationService.AuthorizeAsync(user, region, Operations.Update);
            if (!authResult.Succeeded)
            {
                throw new SecurityException($"User {user.Identity.Name} does not have permission to update regions");
            }
            await _dataEntryContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteRegion(ClaimsPrincipal user, Guid id)
        {
            var region = new Region { ID = id };
            var authResult = await _authorizationService.AuthorizeAsync(user, region, Operations.Delete);
            if (!authResult.Succeeded)
            {
                throw new SecurityException($"User {user.Identity.Name} does not have permission to delete regions");
            }
            _dataEntryContext.Attach(region);
            _dataEntryContext.Regions.Remove(region);
            try
            {
                await _dataEntryContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Thrown if the region doesn't exist
                return false;
            }
            return true;
        }

        public async Task UpdateListingSerialization(int id, ListingSerializationType type, string value)
        {
            ListingData listingData;
            ListingSerialization serialization;

            var serializations = await _dataEntryContext.ListingData
                .Where(d => d.ListingID == id && d.DataType == "ListingSerialization")
                .ToListAsync();

            var lookup = serializations
                .Select(d => new { d, s = d.Deserialize<ListingSerialization>() })
                .FirstOrDefault(result => result.s.Type == type);

            if (lookup != null)
            {
                listingData = lookup.d;
                serialization = lookup.s;
            } 
            else
            {
                listingData = new ListingData
                {
                    ListingID = id,
                    DataType = "ListingSerialization"
                };
                _dataEntryContext.Add(listingData);

                serialization = new ListingSerialization
                {
                    Type = type
                };
            }

            serialization.Data = value;
            listingData.Data = ListingExtensions.Serialize(serialization);

            await _dataEntryContext.SaveChangesAsync();
        }

        public async Task<string> GetListingSerialization(int id, ListingSerializationType type)
        {
            var serializations = await _dataEntryContext.ListingData
                .Where(d => d.ListingID == id && d.DataType == "ListingSerialization")
                .ToListAsync();

            var serialization = serializations
                .Select(d => d.Deserialize<ListingSerialization>())
                .FirstOrDefault(result => result.Type == type);

            return serialization?.Data;
        }
    }
}
