namespace Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

/// <summary>
/// Модель dto записи лога перезаливки БД.
/// </summary>
/// <param name="DbRefreshJobId">Id задачи на перезаливку БД.</param>
/// <param name="DbName">Название БД.</param>
/// <param name="RefreshStartDate">Дата начала перезаливки БД.</param>
/// <param name="RefreshEndDate">Дата окончания перезаливки БД.</param>
/// <param name="Code">Код результата.</param>
/// <param name="Result">Текст результата команды перезаливки.</param>
/// <param name="Error">Текст ошибки перезаливки.</param>
/// <param name="ExecutedScript">Выполненный скрипт перезаливки.</param>
/// <param name="Initiator">Инициатор перезаливки.</param>
/// <param name="GroupCssColor">CSS-совместимая строка цвета группы БД.</param>
public record DbRefreshLogDto(
    int DbRefreshJobId,
    string DbName,
    DateTime RefreshStartDate,
    DateTime? RefreshEndDate,
    int? Code,
    string? Result,
    string? Error,
    string? ExecutedScript,
    string? Initiator,
    string GroupCssColor)
{
    public DbRefreshLogDto() : this(
        default,
        string.Empty,
        DateTime.UtcNow,
        null,
        null,
        null,
        null,
        null,
        null,
        string.Empty
        )
    { }
}
