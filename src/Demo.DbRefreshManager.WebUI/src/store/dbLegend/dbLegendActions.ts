import { atom } from 'jotai';
import { dbLegendState, legendLoadingState } from './dbLegendState';
import { pushErrorAction } from '@store/alerts/alertsActions';
import IRequestError from '@shared/types/errors/requestError.interface';
import { GetDbGroups } from '@requests/graphql/queries/dbRefreshJobsQueries';
import { DbGroup } from '@shared/types/api/dbRefreshJobs';

/** Запрос легенды БД. */
export const dbLegendQuery = atom(null,
    async (get, set) => {
        set(legendLoadingState, true);

        try {
            const res = await GetDbGroups() as DbGroup[];
            set(dbLegendState, res);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }

        set(legendLoadingState, false);
    });
