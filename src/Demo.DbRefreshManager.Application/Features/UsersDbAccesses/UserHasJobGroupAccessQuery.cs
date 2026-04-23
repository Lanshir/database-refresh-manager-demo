using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.UsersDbAccesses;

/// <summary>
/// Проверка наличия доступа к группе БД у пользователя.
/// </summary>
public interface IUserHasJobGroupAccessQueryHandler
    : IAsyncHandler<bool, UserHasJobGroupAccessQuery.Dto>;

public static class UserHasJobGroupAccessQuery
{
    public record struct Dto(int JobId, List<string> UserRoles);
}
