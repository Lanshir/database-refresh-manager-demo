using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.HttpResults;

public class ExtendedProblemDetails : ProblemDetails
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }
}
