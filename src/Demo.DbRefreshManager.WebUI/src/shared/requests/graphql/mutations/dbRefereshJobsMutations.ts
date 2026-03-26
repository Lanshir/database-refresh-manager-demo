import { gql } from '@apollo/client';
import { ApolloClient } from '@apiClients';
import { DbRefreshJob, DbRefreshJobKeys } from '@shared/types/api/dbRefreshJobs';

/**
 * Запуск ручной перезаливки БД.
 * @param jobId Id задачи на перезаливку.
 * @param delayMinutes Задержка до перезаливки в минутах.
 * @param comment Комментарий.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function StartManualDbRefresh(
    jobId: number,
    delayMinutes: number,
    comment?: string | null,
    propsToFetch = DbRefreshJobKeys
) {
    type Schema = { v1: { dbRefreshJobs: { manualStart: Partial<DbRefreshJob> } } };

    const variables = { jobId, delayMinutes, comment };

    const mutation = gql`
        mutation StartManualDbRefresh
        ($jobId: Int!, $delayMinutes: Int!, $comment: String){
          v1 {
            dbRefreshJobs{
              manualStart:startManualRefresh
              (jobId: $jobId, delayMinutes: $delayMinutes, comment: $comment) {
                ${propsToFetch.join('\n')}
              }
            }
          }
        }
    `;

    const result = await ApolloClient.mutate<Schema>({ mutation, variables });

    return result.data!.v1.dbRefreshJobs.manualStart;
}

/**
 * Остановка ручной перезаливки БД.
 * @param jobId Id задачи на перезаливку.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function StopManualDbRefresh(
    jobId: number,
    propsToFetch = DbRefreshJobKeys
) {
    type Schema = { v1: { dbRefreshJobs: { manualStop: Partial<DbRefreshJob> } } };

    const variables = { jobId };

    const mutation = gql`
        mutation StopManualDbRefresh($jobId: Int!){
          v1 {
            dbRefreshJobs{
              manualStop:stopManualRefresh(jobId: $jobId) {
                ${propsToFetch.join('\n')}
              }
            }
          }
        }
    `;

    const result = await ApolloClient.mutate<Schema>({ mutation, variables });

    return result.data!.v1.dbRefreshJobs.manualStop;
}

/**
 * Установить активность перезаливки по расписанию.
 * @param jobId Id задачи на перезаливку.
 * @param isActive Активность перезаливки.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function SetScheduledDbRefreshActive(
    jobId: number,
    isActive: boolean,
    propsToFetch = DbRefreshJobKeys
) {
    type Schema = { v1: { dbRefreshJobs: { scheduleChange: Partial<DbRefreshJob> } } };

    const variables = { jobId, isActive };

    const mutation = gql`
        mutation SetScheduledDbRefreshActive
        ($jobId: Int!, $isActive: Boolean!){
          v1 {
            dbRefreshJobs{
              scheduleChange:setScheduledRefreshActive
              (jobId: $jobId, isActive: $isActive) {
                ${propsToFetch.join('\n')}
              }
            }
          }
        }
    `;

    const result = await ApolloClient.mutate<Schema>({ mutation, variables });

    return result.data!.v1.dbRefreshJobs.scheduleChange;
}

/**
 * Установка пользовательского комментария к задаче на перезаливку.
 * @param jobId Id задачи на перезаливку.
 * @param comment Комментарий.
 * @param propsToFetch Массив свойств для запроса.
 */
export async function SetUserComment(
    jobId: number,
    comment: string,
    propsToFetch = DbRefreshJobKeys
) {
    type Schema = { v1: { dbRefreshJobs: { setUserComment: Partial<DbRefreshJob> } } };

    const query = gql`
        mutation SetUserComment($jobId: Int!, $comment: String!){
          v1{
            dbRefreshJobs{
              setUserComment(jobId: $jobId, comment: $comment){
                ${propsToFetch.join('\n')}
              }
            }
          }
        }
    `;

    const variables = { jobId, comment };
    const result = await ApolloClient.query<Schema>({ query, variables });

    return result.data!.v1.dbRefreshJobs.setUserComment;
}
