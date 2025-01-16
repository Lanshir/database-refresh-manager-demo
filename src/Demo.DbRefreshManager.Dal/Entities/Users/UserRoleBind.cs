namespace Demo.DbRefreshManager.Dal.Entities.Users;

/// <summary>
/// Связка пользователь-роль.
/// </summary>
public class UserRoleBind
{
    /// <summary>
    /// Id пользователя.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Id роли.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Роль.
    /// </summary>
    public UserRole? Role { get; set; }
}
