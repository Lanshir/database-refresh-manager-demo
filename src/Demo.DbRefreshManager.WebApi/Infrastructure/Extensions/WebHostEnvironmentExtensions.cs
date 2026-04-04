namespace Demo.DbRefreshManager.WebApi.Infrastructure.Extensions;

/// <summary>
/// Расширения IWebHostEnvironment.
/// </summary>
public static class WebHostEnvironmentExtensions
{
    extension(IWebHostEnvironment env)
    {
        /// <summary>
        /// Окружение работает в контейнере.
        /// </summary>
        public bool IsInContainer()
        {
            var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

            return inContainer == "true";
        }

        /// <summary>
        /// Фоновые задачи Quartz выключены.
        /// </summary>
        public bool IsQuartzDisabled()
        {
            var quartzDisabled = Environment.GetEnvironmentVariable("DISABLE_QUARTZ_JOBS");
            var trueStrings = new string[] { "true", "1", "y", "yes" };

            return trueStrings.Contains(quartzDisabled);
        }
    }
}
