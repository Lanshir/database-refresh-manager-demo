import { ApolloClient, HttpLink, InMemoryCache, from, split } from '@apollo/client';
import { getMainDefinition } from '@apollo/client/utilities';
import { GraphQLWsLink } from '@apollo/client/link/subscriptions';
import { createClient } from 'graphql-ws';
import { Kind, OperationTypeNode } from 'graphql';

const httpLink = new HttpLink({ uri: '/graphql' });

const wsLink = new GraphQLWsLink(createClient({
    url: window.location.protocol === 'https:'
        ? `wss://${location.host}/graphql`
        : `ws://${location.host}/graphql`
}));

const protocolLink = split(
    ({ query }) => {
        const definition = getMainDefinition(query);

        return definition.kind === Kind.OPERATION_DEFINITION
            && definition.operation === OperationTypeNode.SUBSCRIPTION;
    },
    wsLink,
    httpLink,
);

/** Default api client composed apollo link. */
export const composedLink = from([protocolLink]);

/** Site default GraphQL client. */
export const apiClient = new ApolloClient({
    link: composedLink,
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

export default apiClient;
