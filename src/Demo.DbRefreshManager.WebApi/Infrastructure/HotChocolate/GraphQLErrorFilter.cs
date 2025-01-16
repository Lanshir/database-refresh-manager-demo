using Demo.DbRefreshManager.Common.Enums;
using Demo.DbRefreshManager.Common.Exceptions;

namespace Demo.DbRefreshManager.WebApi.Infrastructure.HotChocolate;

/// <summary>
/// Фильтр ошибок HotChocolate.
/// </summary>
public class GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger) : IErrorFilter
{
    public IError OnError(IError error)
    {
        var errBuilder = ErrorBuilder.FromError(error)
                .RemoveException()
                .ClearExtensions()
                .ClearLocations();

        switch (error.Exception)
        {
            case BusinessLogicException blException:
                errBuilder
                    .SetCode(blException.Code.ToString())
                    .SetMessage(blException.Message);
                break;
            // GraphQL errors without exception.
            case null: break;
            // Other Exceptions.
            default:
                {
                    errBuilder
                        .SetCode(((int)DefaultStatusCodes.Error).ToString())
                        .SetMessage("Ошибка сервера при отправке запроса");

                    logger.LogError(error.Exception,
                        $"Ошибка запроса GraphQL. Path: {error.Path}");

                    break;
                }
        };

        return errBuilder.Build();
    }
}
