using Demo.DbRefreshManager.WebApi.GraphQL.Mutations.V1;

namespace Demo.DbRefreshManager.WebApi.GraphQL.Mutations.Base;

/// <summary>
/// GraphQL мутации версии 1.
/// </summary>
public class MutationV1
{
    /// <inheritdoc cref="DbRefreshJobsMutationsV1" />
    public DbRefreshJobsMutationsV1 DbRefreshJob => new();
}
