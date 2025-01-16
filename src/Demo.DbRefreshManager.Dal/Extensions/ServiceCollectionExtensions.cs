using Demo.DbRefreshManager.Common.Config.Abstract;
using Demo.DbRefreshManager.Dal.Context;
using Demo.DbRefreshManager.Dal.Repositories.Abstract.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DbRefreshManager.Dal.Extensions;

/// <summary>
/// Расширения IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация сервисов БД.
    /// </summary>
    public static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool enableSensitiveDataLogging = false)
    {
        var connectionString = configuration
            .GetValue<string>(nameof(IAppConfig.DbConnectionString));

        services.AddPooledDbContextFactory<AppDbContext>(o =>
        {
            o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            o.UseSqlite(connectionString);
            o.EnableSensitiveDataLogging(enableSensitiveDataLogging);
        });

        // Repositories injection.
        var repositoryAssembly = typeof(IRepository<>).Assembly;
        var assemblyTypes = repositoryAssembly.GetTypes();

        var interfaces = assemblyTypes
            .Where(t => t.IsInterface
                && t.GetInterfaces().Any(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
            .ToArray();

        foreach (var i in interfaces)
        {
            var implementation = assemblyTypes
                .Where(t => t.IsAssignableTo(i))
                .First();

            services.AddScoped(i, implementation);
        }

        return services;
    }
}
