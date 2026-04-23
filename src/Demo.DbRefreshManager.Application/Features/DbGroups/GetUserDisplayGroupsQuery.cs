using Demo.DbRefreshManager.Application.Models.DbRefreshJobs;
using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbGroups;

/// <summary>
/// Запрос групп БД для отображения пользователю.
/// </summary>
public interface IGetUserDisplayGroupsQueryHandler : IHandler<IQueryable<DbGroupDto>>;