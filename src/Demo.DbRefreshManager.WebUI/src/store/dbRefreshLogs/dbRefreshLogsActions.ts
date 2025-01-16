import { atom } from 'jotai';
import { RESET } from 'jotai/utils';
import { logsListState, logsLoadingState } from './dbRefreshLogsState';
import { pushErrorAction } from '@store/alerts/alertsActions';
import { DbRefreshLog } from '@shared/types/api/dbRefreshJobs';
import { IRequestError } from '@shared/types/errors';
import { GetDbRefreshLogs } from '@requests/graphql/queries/dbRefreshJobsQueries';

/** Действие сброса состояния страницы. */
export const resetPageStateAction = atom(null,
    (get, set) => {
        set(logsListState, RESET);
        set(logsLoadingState, RESET);
    });

/** Запрос загрузки логов перезаливок БД. */
export const loadDbRefreshLogsQuery = atom(null,
    async (get, set,
        jobId?: number | null,
        startDate?: Date | null
    ) => {
        set(logsLoadingState, true);

        try {
            const logs = await GetDbRefreshLogs(jobId, startDate) as DbRefreshLog[];
            set(logsListState, logs);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }

        set(logsLoadingState, false);
    });