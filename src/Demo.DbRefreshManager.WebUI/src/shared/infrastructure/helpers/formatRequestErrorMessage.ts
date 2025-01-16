import { ServerError } from '@apollo/client';
import { IRequestError } from '@shared/types/errors';
import { ApiResponse } from '@shared/types/api/rest';
import { GetGqlErrorsFromNetworkError } from '@helpers';

/**
 * Получить отформатированное сообщение ошибки IRequestError.
 */
export default function FormatRequestErrorMessage(error: IRequestError | Error): string {
    const reqError = error as IRequestError<ApiResponse>;
    const restApiResponseMessage = reqError.fetchData?.message;

    const gqlServerError = reqError.networkError as ServerError | undefined;
    const gqlNetworkErrors = GetGqlErrorsFromNetworkError(reqError.networkError);

    const message = (reqError.graphQLErrors?.length ?? 0) > 0
        // Ошибки GraphQL с кодом 200.
        ? (reqError.graphQLErrors
            ?.map(e => `${e.message} at ${e.path?.join(' > ')}`)
            ?? []
        ).join('\n')
        // Ошибки GraphQL с кодом 500.
        : (gqlNetworkErrors?.length ?? 0) > 0
            ? (gqlNetworkErrors
                ?.map(e => `(${gqlServerError?.statusCode}) ${e.message} at ${e.path?.join(' > ')}`)
                ?? []
            ).join('\n')
            // Ошибки REST с телом ApiResponse.
            : !!restApiResponseMessage
                ? restApiResponseMessage
                // Стандартное сообщение.
                : reqError.message;

    return message;
}