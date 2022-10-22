using dataentry.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dataentry.Services.Business.Report
{
    public static class ReportServiceCollectionExtensions
    {
        public static void AddReportMappings(this IServiceCollection services)
        {
            services.AddOptions<ReportMappingOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection("Report").Bind(options);
            });
        }
    }
}
