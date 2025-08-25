import { gql } from '@apollo/client';
import { DbRefreshJob, DbRefreshJobKeys } from '@shared/types/api/dbRefreshJobs';

/** Схема результата события изменения задачи на перезаливку БД. */
export type OnDbRefreshJobChangeSchema = {
    dbJobEvent: Partial<DbRefreshJob>
};

/**
 * Получить текст запроса события изменения задачи на перезаливку БД.
 * @param propsToFetch Массив свойств для запроса.
 */
export function GetOnDbRefreshJobChangeQuery(
    propsToFetch = DbRefreshJobKeys
) {
    return gql`
        subscription OnDbRefreshJobChange{
          dbJobEvent:onDbRefreshJobChange {
            ${propsToFetch.join('\n')}
          }
        }
    `;
}
