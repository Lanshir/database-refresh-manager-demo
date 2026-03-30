using System.Text.Json.Serialization;

namespace Demo.DbRefreshManager.WebApi.Models.Api;

/// <summary>
/// Модель ответа api.
/// </summary>
public record ApiResponseDto<TData>
{
    /// <summary>
    /// Код ответа.
    /// </summary>
    [JsonPropertyOrder(0)]
    public int Code { get; set; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    [JsonPropertyOrder(1)]
    public required string Message { get; set; }

    /// <summary>
    /// Флаг успешного выполнения.
    /// </summary>
    [JsonPropertyOrder(2)]
    public required bool IsSuccess { get; set; }

    /// <summary>
    /// Время запроса (UTC).
    /// </summary>
    [JsonPropertyOrder(3)]
    public DateTime RequestTimeUTC { get; } = DateTime.UtcNow;

    /// <summary>
    /// Объект данных.
    /// </summary>
    [JsonPropertyOrder(1000)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TData? Data { get; set; }
}
