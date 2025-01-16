import { ApolloError } from '@apollo/client';

/**
 * Интерфейс ошибки запросов приложения.
 */
interface IRequestError<TFetchData = unknown>
    extends Error, Pick<Partial<ApolloError>, 'graphQLErrors' | 'networkError'> {

    /** Данные ошибки fetch. */
    fetchData?: TFetchData
    redirected?: boolean
}

export default IRequestError;