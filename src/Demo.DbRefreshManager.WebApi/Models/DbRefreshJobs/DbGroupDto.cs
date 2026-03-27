namespace Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

/// <summary>
/// Модель dto группы БД.
/// </summary>
/// <param name="Id">Id записи.</param>
/// <param name="SortOrder">Порядок сортировки.</param>
/// <param name="Description">Описание группы.</param>
/// <param name="CssColor">CSS-совместимая строка цвета.</param>
public record DbGroupDto(int Id, int SortOrder, string Description, string CssColor)
{
    public DbGroupDto() : this(default, default, string.Empty, string.Empty) { }
}