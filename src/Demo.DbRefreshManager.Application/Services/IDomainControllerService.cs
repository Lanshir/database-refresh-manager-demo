using Demo.DbRefreshManager.Domain.Models.ActiveDirectory;

namespace Demo.DbRefreshManager.Application.Services;

/// <summary>
/// Сервис авторизации через контроллер домена (Windows Active Directory).
/// </summary>
public interface IDomainControllerService : IDisposable
{
    /// <summary>
    /// Пользователь авторизован.
    /// </summary>
    bool IsAuthenticated { get; }

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

    /// <summary>
    /// Аутентификация в домене.
    /// </summary>
    /// <param name="login">Логин пользователя</param>
    /// <param name="password">Пароль.</param>
    /// <returns>Пользователь аутентифицирован.</returns>
    bool Authenticate(string login, string password);

    /// <summary>
    /// Получить данные пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="searchDnBases">Базовые Dn для поиска.</param>
    /// <returns>Данные пользователя.</returns>
    LdapUser? GetUserData(string login, IEnumerable<string> searchDnBases);
}
