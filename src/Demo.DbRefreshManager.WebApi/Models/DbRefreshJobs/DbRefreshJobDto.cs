namespace Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

/// <summary>
/// Модель dto задачи перезаливки БД.
/// </summary>
public class DbRefreshJobDto
{
    /// <summary>
    /// Id задачи.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название БД.
    /// </summary>
    public string DbName { get; set; } = string.Empty;

    /// <summary>
    /// Перезаливка в процессе.
    /// </summary>
    public bool InProgress { get; set; }

    /// <summary>
    /// Перезаливка по расписанию активна.
    /// </summary>
    public bool ScheduleIsActive { get; set; }

    /// <summary>
    /// Дата следующей ручной перезаливки.
    /// </summary>
    public DateTime? ManualRefreshDate { get; set; }

    /// <summary>
    /// Дата следующей перезаливки по расписанию.
    /// </summary>
    public DateTime ScheduleRefreshDate { get; set; }

    /// <summary>
    /// Дата последней перезаливки.
    /// </summary>
    public DateTime LastRefreshDate { get; set; }

    /// <summary>
    /// Пользователь изменивший статус перезаливки по раписанию.
    /// </summary>
    public string? ScheduleChangeUser { get; set; }

    /// <summary>
    /// Дата изменения статуса перезаливки по раписанию.
    /// </summary>
    public DateTime? ScheduleChangeDate { get; set; }

    /// <summary>
    /// Комментарий установленных релизов.
    /// </summary>
    public string? ReleaseComment { get; set; }

    /// <summary>
    /// Последний комментарий пользователей.
    /// </summary>
    public string? UserComment { get; set; }

    /// <summary>
    /// Порядок сортировки групп БД.
    /// </summary>
    public int GroupSortOrder { get; set; }

    /// <summary>
    /// CSS-совместимая строка цвета группы БД.
    /// </summary>
    public string GroupCssColor { get; set; } = string.Empty;

    /// <summary>
    /// Роли с доступом к группе БД.
    /// </summary>
    public List<string> GroupAccessRoles { get; set; } = new(0);
}
