using Demo.DbRefreshManager.Application.Repositories.Base;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Demo.DbRefreshManager.Infrastructure.Db.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DbRefreshManager.Infrastructure.Db;

public static class Di
{
    /// <summary>
    /// Регистрация сервисов БД.
    /// </summary>
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration,
        bool enableSensitiveDataLogging = false)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddPooledDbContextFactory<AppDbContext>(o =>
        {
            o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            o.UseSqlite(connectionString);
            o.EnableSensitiveDataLogging(enableSensitiveDataLogging);
        });

        // Repositories injection.
        var interfaces = typeof(IRepository<>).Assembly
            .GetTypes()
            .Where(t => t.IsInterface
                && t.GetInterfaces().Any(i => i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
            .ToArray();

        foreach (var i in interfaces)
        {
            var implementation = typeof(BaseRepository<>).Assembly
                .GetTypes()
                .First(t => t.IsAssignableTo(i));

            services.AddScoped(i, implementation);
        }

        return services;
    }
}
