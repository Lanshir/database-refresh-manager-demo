using AutoMapper;
using AutoMapper.QueryableExtensions;
using Demo.DbRefreshManager.Dal.Repositories.Abstract;
using Demo.DbRefreshManager.WebApi.Infrastructure.Helpers.Abstract;
using Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;
using HotChocolate.Authorization;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Queries.V1;

public class DbRefreshJobsQueriesV1
{
    /// <summary>
    /// Получить список задач на перезаливку БД.
    /// </summary>
    /// <param name="id">Фильтр по id.</param>
    /// <param name="dbName">Фильтр по названию БД.</param>
    [UseProjection]
    public async Task<IQueryable<DbRefreshJobDto>> GetList(
        IMapper mapper,
        IDbRefreshJobsRepository jobsRepository,
        int? id = null, string? dbName = null)
        => jobsRepository.GetUserDisplayJobsListQuery(id, dbName)
            .ProjectTo<DbRefreshJobDto>(mapper.ConfigurationProvider);

    /// <summary>
    /// Получить группы БД.
    /// </summary>
    [UseProjection]
    public async Task<IQueryable<DbGroupDto>> GetGroups(
        IMapper mapper,
        IDbGroupsRepository groupsRepository)
        => groupsRepository.GetUserDisplayGroupsQuery()
            .ProjectTo<DbGroupDto>(mapper.ConfigurationProvider);

    /// <summary>
    /// Получить логи перезаливок БД.
    /// </summary>
    [Authorize]
    [UseProjection]
    public async Task<IQueryable<DbRefreshLogDto>> GetLogs(
        IMapper mapper,
        IDbRefreshLogsRepository logsRepository,
        int? jobId = null, DateTime? startDate = null)
        => logsRepository.GetUserDisplayLogsQuery(jobId, startDate)
            .ProjectTo<DbRefreshLogDto>(mapper.ConfigurationProvider);

    /// <summary>
    /// Получить список id задач на перезаливку с персональным доступом.
    /// </summary>
    [Authorize]
    public async Task<int[]> GetPersonalAccessIds(
        IDbPersonalAccessesRepository acessesRepository,
        IUserIdentityHelper userIdentity)
        => await acessesRepository.GetPersonalAccessJobIds(userIdentity.GetUserLogin());
}
