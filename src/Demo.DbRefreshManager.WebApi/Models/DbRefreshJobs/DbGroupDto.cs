namespace Demo.DbRefreshManager.WebApi.Models.DbRefreshJobs;

/// <summary>
/// Модель dto группы БД.
/// </summary>
public class DbGroupDto
{
    /// <summary>
    /// Id записи.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Порядок сортировки.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Описание группы.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// CSS-совместимая строка цвета.
    /// </summary>
    public string CssColor { get; set; } = string.Empty;
}
