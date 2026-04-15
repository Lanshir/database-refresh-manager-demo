namespace Demo.DbRefreshManager.Core.Extensions;

/// <summary>
/// Расширения <typeparamref name="DateTimeOffset" />.
/// </summary>
public static class DateTimeOffsetExtensions
{
    extension(DateTimeOffset offset)
    {
        /// <summary>
        /// Перенести время на сегодняшнюю дату.
        /// </summary>
        /// <returns><typeparamref name="DateTime" />.Now.Date + время из offset</returns>
        public DateTime TimeToTodayDate()
            => DateTime.Now.Date.Add(offset.DateTime.TimeOfDay);

        /// <summary>
        /// Перенести время на сегодняшнюю дату (UTC).
        /// </summary>
        /// <returns><typeparamref name="DateTime" />.UtcNow.Date + время из offset</returns>
        public DateTime TimeToTodayDateUtc()
            => DateTime.UtcNow.Date.Add(offset.UtcDateTime.TimeOfDay);
    }
}
