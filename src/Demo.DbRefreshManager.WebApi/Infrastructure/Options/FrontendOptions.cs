namespace Demo.DbRefreshManager.WebApi.Infrastructure.Options;

/// <summary>
/// Конфигурация frontend.
/// </summary>
public record FrontendOptions
{
    /// <summary>
    /// Url метода просмотра списка обновлённых объектов.
    /// </summary>
    public string ObjectsListUrl { get; init; } = string.Empty;

    /// <summary>
    /// Url интрукции к менеджеру.
    /// </summary>
    public string InstructionUrl { get; init; } = string.Empty;
}
