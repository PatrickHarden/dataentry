using dataentry.Data.Constants;
using dataentry.Data.DBContext;
using dataentry.Data.DBContext.Model;
using dataentry.Data.DBContext.SQL;
using dataentry.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace dataentry.Test.Repository
{
    public class DataEntryRepositoryTest : IDisposable
    {
        private DbContextOptions<DataEntryContext> DbContextOptions;
        private Mock<IAuthorizationService> MockAuthService;
        private Mock<IUserStore<IdentityUser>> MockUserStore;
        private Mock<ApplicationUserManager> MockUserRepository;
        private ILookupNormalizer LookupNormalizer;
        private Mock<IRawSqlProvider> MockRawSqlProvider;
        private Mock<ILogger<DataEntryRepository>> MockLogger;

        public DataEntryRepositoryTest()
        {
            DbContextOptions = new DbContextOptionsBuilder<DataEntryContext>()
                .UseInMemoryDatabase(databaseName: nameof(DataEntryRepositoryTest))
                .Options;
            MockAuthService = new Mock<IAuthorizationService>();
            MockUserStore = new Mock<IUserStore<IdentityUser>>();
            MockUserRepository = new Mock<ApplicationUserManager>();
            LookupNormalizer = new UpperInvariantLookupNormalizer();
            MockRawSqlProvider = new Mock<IRawSqlProvider>();
            MockLogger = new Mock<ILogger<DataEntryRepository>>();
        }

        public void Dispose()
        {
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_AddListing_Success()
        {
            //Arrange
            var listing = ArrangeListing();

            var testStart = DateTime.UtcNow;

            var userNames = new List<string> { "testUser1", "testUser2" };
            var userLookup = SetupUsers(userNames);

            var roleNames = new List<string> { "testRole1", "testRole2" };
            var roleLookup = SetupRoles(roleNames);

            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(i => i.Name).Returns(userNames[0]);
            var principal = new ClaimsPrincipal(mockIdentity.Object);

            MockUserRepository.Setup(r => r.GetUserAsync(principal)).Returns(Task.FromResult(userLookup[userNames[0]]));
            MockUserRepository.Setup(r => r.FindByNamesAsync(It.IsAny<IEnumerable<string>>()))
                .Returns<IEnumerable<string>>(n => Task.FromResult(n.Select(u => userLookup[u])));
            MockUserRepository.Setup(r => r.FindRolesByTeamNamesAsync(It.IsAny<IEnumerable<string>>()))
                .Returns<IEnumerable<string>>(n => Task.FromResult(n.Select(t => roleLookup[t])));

            //Act
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                var result = await repo.AddListing(listing, principal, userNames, roleNames);
            }

            //Assert
            Assert.True(testStart <= listing.CreatedAt, "DataEntryRepository.AddListing should update the CreatedAt property");
            Assert.True(testStart <= listing.UpdatedAt, "DataEntryRepository.AddListing should update the UpdatedAt property");

            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var resultListing = dbContext.Listings.FirstOrDefault(l => l.Name == listing.Name);
                Assert.NotNull(resultListing);
                MockUserRepository.Verify(m => m.AddClaimAsync(It.Is<IdentityUser>(u => u == userLookup[userNames[0]]), It.Is<Claim>(c => c.Type == UserConstants.OwnerClaimName && c.Value == listing.ID.ToString())),
                    "DataEntryRepository.AddListing should add an owner claim for the current user");
                MockUserRepository.Verify(m => m.AddClaimAsync(It.Is<IdentityUser>(u => u == userLookup[userNames[0]]), It.Is<Claim>(c => c.Type == UserConstants.ListingClaimName && c.Value == listing.ID.ToString())),
                    "DataEntryRepository.AddListing should add a listing claim for the specified users");
                MockUserRepository.Verify(m => m.AddClaimAsync(It.Is<IdentityUser>(u => u == userLookup[userNames[1]]), It.Is<Claim>(c => c.Type == UserConstants.ListingClaimName && c.Value == listing.ID.ToString())),
                    "DataEntryRepository.AddListing should add a listing claim for the specified users");
                MockUserRepository.Verify(m => m.AddClaimAsync(It.Is<IdentityRole>(r => r == roleLookup[roleNames[0]]), It.Is<Claim>(c => c.Type == UserConstants.ListingClaimName && c.Value == listing.ID.ToString())),
                    "DataEntryRepository.AddListing should add a listing claim for the specified roles");
                MockUserRepository.Verify(m => m.AddClaimAsync(It.Is<IdentityRole>(r => r == roleLookup[roleNames[1]]), It.Is<Claim>(c => c.Type == UserConstants.ListingClaimName && c.Value == listing.ID.ToString())),
                    "DataEntryRepository.AddListing should add a listing claim for the specified roles");
            };
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_UpdateListing_Success()
        { 
            //Arrange
            var parentListing = ArrangeListing(isParent: true);
            var parentListing2 = ArrangeListing(isParent: true);
            var childListing = ArrangeListing(isParent: false);
            var childListing2 = ArrangeListing(isParent: false);

            parentListing.CreatedAt = DateTime.MinValue;
            parentListing.UpdatedAt = DateTime.MinValue;

            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Add(parentListing);
                dbContext.Add(parentListing2);
                dbContext.SaveChanges();

                childListing.ParentListingID = parentListing.ID;
                childListing2.ParentListingID = parentListing2.ID;
                dbContext.Add(childListing);
                dbContext.Add(childListing2);
                dbContext.SaveChanges();
            }

            var user = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(user, It.IsAny<Listing>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).Returns(Task.Run(() => AuthorizationResult.Success()));

            var testStart = DateTime.UtcNow;

            //Act
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                var workingParentListing = dbContext.Listings.Find(parentListing.ID);
                var result = await repo.UpdateListing(workingParentListing, user);
            }

            //Assert
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var resultParentListing = dbContext.Listings.Find(parentListing.ID);
                Assert.NotNull(resultParentListing);
                Assert.True(testStart > resultParentListing.CreatedAt, "DataEntryRepository.UpdateListing should not alter the CreatedAt property");
                Assert.True(testStart <= resultParentListing.UpdatedAt, "DataEntryRepository.UpdateListing should update the UpdatedAt property");
                Assert.True(dbContext.Listings.Find(childListing.ID) != null, "DataEntryRepository.UpdateListing should not remove any existing children spaces of the listing");
                Assert.True(dbContext.Listings.Find(childListing2.ID) != null, "DataEntryRepository.UpdateListing should not remove any unrelated spaces");
            }
        }

        [Theory(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        [InlineData("Add user", new[] { "user1" }, new[] { "user1", "user2" })]
        [InlineData("Remove user", new[] { "user1", "user2" }, new[] { "user1" })]
        [InlineData("Add and remove users", new[] { "user1", "user2", "user3", "user4" }, new[] { "user1", "user3", "user5", "user6" })]
        public async void DataEntryRepository_UpdateListing_UpdateUserAuthorization (string testName, IEnumerable<string> currentUsers, IEnumerable<string> newUsers)
        {
            Assert.NotNull(testName);

            //Arrange
            var listing = ArrangeListing();
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Add(listing);
                dbContext.SaveChanges();
            }

            var userLookup = SetupUsers(currentUsers.Union(newUsers));

            MockUserRepository.Setup(r => r.FindByNamesAsync(It.IsAny<IEnumerable<string>>()))
                .Returns<IEnumerable<string>>(n => Task.FromResult(n.Select(u => userLookup[u])));

            MockUserRepository.Setup(r => r.GetUsersForListingAsync(listing.ID))
                .Returns(Task.FromResult(currentUsers.Select(u => userLookup[u])));

            var loggedInUser = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(loggedInUser, It.IsAny<Listing>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).Returns(Task.Run(() => AuthorizationResult.Success()));

            //Act
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                var result = await repo.UpdateListing(listing, loggedInUser, newUsers);
            }

            //Assert
            var addedUsers = newUsers.Where(newUser => !currentUsers.Contains(newUser));
            Assert.All(addedUsers, addedUser =>
            {
                MockUserRepository.Verify(
                    userRepo => userRepo.AddClaimAsync(
                        It.Is<IdentityUser>(user => user == userLookup[addedUser]), 
                        It.Is<Claim>(claim => claim.Type == UserConstants.ListingClaimName && claim.Value == listing.ID.ToString())),
                    "DataEntryRepository.UpdateListing should add a listing claim to the provided users if they didn't already have one");
            });

            var removedUsers = currentUsers.Where(currentUser => !newUsers.Contains(currentUser));
            Assert.All(removedUsers, removedUser =>
            {
                MockUserRepository.Verify(
                    userRepo => userRepo.RemoveClaimAsync(
                        It.Is<IdentityUser>(user => user == userLookup[removedUser]),
                        It.Is<Claim>(claim => claim.Type == UserConstants.ListingClaimName && claim.Value == listing.ID.ToString())),
                    "DataEntryRepository.UpdateListing should remove listing claims from users that are not in the new user list");
            });
        }



        [Theory(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        [InlineData("Add team", new[] { "team1" }, new[] { "team1", "team2" })]
        [InlineData("Remove team", new[] { "team1", "team2" }, new[] { "team1" })]
        [InlineData("Add and remove teams", new[] { "team1", "team2", "team3", "team4" }, new[] { "team1", "team3", "team5", "team6" })]
        public async void DataEntryRepository_UpdateListing_UpdateTeamAuthorization(string testName, IEnumerable<string> currentRoles, IEnumerable<string> newRoles)
        {
            Assert.NotNull(testName);

            //Arrange
            var listing = ArrangeListing();
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Add(listing);
                dbContext.SaveChanges();
            }

            var roleLookup = SetupRoles(currentRoles.Union(newRoles));

            MockUserRepository.Setup(r => r.FindRolesByTeamNamesAsync(It.IsAny<IEnumerable<string>>()))
                .Returns<IEnumerable<string>>(n => Task.FromResult(n.Select(u => roleLookup[u])));

            MockUserRepository.Setup(r => r.GetRolesForListingAsync(listing.ID))
                .Returns(Task.FromResult(currentRoles.Select(u => roleLookup[u])));

            MockUserRepository.Setup(r => r.ConvertTeamNameToRoleName(It.IsAny<string>())).Returns<string>(value => value);

            var loggedInTeam = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(loggedInTeam, It.IsAny<Listing>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).Returns(Task.Run(() => AuthorizationResult.Success()));

            //Act
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                var result = await repo.UpdateListing(listing, loggedInTeam, null, newRoles);
            }

            //Assert
            var addedRoles = newRoles.Where(newTeam => !currentRoles.Contains(newTeam));
            Assert.All(addedRoles, addedTeam =>
            {
                MockUserRepository.Verify(
                    userRepo => userRepo.AddClaimAsync(
                        It.Is<IdentityRole>(role => role == roleLookup[addedTeam]),
                        It.Is<Claim>(claim => claim.Type == UserConstants.ListingClaimName && claim.Value == listing.ID.ToString())),
                    "DataEntryRepository.UpdateListing should add a listing claim to the provided teams if they didn't already have one");
            });

            var removedRoles = currentRoles.Where(currentTeam => !newRoles.Contains(currentTeam));
            Assert.All(removedRoles, removedTeam =>
            {
                MockUserRepository.Verify(
                    userRepo => userRepo.RemoveClaimAsync(
                        It.Is<IdentityRole>(role => role == roleLookup[removedTeam]),
                        It.Is<Claim>(claim => claim.Type == UserConstants.ListingClaimName && claim.Value == listing.ID.ToString())),
                    "DataEntryRepository.UpdateListing should remove listing claims from teams that are not in the new team list");
            });
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_UpdateListing_FailsOnUnauthorizedAccess()
        {
            //Arrange
            var listing = ArrangeListing();
            listing.Name = "Good value";


            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Add(listing);
                dbContext.SaveChanges();
            }

            var user = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(user, It.IsAny<Listing>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).Returns(Task.Run(() => AuthorizationResult.Failed()));

            //Act
            Listing result;
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                var workingListing = dbContext.Listings.Find(listing.ID);
                workingListing.Name = "Bad value";
                result = await repo.UpdateListing(workingListing, user);
            }

            //Assert
            //TODO: should return a security exception instead
            Assert.True(result == null, "DataEntryRepository.UpdateListing should return null if access is denied");

            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var resultListing = dbContext.Listings.Find(listing.ID);
                Assert.True(resultListing.Name == "Good value", "DataEntryRepository.UpdateListing should not alter listing if access is denied");
            }
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_GetAllListings_Success()
        {
            //Arrange
            MockRawSqlProvider.Setup(x => x.GetAllUserListings).Returns("test");
            var listings = new List<Listing>
            {
                ArrangeListing(),
                ArrangeListing(),
                ArrangeListing(),
                ArrangeListing(),
                ArrangeListing()
            };

            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                foreach (var listing in listings)
                {
                    dbContext.Add(listing);
                }
                dbContext.SaveChanges();
            }

            var user = new ClaimsPrincipal();
            var filter = new List<FilterOption>();
            bool isAdmin = false;
            IEnumerable<Listing> result;
            var defaultRegionID = Region.DefaultID.ToString();
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var mockRepo = new Mock<DataEntryRepository>(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object);
                mockRepo.Protected().Setup<IQueryable<Listing>>(
                    "GetListingsQueryable", 
                    ItExpr.IsAny<IQueryable<Listing>>(),
                    ItExpr.IsAny<bool>(),
                    ItExpr.IsAny<bool>(),
                    ItExpr.IsAny<bool>(),
                    ItExpr.IsAny<bool>(),
                    ItExpr.IsAny<bool>())
                    .Returns(() => dbContext.Listings);
                //Act
                result = (await mockRepo.Object.GetAllListings(user, isAdmin, filter, defaultRegionID)).ToList();
            }

            //Assert
            Assert.Subset(listings.Select(l => l.ID).ToHashSet(), result.Select(l => l.ID).ToHashSet());
            Assert.Equal(listings.Count, result.Count());
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_GetListingById_Success()
        {
            //Arrange
            var listing = ArrangeListing();

            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Add(listing);
                dbContext.SaveChanges();
            }

            var validUser = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(validUser, It.IsAny<Listing>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(() => Task.Run(() => AuthorizationResult.Success()));

            //Act
            Listing result;
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                result = await repo.GetListingByID(listing.ID, validUser);
            }

            //Assert
            Assert.Equal(listing.ID, result.ID);
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_GetListingById_SuccessWithSpaces()
        {
            //Arrange
            var parentListings = new List<Listing>();
            parentListings.Add(ArrangeListing(isParent: true));
            parentListings.Add(ArrangeListing(isParent: true));
            parentListings.Add(ArrangeListing(isParent: true));

            var childListings = new List<Listing>();
            childListings.Add(ArrangeListing(isParent: false));
            childListings.Add(ArrangeListing(isParent: false));
            childListings.Add(ArrangeListing(isParent: false));
            childListings.Add(ArrangeListing(isParent: false));
            childListings.Add(ArrangeListing(isParent: false));


            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Listings.AddRange(parentListings);
                dbContext.SaveChanges();

                childListings[0].ParentListingID = parentListings[0].ID;
                childListings[1].ParentListingID = parentListings[0].ID;
                childListings[2].ParentListingID = parentListings[0].ID;
                childListings[3].ParentListingID = parentListings[1].ID;
                childListings[4].ParentListingID = parentListings[1].ID;

                dbContext.Listings.AddRange(childListings);
                dbContext.SaveChanges();
            }

            var validUser = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(validUser, It.IsAny<Listing>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(() => Task.Run(() => AuthorizationResult.Success()));

            //Act
            Listing result0, result1, result2;
            IEnumerable<Listing> resultSpaces0, resultSpaces1, resultSpaces2;
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                result0 = await repo.GetListingByID(parentListings[0].ID, validUser);
                result1 = await repo.GetListingByID(parentListings[1].ID, validUser);
                result2 = await repo.GetListingByID(parentListings[2].ID, validUser);

                resultSpaces0 = result0.Spaces.ToList();
                resultSpaces1 = result1.Spaces.ToList();
                resultSpaces2 = result2.Spaces.ToList();
            }

            //Assert
            Assert.Equal(parentListings[0].ID, result0.ID);
            Assert.Equal(3, resultSpaces0.Count());
            Assert.Contains(resultSpaces0, s => s.ID == childListings[0].ID);
            Assert.Contains(resultSpaces0, s => s.ID == childListings[1].ID);
            Assert.Contains(resultSpaces0, s => s.ID == childListings[2].ID);
            Assert.Equal(parentListings[1].ID, result1.ID);
            Assert.Equal(2, resultSpaces1.Count());
            Assert.Contains(resultSpaces1, s => s.ID == childListings[3].ID);
            Assert.Contains(resultSpaces1, s => s.ID == childListings[4].ID);
            Assert.Equal(parentListings[2].ID, result2.ID);
            Assert.Empty(resultSpaces2);
        }

        [Theory(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public async void DataEntryRepository_GetListingById_FailsOnUnknownId(int id)
        {
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                await Assert.ThrowsAsync<ArgumentException>(() => repo.GetListingByID(id, new ClaimsPrincipal()));
            }
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_GetListingById_FailsOnSpaceIds()
        {
            //Arrange
            var parentListing = ArrangeListing(isParent: true);
            var childListing = ArrangeListing(isParent: false);

            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Add(parentListing);
                dbContext.SaveChanges();

                childListing.ParentListingID = parentListing.ID;
                dbContext.Add(childListing);
                dbContext.SaveChanges();
            }

            //Act
            //Assert
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                await Assert.ThrowsAsync<ArgumentException>(() => repo.GetListingByID(childListing.ID, new ClaimsPrincipal()));
            }
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_GetListingById_RequiresAuthorization()
        {
            //Arrange
            var accessibleListing = ArrangeListing();
            var inaccessibleListing = ArrangeListing();
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Add(accessibleListing);
                dbContext.Add(inaccessibleListing);
                dbContext.SaveChanges();
            }

            var user = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(user, It.Is<Listing>(l => l.ID == accessibleListing.ID), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(() => Task.Run(() => AuthorizationResult.Success()));
            MockAuthService.Setup(m => m.AuthorizeAsync(user, It.Is<Listing>(l => l.ID == inaccessibleListing.ID), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(() => Task.Run(() => AuthorizationResult.Failed()));

            //Act
            Listing result;
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                result = await repo.GetListingByID(accessibleListing.ID, user);
            }

            //Assert
            Assert.Equal(accessibleListing.ID, result.ID);

            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                await Assert.ThrowsAsync<SecurityException>(() => repo.GetListingByID(inaccessibleListing.ID, user));
            }
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_DeleteListing_Success()
        {
            //Arrange
            var listing = ArrangeListing();
            var otherListing = ArrangeListing();
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Listings.Add(listing);
                dbContext.Listings.Add(otherListing);
                dbContext.SaveChanges();
            }

            var user = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(user, It.Is<Listing>(l => l.ID == listing.ID), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(() => Task.Run(() => AuthorizationResult.Success()));

            var userLookup = SetupUsers(new List<string>
            {
                "user1",
                "user2"
            });
            MockUserRepository.Setup(m => m.GetUsersForListingAsync(listing.ID)).Returns(Task.FromResult<IEnumerable<IdentityUser>>(userLookup.Values));

            var owner = userLookup.Values.First();
            MockUserRepository.Setup(m => m.GetListingOwnerAsync(listing.ID)).Returns(Task.FromResult(owner));

            var roleLookup = SetupRoles(new List<string>
            {
                "team1",
                "team2"
            });
            MockUserRepository.Setup(m => m.GetRolesForListingAsync(listing.ID)).Returns(Task.FromResult<IEnumerable<IdentityRole>>(roleLookup.Values));

            //Act
            bool result;
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                result = await repo.DeleteListing(listing.ID, user);
            }

            //Assert
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                Assert.True(!dbContext.Listings.Any(l => l.ID == listing.ID), "DataEntryRepository.DeleteListing should remove the listing from the db");
            }
            Assert.True(result, "DataEntryRepository.DeleteListing should return true on success");
            MockUserRepository.Verify(repo => repo.RemoveClaimAsync(owner, It.Is<Claim>(claim => claim.Type == UserConstants.OwnerClaimName && claim.Value == listing.ID.ToString())),
                "DataEntryRepository.DeleteListing should remove owner claim");
            Assert.All(userLookup.Values, removedUser =>
            {
                MockUserRepository.Verify(repo => repo.RemoveClaimAsync(removedUser, It.Is<Claim>(claim => claim.Type == UserConstants.ListingClaimName && claim.Value == listing.ID.ToString())),
                    "DataEntryRepository.DeleteListing should remove user claims");
            });
            Assert.All(roleLookup.Values, removedRole =>
            {
                MockUserRepository.Verify(repo => repo.RemoveClaimAsync(removedRole, It.Is<Claim>(claim => claim.Type == UserConstants.ListingClaimName && claim.Value == listing.ID.ToString())),
                    "DataEntryRepository.DeleteListing should remove role claims");
            });
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_DeleteListing_Unauthorized()
        {
            var listing = ArrangeListing();
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Listings.Add(listing);
                dbContext.SaveChanges();
            }

            var user = new ClaimsPrincipal();
            MockAuthService.Setup(m => m.AuthorizeAsync(user, It.IsAny<Listing>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
                .Returns(() => Task.Run(() => AuthorizationResult.Failed()));

            //Act
            bool result;
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                result = await repo.DeleteListing(listing.ID, user);
            }

            //Assert
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                Assert.True(dbContext.Listings.Any(l => l.ID == listing.ID), "DataEntryRepository.DeleteListing should not remove the listing from the db when the user is not authorized to do so");
            }
            Assert.False(result, "DataEntryRepository.DeleteListing should return false on unauthorized requests");
        }

        [Fact(Skip = "Needs rewrite due to ApplicationUserManager changes")]
        public async void DataEntryRepository_GetAllBrokers_Success()
        {
            //Arrange
            var broker = new Broker();
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                dbContext.Brokers.Add(broker);
                dbContext.SaveChanges();
            }

            //Act
            IEnumerable<Broker> result;
            using (var dbContext = new DataEntryContext(DbContextOptions, null))
            {
                var repo = new DataEntryRepository(dbContext, MockAuthService.Object, MockUserRepository.Object, LookupNormalizer, MockRawSqlProvider.Object, MockLogger.Object);
                result = await repo.GetAllBrokers();
            }

            //Assert
            Assert.Single(result);
        }

        private int __listingNameIndex = 0;
        private Listing ArrangeListing(string name = null, bool isParent = true, int? parentListingId = null)
        {
            if (name == null) name = isParent ? $"Listing {__listingNameIndex++}" : $"Space {__listingNameIndex++}";
            var listing = new Listing
            {
                Name = name,
                IsParent = isParent,
                ParentListingID = parentListingId
            };
            return listing;
        }

        private Dictionary<string, IdentityUser> SetupUsers(IEnumerable<string> userNames)
        {
            var result = new Dictionary<string, IdentityUser>(StringComparer.OrdinalIgnoreCase);
            foreach (var userName in userNames)
            {
                result[userName] = new IdentityUser(userName) { NormalizedUserName = LookupNormalizer.NormalizeName(userName) };
            }
            return result;
        }

        private Dictionary<string, IdentityRole> SetupRoles(IEnumerable<string> roleNames)
        {
            var result = new Dictionary<string, IdentityRole>(StringComparer.OrdinalIgnoreCase);
            foreach (var roleName in roleNames)
            {
                result[roleName] = new IdentityRole(roleName) { NormalizedName = LookupNormalizer.NormalizeName(roleName) };
            }
            return result;
        }
    }
}
