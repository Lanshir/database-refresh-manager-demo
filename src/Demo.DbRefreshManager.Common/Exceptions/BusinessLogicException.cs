using Demo.DbRefreshManager.Common.Enums;
using Demo.DbRefreshManager.Common.Extensions;

namespace Demo.DbRefreshManager.Common.Exceptions;

/// <summary>
/// Ошибка бизнес логики приложения.
/// </summary>
public class BusinessLogicException : Exception
{
    private const string _defaultMessage = "Ошибка сервера при обработке запроса";

    /// <summary>
    /// Код ошибки.
    /// </summary>
    public int Code { get; set; } = (int)DefaultStatusCodes.Error;

    /// <summary>
    /// Данные ошибки.
    /// </summary>
    public object? ErrorData { get; set; } = null;

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
    public BusinessLogicException(int code, string message = _defaultMessage, Exception? innerException = null)
        : base(MapNestedMessage(message, innerException), innerException)
    {
        Code = code;
    }

    /// <inheritdoc cref="BusinessLogicException" />
    /// <param name="code">Код ошибки.</param>
    /// <param name="message">Сообщение.</param>
    /// <param name="innerException">Вложенный Exception.</param>
    public BusinessLogicException(
        int code,
        object? errData = null,
        string message = _defaultMessage,
        Exception? innerException = null
        ) : base(MapNestedMessage(message, innerException), innerException)
    {
        Code = code;
        ErrorData = errData;
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
