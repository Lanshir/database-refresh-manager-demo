import ApolloClient, { composedLink } from '@apiClients/apollo';
import AxiosClient from '@apiClients/axios';
import { from } from '@apollo/client';
import { onError } from '@apollo/client/link/error';
import GqlErrors from '@constants/gqlErrorCodes';
import Routes from '@constants/routes';
import { authorizationState } from '@store/authorization/authorizationState';
import { AxiosError } from 'axios';
import { useResetAtom } from 'jotai/utils';
import { FC, PropsWithChildren, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMount } from 'react-use';

export const ApiClientsConfigurer: FC<PropsWithChildren> = ({ children }) => {
    const navigate = useNavigate();
    const resetAuthState = useResetAtom(authorizationState);

    const onUnauthorized = () => {
        resetAuthState();
        navigate({ pathname: Routes.login });
    };

    useMount(() => {
        // GraphQL.
        // Сброс авторизации и редирект при ошибке авторизации GraphQL.
        const unauthorizedErrorLink = onError(({ graphQLErrors }) => {
            if (graphQLErrors?.some(e => e.extensions!['code'] === GqlErrors.Unauthenticated)) {
                onUnauthorized();
            }
        });

        ApolloClient.setLink(from([unauthorizedErrorLink, composedLink]));

        /** REST */
        // Сброс авторизации и редирект при 401 коде в запросах axios.
        AxiosClient.interceptors.response.use(undefined, (err: AxiosError) => {
            if (err.response?.status == 401) {
                onUnauthorized();
            }

            throw err;
        });
    });

    return children;
};

export default ApiClientsConfigurer;
