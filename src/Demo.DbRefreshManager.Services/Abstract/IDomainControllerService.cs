using Demo.DbRefreshManager.Services.Models.ActiveDirectory;

namespace Demo.DbRefreshManager.Services.Abstract;

/// <summary>
/// Сервис авторизации через контроллер домена (Windows Active Directory).
/// </summary>
public interface IDomainControllerService : IDisposable
{
    /// <summary>
    /// Пользователь авторизован.
    /// </summary>
    bool IsAuthorized { get; }

    /// <summary>
    /// Подключиться к контроллеру домена windows.
    /// </summary>
    /// <param name="domainHost">Хост домена.</param>
    /// <param name="reconnectCount">Кол-во попыток повторного подключения.</param>
    /// <param name="reconnectDelayMs">Задержка перед повторным подключением в милисекундах.</param>
    /// <returns>Подключен.</returns>
    bool Connect(
        string domainHost,
        int reconnectCount = 0,
        int reconnectDelayMs = 1000);

    bool Authorize(string login, string password);

    /// <summary>
    /// Получить данные пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="searchDnBases">Базовые Dn для поиска.</param>
    /// <returns>Данные пользователя.</returns>
    LdapUser? GetUserData(string login, IEnumerable<string> searchDnBases);
}
