namespace Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

/// <summary>
/// Модель dto записи лога перезаливки БД.
/// </summary>
public class DbRefreshLogDto
{
    /// <summary>
    /// Id задачи на перезаливку БД.
    /// </summary>
    public int DbRefreshJobId { get; set; }

    /// <summary>
    /// Название БД.
    /// </summary>
    public string DbName { get; set; } = string.Empty;

    /// <summary>
    /// Дата начала перезаливки БД.
    /// </summary>
    public DateTime RefreshStartDate { get; set; }

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

    /// <summary>
    /// CSS-совместимая строка цвета группы БД.
    /// </summary>
    public string GroupCssColor { get; set; } = string.Empty;
}
