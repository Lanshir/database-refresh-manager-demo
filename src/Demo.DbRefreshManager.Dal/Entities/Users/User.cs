namespace Demo.DbRefreshManager.Dal.Entities.Users;

/// <summary>
/// Модель пользователя.
/// </summary>
public class User
{
    /// <summary>
    /// Id пользователя.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Отчество пользователя.
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Доменный логин пользователя.
    /// </summary>
    public string LdapLogin { get; set; } = string.Empty;

    /// <summary>
    /// Доменный идентификатор пользователя.
    /// </summary>
    public string LdapDn { get; set; } = string.Empty;

    /// <summary>
    /// Дата изменения в домене.
    /// </summary>
    public DateTime LdapChangeDate { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Дата изменения.
    /// </summary>
    public DateTime ModifyDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Роли пользователя.
    /// </summary>
    public List<UserRole> Roles { get; set; } = new();
}
