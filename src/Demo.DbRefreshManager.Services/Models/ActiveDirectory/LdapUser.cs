namespace Demo.DbRefreshManager.Services.Models.ActiveDirectory;

/// <summary>
/// Модель данных пользователя из LDAP.
/// </summary>
public class LdapUser
{
    /// <summary>
    /// Доменный идентификатор пользователя.
    /// </summary>
    public required string Dn { get; set; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Email адрес пользователя.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Отчетсво пользователя.
    /// </summary>
    public required string Patronymic { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// ФИО пользователя.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Должность пользователя.
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// Отдел.
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Компания.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Дата последнего изменения данных.
    /// </summary>
    public DateTime WhenChanged { get; set; }

    /// <summary>
    /// Группы пользователя.
    /// </summary>
    public List<string> Groups { get; set; } = new(0);
}
