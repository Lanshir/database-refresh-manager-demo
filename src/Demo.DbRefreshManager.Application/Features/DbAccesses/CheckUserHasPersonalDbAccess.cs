using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbAccesses;

/// <summary>
/// Проверка наличия персонального доступа к БД у пользователя.
/// </summary>
public interface ICheckUserHasPersonalDbAccessQueryHandler
    : IAsyncHandler<bool, CheckUserHasPersonalDbAccess.Query>;

public static class CheckUserHasPersonalDbAccess
{
    public record struct Query(int JobId, string Login);
}