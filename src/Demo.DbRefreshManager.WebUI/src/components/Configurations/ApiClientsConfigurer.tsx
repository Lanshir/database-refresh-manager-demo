import { FC, PropsWithChildren } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMount } from 'react-use';
import ApolloClient, { composedLink } from '@apiClients/apollo';
import { from } from '@apollo/client';
import { onError } from '@apollo/client/link/error';
import AxiosClient from '@apiClients/axios';
import { AxiosError } from 'axios';
import GqlErrors from '@constants/gqlErrorCodes';
import Routes from '@constants/routes';

export const ApiClientsConfigurer: FC<PropsWithChildren> = ({ children }) => {
    const navigate = useNavigate();

    useMount(() => {
        // GraphQL.
        // Редирект при ошибке авторизации GraphQL.
        const unauthorizedErrorLink = onError(({ graphQLErrors }) => {
            if (graphQLErrors?.some(e => e.extensions!['code'] === GqlErrors.Unauthenticated)) {
                navigate({ pathname: Routes.login });
            }
        });

        ApolloClient.setLink(from([unauthorizedErrorLink, composedLink]));

        /** REST */
        // Редирект при 401 коде в запросах axios.
        AxiosClient.interceptors.response.use(undefined, (err: AxiosError) => {
            if (err.response?.status == 401) {
                navigate({ pathname: Routes.login });
            }

            throw err;
        });
    });

    return children;
};

export default ApiClientsConfigurer;
