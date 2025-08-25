import { ServerError } from '@apollo/client';
import { GraphQLErrors, NetworkError } from '@apollo/client/errors';

/**
 * Получить ошибки GraphQL из Apollo Network Errors.
 */
export default function GetGqlErrorsFromNetworkError(networkError?: NetworkError): GraphQLErrors | null {
    if (!networkError) {
        return null;
    }

    const serverError = networkError as ServerError;

    const gqlErrors = (
        <Record<string, unknown>>serverError?.result
    )['errors'] as GraphQLErrors;

    return gqlErrors;
}
