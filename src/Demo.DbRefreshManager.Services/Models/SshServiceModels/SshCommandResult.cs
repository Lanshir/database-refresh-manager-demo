namespace Demo.DbRefreshManager.Services.Models.SshServiceModels;

/// <summary>
/// Модель результата команды SSH.
/// </summary>
public class SshCommandResult
{
    /// <summary>
    /// Команда выполнена успешно.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Код результата.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Текст ошибки.
    /// </summary>
    public string Error { get; set; } = string.Empty;

    /// <summary>
    /// Текст результата.
    /// </summary>
    public string Result { get; set; } = string.Empty;
}
