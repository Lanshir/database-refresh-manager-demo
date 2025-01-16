namespace Demo.DbRefreshManager.Common.Extensions;

/// <summary>
/// Расширения String.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Строка с обрезанными пробелами, либо null если входная строка null or white space.
    /// </summary>
    /// <param name="value">Входная строка.</param>
    /// <returns>Строка или null.</returns>
    public static string? TrimOrNull(this string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
