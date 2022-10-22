using dataentry.Publishing.Commands;
using dataentry.Publishing.Repository;
using dataentry.Publishing.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(dataentry.Publishing.Startup))]
namespace dataentry.Publishing
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            // Commands
            services.AddSingleton<ICommand, UpdatePublishListingsState>();

            // Dependencies
            services.AddSingleton<IPublishingDataEntryRepository, PublishingDataEntryRepository>();
            services.AddSingleton<IStorePublishService, StorePublishService>();
        }
    }
}
