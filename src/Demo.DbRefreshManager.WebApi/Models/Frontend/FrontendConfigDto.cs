namespace Demo.DbRefreshManager.WebApi.Models.Frontend;

/// <summary>
/// Модель dto конфигуарции frontend.
/// </summary>
/// <param name="ObjectsListUrl">Url метода просмотра списка обновлённых объектов.</param>
/// <param name="InstructionUrl">Url интрукции к менеджеру.</param>
public record FrontendConfigDto(
    string ObjectsListUrl,
    string InstructionUrl);
