using Demo.DbRefreshManager.Dal.Entities.Users;

namespace Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;

/// <summary>
/// Модель связи групп БД с ролями.
/// </summary>
public class DbGroupRoleBind
{
    /// <summary>
    /// Id группы БД.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Группа БД.
    /// </summary>
    public DbGroup? Group { get; set; }

    /// <summary>
    /// Id роли пользователя.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public UserRole? Role { get; set; }
}
