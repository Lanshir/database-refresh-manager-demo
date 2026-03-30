import { ApolloClient } from '@apiClients';
import { gql } from '@apollo/client';
import {
    DbGroup, DbGroupKeys, DbRefreshJob, DbRefreshJobKeys, DbRefreshLog, DbRefreshLogKeys
} from '@shared/types/api/dbRefreshJobs';

/**
 * GraphQL запросы задач на перезаливку БД.
 */

/**
 * Получить список задач на перезаливку БД.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function GetDbRefreshJobs(
    propsToFetch = DbRefreshJobKeys
) {
    type Schema = { v1: { dbRefreshJobs: Partial<DbRefreshJob>[] } };

    const query = gql`
        query GetDbRefreshJobs{
            v1{
              dbRefreshJobs{
                ${propsToFetch.join('\n')}
              }
            }
        }
    `;

    const response = await ApolloClient.query<Schema>({ query });

    return response.data.v1.dbRefreshJobs;
}

/**
 * Получить группы БД.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function GetDbGroups(
    propsToFetch = DbGroupKeys
) {
    type Schema = { v1: { dbGroups: Partial<DbGroup>[] } };

    const query = gql`
        query GetDbGroups{
          v1{
            dbGroups{
              ${propsToFetch.join('\n')}
            }
          }
        }
    `;

    const response = await ApolloClient.query<Schema>({ query });

    return response.data.v1.dbGroups;
}

/**
 * Получить список id задач на перезаливку с персональным доступом.
 */
export async function GetDbPersonalAccessIds() {
    type Schema = { v1: { dbPersonalAccessIds: number[] } };

    const query = gql`
        query GetDbPersonalAccessIds{
          v1{
            dbPersonalAccessIds
          }
        }
    `;

    const response = await ApolloClient.query<Schema>({ query });

    return response.data.v1.dbPersonalAccessIds;
}

/**
 * Получить логи перезаливок БД.
 * @param jobId Фильтр по id задачи на перезаливку.
 * @param startDate Фильтр по дате начала перезаливки.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function GetDbRefreshLogs(
    jobId?: number | null,
    startDate?: Date | null,
    propsToFetch = DbRefreshLogKeys
) {
    type Schema = { v1: { dbRefreshLogs: Partial<DbRefreshLog>[] } };

    const query = gql`
        query GetDbRefreshLogs($jobId: Int, $startDate: DateTime) {
          v1{
            dbRefreshLogs(jobId: $jobId, startDate: $startDate) {
              ${propsToFetch.join('\n')}
            }
          }
        }
    `;

    const variables = {
        jobId: jobId,
        startDate: !!startDate ? startDate.toISOString() : null
    };

    const response = await ApolloClient.query<Schema>({ query, variables });

    return response.data?.v1.dbRefreshLogs;
}
