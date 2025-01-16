namespace Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;

/// <summary>
/// Расширения IWebHostEnvironment.
/// </summary>
public static class WebHostEnvironmentExtensions
{
    /// <summary>
    /// Окружение работает в контейнере.
    /// </summary>
    public static bool IsInContainer(this IWebHostEnvironment _)
    {
        var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

        return inContainer == "true";
    }

    /// <summary>
    /// Фоновые задачи Quartz выключены.
    /// </summary>
    public static bool IsQuartzDisabled(this IWebHostEnvironment _)
    {
        var quartzDisabled = Environment.GetEnvironmentVariable("DISABLE_QUARTZ_JOBS");
        var trueStrings = new string[] { "true", "1", "y", "yes" };

        return trueStrings.Contains(quartzDisabled);
    }
}
