using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.Static;

public static class HealthcheckResponseWriters
{
    /// <summary>
    /// Запись текстового ответа healthcheck.
    /// </summary>
    public static Task WriteTextResponse(HttpContext ctx, HealthReport res)
    {
        var msg = new StringBuilder();

        msg.AppendLine($"API Status: {res.Status}");

        foreach (var entry in res.Entries)
        {
            msg.AppendLine();
            msg.AppendLine($"{entry.Key}");
            msg.AppendLine($"- Status: {entry.Value.Status}");

            if (!string.IsNullOrWhiteSpace(entry.Value.Description))
            {
                msg.AppendLine($"- Message: {entry.Value.Description}");
            }
        }

        return ctx.Response.WriteAsync(msg.ToString());
    }
}
