namespace Demo.DbRefreshManager.WebApi.Models.Frontend;

/// <summary>
/// Модель dto конфигуарции frontend.
/// </summary>
public class FrontendConfigDto
{
    /// <summary>
    /// Url метода просмотра списка обновлённых объектов.
    /// </summary>
    public string ObjectsListUrl { get; set; } = string.Empty;

    /// <summary>
    /// Url интрукции к менеджеру.
    /// </summary>
    public string InstructionUrl { get; set; } = string.Empty;
}
