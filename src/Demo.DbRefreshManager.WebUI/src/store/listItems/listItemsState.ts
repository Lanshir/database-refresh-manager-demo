import { atom } from 'jotai';
import { DbRefreshJob } from '@shared/types/api/dbRefreshJobs';
import { chain } from 'underscore';

/** Пункты для списков задач на перезаливку БД. */
export const dbRefreshJobsListItemsState = atom<DbRefreshJobListItem[]>([]);

/** Сортированные пункты списка задач на перезаливку. */
export const dbRefreshJobsSortedItemsState = atom(get =>
    chain(get(dbRefreshJobsListItemsState))
        .sortBy(i => Number(i.dbName.replaceAll(/[^0-9]/g, '')))
        .sortBy(i => i.dbName.replaceAll(/[0-9]/g, ''))
        .value()
);

export type DbRefreshJobListItem = Pick<DbRefreshJob, 'id' | 'dbName'>;
