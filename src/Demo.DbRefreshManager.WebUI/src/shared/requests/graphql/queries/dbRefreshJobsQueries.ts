import { ApolloClient } from '@apiClients';
import { gql } from '@apollo/client';
import {
    DbGroup, DbGroupKeys, DbRefreshJob, DbRefreshJobKeys, DbRefreshLog, DbRefreshLogKeys
} from '@shared/types/api/dbRefreshJobs';
import dayjs from 'dayjs';

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
    type Schema = { v1: { dbRefreshJob: { list: Partial<DbRefreshJob>[] } } };

    const query = gql`
        query GetDbRefreshJobs{
          v1{
            dbRefreshJob{
              list{
                ${propsToFetch.join('\n')}
              }
            }
          }
        }
    `;

    const response = await ApolloClient.query<Schema>({ query });

    return response.data.v1.dbRefreshJob.list;
}

/**
 * Получить группы БД.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function GetDbGroups(
    propsToFetch = DbGroupKeys
) {
    type Schema = { v1: { dbRefreshJob: { groups: Partial<DbGroup>[] } } };

    const query = gql`
        query GetDbGroups{
          v1{
            dbRefreshJob{
              groups{
                ${propsToFetch.join('\n')}
              }
            }
          }
        }
    `;

    const response = await ApolloClient.query<Schema>({ query });

    return response.data.v1.dbRefreshJob.groups;
}

/**
 * Получить список id задач на перезаливку с персональным доступом.
 */
export async function GetPersonalAccessIds() {
    type Schema = { v1: { dbRefreshJob: { accessIds: number[] } } };

    const query = gql`
        query GetPersonalAccessIds{
          v1{
            dbRefreshJob{
              accessIds: personalAccessIds
            }
          }
        }
    `;

    const response = await ApolloClient.query<Schema>({ query });

    return response.data.v1.dbRefreshJob.accessIds;
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
    type Schema = { v1: { dbRefreshJob: { logs: Partial<DbRefreshLog>[] } } };

    const query = gql`
        query GetDbRefreshLogs($jobId: Int, $startDate: DateTime) {
          v1{
            dbRefreshJob{
              logs(jobId: $jobId, startDate: $startDate) {
                ${propsToFetch.join('\n')}
              }
            }
          }
        }
    `;

    const variables = {
        jobId: jobId,
        startDate: !!startDate ? dayjs(startDate).toISOString() : null
    };

    const response = await ApolloClient.query<Schema>({ query, variables });

    return response.data?.v1.dbRefreshJob.logs;
}