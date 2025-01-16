import { FC, useCallback } from 'react';
import { useMount } from 'react-use';
import { useAtomValue, useSetAtom } from 'jotai';
import {
    pageLoadingState, jobsListFilteredState, personalAccessIdsState
} from '@store/dbRefresh/dbRefreshState';
import { authorizationState } from '@store/authorization/authorizationState';
import {
    jobsListQuery, personalAccessesQuery, setUserCommentQuery
} from '@store/dbRefresh/dbRefreshActions';
import {
    DataGrid, GridCellParams, GridColDef, GridRenderCellParams,
} from '@mui/x-data-grid';
import { Box } from '@mui/material';
import { nameof } from '@helpers';
import { NameWithNumberComparer } from '@infrastructure/muiDataGrid/cellSortComparers';
import { DbRefreshJob } from '@shared/types/api/dbRefreshJobs';
import Subscriptions from './Chunks/SubscriptionsChunk';
import ManualRefreshButton from './Chunks/ManualRefreshButtonChunk';
// import ObjectsListLink from './Chunks/ObjectsListLinkChunk';
import RefreshSchedule from './Chunks/RefreshScheduleChunk';
import ScheduledRefreshSwitch from './Chunks/ScheduledRefreshSwitchChunk';
import { chain } from 'underscore';
import dayjs from 'dayjs';

/** Таблица списка задач а перезаливку БД. */
const DbJobsListGrid: FC = () => {
    const isLoading = useAtomValue(pageLoadingState);
    const jobsList = useAtomValue(jobsListFilteredState);
    const personalAccessIds = useAtomValue(personalAccessIdsState);
    const { user } = useAtomValue(authorizationState);

    const loadJobs = useSetAtom(jobsListQuery);
    const loadPersonalAccess = useSetAtom(personalAccessesQuery);
    const setUserComment = useSetAtom(setUserCommentQuery);

    const isCellEditable = useCallback(
        (p: GridCellParams<DbRefreshJob>) =>
            p.row.groupAccessRoles.some(r => user.roles.includes(r))
            || personalAccessIds.includes(p.row.id),
        [user.roles, personalAccessIds]);

    const onRowUpdate = useCallback(
        async (newRow: DbRefreshJob, oldRow: DbRefreshJob) => {
            if ((oldRow.userComment ?? '') !== (newRow.userComment ?? '')) {
                setUserComment(newRow.id, newRow.userComment ?? '');
            }

            return newRow;
        },
        [setUserComment]);

    // Начальная загрузка данных.
    useMount(() => {
        loadJobs();
        loadPersonalAccess();
    });

    return (<>
        <Subscriptions />
        <Box className="grid-wrap">
            <DataGrid columns={columns} rows={jobsList}
                columnHeaderHeight={64}
                getRowHeight={useCallback(() => 'auto', [])}
                getRowClassName={useCallback(() => 'grid-row', [])}
                loading={isLoading}
                isCellEditable={isCellEditable}
                processRowUpdate={onRowUpdate}
                disableColumnMenu
                disableRowSelectionOnClick
                hideFooter
            />
        </Box>
    </>);
};

const columns: GridColDef<DbRefreshJob>[] =
    [
        {
            field: 'legendCell',
            headerClassName: 'legend-header',
            cellClassName: 'legend-cell',
            headerName: '',
            minWidth: 1,
            maxWidth: 1,
            valueGetter: (val, row, col, api) => row.groupCssColor,
            renderCell: ({ value }: GridRenderCellParams<DbRefreshJob, string>) =>
                <Box sx={{
                    backgroundColor: value ?? 'transparent',
                    position: 'absolute',
                    height: '100%',
                    width: '100%',
                    zIndex: '-1'
                }} />
        },
        {
            field: nameof<DbRefreshJob>('dbName'),
            headerName: '№ Базы',
            width: 140,
            sortComparator: NameWithNumberComparer
        },
        {
            field: 'manualRefresh',
            headerName: 'Перезаливка',
            width: 160,
            valueGetter: (val, row, col, api) => row.inProgress ? 0
                : !!row.manualRefreshDate ? 1 : 2,
            renderCell: ({ row }) =>
                <ManualRefreshButton
                    jobId={row.id}
                    updateIsQueued={!!row.manualRefreshDate}
                    jobInProgress={row.inProgress}
                    accessRoles={row.groupAccessRoles}
                />
        },
        {
            field: 'scheduledRefresh',
            headerName: 'Перезаливка по расписанию',
            width: 125,
            align: 'center',
            valueGetter: (val, row, col, api) => row.scheduleIsActive,
            renderCell: ({ value, row }: GridRenderCellParams<DbRefreshJob, boolean>) =>
                <ScheduledRefreshSwitch
                    jobId={row.id}
                    checked={value}
                    accessRoles={row.groupAccessRoles}
                />
        },
        {
            field: 'refreshSchedule',
            headerName: 'Расписание',
            width: 200,
            sortable: false,
            headerAlign: 'center',
            align: 'center',
            cellClassName: 'schedule-cell cell-padding',
            renderCell: ({ row }: GridRenderCellParams<DbRefreshJob>) =>
                <RefreshSchedule
                    manualRefreshDate={row.manualRefreshDate}
                    scheduleRefreshDate={row.scheduleRefreshDate}
                    lastRefreshDate={row.lastRefreshDate}
                    scheduleIsActive={row.scheduleIsActive}
                />
        },
        {
            field: 'lastChange',
            headerName: 'Последнее изменение',
            width: 150,
            headerAlign: 'center',
            align: 'center',
            cellClassName: 'pre-line-text',
            valueGetter: (val, row, col, api) =>
                !row.scheduleChangeDate ? '' : [
                    `${row.scheduleChangeUser}`,
                    `${dayjs(row.scheduleChangeDate).format('dd DD.MM.YY HH:mm')}`
                ].join('\n')
        },
        {
            field: nameof<DbRefreshJob>('releaseComment'),
            headerName: 'Установлено',
            flex: 1,
            minWidth: 200,
            sortable: false,
            cellClassName: 'cell-padding pre-line-text release-comment-cell',
            valueGetter: (val: string | undefined, row, col, api) => {
                const strArr = (val ?? '').split('\n');

                return strArr.length < 2
                    ? val ?? ''
                    : chain(strArr).unique()
                        .sortBy(v => v.toUpperCase())
                        .join('\n')
                        .value();
            }
        },
        {
            field: nameof<DbRefreshJob>('userComment'),
            headerName: 'Комментарии',
            flex: 1,
            minWidth: 200,
            sortable: false,
            editable: true,
            cellClassName: 'cell-padding pre-line-text'
        },
        /*
        {
            field: 'objectsLog',
            headerName: 'Список изменённых объектов',
            width: 110,
            sortable: false,
            align: 'center',
            valueGetter: (val, row, col, api) => row.dbName,
            renderCell: ({ value }: GridRenderCellParams<DbRefreshJob, string>) =>
                <ObjectsListLink dbName={value ?? ''} />
        }
        */
    ];

export default DbJobsListGrid;