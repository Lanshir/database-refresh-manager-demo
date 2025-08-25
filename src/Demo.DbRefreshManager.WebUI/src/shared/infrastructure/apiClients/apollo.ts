import { ApolloClient, HttpLink, InMemoryCache, from, split } from '@apollo/client';
import { getMainDefinition } from '@apollo/client/utilities';
import { GraphQLWsLink } from '@apollo/client/link/subscriptions';
import { onError } from '@apollo/client/link/error';
import { createClient } from 'graphql-ws';
import { Kind, OperationTypeNode } from 'graphql';
import GqlErrors from '@enums/graphQL/gqlErrorCodes';
import Navigation from './utils/navigationProvider';

const httpLink = new HttpLink({ uri: '/graphql' });

const wsLink = new GraphQLWsLink(createClient({
    url: window.location.protocol === 'https:'
        ? `wss://${location.host}/graphql`
        : `ws://${location.host}/graphql`
}));

const splitLink = split(
    ({ query }) => {
        const definition = getMainDefinition(query);

        return (
            definition.kind === Kind.OPERATION_DEFINITION
            && definition.operation === OperationTypeNode.SUBSCRIPTION
        );
    },
    wsLink,
    httpLink,
);

/** Обработка ошибок. */
const errorLink = onError(({ graphQLErrors }) => {
    if (graphQLErrors?.some(e =>
        e.extensions!['code'] === GqlErrors.Unauthorized)
    ) {
        Navigation.toLoginPage();
    }
});

/** Site default GraphQL client. */
const client = new ApolloClient({
    link: from([errorLink, splitLink]),
    cache: new InMemoryCache(),
    defaultOptions: {
        query: {
            fetchPolicy: 'no-cache'
        },
        mutate: {
            fetchPolicy: 'no-cache'
        },
        watchQuery: {
            fetchPolicy: 'no-cache'
        }
    }
});

export default client;
