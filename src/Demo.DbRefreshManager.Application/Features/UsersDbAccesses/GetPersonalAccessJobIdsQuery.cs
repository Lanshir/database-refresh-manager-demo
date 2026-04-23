using Demo.DbRefreshManager.Core.Handlers;

namespace Demo.DbRefreshManager.Application.Features.UsersDbAccesses;

/// <summary>
/// Запрос id задач с персональным доступом для пользователя.
/// </summary>
public interface IGetPersonalAccessJobIdsQueryHandler
    : IAsyncHandler<int[], GetPersonalAccessJobIdsQuery.Dto>;

public static class GetPersonalAccessJobIdsQuery
{
    public record struct Dto(string Login);
}