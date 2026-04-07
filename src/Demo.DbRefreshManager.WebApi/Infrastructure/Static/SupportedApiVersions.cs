using System.Collections.Immutable;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Static;

/// <summary>
/// Версионирование api с генерацией OpenApi документации
/// требует указания списка версий в нескольких местах.
/// Этот класс используется как централизованное хранилище списка возможных версий.
/// </summary>
public static class SupportedApiVersions
{
    /// <summary>
    /// Список поддерживаемых версий api.
    /// </summary>
    public static readonly ImmutableList<double> VersionsList = [1.0];
}
