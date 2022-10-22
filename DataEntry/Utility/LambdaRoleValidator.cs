using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Utility
{
    public class LambdaRoleValidator<TRole> : IRoleValidator<TRole> where TRole : class
    {
        private readonly Func<RoleManager<TRole>, TRole, Task<IdentityResult>> _func;

        public LambdaRoleValidator(Func<RoleManager<TRole>, TRole, Task<IdentityResult>> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Task<IdentityResult> ValidateAsync(RoleManager<TRole> manager, TRole role)
        {
            return _func(manager, role);
        }
    }
}
