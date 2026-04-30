using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.DbAccesses;

/// <summary>
/// Запрос id задач с персональным доступом для пользователя.
/// </summary>
public interface IGetPersonalAccessJobIdsQueryHandler
    : IAsyncHandler<int[], GetPersonalAccessJobIds.Query>;

public static class GetPersonalAccessJobIds
{
    public record struct Query(string Login);
}