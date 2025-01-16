using System.ComponentModel;

namespace Demo.DbRefreshManager.Common.Enums;

/// <summary>
/// Словарь стандартных кодов ответа (0-9).
/// </summary>
public enum DefaultStatusCodes
{
    /// <summary>
    /// Код успешного выполенения запроса.
    /// </summary>
    [Description("Успех")]
    Success = 0,

    /// <summary>
    /// Код ошибки при запросе.
    /// </summary>
    [Description("Ошибка")]
    Error = 1
}
