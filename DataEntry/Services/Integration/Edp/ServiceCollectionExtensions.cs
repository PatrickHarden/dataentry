using dataentry.Services.Business.Publishing;
using dataentry.Services.Integration.ApiStore;
using dataentry.Services.Integration.Edp.Consumption;
using dataentry.Services.Integration.Edp.Ingestion;
using dataentry.Services.Integration.Edp.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace dataentry.Services.Integration.Edp
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEdpIntegration(this IServiceCollection services)
        {
            #region Common
            services.AddOptions<AwsS3BucketOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection("Edp").GetSection("AwsS3Bucket").Bind(options);
            });
            services.AddOptions<PropertyMapperOptions>().Configure<IConfiguration>((options, configuration) => 
            {
                configuration.GetSection("Edp").GetSection("MappingOptions").Bind(options);
            });
            services.AddTransient<IListingAdapter, ListingAdapter>();
            services.AddTransient<IPropertyMapper, PropertyMapper>();
            services.AddTransient<IPublishingTarget, PublishingTarget>();
            #endregion Common

            #region Ingestion
            services.AddOptions<IngestionApiStoreOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection("ApiStore").GetSection("Ingestion").Bind(options);
            }); 
            services.AddOptions<IngestionOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection("Edp").GetSection("Ingestion").Bind(options);
            });

            services.AddTransient<IngestionHttpClientHandler>();
            services.AddTransient<IIngestionService, IngestionService>();
            services
                .AddHttpClient<IIngestionService, IngestionService>("EdpHttpClient", (serviceProvider, httpClient) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<IngestionApiStoreOptions>>().Value;
                    httpClient.BaseAddress = options.Url;
                })
                .ConfigurePrimaryHttpMessageHandler(serviceProvider => serviceProvider.GetRequiredService<IngestionHttpClientHandler>());

            #endregion Ingestion

            #region Consumption
            services.AddOptions<ConsumptionApiStoreOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection("ApiStore").GetSection("Consumption").Bind(options);
            }); 
            services.AddOptions<ConsumptionOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection("Edp").GetSection("Consumption").Bind(options);
            });
            
            services.AddTransient<ConsumptionHttpClientHandler>();
            services.AddTransient<IConsumptionService, ConsumptionService>();
            services
                .AddHttpClient<IConsumptionService, ConsumptionService>("EdpConsumptionHttpClient", (serviceProvider, httpClient) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<ConsumptionApiStoreOptions>>().Value;
                    httpClient.BaseAddress = options.Url;
                })
                .ConfigurePrimaryHttpMessageHandler(serviceProvider => serviceProvider.GetRequiredService<ConsumptionHttpClientHandler>());
            #endregion Consumption
        }
    }
}
