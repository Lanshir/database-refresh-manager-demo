import { atom } from 'jotai';
import { dbRefreshJobsListItemsState, DbRefreshJobListItem } from './listItemsState';
import { pushErrorAction } from '@store/alerts/alertsActions';
import { IRequestError } from '@shared/types/errors';
import { GetDbRefreshJobs } from '@requests/graphql/queries/dbRefreshJobsQueries';

/** Запрос пунктов списка задач на перезаливку БД. */
export const dbRefreshJobsListQuery = atom(null,
    async (get, set) => {
        try {
            const listItems = await GetDbRefreshJobs(
                ['id', 'dbName']
            ) as DbRefreshJobListItem[];

            set(dbRefreshJobsListItemsState, listItems);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }
    });
