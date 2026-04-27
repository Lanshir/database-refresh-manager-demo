using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.UsersDbAccesses;

/// <summary>
/// Проверка наличия доступа к группе БД у пользователя.
/// </summary>
public interface ICheckUserHasJobGroupAccessQueryHandler
    : IAsyncHandler<bool, CheckUserHasJobGroupAccess.Query>;

public static class CheckUserHasJobGroupAccess
{
    public record struct Query(int JobId, List<string> UserRoles);
}
