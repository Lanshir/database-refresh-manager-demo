namespace Demo.DbRefreshManager.Common.Config.Concrete;

/// <summary>
/// Конфигурация frontend.
/// </summary>
public class FrontendConfig
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
