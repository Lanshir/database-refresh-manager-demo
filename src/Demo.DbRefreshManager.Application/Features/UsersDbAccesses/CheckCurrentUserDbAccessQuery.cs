using Demo.DbRefreshManager.Application.Services;
using Demo.DbRefreshManager.Core.Handlers;
using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.Domain.Errors;

namespace Demo.DbRefreshManager.Application.Features.UsersDbAccesses;

/// <summary>
/// Проверка наличия у текущего пользователя доступа к задаче на перезаливку БД.
/// </summary>
public interface ICheckCurrentUserDbAccessQueryHandler
    : IAsyncHandler<Result, CheckCurrentUserDbAccessQuery.Dto>;

public static class CheckCurrentUserDbAccessQuery
{
    public record struct Dto(int JobId);

    public class Handler(
        IUserIdentityProvider userIdentity,
        IUserHasJobGroupAccessQueryHandler userHasGroupAccessQuery,
        IUserHasPersonalDbAccessQueryHandler userHasPersonalAccessQuery)
        : ICheckCurrentUserDbAccessQueryHandler
    {
        public async Task<Result> HandleAsync(Dto query, CancellationToken ct)
        {
            var login = userIdentity.GetUserLogin();
            var userRoles = userIdentity.GetRoles();

            // Проверка доступа к группе.
            var hasGroupAccess = await userHasGroupAccessQuery
                .HandleAsync(new(query.JobId, userRoles), ct);

            if (hasGroupAccess)
                return Result.Success();

            // Проверка персонального доступа.
            var hasPersonalAccess = await userHasPersonalAccessQuery
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
