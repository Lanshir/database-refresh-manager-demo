using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.WebApi.GraphQL.Mutations.Base;
using Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;
using Demo.DbRefreshManager.WebApi.GraphQL.Subscriptons;
using Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;
using Demo.DbRefreshManager.WebApi.Infrastructure.HotChocolate;
using Demo.DbRefreshManager.WebApi.Infrastructure.Options;
using Demo.DbRefreshManager.WebApi.Infrastructure.Serilog;
using Demo.DbRefreshManager.WebApi.Infrastructure.Services;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;
using Demo.DbRefreshManager.WebApi.Jobs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quartz;
using Serilog;
using System.Reflection;

namespace Demo.DbRefreshManager.WebApi;

public static class Di
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Регистрация сервисов проекта web api.
        /// </summary>
        public IServiceCollection AddWebApiServices()
        {
            services.AddScoped<IUserIdentityProvider, UserIdentityProvider>();

            return services;
        }

        /// <summary>
        /// Регистрация провайдеров параметров web api.
        /// </summary>
        public IServiceCollection AddWebApiOptions()
        {
            services.AddOptions<FrontendOptions>()
                .BindConfiguration(nameof(FrontendOptions));

            services.AddOptions<AuthCookieOptions>()
                .BindConfiguration(nameof(AuthCookieOptions));

            services.AddOptions<LdapOptions>()
                .BindConfiguration(nameof(LdapOptions));

            return services;
        }

        /// <summary>
        /// Регистрация логирования web api.
        /// </summary>
        public IServiceCollection AddWebApiLogging(
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
                .GetSection(nameof(AuthCookieOptions))
                .GetValue<int>(nameof(AuthCookieOptions.LifetimeMinutes));

            services.AddOptions<AuthCookieOptions>()
                .BindConfiguration(nameof(AuthCookieOptions));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // Unauthorized return 401.
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    // Access denied return 403.
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
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
        /// Регистрация генерации документов OpenApi для поддерживаемых версий Api.
        /// </summary>
        public IServiceCollection AddVersionedOpenApiDocs()
        {
            SupportedApiVersions.VersionsList.ForEach(v =>
                services.AddOpenApi($"v{v}"));

            return services;
        }

        /// <summary>
        /// Регистрация зависимостей GraphQL.
        /// </summary>
        public IServiceCollection AddGraphQL()
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
        public IServiceCollection AddQuartzJobs(IWebHostEnvironment environment)
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
    }
}
