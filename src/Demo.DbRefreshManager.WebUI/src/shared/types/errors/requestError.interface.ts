import { ApolloError } from '@apollo/client';

/**
 * Интерфейс ошибки запросов приложения.
 */
interface IRequestError<TAxiosData = object> extends
    Error,
    Pick<Partial<ApolloError>, 'graphQLErrors' | 'networkError'> {

    /** Данные ответа Axios. */
    response?: {
        data?: TAxiosData
    }
}

export default IRequestError;
