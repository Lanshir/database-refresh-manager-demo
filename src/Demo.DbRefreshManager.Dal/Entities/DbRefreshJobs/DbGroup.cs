using Demo.DbRefreshManager.Dal.Entities.Users;

namespace Demo.DbRefreshManager.Dal.Entities.DbRefreshJobs;

/// <summary>
/// Модель группы БД.
/// </summary>
public class DbGroup
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

    /// <summary>
    /// Показывать пункт легенды.
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Роли, имеющие доступ к группе БД.
    /// </summary>
    public List<UserRole> AccessRoles { get; set; } = new();
}
