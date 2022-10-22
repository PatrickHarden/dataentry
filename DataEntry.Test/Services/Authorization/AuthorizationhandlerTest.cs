using dataentry.Data.Constants;
using dataentry.Data.DBContext;
using dataentry.Data.DBContext.Model;
using dataentry.Repository;
using dataentry.Services.Integration.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.Services.Authorization
{
    public class AuthorizationhandlerTest : IDisposable
    {
        private readonly ApplicationUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Mock<ILogger<ListingAuthorizationHandler>> _logger;
        private readonly ListingAuthorizationHandler _listingAuthorizationHandler;
        private readonly IdentityUser _testAdmin;
        private readonly IdentityUser _testUser;

        public IdentityRole _teamRole { get; private set; }

        private readonly IdentityRole _otherRole;
        private readonly IdentityRole _adminRole;
        private readonly ClaimsPrincipal _testAdminPrincipal;
        private readonly ClaimsPrincipal _testUserPrincipal;
        private readonly Claim _adminClaim;
        private readonly UserContext _userContext;
        private readonly DataEntryContext _dataEntryContext;
        private Listing _userListing;
        private Listing _otherListing;
        private Listing _teamListing;
        private readonly Listing _otherTeamListing;
        private Region _userRegion;
        private Region _otherRegion;

        public AuthorizationhandlerTest()
        {
            var _userContextOptions = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: $"{nameof(AuthorizationhandlerTest)}_{nameof(UserContext)}")
                .Options;
            _userContext = new UserContext(_userContextOptions, null);

            var _dataEntryContextOptions = new DbContextOptionsBuilder<DataEntryContext>()
                .UseInMemoryDatabase(databaseName: $"{nameof(AuthorizationhandlerTest)}_{nameof(DataEntryContext)}")
                .Options;
            _dataEntryContext = new DataEntryContext(_dataEntryContextOptions, null);
            
            var userStore = new ApplicationUserStore(_userContext, null);
            var roleStore = new RoleStore<IdentityRole>(_userContext);

            _roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);

            _userManager = new ApplicationUserManager(userStore, null, null, null, null, null, null, null, null, _roleManager, _dataEntryContext);

            _logger = new Mock<ILogger<ListingAuthorizationHandler>>();

            _listingAuthorizationHandler = new ListingAuthorizationHandler(_userManager, _logger.Object);

            _dataEntryContext.AddRange(
                _userRegion = new Region { ID = Guid.NewGuid() },
                _otherRegion = new Region { ID = Guid.NewGuid() },
                _userListing = new Listing { RegionID = _userRegion.ID },
                _otherListing = new Listing { RegionID = _otherRegion.ID },
                _teamListing = new Listing { RegionID = _userRegion.ID },
                _otherTeamListing = new Listing { RegionID = _userRegion.ID }
            );
            _dataEntryContext.SaveChanges();

            _userContext.AddRange(
                _testAdmin = new IdentityUser("testAdmin"),
                _testUser = new IdentityUser("testUser"),
                _teamRole = new IdentityRole(UserConstants.TeamRoleNamePrefix + "test"),
                _otherRole = new IdentityRole(UserConstants.TeamRoleNamePrefix + "other"),
                _adminRole = new IdentityRole(UserConstants.AdminRoleName));

            _userContext.AddRange(
                new IdentityRoleClaim<string>
                {
                    ClaimType = UserConstants.TeamRoleClaimName,
                    ClaimValue = _teamRole.Id,
                    RoleId = _adminRole.Id
                },
                new IdentityRoleClaim<string>
                {
                    ClaimType = UserConstants.TeamRoleClaimName,
                    ClaimValue = _otherRole.Id,
                    RoleId = _otherRole.Id
                },
                new IdentityRoleClaim<string>
                {
                    ClaimType = UserConstants.AdminClaimType,
                    ClaimValue = null,
                    RoleId = _adminRole.Id
                },
                new IdentityUserRole<string>{
                    RoleId = _teamRole.Id,
                    UserId = _testUser.Id
                },
                new IdentityUserRole<string>{
                    RoleId = _adminRole.Id,
                    UserId = _testAdmin.Id
                });

            _userContext.AddRange(
                new IdentityUserClaim<string>{
                    ClaimType = UserConstants.ListingClaimName,
                    ClaimValue = _userListing.ID.ToString(),
                    UserId = _testUser.Id
                },
                new IdentityRoleClaim<string>{
                    ClaimType = UserConstants.ListingClaimName,
                    ClaimValue = _teamListing.ID.ToString(),
                    RoleId = _teamRole.Id
                },
                new IdentityRoleClaim<string>{
                    ClaimType = UserConstants.ListingClaimName,
                    ClaimValue = _otherTeamListing.ID.ToString(),
                    RoleId = _otherRole.Id
                });

            _userContext.SaveChanges();

            _testAdminPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>{new Claim(ClaimTypes.NameIdentifier, _testAdmin.Id)}));
            _testUserPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>{new Claim(ClaimTypes.NameIdentifier, _testUser.Id)}));
        }

        public void Dispose() {
            _userContext.Database.EnsureDeleted();
            _userContext.Dispose();
            _dataEntryContext.Database.EnsureDeleted();
            _dataEntryContext.Dispose();
        }

        [Fact]
        public async Task ListingAuthorizationHandler_UserIsAdmin()
        {
            // Arrange
            var context = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>() { Operations.Read }, 
                _testAdminPrincipal, 
                _otherListing);

            // Act
            await _listingAuthorizationHandler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded);
        }

        [Fact]
        public async Task ListingAuthorizationHandler_UserHasClaim()
        {
            // Arrange
            var context = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>() { Operations.Read }, 
                _testUserPrincipal, 
                _userListing);

            // Act
            await _listingAuthorizationHandler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded);
        }

        [Fact]
        public async Task ListingAuthorizationHandler_UserHasNoClaim()
        {
            // Arrange
            var context = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>() { Operations.Read }, 
                _testUserPrincipal, 
                _otherListing);

            // Act
            await _listingAuthorizationHandler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded); // Yes this is intended
        }

        [Fact]
        public async Task ListingAuthorizationHandler_UserHasRoleClaim()
        {
            // Arrange
            var context = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>() { Operations.Read }, 
                _testUserPrincipal, 
                _teamListing);

            // Act
            await _listingAuthorizationHandler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded);
        }

        [Fact]
        public async Task ListingAuthorizationHandler_UserHasNoRoleClaim()
        {
            // Arrange
            var context = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>() { Operations.Read }, 
                _testUserPrincipal, 
                _otherTeamListing);

            // Act
            await _listingAuthorizationHandler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded); // Yes this is intended
        }
    }
}
