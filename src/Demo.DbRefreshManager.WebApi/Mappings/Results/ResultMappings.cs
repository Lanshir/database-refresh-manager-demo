using Demo.DbRefreshManager.Core.Results;
using Demo.DbRefreshManager.WebApi.Infrastructure.HttpResults;

namespace Demo.DbRefreshManager.WebApi.Mappings.Results;

public static class ResultMappings
{
    extension(Result result)
    {
        /// <summary>
        /// Конвертация результата операции приложения в ProblemDetails.
        /// </summary>
        public ExtendedProblemDetails ToProblemDetails(
            string title,
            int status = StatusCodes.Status400BadRequest)
            => new()
            {
                Title = title,
                Status = status,
                ErrorCode = result.Error.Code,
                Detail = result.Error.Message
            };
    }
}
