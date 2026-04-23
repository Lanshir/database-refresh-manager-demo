using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.UsersDbAccesses;

/// <summary>
/// Проверка наличия персонального доступа к БД у пользователя.
/// </summary>
public interface IUserHasPersonalDbAccessQueryHandler
    : IAsyncHandler<bool, UserHasPersonalDbAccessQuery.Dto>;

public static class UserHasPersonalDbAccessQuery
{
    public record struct Dto(int JobId, string Login);
}