namespace Demo.DbRefreshManager.WebApi.Models.Api;

/// <summary>
/// Модель dto првоерки email.
/// </summary>
public class EmailTestDto
{
    /// <summary>
    /// Список Email для отправки.
    /// </summary>
    public List<string> ToEmails { get; set; } = new(0);
}
