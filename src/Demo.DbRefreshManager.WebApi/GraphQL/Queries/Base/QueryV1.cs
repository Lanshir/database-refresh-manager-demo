using Demo.DbRefreshManager.WebApi.GraphQL.Queries.V1;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;

/// <summary>
/// GraphQL запросы версии 1.
/// </summary>
public class QueryV1
{
    /// <inheritdoc cref="DbRefreshJobsQueriesV1" />
    public DbRefreshJobsQueriesV1 DbRefreshJob => new();
}
