using Demo.DbRefreshManager.Application.Repositories;
using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Domain.Entities.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;
using Demo.DbRefreshManager.WebApi.Mappings.DbRefreshJobs;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;
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
        IDbRefreshJobsRepository jobsRepository,
        int? id = null,
        string? dbName = null)
        => jobsRepository
            .GetUserDisplayJobsListQuery(id, dbName)
            .Select(DbRefreshJob.ToDtoProjectionExpression);

    /// <summary>
    /// Получить группы БД.
    /// </summary>
    [UseProjection]
    public async Task<IQueryable<DbGroupDto>> GetDbGroups(IDbGroupsRepository groupsRepository)
        => groupsRepository
            .GetUserDisplayGroupsQuery()
            .Select(DbGroup.ToDtoProjectionExpression);

    /// <summary>
    /// Получить логи перезаливок БД.
    /// </summary>
    [Authorize]
    [UseProjection]
    public async Task<IQueryable<DbRefreshLogDto>> GetDbRefreshLogs(
        IDbRefreshLogsRepository logsRepository,
        int? jobId = null,
        DateTime? startDate = null)
        => logsRepository
            .GetUserDisplayLogsQuery(jobId, startDate)
            .Select(DbRefreshLog.ToDtoProjectionExpression);

    /// <summary>
    /// Получить список id баз с персональным доступом.
    /// </summary>
    [Authorize]
    public async Task<int[]> GetDbPersonalAccessIds(
        IDbPersonalAccessesRepository acessesRepository,
        IUserIdentityProvider userIdentity)
        => await acessesRepository.GetPersonalAccessJobIds(userIdentity.GetUserLogin());
}
