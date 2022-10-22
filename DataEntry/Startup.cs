using dataentry.AutoGraph;
using dataentry.Data.DBContext;
using dataentry.Data.DBContext.SQL;
using dataentry.Extensions;
using dataentry.Middleware;
using dataentry.Repository;
using dataentry.Services.Business.BulkUpload;
using dataentry.Services.Business.Configs;
using dataentry.Services.Business.Contacts;
using dataentry.Services.Business.Images;
using dataentry.Services.Business.Listings;
using dataentry.Services.Business.Publishing;
using dataentry.Services.Business.Users;
using dataentry.Services.Integration.Authorization;
using dataentry.Services.Integration.StoreApi;
using dataentry.Services.Integration.StoreApi.Model;
using dataentry.Utility;
using dataentry.ViewModels.GraphQL;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.IdentityModel.Tokens;
using ProxyKit;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Hosting;
using dataentry.Services.Background.WatermarkDetection;
using dataentry.Services.Integration.Edp;
using dataentry.Services.Business.Regions;
using dataentry.Data;
using dataentry.Services.Business.Report;
using dataentry.Services.Integration.SearchApi;

namespace dataentry
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public static IConfiguration Configuration { get; private set; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Env.IsEnvironment("Local"))
            {
                // Prevent local debugging console spew
                Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryDebugWriter.IsTracingDisabled = true;
            }
            services.AddApplicationInsightsTelemetry();
            services.Configure<Configs>(Configuration);

            var configs = Configuration.Get<Configs>();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddAzureAdBearer(options => Configuration.Bind("AzureAd", options))
            .AddJwtBearer("IdentityLogin", options =>
            {
                var key = Encoding.ASCII.GetBytes(configs.IdentitySettings.Key);
                options.Audience = "localhost";
                options.ClaimsIssuer = "cbrelistings.com";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            })
            .AddTestIdentity(options => Configuration.Bind("IdentitySettings", options))
            .AddIdentityCookies();

            services.AddMemoryCache();

            services.AddDistributedMemoryCache();

            services.AddSession();

            services.AddAdminOptions();

            services.AddScoped<IAuthorizationHandler, ListingAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, TeamAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, RegionAuthorizationHandler>();

            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<RoleStore<IdentityRole, UserContext>>()
                .AddUserManager<ApplicationUserManager>()
                .AddSignInManager<SignInManager<IdentityUser>>();

            services.AddScoped<IUserLoginService, UserLoginService>();

            services.AddMvc(options =>
                {
                    // Give all controllers Authorize("Bearer, IdentityLogin")
                    var policy = new AuthorizationPolicyBuilder("Bearer", "IdentityLogin")
                                    .RequireAuthenticatedUser()
                                    .Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddDbContext<DataEntryContext>(ServiceLifetime.Scoped);
            services.AddDbContext<UserContext>(ServiceLifetime.Scoped);
            services.AddTransient<IDataEntryRepository, DataEntryRepository>();
            services.AddTransient<IAzureStorageRepository, AzureStorageRepository>();

            services.AddTransient<ISiteMapsConfigDataProvider, SiteMapsConfigDataProvider>();
            services.AddTransient<IListingService, ListingService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IListingAdapter, ListingAdapter>();
            services.AddSingleton<IListingMapper, ListingMapper>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<ITeamMapper, TeamMapper>();
            services.AddSingleton<IClaimantMapper, ClaimantMapper>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddSingleton<IRegionMapper, RegionMapper>();
            services.AddOptions<SearchApiServiceOptions>().Bind(Configuration.GetSection("SearchApiService"));
            services.AddTransient<ISearchApiService, SearchApiService>();
            services.AddSingleton<IStoreApiListingMapper, StoreApiListingMapper>();
            services.AddHttpClient<ISearchApiService, SearchApiService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration["SearchApiService:Url"]);
            });
            EnumAliasExtensions.Init(Assembly.GetExecutingAssembly(),
                $"{nameof(dataentry)}.{nameof(dataentry.Data)}.{nameof(dataentry.Data.Enums)}");

            // GraphQL Services
            services.AddAllGraphTypes(Assembly.GetExecutingAssembly());

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<ISchema, DataEntrySchema>(sp =>
            {
                return new DataEntrySchema(new FuncDependencyResolver(type => sp.GetService(type)));
            });

            // Publishing
            services.AddTransient<IPublishingService, PublishingService>();
            services.AddTransient<IPublishingTarget, Services.Integration.StoreApi.PublishingTarget>();

            // Background Service
            services.AddSingleton<IHostedService, WatermarkDetectionService>();

            services
                .AddHttpClient<IStoreService, StoreService>("StoreApiHttpClient", httpClient =>
                {
                    httpClient.BaseAddress = new Uri(Configuration["StoreSettings:StoreUrl"]);
                })
                .ConfigurePrimaryHttpMessageHandler(() =>
                    new HttpClientHandler
                    {
                        Credentials = new NetworkCredential(
                            Configuration["StoreSettings:StoreApiServiceAccountUsername"],
                            Configuration["StoreSettings:StoreApiServiceAccountPassword"],
                            Configuration["StoreSettings:StoreApiServiceAccountDomain"])
                    }
                );

            services.AddHttpClient("WatermarkDetectionHttpClient", httpClient =>
                {
                    httpClient.BaseAddress = new Uri(Configuration["WatermarkDetection:WatermarkDetectAPI"]);
                    httpClient.Timeout = new TimeSpan(0, 1, 0);
                });

            services.AddHttpClient<ISiteMapsConfigDataProvider, SiteMapsConfigDataProvider>();

            services.AddEdpIntegration();
            services.AddReportMappings();

            services.AddTransient<IBulkUploadService, BulkUploadService>();

            services.AddSingleton<IRawSqlProvider, RawPSQLProvider>();

            services.AddSingleton<IListingSerializer, ListingSerializer>();
            services.AddSingleton<IJsonDeltaEvaluator, JsonDeltaEvaluator>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddProxy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsEnvironment("Local") || env.IsEnvironment("Dev"))
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseGraphiQl();

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAdminRoles();

            app.UseSession();
            app.UseMiddleware<CreateUserMiddleware>();
            app.UseMiddleware<AddAdminMiddleware>();
            app.UseMiddleware<SyncClaimsMiddleware>();
            app.UseAuthorization();

            // Setup routes for preview controller
            app.UseEndpoints(routes =>
            {
                // Get preview site setting configs
                var sites = new List<Site>();
                Configuration.Bind("PreviewSettings:Sites", sites);

                foreach (var site in sites)
                {
                    routes.MapControllerRoute(
                        name: site.HomeSiteId,
                        pattern: $"{{homeSiteId}}{site.ControllerPath}{{usageType}}/details/{{propertyId}}",
                        defaults: new { controller = "Preview", action = "Index" });
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            // Get proxy settings
            var proxySettings = new List<Proxy>();
            Configuration.Bind("PreviewSettings:Proxy", proxySettings);

            // Set proxy for each url
            proxySettings.ForEach(proxySetting =>
            {
                app.Map(proxySetting.PathMatch, appProxy => appProxy.RunProxy(context => context
                    .ForwardTo(proxySetting.UpStreamHost)
                    .AddXForwardedHeaders()
                    .Send()));
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsEnvironment("Local"))
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            app.UseDefaultRegionConfigValues();
        }
    }
}
