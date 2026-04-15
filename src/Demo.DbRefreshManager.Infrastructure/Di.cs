using Demo.DbRefreshManager.Application.Converters;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Infrastructure.Converters;
using Demo.DbRefreshManager.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DbRefreshManager.Infrastructure;

public static class Di
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Регистрация ключевых зависимостей приложения.
        /// </summary>
        public IServiceCollection AddInfrastructure()
        {
            services.AddSingleton<IJsonConverter, JsonConverter>();
            services.AddScoped<IDomainControllerService, DomainControllerService>();

            services.AddTransient<ISshClientService, SshClientService>();

            return services;
        }
    }
}
