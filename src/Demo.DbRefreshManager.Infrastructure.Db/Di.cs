using Demo.DbRefreshManager.Application.Repositories.Base;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Infrastructure.Db.Context;
using Demo.DbRefreshManager.Infrastructure.Db.Repositories.Base;
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

        /// <summary>
        /// Регистрация фич БД.
        /// </summary>
        public IServiceCollection AddDatabaseFeatures()
        {
            // Поиск всех реализаций IHandlerBase,
            // регистрация в di под близжайшим реализованным интерфейсом.
            typeof(Di).Assembly
                .GetTypes()
                .Where(t => t.IsClass & t.IsAssignableTo(typeof(IHandlerBase)))
                .ToList()
                .ForEach(t => services.AddTransient(t.GetInterfaces().First(), t));

            return services;
        }
    }
}
