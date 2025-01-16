using System.Text.RegularExpressions;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Controllers;

/// <summary>
/// Трансформер названия контроллера в kebab-case.
/// </summary>
public partial class KebabCaseTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value) =>
        value is null || value is not string
            ? null
            : SmallToBigCharRegex().Replace(value.ToString()!, "$1-$2").ToLower();

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex SmallToBigCharRegex();
}
