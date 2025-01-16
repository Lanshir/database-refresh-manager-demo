namespace Demo.DbRefreshManager.WebApi.GraphQL.Mutations.Base;

/// <summary>
/// GraphQL mutation base.
/// </summary>
public class MutationBase
{
    /// <inheritdoc cref="MutationV1" />
    public MutationV1 V1 => new();
}
