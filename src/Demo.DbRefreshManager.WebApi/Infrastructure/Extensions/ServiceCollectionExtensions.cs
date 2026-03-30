using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Demo.DbRefreshManager.Common.Config.Abstract;
using Demo.DbRefreshManager.Common.Converters.Abstract;
using Demo.DbRefreshManager.Services.Abstract;
using Demo.DbRefreshManager.Services.Concrete;
using Demo.DbRefreshManager.WebApi.GraphQL.Mutations.Base;
using Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;
using Demo.DbRefreshManager.WebApi.GraphQL.Subscriptons;
using Demo.DbRefreshManager.WebApi.Infrastructure.Config;
using Demo.DbRefreshManager.WebApi.Infrastructure.Converters;
using Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Abstract;
using Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Concrete;
using Demo.DbRefreshManager.WebApi.Infrastructure.HotChocolate;
using Demo.DbRefreshManager.WebApi.Infrastructure.Serilog;
using Demo.DbRefreshManager.WebApi.Jobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi;
using Quartz;
using Serilog;
using System.Reflection;
using Path = System.IO.Path;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;

/// <summary>
/// Расширения IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Регистрация ключевых зависимостей приложения.
        /// </summary>
        public IServiceCollection AddProjectCoreDependencies()
        {
            services.AddSingleton<IAppConfig, AppConfig>();
            services.AddSingleton<IJsonConverter, JsonConverter>();

            services.AddScoped<IUserIdentityHelper, UserIdentityHelper>();
            services.AddScoped<IDomainControllerService, DomainControllerService>();

            services.AddTransient<ISshClientService, SshClientService>();

            return services;
        }

        /// <summary>
        /// Регистрация логирования.
        /// </summary>
        public IServiceCollection AddLoggingServices(
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            services.AddLogging(builder => builder.AddSerilog(
                SerilogConfig.GetConfiguredLogger(environment, configuration)));

            return services;
        }

        /// <summary>
        /// Регистрация cookie авториации.
        /// </summary>
        public IServiceCollection AddCookieAuthentication(IConfiguration configuration)
        {
            var cookieLifetimeMinutes = configuration
                .GetValue<int>(nameof(IAppConfig.AuthCookieLifetimeMinutes));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // Unauthorized return 401.
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                    // Access denied return 403.
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    };
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieLifetimeMinutes);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                });

            return services;
        }

        /// <summary>
        /// Регистрация версионирования REST API.
        /// </summary>
        public IServiceCollection AddRestVersioning()
        {
            services
                .AddApiVersioning(o =>
                {
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.ReportApiVersions = true;
                })
                .AddApiExplorer(o =>
                {
                    o.GroupNameFormat = "'v'VVV";
                    o.SubstituteApiVersionInUrl = true;
                });

            return services;
        }

        /// <summary>
        /// Регистрация зависимостей GraphQL.
        /// </summary>
        public IServiceCollection AddGraphQLServices()
        {
            var builder = services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<QueryBase>()
                .AddMutationType<MutationBase>()
                .AddSubscriptionType<SubscriptionsBase>()
                .AddInMemorySubscriptions()
                .AddProjections()
                .AddErrorFilter<GraphQLErrorFilter>();

            // Регистрация расширений типов схемы graphQl через ExtendObjectTypeAttribute.
            _ = typeof(QueryBase).Assembly
                .GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(ExtendObjectTypeAttribute<>)).Any())
                .Select(builder.AddTypeExtension)
                .ToArray();

            return services;
        }

        /// <summary>
        /// Регистрация фоновых задач Quartz.
        /// </summary>
        public IServiceCollection AddQuartzJobsServices(IWebHostEnvironment environment)
        {
            // Отключение фоновых задач через окружение.
            if (environment.IsQuartzDisabled())
            {
                Console.WriteLine(string.Join(" ",
                    "Quartz jobs are disabled.",
                    "Set environment DISABLE_QUARTZ_JOBS = false to enable."));

                return services;
            }

            services.AddQuartz(q =>
            {
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(maxConcurrency: 10);
                q.UseTimeZoneConverter();

                // Jobs.
                // Quartz CRON description:
                // https://www.freeformatter.com/cron-expression-generator-quartz.html

                // (!!!) Docker контейнер по ум. работает в UTC.
                var ruTimezone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

                q.ScheduleJob<DatabaseRefreshCallerJob>(tg =>
                    // 0 sec of every minute.
                    tg.WithCronSchedule("0 * * ? * *")
                    .StartNow());

                q.ScheduleJob<LogsCleanerJob>(tg =>
                    // 0 sec 0 min 0 hour every day.
                    tg.WithCronSchedule("0 0 0 ? * *",
                        b => b.InTimeZone(ruTimezone))
                    .StartNow());
            });

            services.AddQuartzHostedService(o =>
            {
                o.WaitForJobsToComplete = true;
                o.AwaitApplicationStarted = true;
            });

            return services;
        }

        /// <summary>
        /// Добавление генерации документации swagger.
        /// </summary>
        public IServiceCollection AddSwaggerGeneration()
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                using var serviceProvider = services.BuildServiceProvider();

                var assembly = typeof(Program).Assembly;

                var environment = serviceProvider
                    .GetRequiredService<IWebHostEnvironment>();

                var provider = serviceProvider
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                var assemblyDescription = assembly
                    .GetCustomAttribute<AssemblyDescriptionAttribute>()
                    ?.Description ?? string.Empty;

                var assemblyProduct = assembly
                    .GetCustomAttribute<AssemblyProductAttribute>()
                    ?.Product ?? string.Empty;

                // Add docs generation for each api version.
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo()
                    {
                        Title = $"{assemblyProduct} {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = description.IsDeprecated
                            ? $"{assemblyDescription} - DEPRECATED"
                            : $"{assemblyDescription}"
                    });
                }

                // Add xml comments.
                var currentAssembly = Assembly.GetExecutingAssembly();
                var contentPath = environment.ContentRootPath;

                var xmlDocs = currentAssembly.GetReferencedAssemblies()
                    .Append(currentAssembly.GetName())
                    .Select(a => Path.Combine(contentPath, $"{a.Name}.xml"))
                    .Where(f => File.Exists(f))
                    .ToArray();

                foreach (var xmlDoc in xmlDocs)
                {
                    options.IncludeXmlComments(xmlDoc);
                }
            });

            return services;
        }
    }
}
