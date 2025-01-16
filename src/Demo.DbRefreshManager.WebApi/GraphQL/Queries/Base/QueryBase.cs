namespace Demo.DbRefreshManager.WebApi.GraphQL.Queries.Base;

/// <summary>
/// GraphQL query base.
/// </summary>
public class QueryBase
{
    /// <inheritdoc cref="QueryV1" />
    public QueryV1 V1 => new();
}
