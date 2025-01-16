namespace Demo.DbRefreshManager.Common.Extensions;

/// <summary>
/// Расширения <typeparamref name="DateTime"/>.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Округлить секунды до минут в большую сторону.
    /// </summary>
    /// <returns>Возвращает копию <typeparamref name="DateTime"/> с округлёнными секундами.</returns>
    public static DateTime RoundToMinutes(this DateTime dt)
        => dt.Date
            .AddHours(dt.Hour)
            .AddMinutes(dt.Second > 0 ? dt.Minute + 1 : dt.Minute);

    /// <summary>
    /// Округлить секунды до минут в меньшую сторону.
    /// </summary>
    /// <returns>Возвращает копию <typeparamref name="DateTime"/> с округлёнными секундами.</returns>
    public static DateTime CeilToMinutes(this DateTime dt)
     => dt.Date.AddHours(dt.Hour).AddMinutes(dt.Minute);

    /// <summary>
    /// Перевод даты в RU таймзону.
    /// </summary>
    /// <returns>Возвращает копию <typeparamref name="DateTime"/> с российским временем.</returns>
    public static DateTime ToRuTZ(this DateTime dt)
    {
        var newDate = TimeZoneInfo.ConvertTime(dt,
            TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

        return newDate;
    }

    /// <summary>
    /// Установить DateTimeKind для даты.
    /// </summary>
    /// <returns>Возвращает копию <typeparamref name="DateTime"/> с указанным DateTimeKind.</returns>
    public static DateTime SetKind(this DateTime dt, DateTimeKind kind)
        => DateTime.SpecifyKind(dt, kind);
}
