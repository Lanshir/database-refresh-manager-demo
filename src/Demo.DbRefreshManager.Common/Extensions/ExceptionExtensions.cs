namespace Demo.DbRefreshManager.Common.Extensions;

/// <summary>
/// Расширения <typeparamref name="Exception">.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Получить сообщение из вложенных Exception.
    /// </summary>
    public static string GetNestedMessage(this Exception exc)
    {
        var childExc = exc?.InnerException;
        var nestedMessage = exc?.Message ?? "";

        while (!string.IsNullOrWhiteSpace(childExc?.Message))
        {
            nestedMessage = $"{nestedMessage}\n{childExc.Message}";
            childExc = childExc.InnerException;
        }

        return nestedMessage;
    }
}
