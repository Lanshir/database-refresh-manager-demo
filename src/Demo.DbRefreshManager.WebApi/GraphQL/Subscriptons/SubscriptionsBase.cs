using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;
using HotChocolate.Authorization;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Subscriptons;

/// <summary>
/// GraphQL subscription base.
/// </summary>
public class SubscriptionsBase
{
    /// <summary>
    /// Событие изменения задачи перезаливки БД.
    /// </summary>
    [Subscribe]
    [Authorize]
    public async Task<DbRefreshJobDto> OnDbRefreshJobChange
        ([EventMessage] DbRefreshJobDto message) => message;
}