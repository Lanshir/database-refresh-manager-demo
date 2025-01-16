using Demo.DbRefreshManager.Common.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Demo.DbRefreshManager.Dal.Context;

/// <summary>
/// Конвертеры значений EF.
/// </summary>
internal static class ValueConverters
{
    /// <summary>
    /// Конвертер DateTime Kind.Unspecified в Utc.
    /// </summary>
    public class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeToUtcConverter() : base(
            Serialize,
            Deserialize,
            null)
        {
        }

        private static Expression<Func<DateTime, DateTime>> Serialize = dt => dt;

        private static Expression<Func<DateTime, DateTime>> Deserialize =
                dt => dt.Kind == DateTimeKind.Unspecified ? dt.SetKind(DateTimeKind.Utc) : dt;
    }
}
