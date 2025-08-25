import { ApolloClient } from '@apiClients';
import { gql } from '@apollo/client';
import { FrontendConfig, FrontendConfigKeys } from '@shared/types/api/frontendConfig/frontendConfig';

/**
 * Получение конфигурации с сервера.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function GetConfig(
    propsToFetch = FrontendConfigKeys
) {
    type Schema = { v1: { config: Partial<FrontendConfig> } };

    const query = gql`query GetConfig{
        v1{
            config:frontendConfig{${propsToFetch.join('\n')}}
        }
    }`;

    const res = await ApolloClient.query<Schema>({ query });

    return res.data.v1.config;
}
