using Demo.DbRefreshManager.Domain.Errors;
using Demo.DbRefreshManager.Domain.Exceptions;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.HotChocolate;

/// <summary>
/// Фильтр ошибок HotChocolate.
/// </summary>
public class GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger) : IErrorFilter
{
    public IError OnError(IError error)
    {
        var errBuilder = ErrorBuilder.FromError(error)
                .ClearExtensions()
                .ClearLocations();

        switch (error.Exception)
        {
            case BusinessLogicException blException:
                errBuilder
                    .SetCode(blException.Code)
                    .SetMessage(blException.Message);
                break;
            // GraphQL errors without exception.
            case null: break;
            // Other Exceptions.
            default:
                {
                    errBuilder
                        .SetCode(DefaultErrors.Unexpected)
                        .SetMessage("Ошибка сервера при отправке запроса");

                    logger.LogError(error.Exception,
                        $"Ошибка запроса GraphQL. Path: {error.Path}");

                    break;
                }
        }

        return errBuilder.Build().WithException(null);
    }
}
