using System.Text.Json.Serialization;

namespace Demo.DbRefreshManager.WebApi.Models.Api;

public record ApiPaginatedResponseDto<TData> : ApiResponseDto<TData>
{
    /// <summary>
    /// Текущая страница.
    /// </summary>
    [JsonPropertyOrder(100)]
    public int Page { get; set; }

    /// <summary>
    /// Всего страниц.
    /// </summary>
    [JsonPropertyOrder(101)]
    public int TotalPages { get; set; }

    /// <summary>
    /// Кол-во элементов на всех страницах.
    /// </summary>
    [JsonPropertyOrder(102)]
    public int TotalItems { get; set; }
}
