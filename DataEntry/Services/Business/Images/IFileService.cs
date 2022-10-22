using dataentry.ViewModels.GraphQL;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dataentry.Services.Business.Images
{
    public interface IFileService
    {
        Task<string> GetFileBinary(string url);
    }
}
