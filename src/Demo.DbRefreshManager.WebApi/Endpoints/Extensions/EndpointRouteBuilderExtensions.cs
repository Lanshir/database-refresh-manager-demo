using Asp.Versioning;
using Asp.Versioning.Conventions;
using Demo.DbRefreshManager.WebApi.Endpoints.Abstract;
using Demo.DbRefreshManager.WebApi.Infrastructure.Endpoints;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;

namespace Demo.DbRefreshManager.WebApi.Endpoints.Extensions;

public static class EndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        /// <summary>
        /// Регистрация групп эндпоинтов api.
        /// </summary>
        public IEndpointRouteBuilder MapApiEndpoints()
        {
            // Настройка базовой группы /api/v{version}.
            var apiVersionSet = builder
                .NewApiVersionSet()
                .HasApiVersions(SupportedApiVersions.VersionsList.Select(v => new ApiVersion(v)))
                .ReportApiVersions()
                .Build();

            var baseApiGroup = builder
                .MapGroup("api/v{version:apiVersion}")
                .WithApiVersionSet(apiVersionSet)
                .AddEndpointFilter<EndpointExceptionsFilter>();

            // Получение реализаций и вызов конфигураторов эндпоинтов.
            var assembly = typeof(IEndpointsMapper).Assembly;
            var grpSetupTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && t.IsAssignableTo(typeof(IEndpointsMapper)));

            foreach (var setupType in grpSetupTypes)
            {
                var grpSetup = (Activator.CreateInstance(setupType) as IEndpointsMapper)!;
                var grpBuilder = grpSetup.MapEndpoints(baseApiGroup);
            }

            return builder;
        }
    }
}
