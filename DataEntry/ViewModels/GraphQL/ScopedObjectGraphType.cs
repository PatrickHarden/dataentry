using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace dataentry.ViewModels.GraphQL
{
    /// <summary>
    /// This class allows GraphQL to utilize DI Scoped Lifetime services.
    /// It appears that this GraphQL lib has a known issue with Scoped services.
    /// See this github issue: https://github.com/graphql-dotnet/graphql-dotnet/issues/648
    /// If this class is not used - GraphQL's DocumentExecuter seems to treat Scoped services
    /// like EF's DbContext as Singletons. Using DbContext as a Singleton leads to corruption and deadlocks.
    /// </summary>
    public class ScopedObjectGraphType : ObjectGraphType
    {
        IHttpContextAccessor _httpContextAccessor;
        public ScopedObjectGraphType(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// A service locator which utilizes Asp.Net Core HttpContext so that GraphQL
        /// respects proper request scope.
        /// </summary>

        protected T GetService<T>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService<T>();
        }
    }
}
