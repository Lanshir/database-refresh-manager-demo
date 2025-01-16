namespace Demo.DbRefreshManager.Common.Converters.Abstract;

/// <summary>
/// Конвертер типов.
/// </summary>
public interface ITypeMapper
{
    /// <summary>
    /// Маппинг объекта в новый объект указанного типа.
    /// </summary>
    T Map<T>(object obj);

    /// <summary>
    /// Маппинг объекта - источника (TSource) в существующий объект (TDestination).
    /// </summary>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}
