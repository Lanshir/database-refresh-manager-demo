import { FC, useCallback } from 'react';
import { useSetAtom } from 'jotai';
import { confirmRefreshDialogState, cancelRefreshDialogState } from '@store/dbRefresh/dbRefreshState';
import { useJobDisabledAtomValue } from '@store/dbRefresh/dbRefreshHooks';
import { Button } from '@mui/material';
import { PlayArrow, Stop } from '@mui/icons-material';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from '@fortawesome/free-solid-svg-icons/faSpinner';

interface ChunkProps {
    jobId: number
    updateIsQueued: boolean
    jobInProgress: boolean
    accessRoles: string[]
}

/** Кнопка запуска ручной перезаливки БД. */
const ManualRefreshButtonChunk: FC<ChunkProps> = (
    { jobId, updateIsQueued, jobInProgress, accessRoles }
) => {
    const setConfirmDialog = useSetAtom(confirmRefreshDialogState);
    const setCancelDialog = useSetAtom(cancelRefreshDialogState);

    const jobDisabled = useJobDisabledAtomValue(jobId, accessRoles);

    const onStartClick = useCallback(
        () => setConfirmDialog({ jobId }),
        [setConfirmDialog, jobId]);

    const onStopClick = useCallback(
        () => setCancelDialog({ jobId }),
        [setCancelDialog, jobId]);

    return jobInProgress
        ? (
            <Button className="update-db-btn"
                startIcon={(
                    <FontAwesomeIcon icon={faSpinner} spinPulse
                        className="spinner-icon"
                    />
                )}
                variant="outlined" size="large"
                disabled={jobDisabled}
            >
                В процессе...
            </Button>
        )

        : !updateIsQueued
            ? (
                <Button className="update-db-btn"
                    startIcon={<PlayArrow />}
                    variant="outlined" color="success"
                    onClick={onStartClick}
                    disabled={jobDisabled}
                >
                    Старт
                </Button>
            )

            : (
                <Button className="update-db-btn"
                    startIcon={<Stop />}
                    variant="outlined" color="warning"
                    onClick={onStopClick}
                    disabled={jobDisabled}
                >
                    Стоп
                </Button>
            );
};

export default ManualRefreshButtonChunk;
