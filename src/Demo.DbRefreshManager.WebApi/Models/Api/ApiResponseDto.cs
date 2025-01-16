using System.Text.Json.Serialization;

namespace Demo.DbRefreshManager.WebApi.Models.Api;

/// <summary>
/// Модель ответа api.
/// </summary>
public class ApiResponseDto<TData>
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
    /// Текущая страница.
    /// </summary>
    [JsonPropertyOrder(4)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Page { get; set; }

    /// <summary>
    /// Всего страниц.
    /// </summary>
    [JsonPropertyOrder(5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalPages { get; set; }

    /// <summary>
    /// Кол-во элементов на всех страницах.
    /// </summary>
    [JsonPropertyOrder(6)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalItems { get; set; }

    /// <summary>
    /// Объект данных.
    /// </summary>
    [JsonPropertyOrder(7)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TData? Data { get; set; }
}
