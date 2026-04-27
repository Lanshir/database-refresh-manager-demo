using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.Domain.Errors;

namespace Demo.DbRefreshManager.Application.Features.UsersDbAccesses;

/// <summary>
/// Проверка наличия у текущего пользователя доступа к задаче на перезаливку БД.
/// </summary>
public interface ICheckCurrentUserDbAccessQueryHandler
    : IAsyncHandler<Result, CheckCurrentUserDbAccess.Query>;

public static class CheckCurrentUserDbAccess
{
    public record struct Query(int JobId);

    internal class QueryHandler(
        IUserIdentityProvider userIdentity,
        ICheckUserHasJobGroupAccessQueryHandler checkUserHasGroupAccess,
        ICheckUserHasPersonalDbAccessQueryHandler checkUserHasPersonalAccess)
        : ICheckCurrentUserDbAccessQueryHandler
    {
        public async Task<Result> HandleAsync(Query query, CancellationToken ct)
        {
            var login = userIdentity.GetUserLogin();
            var userRoles = userIdentity.GetRoles();

            // Проверка доступа к группе.
            var hasGroupAccess = await checkUserHasGroupAccess
                .HandleAsync(new(query.JobId, userRoles), ct);

            if (hasGroupAccess)
                return Result.Success();

            // Проверка персонального доступа.
            var hasPersonalAccess = await checkUserHasPersonalAccess
                .HandleAsync(new(query.JobId, login), ct);

            if (hasPersonalAccess)
                return Result.Success();

            return DbRefreshJobErrors.Forbidden with
            {
                Message = $"У пользователя {login} нет прав для изменения задачи id: {query.JobId}"
            };
        }
    }
}
