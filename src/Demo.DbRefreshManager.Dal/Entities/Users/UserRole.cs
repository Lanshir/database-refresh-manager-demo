namespace Demo.DbRefreshManager.Dal.Entities.Users;

/// <summary>
/// Модель роли пользователя.
/// </summary>
public class UserRole
{
    /// <summary>
    /// Id роли.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование роли.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание роли.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Связанная группа в домене.
    /// </summary>
    public string? LdapGroup { get; set; }

    /// <summary>
    /// Активность роли.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Дата создания роли.
    /// </summary>
    public DateTime CreationDate { get; set; }
}
