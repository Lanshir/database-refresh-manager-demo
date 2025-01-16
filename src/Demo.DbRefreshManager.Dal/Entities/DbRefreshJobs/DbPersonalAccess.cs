namespace Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;

/// <summary>
/// Модель записи персонального доступа к БД.
/// </summary>
public class DbPersonalAccess
{
    /// <summary>
    /// Id задачи на перезаливку.
    /// </summary>
    public int JobId { get; set; }

    /// <summary>
    /// Логин пользователя с доступом.
    /// </summary>
    public required string Login { get; set; }
}
