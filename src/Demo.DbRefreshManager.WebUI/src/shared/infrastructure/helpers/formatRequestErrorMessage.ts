import { ServerError } from '@apollo/client';
import { IRequestError } from '@shared/types/errors';
import { ProblemDetails } from '@shared/types/api/rest/problemDetails.interface';
import { GetGqlErrorsFromNetworkError } from '@helpers';

/**
 * Получить отформатированное сообщение ошибки IRequestError.
 */
export default function FormatRequestErrorMessage(error: IRequestError | Error): string {
    const reqError = error as IRequestError<ProblemDetails>;
    const restApiResponseMessage = reqError.response?.data?.detail;

    const gqlServerError = reqError.networkError as ServerError | undefined;
    const gqlNetworkErrors = GetGqlErrorsFromNetworkError(reqError.networkError);

    let message = '';

    // Ошибки GraphQL с кодом 200.
    if (!!reqError.graphQLErrors && reqError.graphQLErrors.length > 0) {
        message = reqError.graphQLErrors
            .map(e => `${e.message} at ${e.path?.join(' > ')}`)
            .join('\n');
    }
    // Ошибки GraphQL с кодом 500.
    else if (!!gqlNetworkErrors && gqlNetworkErrors.length > 0) {
        message = gqlNetworkErrors
            .map(e => `(${gqlServerError?.statusCode}) ${e.message} at ${e.path?.join(' > ')}`)
            .join('\n');
    }
    // Ошибки REST с телом ApiResponse.
    else if (!!restApiResponseMessage) {
        message = restApiResponseMessage;
    }
    else {
        message = reqError.message;
    }

    return message;
}
