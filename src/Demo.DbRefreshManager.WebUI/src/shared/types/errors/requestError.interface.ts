import { ApolloError } from '@apollo/client';

/**
 * Интерфейс ошибки запросов приложения.
 */
interface IRequestError<TResponseData = object>
    extends Error, Pick<Partial<ApolloError>, 'graphQLErrors' | 'networkError'> {

    /** Данные из json ответа. */
    responseData?: TResponseData
}

export default IRequestError;