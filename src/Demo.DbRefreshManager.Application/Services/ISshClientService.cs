using Demo.DbRefreshManager.Application.Models.SshService;

namespace Demo.DbRefreshManager.Application.Services;

/// <summary>
/// Сервис работы с клиентом SSH.
/// </summary>
public interface ISshClientService : IDisposable
{
    /// <summary>
    /// Клиент подключен.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Подключение клиента к серверу.
    /// </summary>
    /// <param name="host">Хост подключения.</param>
    /// <param name="user">Имя пользователя.</param>
    /// <param name="pasword">Пароль.</param>
    void Connect(string host, string user, string pasword);

    /// <summary>
    /// Подключение клиента к серверу.
    /// </summary>
    /// <param name="host">Хост подключения.</param>
    /// <param name="user">Имя пользователя.</param>
    /// <param name="pasword">Пароль.</param>
    Task ConnectAsync(string host, string user, string pasword);

    /// <summary>
    /// Разрыв подключения с сервером.
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Запуск команды.
    /// </summary>
    /// <param name="commandText">Текст команды.</param>
    /// <returns>Результат выполнения.</returns>
    SshCommandResult RunCommand(string commandText);
}
