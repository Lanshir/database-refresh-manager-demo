using Demo.DbRefreshManager.Application.Features.DbAccesses;
using Demo.DbRefreshManager.Application.Features.DbGroups;
using Demo.DbRefreshManager.Application.Features.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Features.Logs;
using Demo.DbRefreshManager.Application.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Domain.Models.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;
using HotChocolate.Authorization;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Queries.V1;

[ExtendObjectType<QueryV1>]
public class DbRefreshJobsQueriesV1
{
    /// <summary>
    /// Получить список задач на перезаливку БД.
    /// </summary>
    /// <param name="id">Фильтр по id.</param>
    /// <param name="dbName">Фильтр по названию БД.</param>
    [UseProjection]
    public async Task<IQueryable<DbRefreshJobDto>> GetDbRefreshJobs(
        IGetDbRefreshJobsForDisplayHandler getDbRefreshJobsForDisplay,
        int? id = null,
        string? dbName = null)
        => getDbRefreshJobsForDisplay.Handle(new(id, dbName));

    /// <summary>
    /// Получить группы БД.
    /// </summary>
    [UseProjection]
    public async Task<IQueryable<DbGroupDto>> GetDbGroups(
        IGetUserDisplayGroupsHandler getUserDisplayGroups)
        => getUserDisplayGroups.Handle();

    /// <summary>
    /// Получить логи перезаливок БД.
    /// </summary>
    [Authorize]
    [UseProjection]
    public async Task<IQueryable<DbRefreshLogDto>> GetDbRefreshLogs(
        IGetLogsForDisplayHandler getLogsForDisplay,
        int? jobId = null,
        DateTime? startDate = null)
        => getLogsForDisplay
            .Handle(new(jobId, startDate))
            .Select(DbRefreshLog.ToDtoProjectionExpression);

    /// <summary>
    /// Получить список id баз с персональным доступом.
    /// </summary>
    [Authorize]
    public async Task<int[]> GetDbPersonalAccessIds(
        IGetPersonalAccessJobIdsHandler getPersonalAccesses,
        IUserIdentityProvider userIdentity,
        CancellationToken ct)
        => await getPersonalAccesses.HandleAsync(new(userIdentity.GetUserLogin()), ct);
}
