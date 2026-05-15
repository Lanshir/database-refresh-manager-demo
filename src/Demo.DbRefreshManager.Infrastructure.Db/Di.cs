using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DbRefreshManager.Infrastructure.Db;

public static class Di
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Регистрация сервисов БД.
        /// </summary>
        public IServiceCollection AddDatabase(
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

            return services;
        }

        /// <summary>
        /// Регистрация фич БД.
        /// </summary>
        public IServiceCollection AddDatabaseFeatures()
        {
            // Поиск всех реализаций IHandlerBase,
            // регистрация в di под ближайшим реализованным интерфейсом.
            typeof(Di).Assembly
                .GetTypes()
                .Where(t => t.IsClass && t.IsAssignableTo(typeof(IHandlerBase)))
                .ToList()
                .ForEach(t => services.AddTransient(t.GetInterfaces().First(), t));

            return services;
        }
    }
}
