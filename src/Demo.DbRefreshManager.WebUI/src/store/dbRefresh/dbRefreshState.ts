import { atom } from 'jotai';
import { atomWithReset, atomFamily } from 'jotai/utils';
import { focusAtom } from 'jotai-optics';
import { authorizationState } from '@store/authorization/authorizationState';
import { DbRefreshJob } from '@shared/types/api/dbRefreshJobs';
import { chain } from 'underscore';

/** Состояние загрузки на странице. */
export const pageLoadingState = atomWithReset(false);

/** Состояния загрузки задач на перезаливку. */
export const jobLoadingState = atomFamily((id: number) => atom(false));

/** Список задач на перезалику. */
export const jobsListState = atomWithReset<DbRefreshJob[]>([]);

/** Состояние фильтрации списка задач на перезаливку. */
export const jobsListFilterState = atomWithReset<IJobsListFiltersState>({});

/** Состояние списка персональных доступов к задачам перезаливки. */
export const personalAccessIdsState = atomWithReset<number[]>([]);

/** Задержка до перезаливки в минутах. */
export const confirmRefreshDelayMinutesState = atomWithReset(10);

/** Комментарий подтверждения перезаливки. */
export const confirmRefreshCommentState = atomWithReset('');

/** Состояние диалогового окна подтверждения перезаливки. */
export const confirmRefreshDialogState = atomWithReset<IRefreshDialogState>({});

/** Состояние диалогового окна отмены перезаливки. */
export const cancelRefreshDialogState = atomWithReset<IRefreshDialogState>({});

/** Отсортированный список задач на перезаливку. */
export const jobsListSortedState = atom((get) => {
    const { user } = get(authorizationState);
    const jobs = get(jobsListState);
    const personalAccessIds = get(personalAccessIdsState);

    return chain(jobs)
        // 3 - Сортировка по названию без цифры > цифре из названия.
        .sortBy(j => Number(j.dbName.replaceAll(/[^0-9]/g, '')))
        .sortBy(j => j.dbName.replaceAll(/[0-9]/g, ''))
        // 2 - Сортировка по группам.
        .sortBy(j => j.groupSortOrder)
        // 1 - БД к которым есть доступ.
        .sortBy(j => !personalAccessIds.includes(j.id)
            && !j.groupAccessRoles.some(r => user.roles.includes(r)))
        .value();
});

/** Отфильтрованный список задач на перезаливку. */
export const jobsListFilteredState = atom((get) => {
    const jobs = get(jobsListSortedState);
    const { jobId } = get(jobsListFilterState);

    return jobs.filter(j => !!jobId ? j.id === jobId : true);
});

export interface IJobsListFiltersState {
    jobId?: number | null
}

export interface IRefreshDialogState {
    jobId?: number | null
};