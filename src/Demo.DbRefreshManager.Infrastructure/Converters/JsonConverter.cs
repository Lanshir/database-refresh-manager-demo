using Demo.DbRefreshManager.Application.Converters;
using Demo.DbRefreshManager.Core.Enums;
using System.Text.Json;

namespace Demo.DbRefreshManager.Infrastructure.Converters;

/// <inheritdoc cref="IJsonConverter" />
public class JsonConverter : IJsonConverter
{
    private readonly JsonSerializerOptions _caseInsensitiveOptions = new(JsonSerializerDefaults.General)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly JsonSerializerOptions _camelCaseOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = false
    };

    public string Serialize<T>(T obj, TextStyle textStyle = TextStyle.CamelCase)
        => textStyle switch
        {
            TextStyle.None => JsonSerializer.Serialize(obj, _caseInsensitiveOptions),
            TextStyle.CamelCase => JsonSerializer.Serialize(obj, _camelCaseOptions),
            _ => JsonSerializer.Serialize(obj, _caseInsensitiveOptions),
        };

    public T? Deserialize<T>(string json, TextStyle textStyle = TextStyle.None)
        => textStyle switch
        {
            TextStyle.None => JsonSerializer.Deserialize<T>(json, _caseInsensitiveOptions),
            TextStyle.CamelCase => JsonSerializer.Deserialize<T>(json, _camelCaseOptions),
            _ => JsonSerializer.Deserialize<T>(json, _caseInsensitiveOptions),
        };
}
