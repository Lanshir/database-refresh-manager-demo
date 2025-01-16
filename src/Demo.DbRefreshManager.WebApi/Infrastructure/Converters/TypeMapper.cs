using AutoMapper;
using Demo.DbRefreshManager.Common.Converters.Abstract;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Converters;

/// <inheritdoc cref="ITypeMapper" />
public class TypeMapper(IMapper mapper) : ITypeMapper
{
    public T Map<T>(object obj) => mapper.Map<T>(obj);

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        => mapper.Map(source, destination);
}
