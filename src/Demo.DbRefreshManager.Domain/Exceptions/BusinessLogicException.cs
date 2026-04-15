using Demo.DbRefreshManager.Core.Extensions;
using Demo.DbRefreshManager.Domain.Errors;

namespace Demo.DbRefreshManager.Domain.Exceptions;

/// <summary>
/// Ошибка бизнес логики приложения.
/// </summary>
public class BusinessLogicException : Exception
{
    /// <summary>
    /// Код ошибки.
    /// </summary>
    public string Code { get; set; } = DefaultErrors.Unexpected;

    /// <inheritdoc cref="BusinessLogicException" />
    /// <param name="message">Сообщение.</param>
    /// <param name="innerException">Вложенный Exception.</param>
    public BusinessLogicException(string message, Exception? innerException = null)
        : base(MapNestedMessage(message, innerException), innerException)
    { }

    /// <inheritdoc cref="BusinessLogicException" />
    /// <param name="code">Код ошибки.</param>
    /// <param name="message">Сообщение.</param>
    /// <param name="innerException">Вложенный Exception.</param>
    public BusinessLogicException(string code, string message, Exception? innerException = null)
        : base(MapNestedMessage(message, innerException), innerException)
    {
        Code = code;
    }

    /// <summary>
    /// Маппинг сообщения из вложенных Exception.
    /// </summary>
    private static string MapNestedMessage(string message, Exception? innerException)
    {
        var innerExcMessage = innerException?.GetNestedMessage();

        if (!string.IsNullOrEmpty(innerExcMessage))
            return $"{message}\n{innerExcMessage}";

        return message;
    }
}
