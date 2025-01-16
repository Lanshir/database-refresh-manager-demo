namespace Demo.DbRefreshManager.Common.Extensions;

/// <summary>
/// Расширения <typeparamref name="DateTimeOffset" />.
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Перенести время на сегодняшнюю дату.
    /// </summary>
    /// <returns><typeparamref name="DateTime" />.Now.Date + время из offset</returns>
    public static DateTime TimeToToday(this DateTimeOffset offset)
        => DateTime.Now.Date.Add(offset.DateTime.TimeOfDay);

    /// <summary>
    /// Перенести время на сегодняшнюю дату (UTC).
    /// </summary>
    /// <returns><typeparamref name="DateTime" />.UtcNow.Date + время из offset</returns>
    public static DateTime UtcTimeToToday(this DateTimeOffset offset)
        => DateTime.UtcNow.Date.Add(offset.UtcDateTime.TimeOfDay);
}
