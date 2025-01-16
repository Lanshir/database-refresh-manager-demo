namespace Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;

/// <summary>
/// Модель записи лога перезаливки БД.
/// </summary>
public class DbRefreshLog
{
    /// <summary>
    /// Id задачи на перезаливку БД.
    /// </summary>
    public required int DbRefreshJobId { get; set; }

    /// <summary>
    /// Задача на перезаливку БД.
    /// </summary>
    public DbRefreshJob? DbRefreshJob { get; set; }

    /// <summary>
    /// Дата начала перезаливки БД.
    /// </summary>
    public required DateTime RefreshStartDate { get; set; }

    /// <summary>
    /// Дата окончания перезаливки БД.
    /// </summary>
    public DateTime? RefreshEndDate { get; set; }

    /// <summary>
    /// Код результата.
    /// </summary>
    public int? Code { get; set; }

    /// <summary>
    /// Текст результата команды перезаливки.
    /// </summary>
    public string? Result { get; set; }

    /// <summary>
    /// Текст ошибки перезаливки.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Выполненный скрипт перезаливки.
    /// </summary>
    public string? ExecutedScript { get; set; }

    /// <summary>
    /// Инициатор перезаливки.
    /// </summary>
    public string? Initiator { get; set; }
}
