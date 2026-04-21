using Demo.DbRefreshManager.Application.Models.Options;
using Demo.DbRefreshManager.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.DbRefreshManager.Application;

public static class Di
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Регистрация опций приложения.
        /// </summary>
        public IServiceCollection AddApplicationOptions()
        {
            services.AddOptions<LdapOptions>()
                .BindConfiguration(nameof(LdapOptions));

            return services;
        }

        /// <summary>
        /// Регистрация обработчиков фич приложения.
        /// </summary>
        public IServiceCollection AddApplicationFeatures()
        {
            // Поиск всех реализаций IHandlerBase,
            // регистрация в di под ближайшим реализованным интерфейсом.
            typeof(Di).Assembly
                .GetTypes()
                .Where(t => t.IsClass & t.IsAssignableTo(typeof(IHandlerBase)))
                .ToList()
                .ForEach(t => services.AddTransient(t.GetInterfaces().First(), t));

            return services;
        }
    }
}
