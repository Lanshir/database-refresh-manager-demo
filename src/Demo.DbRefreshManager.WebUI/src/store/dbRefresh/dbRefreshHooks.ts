import { useMemo } from 'react';
import { atom, useAtomValue } from 'jotai';
import {
    pageLoadingState, jobLoadingState, jobsListState, personalAccessIdsState
} from './dbRefreshState';
import { authorizationState } from '@store/authorization/authorizationState';

/** Использовать атом, который содержит все условия отключения отдельной задачи. */
export const useJobDisabledAtomValue = (id: number, accessRoles: string[]) =>
    useAtomValue(useMemo(() =>
        atom((get) => {
            const { user } = get(authorizationState);
            const personalAccessIds = get(personalAccessIdsState);

            const pageLoading = get(pageLoadingState);
            const jobLoading = get(jobLoadingState(id));
            const jobInProgress = get(jobsListState).find(j => j.id === id)?.inProgress ?? false;

            const hasAccess = personalAccessIds.includes(id)
                || user.roles.some(r => accessRoles.includes(r));

            return pageLoading || jobLoading || jobInProgress || !hasAccess;
        }), [id, accessRoles]));
