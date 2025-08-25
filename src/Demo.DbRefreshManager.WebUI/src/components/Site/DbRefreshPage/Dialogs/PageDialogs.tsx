import { FC, Fragment, useCallback, useMemo } from 'react';
import { useAtomValue, useSetAtom } from 'jotai';
import { useResetAtom } from 'jotai/utils';
import {
    cancelRefreshDialogState, confirmRefreshDialogState
} from '@store/dbRefresh/dbRefreshState';
import {
    startJobManualRefreshQuery, stopJobManualRefreshQuery, getJobByIdAction
} from '@store/dbRefresh/dbRefreshActions';
import { ConfirmDialog } from '@shared/components';
import ConfirmDbRefreshDialog from './ConfirmDbRefreshDialog';

/** Дилоговые окна страницы перезаливки БД. */
export const PageDialogs: FC = () => {
    const getJobById = useSetAtom(getJobByIdAction);

    const { jobId: jobIdToRefresh } = useAtomValue(confirmRefreshDialogState);
    const startJobRefresh = useSetAtom(startJobManualRefreshQuery);
    const resetConfirmDialog = useResetAtom(confirmRefreshDialogState);

    const jobToRefresh = useMemo(() => getJobById(jobIdToRefresh), [getJobById, jobIdToRefresh]);

    const { jobId: jobIdToCancel } = useAtomValue(cancelRefreshDialogState);
    const stopJobRefresh = useSetAtom(stopJobManualRefreshQuery);
    const resetCancelDialog = useResetAtom(cancelRefreshDialogState);

    const jobToCancel = useMemo(() => getJobById(jobIdToCancel), [getJobById, jobIdToCancel]);

    const onConfirmDialogYes = useCallback(
        () => {
            startJobRefresh(jobIdToRefresh!);
            resetConfirmDialog();
        },
        [startJobRefresh, resetConfirmDialog, jobIdToRefresh]
    );

    const onCancelDialogYes = useCallback(
        () => {
            stopJobRefresh(jobIdToCancel!);
            resetCancelDialog();
        },
        [stopJobRefresh, resetCancelDialog, jobIdToCancel]
    );

    return (
        <Fragment>
            <ConfirmDbRefreshDialog open={!!jobToRefresh}
                title={`Запустить перезаливку БД ${jobToRefresh?.dbName ?? ''}?`}
                onYes={onConfirmDialogYes}
                onNo={resetConfirmDialog}
            />
            <ConfirmDialog open={!!jobToCancel}
                title=""
                text={`Отменить перезаливку БД ${jobToCancel?.dbName ?? ''}?`}
                buttons={['Yes', 'No']}
                onYes={onCancelDialogYes}
                onClose={resetCancelDialog}
                onNo={resetCancelDialog}
            />
        </Fragment>
    );
};

export default PageDialogs;
