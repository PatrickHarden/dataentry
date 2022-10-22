using dataentry.ViewModels.GraphQL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Configs
{
    public interface IConfigService
    {
        Task<ConfigsViewModel> GetConfigs();
        Task<string> GetDefaultCultureCode();
    }
}
