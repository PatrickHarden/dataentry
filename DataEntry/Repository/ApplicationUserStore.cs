using dataentry.Data.Constants;
using dataentry.Data.DBContext;
using dataentry.Data.DBContext.Model;
using dataentry.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace dataentry.Repository
{
    public class ApplicationUserStore : UserStore<IdentityUser, IdentityRole, UserContext>
    {
        private static readonly string[] _userSearchClaimTypes = new[] { ClaimTypes.GivenName, ClaimTypes.Surname };

        private DbSet<IdentityRole> Roles => Context.Set<IdentityRole>();

        private DbSet<IdentityUserClaim<string>> UserClaims => Context.Set<IdentityUserClaim<string>>();

        private DbSet<IdentityUserRole<string>> UserRoles => Context.Set<IdentityUserRole<string>>();

        private DbSet<IdentityRoleClaim<string>> RoleClaims => Context.Set<IdentityRoleClaim<string>>();


        public ApplicationUserStore(UserContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        public async Task<IEnumerable<IdentityUser>> FindByNameAsync(IEnumerable<string> normalizedUserNames, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (normalizedUserNames == null) return new List<IdentityUser>();
            return await Users.Where(u => normalizedUserNames.Contains(u.NormalizedUserName)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<IdentityUser>> FindBySearchTermAsync(string normalizedSearchTerm, IEnumerable<string> normalizedBlacklist, int? skip, int? take, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            // Search the user table
            var claimQuery = Users

                //Include the user claims for each user
                .GroupJoin(
                    UserClaims,
                    user => user.Id,
                    userClaim => userClaim.UserId,
                    (user, userClaims) => new { user, userClaims })
                .SelectMany(
                    joined => joined.userClaims.DefaultIfEmpty(),
                    (joined, userClaim) => new { joined.user, userClaim });

            if (normalizedSearchTerm != null)
            {
                claimQuery = claimQuery
                    .Where(joined =>
                        EF.Functions.Like(joined.user.NormalizedUserName, $"%{normalizedSearchTerm}%")
                        || (_userSearchClaimTypes.Contains(joined.userClaim.ClaimType)
                            && EF.Functions.ILike(joined.userClaim.ClaimValue, $"%{normalizedSearchTerm}%")));
            }

            if (normalizedBlacklist != null)
            {
                claimQuery = claimQuery.Where(joined => !normalizedBlacklist.Contains(joined.user.NormalizedUserName));
            }

            claimQuery = claimQuery.OrderBy(joined => joined.user.NormalizedUserName);

            var userQuery = claimQuery.Select(joined => joined.user).Distinct();

            if (skip != null) userQuery = userQuery.Skip(skip.Value);
            if (take != null) userQuery = userQuery.Take(take.Value);

            return await userQuery.ToListAsync(cancellationToken);
        } 

        public async Task<bool> HasClaimAsync(string userId, Expression<Func<string, string, bool>> condition)
        {
            var parameterName = condition.Parameters[0].Name;
            var userClaimCondition = new ClaimExpressionVisitor<IdentityUserClaim<string>, bool>().VisitClaimExpression(condition);
            var roleClaimCondition = new ClaimExpressionVisitor<IdentityRoleClaim<string>, bool>().VisitClaimExpression(condition);
            Expression.Parameter(typeof(IdentityUserClaim<string>), nameof(IdentityUserClaim<string>.ClaimType));
            if (userId == null) return false;
            var query = UserClaims
                .Where(uc => uc.UserId == userId)
                .Where(userClaimCondition)
                .Select(uc => 1)
                .Union(UserRoles
                    .Where(ur => userId == ur.UserId)
                    .Join(
                        RoleClaims,
                        ur => ur.RoleId,
                        rc => rc.RoleId,
                        (ur, rc) => rc
                    )
                    .Where(roleClaimCondition)
                    .Select(rc => 1));

            return await query.AnyAsync();
        }

        private class ClaimExpressionVisitor<TClaim, TOut> : ExpressionVisitor
        {
            private ParameterExpression _claimTypeParameter;
            private MemberExpression _claimTypeProperty;
            private ParameterExpression _claimValueParameter;
            private MemberExpression _claimValueProperty;

            public Expression<Func<TClaim, TOut>> VisitClaimExpression(Expression<Func<string, string, TOut>> node) => (Expression<Func<TClaim,TOut>>)Visit(node);

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var parameter = Expression.Parameter(typeof(TClaim));

                _claimTypeParameter = node.Parameters[0];
                _claimTypeProperty = Expression.Property(parameter, "ClaimType");
                _claimValueParameter = node.Parameters[1];
                _claimValueProperty = Expression.Property(parameter, "ClaimValue");

                return Expression.Lambda<Func<TClaim, TOut>>(Visit(node.Body), parameter);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _claimTypeParameter ? _claimTypeProperty : 
                    node == _claimValueParameter ? _claimValueProperty :
                    base.VisitParameter(node);
            }
        }

        public async Task<IEnumerable<Claimant>> FindClaimantsBySearchTermAsync(string normalizedSearchTerm, IEnumerable<string> normalizedBlacklist, int? skip, int? take, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            // Search the user table
            var query = Users

                //Include the user claims for each user
                .GroupJoin(
                    UserClaims,
                    user => user.Id,
                    userClaim => userClaim.UserId,
                    (user, userClaims) => new { user, userClaims })
                .Select(
                    joined => new Claimant
                    {
                        Name = joined.user.UserName,
                        NormalizedName = joined.user.NormalizedUserName,
                        FirstName = joined.userClaims
                            .Where(c => c.ClaimType == ClaimTypes.GivenName)
                            .Select(c => c.ClaimValue)
                            .FirstOrDefault(),
                        LastName = joined.userClaims
                            .Where(c => c.ClaimType == ClaimTypes.Surname)
                            .Select(c => c.ClaimValue)
                            .FirstOrDefault(),
                        FullName = joined.userClaims
                            .Where(c => c.ClaimType == "name")
                            .Select(c => c.ClaimValue)
                            .FirstOrDefault(),
                        IsTeam = false
                    })
                .Concat(
                    Roles.Join(
                        RoleClaims,
                        role => role.Id,
                        roleClaim => roleClaim.RoleId,
                        (role, roleClaim) => new { role, roleClaim })
                    .Where(joined => joined.roleClaim.ClaimType == UserConstants.TeamRoleClaimName)
                    .Select(joined => new Claimant
                    {
                        Name = joined.role.Name.Replace(UserConstants.TeamRoleNamePrefix, ""),
                        NormalizedName = joined.role.NormalizedName.Replace(UserConstants.TeamRoleNamePrefixNormalized, ""),
                        FirstName = null,
                        LastName = null,
                        IsTeam = true
                    }))
                .Distinct();

            if (normalizedSearchTerm != null)
            {
                //Filter by search term
                query = query
                    .Where(claimant =>
                        EF.Functions.ILike(claimant.NormalizedName, $"%{normalizedSearchTerm}%")
                        || EF.Functions.ILike(claimant.FirstName, $"%{normalizedSearchTerm}%")
                        || EF.Functions.ILike(claimant.LastName, $"%{normalizedSearchTerm}%"));
            }

            if (normalizedBlacklist != null)
            {
                query = query.Where(claimant => !normalizedBlacklist.Contains(claimant.NormalizedName));
            }

            query = query.OrderBy(claimant => claimant.NormalizedName);

            if (skip != null) query = query.Skip(skip.Value);
            if (take != null) query = query.Take(take.Value);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<IdentityRole>> FindRolesByNamesAsync(IEnumerable<string> normalizedRoleNames, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (normalizedRoleNames == null) return new List<IdentityRole>();

            var roleClaimQuery = RoleClaims
                .Where(roleClaim => roleClaim.ClaimType == UserConstants.TeamRoleClaimName)
                .Join(Roles,
                    roleClaim => roleClaim.RoleId,
                    role => role.Id,
                    (roleClaim, role) => new { roleClaim, role })
                .Where(joined => normalizedRoleNames.Contains(joined.role.NormalizedName));

            var roleQuery = roleClaimQuery
                .OrderBy(joined => joined.role.NormalizedName)
                .Select(joined => joined.role);

            return await roleQuery.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<IdentityRole>> GetRolesForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var query = RoleClaims

                //Search for the queried claim
                 .Where(roleClaim => roleClaim.ClaimType == claim.Type && roleClaim.ClaimValue == claim.Value)

                 //Return the role
                 .Join(Roles,
                     roleClaim => roleClaim.RoleId,
                     role => role.Id,
                     (roleClaim, role) => role);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<IdentityRole>> GetTeamsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) return new List<IdentityRole>();
            var isAdmin = await HasClaimAsync(user.Id, (claimType, claimValue) => claimType == UserConstants.AdminClaimType);

            var query = RoleClaims

                //All team RoleClaims
                .Where(roleClaim => roleClaim.ClaimType == UserConstants.TeamRoleClaimName)

                //Get the roles
                .Join(Roles,
                    roleClaim => roleClaim.RoleId,
                    role => role.Id,
                    (roleClaim, role) => role);
            
            if (!isAdmin)
            {
                query = query
                //Join users
                .Join(UserRoles,
                        role => role.Id,
                        userRole => userRole.RoleId,
                        (role, userRole) => new { role, userRole })

                //Match queried user
                .Where(joined => joined.userRole.UserId == user.Id)

                //Return the role
                .Select(joined => joined.role)
                
                .Distinct();
            }

            query = query.OrderBy(role => role.NormalizedName);
            
            return await query.ToListAsync(cancellationToken);
        }

        public class ExpressionClaim
        {
            //
            // Summary:
            //     Gets or sets the claim type for this claim.
            public virtual string ClaimType { get; set; }
            //
            // Summary:
            //     Gets or sets the claim value for this claim.
            public virtual string ClaimValue { get; set; }
        }
    }
}
