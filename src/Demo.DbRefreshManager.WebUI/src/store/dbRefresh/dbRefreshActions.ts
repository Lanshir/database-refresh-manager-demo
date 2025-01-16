import { atom } from 'jotai';
import { RESET } from 'jotai/utils';
import {
    pageLoadingState, jobsListState, jobLoadingState,
    jobsListFilterState, personalAccessIdsState,
    confirmRefreshInputState, confirmRefreshDialogState, cancelRefreshDialogState
} from './dbRefreshState';
import { dbRefreshJobsListItemsState, DbRefreshJobListItem } from '@store/listItems/listItemsState';
import { pushErrorAction } from '@store/alerts/alertsActions';
import { GetDbRefreshJobs, GetPersonalAccessIds } from '@requests/graphql/queries/dbRefreshJobsQueries';
import {
    StartManualDbRefresh, StopManualDbRefresh, SetScheduledDbRefreshActive, SetUserComment
} from '@requests/graphql/mutations/dbRefereshJobsMutations';
import { DbRefreshJob } from '@shared/types/api/dbRefreshJobs';
import { IRequestError } from '@shared/types/errors';

/** Сброс состояний на значение по ум. */
export const resetPageStateAction = atom(null,
    (get, set) => {
        set(pageLoadingState, RESET);
        set(jobsListState, RESET);
        set(jobsListFilterState, RESET);
        set(personalAccessIdsState, RESET);
        set(confirmRefreshInputState, RESET);
        set(confirmRefreshDialogState, RESET);
        set(cancelRefreshDialogState, RESET);
    });

/** Действие изменения свойств задачи по id. */
export const changeJobPropsAction = atom(null,
    (get, set, id: number, changes: Partial<DbRefreshJob>) => {
        set(jobsListState, prev =>
            prev.map(j => j.id === id ? { ...j, ...changes } : j));
    });

/** Действие возвращает задачу по id */
export const getJobByIdAction = atom(null,
    (get, set, id: number | null | undefined) => {
        return get(jobsListState).find(j => j.id === id) ?? null;
    });

/** Запроса задач на перезаливку БД. */
export const jobsListQuery = atom(null,
    async (get, set) => {
        set(pageLoadingState, true);

        try {
            const res = await GetDbRefreshJobs() as DbRefreshJob[];
            const listItems = res.map(j =>
                <DbRefreshJobListItem>{ id: j.id, dbName: j.dbName });

            set(jobsListState, res);
            set(dbRefreshJobsListItemsState, listItems);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }

        set(pageLoadingState, false);
    });

/** Запрос персональных доступов к БД. */
export const personalAccessesQuery = atom(null,
    async (get, set) => {
        try {
            const accessIds = await GetPersonalAccessIds();
            set(personalAccessIdsState, accessIds);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }
    });

/** Запрос изменения активности перезаливки по расписанию. */
export const setJobScheduledRefreshActiveQuery = atom(null,
    async (get, set, jobId: number, active: boolean) => {
        set(jobLoadingState(jobId), true);

        try {
            const job = await SetScheduledDbRefreshActive(
                jobId, active,
                [
                    'id',
                    'scheduleIsActive',
                    'scheduleChangeUser',
                    'scheduleChangeDate'
                ]);

            set(changeJobPropsAction, job.id!, job);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }

        set(jobLoadingState(jobId), false);
    });

/** Запрос запуска ручной перезаливки БД. */
export const startJobManualRefreshQuery = atom(null,
    async (get, set, jobId: number) => {
        const { delayMinutes, comment } = get(confirmRefreshInputState);

        set(jobLoadingState(jobId), true);

        try {
            const job = await StartManualDbRefresh(
                jobId, delayMinutes, comment,
                ['id', 'manualRefreshDate', 'userComment']);

            set(changeJobPropsAction, job.id!, job);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }

        set(jobLoadingState(jobId), false);
    });

/** Запрос остановки ручной перезаливки БД. */
export const stopJobManualRefreshQuery = atom(null,
    async (get, set, jobId: number) => {
        set(jobLoadingState(jobId), true);

        try {
            const job = await StopManualDbRefresh(
                jobId, ['id', 'manualRefreshDate']);

            set(changeJobPropsAction, job.id!, job);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }

        set(jobLoadingState(jobId), false);
    });

/** Запрос установки пользовательского комментария для задачи на перезаливку БД. */
export const setUserCommentQuery = atom(null,
    async (get, set, jobId: number, comment: string) => {
        set(jobLoadingState(jobId), true);

        try {
            const result = await SetUserComment(
                jobId, comment,
                ['id', 'userComment']);

            set(changeJobPropsAction, result.id!, result);
        }
        catch (e) {
            set(pushErrorAction, e as IRequestError);
        }

        set(jobLoadingState(jobId), false);
    });