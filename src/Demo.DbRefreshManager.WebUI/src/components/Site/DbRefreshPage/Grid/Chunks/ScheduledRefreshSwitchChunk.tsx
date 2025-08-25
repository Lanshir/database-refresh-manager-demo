import { FC, useCallback } from 'react';
import { useSetAtom } from 'jotai';
import { setJobScheduledRefreshActiveQuery } from '@store/dbRefresh/dbRefreshActions';
import { useJobDisabledAtomValue } from '@store/dbRefresh/dbRefreshHooks';
import { Switch } from '@mui/material';

interface ChunkProps {
    jobId: number
    checked?: boolean
    accessRoles: string[]
}

/** Переключатель задач перезаливки по расписанию. */
const ScheduledRefreshSwitchChunk: FC<ChunkProps> = ({ jobId, checked, accessRoles }) => {
    const setRefreshActiveQuery = useSetAtom(setJobScheduledRefreshActiveQuery);

    const jobDisabled = useJobDisabledAtomValue(jobId, accessRoles);

    const onClick = useCallback(
        () => setRefreshActiveQuery(jobId, !checked),
        [setRefreshActiveQuery, jobId, checked]);

    return <Switch disabled={jobDisabled} checked={checked} onClick={onClick} />;
};

export default ScheduledRefreshSwitchChunk;
