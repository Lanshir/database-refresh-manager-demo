using Demo.DbRefreshManager.Dal.Entities.Users;

namespace Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;

/// <summary>
/// Модель задачи перезаливки БД.
/// </summary>
public class DbRefreshJob
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
    /// Признак удаления записи.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Перезаливка по расписанию активна.
    /// </summary>
    public bool ScheduleIsActive { get; set; }

    /// <summary>
    /// Дата ручной перезаливки.
    /// </summary>
    public DateTime? ManualRefreshDate { get; set; }

    /// <summary>
    /// Инициатор ручной перезаливки.
    /// </summary>
    public string? ManualRefreshInitiator { get; set; }

    /// <summary>
    /// Время перезаливки по расписанию.
    /// </summary>
    public DateTimeOffset ScheduleRefreshTime { get; set; }

    /// <summary>
    /// Дата последней перезаливки.
    /// </summary>
    public DateTime LastRefreshDate { get; set; }

    /// <summary>
    /// Комментарий установленных релизов.
    /// </summary>
    public string? ReleaseComment { get; set; }

    /// <summary>
    /// Последний комментарий пользователей.
    /// </summary>
    public string? UserComment { get; set; }

    /// <summary>
    /// Дата изменения статуса перезаливки по раписанию.
    /// </summary>
    public DateTime? ScheduleChangeDate { get; set; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// SSH скрипт запуска перезаливки.
    /// </summary>
    public string SshScript { get; set; } = string.Empty;

    /// <summary>
    /// Id связанной группы БД.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Группа БД.
    /// </summary>
    public DbGroup? Group { get; set; }

    /// <summary>
    /// Id пользователя изменившего статус перезаливки по раписанию.
    /// </summary>
    public int? ScheduleChangeUserId { get; set; }

    /// <summary>
    /// Пользователь изменивший статус перезаливки по раписанию.
    /// </summary>
    public User? ScheduleChangeUser { get; set; }
}
