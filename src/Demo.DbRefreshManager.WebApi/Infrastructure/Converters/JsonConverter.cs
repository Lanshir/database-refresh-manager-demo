using Demo.DbRefreshManager.Common.Converters.Abstract;
using Demo.DbRefreshManager.Common.Enums;
using System.Text.Json;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Converters;

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
    {
        switch (textStyle)
        {
            case TextStyle.None:
                return JsonSerializer.Serialize(obj, _caseInsensitiveOptions);
            case TextStyle.CamelCase:
                return JsonSerializer.Serialize(obj, _camelCaseOptions);
            default:
                return JsonSerializer.Serialize(obj, _caseInsensitiveOptions);
        }
    }

    public T? Deserialize<T>(string json, TextStyle textStyle = TextStyle.None)
    {
        switch (textStyle)
        {
            case TextStyle.None:
                return JsonSerializer.Deserialize<T>(json, _caseInsensitiveOptions);
            case TextStyle.CamelCase:
                return JsonSerializer.Deserialize<T>(json, _camelCaseOptions);
            default:
                return JsonSerializer.Deserialize<T>(json, _caseInsensitiveOptions);
        }
    }
}
