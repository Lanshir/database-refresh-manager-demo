namespace Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

/// <summary>
/// Модель dto задачи перезаливки БД.
/// </summary>
/// <param name="Id">Id задачи.</param>
/// <param name="DbName">Название БД.</param>
/// <param name="InProgress">Перезаливка в процессе.</param>
/// <param name="ScheduleIsActive">Перезаливка по расписанию активна.</param>
/// <param name="ManualRefreshDate">Дата следующей ручной перезаливки.</param>
/// <param name="ScheduleRefreshDate">Дата следующей перезаливки по расписанию.</param>
/// <param name="LastRefreshDate">Дата последней перезаливки.</param>
/// <param name="ScheduleChangeUser">Пользователь изменивший статус перезаливки по раписанию.</param>
/// <param name="ScheduleChangeDate">Дата изменения статуса перезаливки по раписанию.</param>
/// <param name="ReleaseComment">Комментарий установленных релизов.</param>
/// <param name="UserComment">Последний комментарий пользователей.</param>
/// <param name="GroupSortOrder">Порядок сортировки групп БД.</param>
/// <param name="GroupCssColor">CSS-совместимая строка цвета группы БД.</param>
/// <param name="GroupAccessRoles">Роли с доступом к группе БД.</param>
public record DbRefreshJobDto(
    int Id,
    string DbName,
    bool InProgress,
    bool ScheduleIsActive,
    DateTime? ManualRefreshDate,
    DateTime ScheduleRefreshDate,
    DateTime LastRefreshDate,
    string? ScheduleChangeUser,
    DateTime? ScheduleChangeDate,
    string? ReleaseComment,
    string? UserComment,
    int GroupSortOrder,
    string GroupCssColor,
    List<string> GroupAccessRoles)
{
    public DbRefreshJobDto() : this(
        default,
        string.Empty,
        default,
        default,
        null,
        DateTime.UtcNow,
        DateTime.UtcNow,
        null,
        null,
        null,
        null,
        default,
        string.Empty,
        [])
    { }
}