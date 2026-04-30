using Asp.Versioning;
using Asp.Versioning.Conventions;
using Demo.DbRefreshManager.WebApi.Endpoints.Abstract;
using Demo.DbRefreshManager.WebApi.Endpoints.EndpointFilters;
using Demo.DbRefreshManager.WebApi.Infrastructure.Static;

namespace Demo.DbRefreshManager.WebApi.Endpoints.Extensions;

public static class EndpointRouteBuilderExtensions
{
    extension(IEndpointRouteBuilder builder)
    {
        /// <summary>
        /// Регистрация групп эндпоинтов api.
        /// </summary>
        public IEndpointRouteBuilder MapVersionedApiEndpoints()
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

            // Создание и вызов конфигураторов эндпоинтов.
            var assembly = typeof(IEndpointsMapper).Assembly;
            var endpointMappersTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && t.IsAssignableTo(typeof(IEndpointsMapper)));

            foreach (var mapperType in endpointMappersTypes)
            {
                var endpointMapper = (Activator.CreateInstance(mapperType) as IEndpointsMapper)!;

                endpointMapper.MapEndpoints(baseApiGroup);
            }

            return builder;
        }
    }
}
