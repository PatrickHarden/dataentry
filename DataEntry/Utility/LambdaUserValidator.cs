using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Utility
{
    public class LambdaUserValidator<TUser> : IUserValidator<TUser> where TUser : class
    {
        private readonly Func<UserManager<TUser>, TUser, Task<IdentityResult>> _func;

        public LambdaUserValidator(Func<UserManager<TUser>, TUser, Task<IdentityResult>> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            return _func(manager, user);
        }
    }
}
